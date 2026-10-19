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
      ///    Saves the options edited in the wizard to the user settings
      /// </summary>
      void SaveOptions();
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
         //Edit copies so that the user settings are only updated when the wizard is not canceled
         _validationSettings = _userSettings.ValidationSettings.Clone();
         _validationOptionsPresenter.Edit(_validationSettings);

         _finalOptionsDTO = new FinalOptionsDTO { WarnForNonFiniteQuantities = _userSettings.WarnForNonFiniteQuantities };
         _view.BindTo(_finalOptionsDTO);
      }

      public void SaveOptions()
      {
         _userSettings.ValidationSettings.UpdatePropertiesFrom(_validationSettings);
         _userSettings.WarnForNonFiniteQuantities = _finalOptionsDTO.WarnForNonFiniteQuantities;
      }
   }
}
