using MoBi.Core.Domain.Model;
using OSPSuite.Core.Converters.v12;
using System.Xml.Linq;

namespace MoBi.Core.Serialization.Converter.v12
{
   public class Converter11To12 : IMoBiObjectConverter
   {
      private readonly Converter110To120 _coreConverter;

      public Converter11To12(Converter110To120 coreConverter)
      {
         _coreConverter = coreConverter;
      }

      public bool IsSatisfiedBy(int version) => version == ProjectVersions.V11_0;

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