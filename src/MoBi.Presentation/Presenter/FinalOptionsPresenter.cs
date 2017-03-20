using OSPSuite.Assets;
using MoBi.Core;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Presenter.Simulation;
using MoBi.Presentation.Views;

using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface IFinalOptionsPresenter : ISimulationItemPresenter
   {
      void DoFinalActions(IMoBiSimulation simulation);
   }

   public class FinalOptionsPresenter : AbstractSubPresenter<IFinalOptionsView, IFinalOptionsPresenter>, IFinalOptionsPresenter
   {
      private readonly IValidationOptionsPresenter _validationOptionsPresenter;
      private readonly IUserSettings _userSettings;

      public FinalOptionsPresenter(IFinalOptionsView view,  IValidationOptionsPresenter validationOptionsPresenter, IUserSettings userSettings) : base(view)
      {
         _validationOptionsPresenter = validationOptionsPresenter;
         _userSettings = userSettings;
         _view.SetValidationOptionsView(_validationOptionsPresenter.View);
      }

      public void DoFinalActions(IMoBiSimulation simulation)
      {
      }

      public object Subject
      {
         get { return null; }
      }

      public void Edit(IMoBiSimulation simulation)
      {
         _validationOptionsPresenter.Edit(_userSettings.ValidationSettings);
      }

      public override ApplicationIcon Icon
      {
         get { return ApplicationIcons.Run; }
      }
   }
}