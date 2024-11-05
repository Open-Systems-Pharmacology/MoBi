using System.Collections.Generic;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IEventGroupsListView : IView<IEventGroupListPresenter>
   {
      void Show(IReadOnlyList<EventGroupBuilderDTO> dtoEventGroupBuilders);
      void AddFixedNode(ITreeNode treeNode);
   }
}