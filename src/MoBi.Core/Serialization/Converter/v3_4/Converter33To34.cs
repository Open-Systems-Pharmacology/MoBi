using System.Xml.Linq;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Converter.v5_4;

namespace MoBi.Core.Serialization.Converter.v3_4
{
   public class Converter33To34 : Converter531To541, IMoBiObjectConverter
   {
      public (int convertedToVersion, bool conversionHappened) Convert(object objectToUpdate, IMoBiProject project)
      {
         return Convert(objectToUpdate);
      }

      public (int convertedToVersion, bool conversionHappened) ConvertXml(XElement element, IMoBiProject project)
      {
         return ConvertXml(element);
      }
   }
}