using MoBi.Core;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Views;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface IValidationOptionsPresenter : IPresenter<IValidationOptionsView>
   {
      void Edit(ValidationSettings validationOptions);

      /// <summary>
      ///    Edits the validation options of a simulation together with its <paramref name="simulationSolverOptionsDTO" />
      /// </summary>
      void Edit(ValidationSettings validationOptions, SimulationSolverOptionsDTO simulationSolverOptionsDTO);
   }

   public class ValidationOptionsPresenter : AbstractPresenter<IValidationOptionsView, IValidationOptionsPresenter>, IValidationOptionsPresenter
   {
      public ValidationOptionsPresenter(IValidationOptionsView view) : base(view)
      {
      }

      public void Edit(ValidationSettings validationOptions)
      {
         _view.BindTo(validationOptions);
      }

      public void Edit(ValidationSettings validationOptions, SimulationSolverOptionsDTO simulationSolverOptionsDTO)
      {
         Edit(validationOptions);
         _view.BindTo(simulationSolverOptionsDTO);
      }
   }
}
