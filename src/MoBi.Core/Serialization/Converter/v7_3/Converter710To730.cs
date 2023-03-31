using System.Xml.Linq;
using MoBi.Core.Domain.Model;

namespace MoBi.Core.Serialization.Converter.v7_3
{
   public class Converter710To730 : IMoBiObjectConverter
   {
      private readonly OSPSuite.Core.Converters.v7_3.Converter710To730 _coreConverter;

      public Converter710To730(OSPSuite.Core.Converters.v7_3.Converter710To730 coreConverter)
      {
         _coreConverter = coreConverter;
      }

      public bool IsSatisfiedBy(int version)
      {
         return version == ProjectVersions.V7_1_0;
      }

      public (int convertedToVersion, bool conversionHappened) Convert(object objectToUpdate, MoBiProject project)
      {
         return _coreConverter.Convert(objectToUpdate);
      }

      public (int convertedToVersion, bool conversionHappened) ConvertXml(XElement element, MoBiProject project)
      {
         return _coreConverter.ConvertXml(element);
      }
   }
}