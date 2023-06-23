using System.Xml.Linq;
using OSPSuite.Utility;
using MoBi.Core.Domain.Model;

namespace MoBi.Core.Serialization.Converter
{
   public interface IMoBiObjectConverter : ISpecification<int>
   {
      (int convertedToVersion, bool conversionHappened) Convert(object objectToUpdate, MoBiProject project);
      (int convertedToVersion, bool conversionHappened) ConvertXml(XElement element, MoBiProject project);
   }

    internal class MoBiNullConverter : IMoBiObjectConverter
   {
      public (int convertedToVersion, bool conversionHappened) Convert(object objectToUpdate, MoBiProject project)
      {
         return (ProjectVersions.Current, false);
      }

      public (int convertedToVersion, bool conversionHappened) ConvertXml(XElement element, MoBiProject project)
      {
         return (ProjectVersions.Current, false);
      }

      public bool IsSatisfiedBy(int item)
      {
         return false;
      }
   }
}