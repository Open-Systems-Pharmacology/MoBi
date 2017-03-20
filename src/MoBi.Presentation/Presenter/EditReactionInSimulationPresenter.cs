using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter.BasePresenter;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;

namespace MoBi.Presentation.Presenter
{
   public interface IEditReactionInSimulationPresenter : IEditInSimulationPresenter, IEditPresenterWithParameters<IReaction>
   {
   }

   public class EditReactionInSimulationPresenter : AbstractEditPresenter<IEditReactionInSimulationView, IEditReactionInSimulationPresenter, IReaction>, IEditReactionInSimulationPresenter
   {
      private IReaction _reaction;
      private readonly IEditParameterListPresenter _editParameterListPresenter;
      private readonly IReactionToReactionDTOMapper _reactionToReactionDTOMapper;
      private readonly IFormulaPresenterCache _formulaPresenterCache;
      private IEditTypedFormulaPresenter _formulaPresenter;
      public IMoBiSimulation Simulation { get; set; }

      public EditReactionInSimulationPresenter(IEditReactionInSimulationView view, IEditParameterListPresenter editParameterListPresenter, IReactionToReactionDTOMapper reactionToReactionDTOMapper,
         IFormulaPresenterCache formulaPresenterCache)
         : base(view)
      {
         _editParameterListPresenter = editParameterListPresenter;
         _formulaPresenterCache = formulaPresenterCache;
         _reactionToReactionDTOMapper = reactionToReactionDTOMapper;
         _editParameterListPresenter.EditMode = EditParameterMode.ValuesOnly;
         _view.SetParameterView(_editParameterListPresenter.BaseView);
         AddSubPresenters(_editParameterListPresenter);
      }

      public void Edit(IReaction reaction, IEnumerable<IObjectBase> existingObjectsInParent)
      {
         _reaction = reaction;
         _editParameterListPresenter.Edit(_reaction);
         _view.BindTo(_reactionToReactionDTOMapper.MapFrom(_reaction));
         initializeFormulaPresenter(reaction);
      }

      public override void Edit(IReaction reaction)
      {
         Edit(reaction, Enumerable.Empty<IObjectBase>());
      }

      private void initializeFormulaPresenter(IReaction reaction)
      {
         _formulaPresenter = _formulaPresenterCache.PresenterFor(reaction.Formula);
         _formulaPresenter.InitializeWith(CommandCollector);
         _view.SetFormulaView(_formulaPresenter.BaseView);
         _formulaPresenter.ReadOnly = true;
         _formulaPresenter.Edit(reaction.Formula, reaction);
      }

      public override object Subject
      {
         get { return _reaction; }
      }

      public void SelectParameter(IParameter parameter)
      {
         _view.ShowParameters();
         _editParameterListPresenter.Select(parameter);
      }
   }
}