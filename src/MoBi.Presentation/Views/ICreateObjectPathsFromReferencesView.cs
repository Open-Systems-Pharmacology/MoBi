using System.Collections.Generic;
using System.Drawing;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface ICreateObjectPathsFromReferencesView  :IView<ICreateObjectPathsFromReferencesPresenter>
   {
      void AddReferenceSelectionView(IView view);
      IReadOnlyList<string> AllPaths { get; }
      Size? ModalSize { get; }
      void CanAdd(bool canAdd);
      void AddSelectedPaths(IReadOnlyList<string> pathsToAdd);
   }
}
