using MoBi.Core;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter.Simulation;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface IFinalOptionsPresenter : ISimulationConfigurationItemPresenter
   {
      /// <summary>
      ///    Edits the options of the simulation configured with <paramref name="simulationConfiguration" />. Options that
      ///    are not set in <paramref name="validationOptions" /> start from the user settings
      /// </summary>
      void Edit(SimulationConfiguration simulationConfiguration, SimulationValidationOptions validationOptions);

      /// <summary>
      ///    Applies the solver options edited in the wizard to the <paramref name="simulationConfiguration" />
      /// </summary>
      void ApplyOptionsTo(SimulationConfiguration simulationConfiguration);

      /// <summary>
      ///    The validation options edited in the wizard, with all options set
      /// </summary>
      SimulationValidationOptions ValidationOptions { get; }
   }

   public class FinalOptionsPresenter : AbstractSubPresenter<IFinalOptionsView, IFinalOptionsPresenter>, IFinalOptionsPresenter
   {
      private readonly IValidationOptionsPresenter _validationOptionsPresenter;
      private readonly IUserSettings _userSettings;
      private ValidationSettings _validationSettings;
      private SimulationSolverOptionsDTO _simulationSolverOptionsDTO;

      public FinalOptionsPresenter(IFinalOptionsView view, IValidationOptionsPresenter validationOptionsPresenter, IUserSettings userSettings) : base(view)
      {
         _validationOptionsPresenter = validationOptionsPresenter;
         _userSettings = userSettings;
         _view.SetValidationOptionsView(_validationOptionsPresenter.View);
         AddSubPresenters(_validationOptionsPresenter);
      }

      public void Edit(SimulationConfiguration simulationConfiguration) => Edit(simulationConfiguration, new SimulationValidationOptions());

      public void Edit(SimulationConfiguration simulationConfiguration, SimulationValidationOptions validationOptions)
      {
         //Edit copies so that nothing is changed when the wizard is canceled
         _validationSettings = validationOptions.ValidationSettingsWith(_userSettings);
         _simulationSolverOptionsDTO = new SimulationSolverOptionsDTO
         {
            CheckForNegativeValues = simulationConfiguration.SimulationSettings.Solver.CheckForNegativeValues
         };
         _validationOptionsPresenter.Edit(_validationSettings, _simulationSolverOptionsDTO);
      }

      public void ApplyOptionsTo(SimulationConfiguration simulationConfiguration)
      {
         simulationConfiguration.SimulationSettings.Solver.CheckForNegativeValues = _simulationSolverOptionsDTO.CheckForNegativeValues;
      }

      public SimulationValidationOptions ValidationOptions => SimulationValidationOptions.From(_validationSettings);
   }
}
