using Inverse.Domain;

namespace Inverse.Plugin.FileManager.EncryptedXml
{
    public sealed class InversedFileManagerStrategy : IFileManagerStrategy
    {
        private readonly EncryptedXmlFileManagerStrategy encryptedXmlFileManagerStrategy;

        public string Extension => ".inversed";
        public string Name => nameof(InversedFileManagerStrategy);
        public string Description => "Arquivo de Modelo de dados V2";

        public InversedFileManagerStrategy()
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
