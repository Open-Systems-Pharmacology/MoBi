using System.Collections.Generic;
using OSPSuite.Presentation.Nodes;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IHierarchicalStructureView : IView<IHierarchicalStructurePresenter>
   {
      void Show(IEnumerable<ObjectBaseDTO> roots);
      void Add(ObjectBaseDTO newChild, ObjectBaseDTO parent);
      void Remove(ObjectBaseDTO dtoObjectBaseToRemove);
      void AddNode(ITreeNode newNode);
      void AddRoot(ObjectBaseDTO dto);
      void Select(string id);
      void Clear();
   }
}