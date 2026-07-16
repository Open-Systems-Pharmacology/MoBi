using System;
using System.Collections.Generic;
using MoBi.Presentation.Presenter.BasePresenter;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility.Events;

namespace MoBi.Presentation.Presenter
{
   public abstract class EditProcessInSimulationPresenter<TView, TPresenter, TProcess>
      : AbstractEditPresenter<TView, TPresenter, TProcess>, IEditInSimulationPresenter
      where TView : class, IEditProcessInSimulationView<TPresenter>
      where TPresenter : IEditPresenter
      where TProcess : Process
   {
      protected TProcess _process;
      private readonly IEditParametersInContainerPresenter _editParametersInContainerPresenter;
      private readonly IFormulaPresenterCache _formulaPresenterCache;
      private IEditTypedFormulaPresenter _formulaPresenter;

      public TrackableSimulation TrackableSimulation
      {
         get;
         set
         {
            field = value;
            _editParametersInContainerPresenter.EnableSimulationTracking(value);
         }
      }

      protected EditProcessInSimulationPresenter(TView view,
         IEditParametersInContainerPresenter editParametersInContainerPresenter,
         IFormulaPresenterCache formulaPresenterCache)
         : base(view)
      {
         _editParametersInContainerPresenter = editParametersInContainerPresenter;
         _formulaPresenterCache = formulaPresenterCache;
         _editParametersInContainerPresenter.EditMode = EditParameterMode.ValuesOnly;
         _view.SetParameterView(_editParametersInContainerPresenter.BaseView);
         AddSubPresenters(_editParametersInContainerPresenter);
      }

      public void Edit(TProcess process, IReadOnlyList<IObjectBase> existingObjectsInParent)
      {
         _process = process;
         _editParametersInContainerPresenter.Edit(_process);
         BindProcessToView(_process);
         initializeFormulaPresenter(_process);
      }

      public override void Edit(TProcess process)
      {
         Edit(process, Array.Empty<IObjectBase>());
      }

      protected abstract void BindProcessToView(TProcess process);

      private void initializeFormulaPresenter(TProcess process)
      {
         _formulaPresenter = _formulaPresenterCache.PresenterFor(process.Formula);
         _formulaPresenter.InitializeWith(CommandCollector);
         _view.SetFormulaView(_formulaPresenter.BaseView);
         _formulaPresenter.ReadOnly = true;
         _formulaPresenter.Edit(process.Formula, process);
      }

      public override object Subject => _process;

      public void SelectParameter(IParameter parameter)
      {
         _view.ShowParameters();
         _editParametersInContainerPresenter.Select(parameter);
      }

      public override void ReleaseFrom(IEventPublisher eventPublisher)
      {
         base.ReleaseFrom(eventPublisher);
         _formulaPresenterCache.ReleaseFrom(eventPublisher);
      }
   }
}
