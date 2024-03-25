using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface ISelectSpatialStructureAndMoleculesView : IModalView<ISelectSpatialStructureAndMoleculesPresenter>
   {
      void Show(SelectSpatialStructureDTO dto);
      void AddMoleculeSelectionView(IView view);
      void MoleculeSelectionChanged();
      void SetDescriptionText(string description);
   }
}