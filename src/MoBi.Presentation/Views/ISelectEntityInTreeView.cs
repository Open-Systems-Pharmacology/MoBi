using System.Collections.Generic;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface ISelectEntityInTreeView : IView<ISelectEntityInTreePresenter>
   {
      void Display(IReadOnlyList<ITreeNode> treeNodes);
      ObjectBaseDTO Selected { get; }
      ITreeNode GetNode(string id);
      void ExpandRootNodes();
   }
}