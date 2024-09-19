using System.Collections.Generic;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface ICreateObjectPathsFromReferencesView  :IView<ICreateObjectPathsFromReferencesPresenter>
   {
      void AddReferenceSelectionView(IView view);
      IReadOnlyList<string> AllPaths { get; }
      void CanAdd(bool canAdd);
      void AddSelectedPaths(IReadOnlyList<string> pathsToAdd);
   }
}
