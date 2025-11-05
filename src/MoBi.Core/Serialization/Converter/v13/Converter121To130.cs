using System.Xml.Linq;
using MoBi.Core.Domain.Model;
using CoreConverter121To130 = OSPSuite.Core.Converters.v13.Converter121To130;

namespace MoBi.Core.Serialization.Converter.v13;

public class Converter121To130(CoreConverter121To130 coreConverter) : IMoBiObjectConverter
{
   public bool IsSatisfiedBy(int version) => version == ProjectVersions.V12_1;

   public (int convertedToVersion, bool conversionHappened) Convert(object objectToUpdate, MoBiProject project)
   {
      (_, bool converted) = coreConverter.Convert(objectToUpdate);
      return (ProjectVersions.V13_0, converted);
   }

   public (int convertedToVersion, bool conversionHappened) ConvertXml(XElement element, MoBiProject project)
   {
      (_, bool converted) = coreConverter.ConvertXml(element);
      return (ProjectVersions.V13_0, converted);
   }
}