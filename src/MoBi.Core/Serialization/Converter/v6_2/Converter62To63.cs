using System.Xml.Linq;
using MoBi.Core.Domain.Model;

namespace MoBi.Core.Serialization.Converter.v6_2
{
   public class Converter62To63 : IMoBiObjectConverter
   {
      private readonly OSPSuite.Core.Converter.v6_3.Converter62To63 _coreConverter;

      public Converter62To63(OSPSuite.Core.Converter.v6_3.Converter62To63 coreConverter)
      {
         _coreConverter = coreConverter;
      }

      public bool IsSatisfiedBy(int version)
      {
         //no converter from 61 to 62
         return version == ProjectVersions.V6_2_1 || version == ProjectVersions.V6_1_1;
      }

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