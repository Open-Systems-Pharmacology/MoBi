using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace MoBi.Core
{
   public static class XmlHelper
   {
      public static XElement RootElementFromString(string serializationString)
      {
         var document = DocumentFromString(serializationString);
         return document.Root;
      }

      public static XDocument DocumentFromString(string serializationString)
      {
         return XDocument.Load(new StringReader(serializationString));
      }
       
      public static XElement RootElementFromFile(string fileName)
      {
         var document = XDocument.Load(fileName);
         return document.Root;
      }

      /// <summary>
      /// Return the content of the file whose path was specified with the given parameter
      /// </summary>
      public static Func<string, string> XmlContentFromFile = xmlContentFromFile;

      private static string xmlContentFromFile(string fileName)
      {
         var root = RootElementFromFile(fileName);
         if (root == null) return string.Empty;
         return XmlContentToString(root);
      }

      /// <summary>
      /// returns the string representing the xml element and remove the formatting
      /// </summary>
      public static string XmlContentToString(XElement element)
      {
         return element.ToString(SaveOptions.DisableFormatting);
      }

    

      /// <summary>
      /// Save the content of the first parameter to the file whose path was specified with the second parameter
      /// </summary>
      public static Action<string, string> SaveXmlContentToFile = saveXmlContentToFile;

      private static void saveXmlContentToFile(string xmlContent, string fileName)
      {
         var doc = DocumentFromString(xmlContent);
         doc.Save(fileName);
      }

      /// <summary>
      /// Only use for tests
      /// </summary>
      public static void Reset()
      {
         SaveXmlContentToFile = saveXmlContentToFile;
         XmlContentFromFile = xmlContentFromFile;
      }

      public static byte[] XmlContentToByte(XElement element)
      {
         using (var stream = new MemoryStream())
         {
            using (var textWriter = new XmlTextWriter(stream, Encoding.UTF8))
            {
               textWriter.Formatting = Formatting.None;
               textWriter.Indentation = 0;
               element.Save(textWriter);
            }
            return stream.ToArray();
         }
      }

      public static XElement ElementFromBytes(byte[] serializationByte)
      {
         using (var stream = new MemoryStream(serializationByte))
         using (var reader = new StreamReader(stream, Encoding.UTF8, false))
         {
            return XElement.Load(reader);
         }
      }
   }
}