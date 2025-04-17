using System;
using System.Collections.Generic;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter.BasePresenter;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Utility.Events;

namespace MoBi.Presentation.Presenter
{
   public interface IEditReactionInSimulationPresenter : IEditInSimulationPresenter, IEditPresenterWithParameters<Reaction>
   {
   }

   public class EditReactionInSimulationPresenter : AbstractEditPresenter<IEditReactionInSimulationView, IEditReactionInSimulationPresenter, Reaction>, IEditReactionInSimulationPresenter
   {
      private Reaction _reaction;
      private readonly IEditParametersInContainerPresenter _editParametersInContainerPresenter;
      private readonly IReactionToReactionDTOMapper _reactionToReactionDTOMapper;
      private readonly IFormulaPresenterCache _formulaPresenterCache;
      private IEditTypedFormulaPresenter _formulaPresenter;
      public TrackableSimulation TrackableSimulation { get; set; }

      public EditReactionInSimulationPresenter(IEditReactionInSimulationView view, IEditParametersInContainerPresenter editParametersInContainerPresenter, IReactionToReactionDTOMapper reactionToReactionDTOMapper,
         IFormulaPresenterCache formulaPresenterCache)
         : base(view)
      {
         _editParametersInContainerPresenter = editParametersInContainerPresenter;
         _formulaPresenterCache = formulaPresenterCache;
         _reactionToReactionDTOMapper = reactionToReactionDTOMapper;
         _editParametersInContainerPresenter.EditMode = EditParameterMode.ValuesOnly;
         _view.SetParameterView(_editParametersInContainerPresenter.BaseView);
         AddSubPresenters(_editParametersInContainerPresenter);
      }

      public void Edit(Reaction reaction, IReadOnlyList<IObjectBase> existingObjectsInParent)
      {
         _reaction = reaction;
         _editParametersInContainerPresenter.Edit(_reaction);
         _view.BindTo(_reactionToReactionDTOMapper.MapFrom(_reaction));
         initializeFormulaPresenter(reaction);
      }

      public override void Edit(Reaction reaction)
      {
         Edit(reaction, Array.Empty<IObjectBase>());
      }

      private void initializeFormulaPresenter(Reaction reaction)
      {
         _formulaPresenter = _formulaPresenterCache.PresenterFor(reaction.Formula);
         _formulaPresenter.InitializeWith(CommandCollector);
         _view.SetFormulaView(_formulaPresenter.BaseView);
         _formulaPresenter.ReadOnly = true;
         _formulaPresenter.Edit(reaction.Formula, reaction);
      }

      public override object Subject => _reaction;

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