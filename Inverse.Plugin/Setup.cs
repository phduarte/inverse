using Inverse.Domain.Services;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Inverse.Plugin
{
    public static class Setup
    {
        public static void AddPlugins(this IDatabaseService service)
        {
            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
            var loadedPaths = loadedAssemblies.Select(a => a.Location).ToArray();

            var referencedPaths = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll").Where(r => r.Contains("Inverse.Plugin."));
            var toLoad = referencedPaths.Where(r => !loadedPaths.Contains(r, StringComparer.InvariantCultureIgnoreCase)).ToList();

            toLoad.ForEach(path => loadedAssemblies.Add(AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName(path))));

            var asm = AppDomain.CurrentDomain.GetAssemblies().Where(a => a.FullName.Contains("Inverse.Plugin."));
            foreach (var a in asm)
            {
                foreach (var t in a.ExportedTypes)
                {
                    if (TryGetInstance(t, out IDatabaseGeneratorStrategy databaseGeneratorStrategy))
                    {
                        service.Install(databaseGeneratorStrategy);
                    }
                    else if (TryGetInstance(t, out IScriptingGeneratorStrategy scriptingGeneratorStrategy))
                    {
                        service.Install(scriptingGeneratorStrategy);
                    }
                    else if (TryGetInstance(t, out IFileManagerStrategy fileManagerStrategy))
                    {
                        service.Install(fileManagerStrategy);
                    }
                }
            }
        }

        private static bool TryGetInstance<T>(Type type, out T instance)
        {
            instance = default;

            if (type.IsAssignableTo(typeof(T)))
            {
                instance = (T)Activator.CreateInstance(type);
            }

            return instance != null;
        }
    }
}
