using System.Xml.Linq;
using OSPSuite.Utility;
using MoBi.Core.Domain.Model;

namespace MoBi.Core.Serialization.Converter
{
   public interface IMoBiObjectConverter : ISpecification<int>
   {
      int Convert(object objectToUpdate, IMoBiProject project);
      int ConvertXml(XElement element, IMoBiProject project);
   }

    internal class MoBiNullConverter : IMoBiObjectConverter
   {
      public int Convert(object objectToUpdate, IMoBiProject project)
      {
         return ProjectVersions.Current;
      }

      public int ConvertXml(XElement element, IMoBiProject project)
      {
         return ProjectVersions.Current;
      }

      public bool IsSatisfiedBy(int item)
      {
         return false;
      }
   }
}