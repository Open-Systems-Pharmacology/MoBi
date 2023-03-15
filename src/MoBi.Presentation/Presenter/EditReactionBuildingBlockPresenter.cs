using System.Linq;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Presentation.Presenter.ReactionDiagram;
using MoBi.Presentation.Views;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Presenter
{
   public interface IEditReactionBuildingBlockPresenter : ISingleStartPresenter<IMoBiReactionBuildingBlock>,
      IDiagramBuildingBlockPresenter,
      IListener<RemovedEvent>,
      IListener<EntitySelectedEvent>
   {
      void LayoutInLayers();
      void AddReactionMoleculeNode();
      void UpdateUserDefinedParameters();
   }

   public class EditReactionBuildingBlockPresenter : EditBuildingBlockPresenterBase<IEditReactionBuildingBlockView, IEditReactionBuildingBlockPresenter, IMoBiReactionBuildingBlock, IReactionBuilder>,
      IEditReactionBuildingBlockPresenter
   {
      private readonly IReactionsListSubPresenter _reactionListPresenter;
      private readonly IReactionDiagramPresenter _reactionDiagramPresenter;
      private readonly IEditReactionBuilderPresenter _editReactionBuilderPresenter;
      private readonly IEditFavoritesInReactionsPresenter _editFavoritesInReactionsPresenter;
      private readonly IUserDefinedParametersPresenter _userDefinedParametersPresenter;
      private IMoBiReactionBuildingBlock _reactionBuildingBlock;

      public EditReactionBuildingBlockPresenter(IEditReactionBuildingBlockView view,
         IReactionsListSubPresenter reactionListPresenter,
         IReactionDiagramPresenter reactionDiagramPresenter,
         IEditReactionBuilderPresenter editReactionBuilderPresenter,
         IFormulaCachePresenter formulaCachePresenter,
         IEditFavoritesInReactionsPresenter editFavoritesInReactionsPresenter,
         IUserDefinedParametersPresenter userDefinedParametersPresenter
      )
         : base(view, formulaCachePresenter)
      {
         _editReactionBuilderPresenter = editReactionBuilderPresenter;
         _editFavoritesInReactionsPresenter = editFavoritesInReactionsPresenter;
         _userDefinedParametersPresenter = userDefinedParametersPresenter;
         _reactionDiagramPresenter = reactionDiagramPresenter;
         _reactionListPresenter = reactionListPresenter;
         _view.SetEditReactionView(_editReactionBuilderPresenter.BaseView);
         _view.SetReactionListView(_reactionListPresenter.BaseView);
         _view.SetReactionDiagram(_reactionDiagramPresenter.BaseView);
         _view.SetFavoritesReactionView(_editFavoritesInReactionsPresenter.BaseView);
         _view.SetUserDefinedParametersView(_userDefinedParametersPresenter.BaseView);
         _editFavoritesInReactionsPresenter.ShouldHandleRemovedEvent = shouldHandleRemoved;
         _userDefinedParametersPresenter.ColumnConfiguration = x => x.ConfigureForReaction();
         AddSubPresenters(_editReactionBuilderPresenter, _reactionDiagramPresenter, _reactionListPresenter, _editFavoritesInReactionsPresenter, _userDefinedParametersPresenter);
      }

      public override object Subject => _reactionBuildingBlock;

      public override void Edit(IMoBiReactionBuildingBlock reactionBuildingBlock)
      {
         _reactionBuildingBlock = reactionBuildingBlock;
         _editReactionBuilderPresenter.BuildingBlock = _reactionBuildingBlock;
         EditFormulas(_reactionBuildingBlock);
         _reactionListPresenter.Edit(_reactionBuildingBlock);
         _reactionDiagramPresenter.Edit(_reactionBuildingBlock);
         _editReactionBuilderPresenter.Edit(_reactionBuildingBlock.FirstOrDefault());
         _editFavoritesInReactionsPresenter.Edit(reactionBuildingBlock);
         UpdateUserDefinedParameters();

         UpdateCaption();
         _view.Display();
      }

      protected override void UpdateCaption()
      {
         _view.Caption = AppConstants.Captions.ReactionsBuildingBlockCaption(_reactionBuildingBlock.Name);
      }

      public void ZoomIn()
      {
         _reactionDiagramPresenter.Zoom(Diagram.Reaction.ZoomInFactor);
      }

      public void ZoomOut()
      {
         _reactionDiagramPresenter.Zoom(1 / Diagram.Reaction.ZoomInFactor);
      }

      public void FitToPage()
      {
         _reactionDiagramPresenter.Zoom(0F);
      }

      public void LayoutByForces()
      {
         _reactionDiagramPresenter.Layout(null, 0, null);
      }

      public void AddReactionMoleculeNode()
      {
         _reactionDiagramPresenter.AddMoleculeNode();
      }

      public void UpdateUserDefinedParameters()
      {
         _userDefinedParametersPresenter.ShowUserDefinedParametersIn(_reactionBuildingBlock);
      }

      public void LayoutInLayers()
      {
         _reactionDiagramPresenter.LayerLayout();
      }

      public void Handle(RemovedEvent eventToHandle)
      {
         if (eventToHandle.RemovedObjects.Any(shouldHandleRemoved) && Equals(eventToHandle.Parent, BuildingBlock))
         {
            Edit(_reactionBuildingBlock);
         }
      }

      private bool shouldHandleRemoved(IObjectBase objectBase)
      {
         return objectBase.IsAnImplementationOf<IReactionBuilder>();
      }

      protected override void EnsureItemsVisibility(IContainer parentObject, IParameter parameter = null)
      {
         SelectBuilder(parentObject as IReactionBuilder);
         _editReactionBuilderPresenter.SelectParameter(parameter);
      }

      protected override void SelectBuilder(IReactionBuilder reactionBuilder)
      {
         _editReactionBuilderPresenter.Edit(reactionBuilder);
         _reactionDiagramPresenter.Select(reactionBuilder);
      }
   }
}