using MoBi.Core;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IValidationOptionsView : IView<IValidationOptionsPresenter>
   {
      void BindTo(ValidationSettings validationOptions);

      /// <summary>
      ///    Shows the solver options of a simulation along with the validation options
      /// </summary>
      void BindTo(SimulationSolverOptionsDTO simulationSolverOptionsDTO);
   }
}
