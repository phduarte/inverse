using Inverse.Domain;

namespace Inverse.Plugin.FileManager.EncryptedXml
{
    public sealed class DiagramModelFileManagerStrategy : IFileManagerStrategy
    {
        private readonly EncryptedXmlFileManagerStrategy encryptedXmlFileManagerStrategy;

        public string Extension => ".dm";
        public string Name => nameof(InversedFileManagerStrategy);
        public string Description => "Arquivo de Diagrama";

        public DiagramModelFileManagerStrategy()
        {
            encryptedXmlFileManagerStrategy = new EncryptedXmlFileManagerStrategy();
        }

        public Database OpenFile(string fileName)
        {
            return encryptedXmlFileManagerStrategy.OpenFile(fileName);
        }

        public void SaveFile(Database database, string fileName)
        {
            encryptedXmlFileManagerStrategy.SaveFile(database, fileName);
        }
    }
}