using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface ISelectMoleculesForBuildingBlockView : IModalView<ISelectMoleculesForBuildingBlockPresenter>
   {
      void Show(SelectMoleculesDTO moleculesDTOSelectMolecules);
      void Close();
   }
}