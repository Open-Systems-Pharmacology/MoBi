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
      IEnumerable<string> SelectMoleculeNames(IMoBiReactionBuildingBlock reactionBuildingBlock, IEnumerable<string> unallowedNames, string reactionName, string partnerType);
   }

   public class InteractionTasksForReactionBuilder : InteractionTasksForBuilder<IReactionBuilder, IMoBiReactionBuildingBlock>, IInteractionTasksForReactionBuilder
   {
      private readonly IReactionDimensionRetriever _dimensionRetriever;

      public InteractionTasksForReactionBuilder(IInteractionTaskContext interactionTaskContext, IEditTaskFor<IReactionBuilder> editTask, IReactionDimensionRetriever dimensionRetriever)
         : base(interactionTaskContext, editTask)
      {
         _dimensionRetriever = dimensionRetriever;
      }

      public override IMoBiCommand GetRemoveCommand(IReactionBuilder reactionBuilderToRemove, IMoBiReactionBuildingBlock parent, IBuildingBlock buildingBlock)
      {
         return new RemoveReactionBuilderCommand(parent.DowncastTo<IMoBiReactionBuildingBlock>(), reactionBuilderToRemove);
      }

      public override IMoBiCommand GetRemoveCommand(IReactionBuilder builder, IMoBiReactionBuildingBlock buildingBlock)
      {
         return GetRemoveCommand(builder, buildingBlock, null);
      }

      public override IMoBiCommand GetAddCommand(IReactionBuilder reactionBuilderToAdd, IMoBiReactionBuildingBlock parent, IBuildingBlock buildingBlock)
      {
         return GetAddCommand(reactionBuilderToAdd, parent);
      }

      public override IReactionBuilder CreateNewEntity(IMoBiReactionBuildingBlock reactionBuildingBlock)
      {
         return base.CreateNewEntity(reactionBuildingBlock)
            .WithDimension(_dimensionRetriever.ReactionDimension);
      }

      public override IMoBiCommand GetAddCommand(IReactionBuilder builder, IMoBiReactionBuildingBlock buildingBlock)
      {
         return new AddReactionBuilderCommand(buildingBlock.DowncastTo<IMoBiReactionBuildingBlock>(), builder);
      }

      public IEnumerable<string> SelectMoleculeNames(IMoBiReactionBuildingBlock reactionBuildingBlock, IEnumerable<string> unallowedNames, string reactionName, string partnerType)
      {
         using (var moleculeSelectionPresenter = ApplicationController.Start<IMultipleStringSelectionPresenter>())
         {
            return moleculeSelectionPresenter.Show(AppConstants.Captions.SelectMolecules, AppConstants.Captions.SelectMoleculePartnersFor(reactionName, partnerType), reactionBuildingBlock.AllMolecules.Except(unallowedNames), string.Empty, canAdd:false);
         }
      }
   }
}