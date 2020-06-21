using System.Xml.Linq;
using MoBi.Core.Domain.Model;

namespace MoBi.Core.Serialization.Converter.v9
{
   public class Converter730To90 : IMoBiObjectConverter
   {
      private readonly OSPSuite.Core.Converters.v9.Converter730To90 _coreConverter;

      public Converter730To90(OSPSuite.Core.Converters.v9.Converter730To90 coreConverter)
      {
         _coreConverter = coreConverter;
      }

      public bool IsSatisfiedBy(int version) => version == ProjectVersions.V7_3_0;

      public (int convertedToVersion, bool conversionHappened) Convert(object objectToUpdate, IMoBiProject project)
      {
         return _coreConverter.Convert(objectToUpdate);
      }

      public (int convertedToVersion, bool conversionHappened) ConvertXml(XElement element, IMoBiProject project)
      {
         return _coreConverter.ConvertXml(element);
      }
   }
}