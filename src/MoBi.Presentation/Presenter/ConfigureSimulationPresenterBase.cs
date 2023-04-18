using System.Collections.Generic;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Presenter.Simulation;
using MoBi.Presentation.Settings;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Presenter
{
   public abstract class ConfigureSimulationPresenterBase<TView, TPresenter> : WizardPresenter<TView, TPresenter, ISimulationConfigurationItemPresenter>
      where TPresenter : IWizardPresenter
      where TView : IWizardView, IModalView<TPresenter>
   {
      protected IMoBiContext _context;
      protected IMoBiMacroCommand _commands;
      protected readonly IUserSettings _userSettings;

      protected ConfigureSimulationPresenterBase(TView view, ISubPresenterItemManager<ISimulationConfigurationItemPresenter> subPresenterManager, IDialogCreator dialogCreator, IMoBiContext context, IUserSettings userSettings, IReadOnlyList<ISubPresenterItem> subPresenterItems)
         : base(view, subPresenterManager, subPresenterItems, dialogCreator)
      {
         _context = context;
         _commands = new MoBiMacroCommand();
         _userSettings = userSettings;
         InitializeWith(_commands);
         AllowQuickFinish = true;
      }

      protected override void UpdateControls(int currentIndex)
      {
         View.OkEnabled = CanClose;
      }
   }
}