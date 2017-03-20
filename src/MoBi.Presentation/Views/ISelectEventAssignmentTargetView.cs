using System.Collections.Generic;
using OSPSuite.Presentation.Nodes;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface ISelectEventAssignmentTargetView : IModalView<ISelectEventAssingmentTargetPresenter>
   {
      void BindTo(IEnumerable<IObjectBaseDTO> dtos);
      IObjectBaseDTO Selected { get; }
      ITreeNode GetNode(string id);
   }
}