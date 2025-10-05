using Inverse.Domain;
using System.Reflection;

namespace Inverse.Application;

public static class Setup
{
    public static void InstallPlugins(this IDatabaseService service)
    {
        IEnumerable<Assembly> plugins = GetAllPlugins();

        foreach (var t in plugins.SelectMany(x => x.ExportedTypes))
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

    private static IEnumerable<Assembly> GetAllPlugins()
    {
        var pluginPrefix = string.Concat(nameof(Inverse), ".", nameof(Inverse.Plugin), ".");
        var referencedPaths = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll").Where(r => r.Contains(pluginPrefix));

        foreach (var path in referencedPaths)
        {
            AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName(path));
        }

        var plugins = AppDomain.CurrentDomain.GetAssemblies().Where(a => a.GetName().Name.StartsWith(pluginPrefix));

        return plugins;
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