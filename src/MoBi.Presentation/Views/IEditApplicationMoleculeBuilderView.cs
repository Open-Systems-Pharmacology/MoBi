using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IEditApplicationMoleculeBuilderView : IView<IEditApplicationMoleculeBuilderPresenter>, IActivatableView
   {
      void Show(ApplicationMoleculeBuilderDTO dtoApplicationMoleculeBuilder);
      void SetFormulaView(IView view);
   }
}