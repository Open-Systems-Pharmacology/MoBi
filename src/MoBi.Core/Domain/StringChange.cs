using MoBi.Core.Commands;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Domain
{
   public interface IStringChange
   {
      IMoBiCommand ChangeCommand { set; get; }
      string ChangeDescription { get; }
      IBuildingBlock BuildingBlock { get; set; }
   }

   public class StringChange<T> : IStringChange
   {
      public IBuildingBlock BuildingBlock { get; set; }
      public T EntityToEdit { get; set; }
      public IMoBiCommand ChangeCommand { get; set; }
      public string ChangeDescription { get; set; }
   }
}