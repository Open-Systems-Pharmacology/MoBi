using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IEditActiveTransportBuilderContainerView : IView<IEditTransporterMoleculeContainerPresenter>
   {
      void Show(TransporterMoleculeContainerDTO dtoTransporterMoleculeContainer);
      void SetParameterView(IView view);
      void ShowParameters();
   }
}