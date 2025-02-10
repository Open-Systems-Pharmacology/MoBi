using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface ISelectOrganAndProteinsView : IModalView<ISelectOrganAndProteinsPresenter>
   {
      void AddMoleculeSelectionView(IView view);
      void AddOrganSelectionView(IView view);
      void SelectionChanged();
   }
}