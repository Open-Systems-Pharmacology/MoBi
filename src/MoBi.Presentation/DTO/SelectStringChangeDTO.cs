using OSPSuite.Utility.Reflection;
using MoBi.Core.Domain;

namespace MoBi.Presentation.DTO
{
   public class SelectStringChangeDTO : Notifier
   {
      private bool _selected;
      public IStringChange Change { get; set; }
      public string Description { get; set; }
      public string BuildingBlock { get; set; }

      public bool Selected
      {
         get { return _selected; }
         set
         {
            _selected = value;
            OnPropertyChanged(() => Selected);
         }
      }

   }
}