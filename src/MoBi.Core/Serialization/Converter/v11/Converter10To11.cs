using System.Xml.Linq;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Converters.v11;

namespace MoBi.Core.Serialization.Converter.v11
{
   public class Converter10To11 : IMoBiObjectConverter
   {
      private readonly Converter100To110 _coreConverter;

      public Converter10To11(Converter100To110 coreConverter)
      {
         _coreConverter = coreConverter;
      }

      public bool IsSatisfiedBy(int version) => version == ProjectVersions.V10_0;

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