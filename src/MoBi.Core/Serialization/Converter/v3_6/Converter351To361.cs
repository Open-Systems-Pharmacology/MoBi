using System.Xml.Linq;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Converter.v5_6;

namespace MoBi.Core.Serialization.Converter.v3_6
{
   public class Converter351To361 : IMoBiObjectConverter
   {
      private readonly Converter551To561 _coreConverter;

      public Converter351To361(Converter551To561 coreConverter)
      {
         _coreConverter = coreConverter;
      }

      public bool IsSatisfiedBy(int item)
      {
         return _coreConverter.IsSatisfiedBy(item);
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