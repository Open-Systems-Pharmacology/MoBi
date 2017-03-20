using OSPSuite.Core.Domain;

namespace MoBi.Core.Services
{
   public class SearchResult
   {
      public IObjectBase ProjectItem { get; set; }
      public IObjectBase FoundObject { get; set; }

      public SearchResult(IObjectBase foundObject, IObjectBase simulationItem)
      {
         ProjectItem = simulationItem;
         FoundObject = foundObject;
      }
   }
}