using OSPSuite.Core.Domain;

namespace MoBi.Presentation.DTO
{
   public class SearchResultDTO
   {
      public IObjectBase Object { get; set; }

      //Building block or simulation
      public IObjectBase ProjectItem { set; get; }
      public string TypeName { get; set; }
      public string Path { get; set; }
      public string ProjectItemName { get;set; }
   }
}