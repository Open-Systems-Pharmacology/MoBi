using System.Collections.Generic;
using OSPSuite.Presentation.Nodes;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IHierarchicalStructureView : IView<IHierarchicalStructurePresenter>
   {
      void Show(IEnumerable<IObjectBaseDTO> roots);
      void Add(IObjectBaseDTO newChild, IObjectBaseDTO parent);
      void Remove(IObjectBaseDTO dtoObjectBaseToRemove);
      void AddNode(ITreeNode newNode);
      void AddRoot(IObjectBaseDTO dto);
      void Select(string id);
      void Clear();
   }
}