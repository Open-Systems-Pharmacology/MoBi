using MoBi.Core.Domain.Model;
using MoBi.Presentation.Presenter.Simulation;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Views;
using OSPSuite.Assets;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface IFinalOptionsPresenter : ISimulationItemPresenter
   {
   }

   public class FinalOptionsPresenter : AbstractSubPresenter<IFinalOptionsView, IFinalOptionsPresenter>, IFinalOptionsPresenter
   {
      private readonly IValidationOptionsPresenter _validationOptionsPresenter;
      private readonly IUserSettings _userSettings;

      public FinalOptionsPresenter(IFinalOptionsView view, IValidationOptionsPresenter validationOptionsPresenter, IUserSettings userSettings) : base(view)
      {
         _validationOptionsPresenter = validationOptionsPresenter;
         _userSettings = userSettings;
         _view.SetValidationOptionsView(_validationOptionsPresenter.View);
      }

      public object Subject => null;

      public void Edit(IMoBiSimulation simulation)
      {
         _validationOptionsPresenter.Edit(_userSettings.ValidationSettings);
      }

      public override ApplicationIcon Icon => ApplicationIcons.Run;
   }
}