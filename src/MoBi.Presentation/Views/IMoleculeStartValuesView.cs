using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IMoleculeStartValuesView : IView<IMoleculeStartValuesPresenter>, IStartValuesView<MoleculeStartValueDTO>
   {
      void AddIsPresentSelectionView(IView view);

      void AddNegativeValuesAllowedSelectionView(IView view);
   }
}