using System.Collections.Generic;
using OSPSuite.Presentation.Nodes;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IHierarchicalStructureView : IView<IHierarchicalStructurePresenter>
   {
      void AddRoot(ObjectBaseDTO dto);
      void Refresh(ObjectBaseDTO objectToRefresh);
      void Show(IEnumerable<ObjectBaseDTO> roots);
      void Add(ObjectBaseDTO newChild, ObjectBaseDTO parent);

      void Remove(IWithId withId);
      void AddNode(ITreeNode newNode);
      void Select(IWithId withId);
      void CopyToClipBoard(string text);
        void Clear();
   }
}