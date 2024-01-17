using System.Collections.Generic;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IPathAndValueEntitySelectionView : IModalView<IPathAndValueEntitySelectionPresenter>
   {
      void AddSelectableEntities(IReadOnlyList<SelectableReplacePathAndValueDTO> selectableObjectPaths);
      void SetDescription(string description);
   }
}