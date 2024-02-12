using OSPSuite.Presentation.DTO;

namespace MoBi.Presentation.DTO
{
   public abstract class SelectableDTO<TSelectable, TParentDTO> : DxValidatableDTO where TParentDTO : ContainsMultiSelectDTO<TSelectable, TParentDTO> where TSelectable : SelectableDTO<TSelectable, TParentDTO>
   {
      private bool _selected;
      public TParentDTO ParentDTO { set; get; }

      public bool Selected
      {
         get => _selected;
         set
         {
            _selected = value;
            ParentDTO?.SelectionUpdated();
         }
      }
   }
}