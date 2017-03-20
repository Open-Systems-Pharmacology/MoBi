using System.Collections.Generic;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface ISelectRenamingView : IModalView<ISelectRenamingPresenter>
   {
      void SetData(IEnumerable<SelectStringChangeDTO> dtos, bool renameDependentObjectsDefault);
      bool RenameDefault { get; set; }
   }
}