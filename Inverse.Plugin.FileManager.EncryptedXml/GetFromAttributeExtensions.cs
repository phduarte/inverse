using System.Xml;

namespace Inverse.Plugin.FileManager.EncryptedXml;

internal static class GetFromAttributeExtensions
{
    public static string GetFromAttribute(this XmlNode xmlNode, string name, bool isBase64 = false)
    {
        var txt = xmlNode.Attributes[name]?.Value;

        if (isBase64)
        {
            return txt.FromBase64();
        }

        return txt;
    }

    public static string GetFromAttribute(this XmlElement xmlElement, string name, bool isBase64 = false)
    {
        var txt = xmlElement.GetAttribute(name);

        if (isBase64)
        {
            txt = txt.FromBase64();
        }

        return txt;
    }
}