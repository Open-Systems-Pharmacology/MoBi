using System.Collections.Generic;
using OSPSuite.Presentation.Nodes;
using MoBi.Core.Domain;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface ISelectReferenceView : IView<ISelectReferencePresenter>
   {
      /// <summary>
      ///    Clear the tree and show the nodes
      /// </summary>
      void Show(IEnumerable<ITreeNode> nodes);

      IObjectBaseDTO SelectedDTO { get; }
      ObjectPathType ObjectPathType { get; set; }
      string Localisation { get; set; }
      bool ChangeLocalisationAllowed { get; set; }

      /// <summary>
      ///    Add nodes to the view
      /// </summary>
      void AddNodes(IEnumerable<ITreeNode> nodes);

      /// <summary>
      ///    Add node to the view. If many nodes are added at once, prefer the AddNodes method
      /// </summary>
      void AddNode(ITreeNode node);

      /// <summary>
      ///    Selects the specified entity in View.
      /// </summary>
      /// <param name="entityToSelect">The entity to select.</param>
      void Select(IEntity entityToSelect);

      bool Shows(IObjectBase entity);

      void Remove(IObjectBase removedObject);
      IReadOnlyList<ITreeNode> GetNodes(IObjectBase objectBase);
   }
}