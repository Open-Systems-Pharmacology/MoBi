using System.Xml.Linq;
using MoBi.Core.Domain.Model;

namespace MoBi.Core.Serialization.Converter.v6_4
{
   public class Converter63To64 : IMoBiObjectConverter
   {
      private readonly OSPSuite.Core.Converter.v6_4.Converter63To64 _coreConverter;

      public Converter63To64(OSPSuite.Core.Converter.v6_4.Converter63To64 coreConverter)
      {
         _coreConverter = coreConverter;
      }

      public bool IsSatisfiedBy(int version)
      {
         return version == ProjectVersions.V6_3_1;
      }

      public int Convert(object objectToUpdate, IMoBiProject project)
      {
         return _coreConverter.Convert(objectToUpdate);
      }

      public int ConvertXml(XElement element, IMoBiProject project)
      {
         return _coreConverter.ConvertXml(element);
     }
   }
}