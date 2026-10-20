using MoBi.Core;
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
      ///    Saves the user options edited in the wizard to the user settings and applies the simulation options
      ///    edited in the wizard to the <paramref name="simulationConfiguration" />
      /// </summary>
      void ApplyOptionsTo(SimulationConfiguration simulationConfiguration);
   }

   public class FinalOptionsPresenter : AbstractSubPresenter<IFinalOptionsView, IFinalOptionsPresenter>, IFinalOptionsPresenter
   {
      private readonly IValidationOptionsPresenter _validationOptionsPresenter;
      private readonly IUserSettings _userSettings;
      private ValidationSettings _validationSettings;
      private FinalOptionsDTO _finalOptionsDTO;

      public FinalOptionsPresenter(IFinalOptionsView view, IValidationOptionsPresenter validationOptionsPresenter, IUserSettings userSettings) : base(view)
      {
         _validationOptionsPresenter = validationOptionsPresenter;
         _userSettings = userSettings;
         _view.SetValidationOptionsView(_validationOptionsPresenter.View);
         AddSubPresenters(_validationOptionsPresenter);
      }

      public void Edit(SimulationConfiguration simulationConfiguration)
      {
         //Edit copies so that the settings are only updated when the wizard is not canceled
         _validationSettings = _userSettings.ValidationSettings.Clone();
         _validationOptionsPresenter.Edit(_validationSettings);

         _finalOptionsDTO = new FinalOptionsDTO
         {
            WarnForNonFiniteQuantities = _userSettings.WarnForNonFiniteQuantities,
            CheckForNegativeValues = simulationConfiguration.SimulationSettings.Solver.CheckForNegativeValues
         };
         _view.BindTo(_finalOptionsDTO);
      }

      public void ApplyOptionsTo(SimulationConfiguration simulationConfiguration)
      {
         _userSettings.ValidationSettings.UpdatePropertiesFrom(_validationSettings);
         _userSettings.WarnForNonFiniteQuantities = _finalOptionsDTO.WarnForNonFiniteQuantities;
         simulationConfiguration.SimulationSettings.Solver.CheckForNegativeValues = _finalOptionsDTO.CheckForNegativeValues;
      }
   }
}
