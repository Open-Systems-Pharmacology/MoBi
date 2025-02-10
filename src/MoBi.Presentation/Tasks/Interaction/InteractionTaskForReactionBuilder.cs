using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Presentation.Tasks.Interaction
{
   public interface IInteractionTasksForReactionBuilder
   {
      IEnumerable<string> SelectMoleculeNames(MoBiReactionBuildingBlock reactionBuildingBlock, IEnumerable<string> unallowedNames, string reactionName, string partnerType);
   }

   public class InteractionTasksForReactionBuilder : InteractionTasksForBuilder<ReactionBuilder, MoBiReactionBuildingBlock>, IInteractionTasksForReactionBuilder
   {
      private readonly IReactionDimensionRetriever _dimensionRetriever;

      public InteractionTasksForReactionBuilder(IInteractionTaskContext interactionTaskContext, IEditTaskFor<ReactionBuilder> editTask, IReactionDimensionRetriever dimensionRetriever)
         : base(interactionTaskContext, editTask)
      {
         _dimensionRetriever = dimensionRetriever;
      }

      public override IMoBiCommand GetRemoveCommand(ReactionBuilder reactionBuilderToRemove, MoBiReactionBuildingBlock parent, IBuildingBlock buildingBlock)
      {
         return new RemoveReactionBuilderCommand(parent.DowncastTo<MoBiReactionBuildingBlock>(), reactionBuilderToRemove);
      }

      public override IMoBiCommand GetRemoveCommand(ReactionBuilder builder, MoBiReactionBuildingBlock buildingBlock)
      {
         return GetRemoveCommand(builder, buildingBlock, null);
      }

      public override IMoBiCommand GetAddCommand(ReactionBuilder reactionBuilderToAdd, MoBiReactionBuildingBlock parent, IBuildingBlock buildingBlock)
      {
         return GetAddCommand(reactionBuilderToAdd, parent);
      }

      public override ReactionBuilder CreateNewEntity(MoBiReactionBuildingBlock reactionBuildingBlock)
      {
         return base.CreateNewEntity(reactionBuildingBlock)
            .WithDimension(_dimensionRetriever.ReactionDimension);
      }

      public override IMoBiCommand GetAddCommand(ReactionBuilder builder, MoBiReactionBuildingBlock buildingBlock)
      {
         return new AddReactionBuilderCommand(buildingBlock.DowncastTo<MoBiReactionBuildingBlock>(), builder);
      }

      public IEnumerable<string> SelectMoleculeNames(MoBiReactionBuildingBlock reactionBuildingBlock, IEnumerable<string> unallowedNames, string reactionName, string partnerType)
      {
         using (var moleculeSelectionPresenter = ApplicationController.Start<IMultipleStringSelectionPresenter>())
         {
            return moleculeSelectionPresenter.Show(AppConstants.Captions.SelectMolecules, AppConstants.Captions.SelectMoleculePartnersFor(reactionName, partnerType), reactionBuildingBlock.AllMolecules.Except(unallowedNames), string.Empty, canAdd:false);
         }
      }
   }
}