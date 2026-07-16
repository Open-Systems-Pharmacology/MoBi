using System.Xml.Linq;
using MoBi.Core.Domain.Model;
using CoreConverter120To121 = OSPSuite.Core.Converters.v12.Converter120To121;

namespace MoBi.Core.Serialization.Converter.v12;

public class Converter120To121 : IMoBiObjectConverter
{
   private readonly CoreConverter120To121 _coreConverter;

   public Converter120To121(CoreConverter120To121 coreConverter)
   {
      _coreConverter = coreConverter;
   }

   public bool IsSatisfiedBy(int version) => version == ProjectVersions.V12_0;

   public (int convertedToVersion, bool conversionHappened) Convert(object objectToUpdate, MoBiProject project)
   {
      (_,bool converted) = _coreConverter.Convert(objectToUpdate);
      return (ProjectVersions.V12_1, converted);
   }

   public (int convertedToVersion, bool conversionHappened) ConvertXml(XElement element, MoBiProject project)
   {
      (_, bool converted) = _coreConverter.ConvertXml(element);
      return (ProjectVersions.V12_1, converted);
   }
}
