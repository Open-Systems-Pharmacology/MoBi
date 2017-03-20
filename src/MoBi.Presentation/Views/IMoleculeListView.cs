using System.Collections.Generic;
using OSPSuite.Presentation.Nodes;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IMoleculeListView : IView<IMoleculeListPresenter>
   {
      void Show(IEnumerable<MoleculeBuilderDTO> dtos);
      void SelectItem(IObjectBase objectBase);
      void AddNode(ITreeNode treeNode);
      void Clear();
   }
}