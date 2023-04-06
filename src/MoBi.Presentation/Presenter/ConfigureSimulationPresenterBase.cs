using System.Collections.Generic;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Presenter.Simulation;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Presenter
{
   public abstract class ConfigureSimulationPresenterBase<TView, TPresenter> : WizardPresenter<TView, TPresenter, ISimulationItemPresenter>
      where TPresenter : IWizardPresenter
      where TView : IWizardView, IModalView<TPresenter>
   {
      protected IMoBiContext _context;
      protected IMoBiMacroCommand _commands;

      protected ConfigureSimulationPresenterBase(TView view, ISubPresenterItemManager<ISimulationItemPresenter> subPresenterManager, IDialogCreator dialogCreator, IMoBiContext context, IReadOnlyList<ISubPresenterItem> subPresenterItems)
         : base(view, subPresenterManager, subPresenterItems, dialogCreator)
      {
         _context = context;
         _commands = new MoBiMacroCommand();

         InitializeWith(_commands);
         AllowQuickFinish = true;
      }

      protected override void UpdateControls(int currentIndex)
      {
         // View.NextEnabled = configReady;
         // _subPresenterItems.Each(p => setControlEnabled(p, configReady));
         View.OkEnabled = CanClose;
      }

      private void setControlEnabled(ISubPresenterItem subPresenterItem, bool configReady)
      {
         View.SetControlEnabled(subPresenterItem, configReady);
      }
   }
}