using System;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Commands;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks.Interaction
{
   //only required for dynamic merging
   public class InteractionTaskForObserverBuilder : IInteractionTasksForBuilder<ObserverBuilder>
   {
      private readonly IInteractionTasksForBuilder<ContainerObserverBuilder> _containerObserverTask;
      private readonly IInteractionTasksForBuilder<AmountObserverBuilder> _amountObserverBuilderTask;

      public InteractionTaskForObserverBuilder(IInteractionTasksForBuilder<ContainerObserverBuilder> containerObserverTask, IInteractionTasksForBuilder<AmountObserverBuilder> amountObserverBuilderTask)
      {
         _containerObserverTask = containerObserverTask;
         _amountObserverBuilderTask = amountObserverBuilderTask;
      }

      public IMoBiCommand GetAddCommand(ObserverBuilder builder, IBuildingBlock buildingBlock)
      {
         if (builder.IsAnImplementationOf<ContainerObserverBuilder>())
            return _containerObserverTask.GetAddCommand(builder.DowncastTo<ContainerObserverBuilder>(), buildingBlock);

         return _amountObserverBuilderTask.GetAddCommand(builder.DowncastTo<AmountObserverBuilder>(), buildingBlock);
      }

      public IMoBiCommand GetRemoveCommand(ObserverBuilder builder, IBuildingBlock buildingBlock)
      {
         if (builder.IsAnImplementationOf<ContainerObserverBuilder>())
            return _containerObserverTask.GetRemoveCommand(builder.DowncastTo<ContainerObserverBuilder>(), buildingBlock);

         return _amountObserverBuilderTask.GetRemoveCommand(builder.DowncastTo<AmountObserverBuilder>(), buildingBlock);
      }

      public void AddToParent(ObserverBuilder builder, IBuildingBlock buildingBlockWithFormulaCache, IMoBiMacroCommand macroCommand,
         Func<ObserverBuilder, IMoBiCommand> getAddCommand)
      {
         if (builder.IsAnImplementationOf<ContainerObserverBuilder>())
            _containerObserverTask.AddToParent(builder.DowncastTo<ContainerObserverBuilder>(), buildingBlockWithFormulaCache, macroCommand,
               getAddCommand);
         else
            _amountObserverBuilderTask.AddToParent(builder.DowncastTo<AmountObserverBuilder>(), buildingBlockWithFormulaCache, macroCommand,
               getAddCommand);
      }
   }

   public abstract class InteractionTasksForObserverBuilder<TObservedBuilder> : InteractionTasksForBuilder<TObservedBuilder, ObserverBuildingBlock>
      where TObservedBuilder : ObserverBuilder
   {

      protected InteractionTasksForObserverBuilder(IInteractionTaskContext interactionTaskContext, IEditTaskFor<TObservedBuilder> editTask) : base(interactionTaskContext, editTask)
      {
      }


      public override IMoBiCommand GetRemoveCommand(TObservedBuilder observerToRemove, ObserverBuildingBlock parent, IBuildingBlock buildingBlock)
      {
         return new RemoveObserverBuilderCommand(parent, observerToRemove);
      }

      public override IMoBiCommand GetRemoveCommand(TObservedBuilder builder, ObserverBuildingBlock buildingBlock)
      {
         return GetRemoveCommand(builder, buildingBlock, null);
      }

      public override IMoBiCommand GetAddCommand(TObservedBuilder newObserver, ObserverBuildingBlock parent, IBuildingBlock buildingBlock)
      {
         return GetAddCommand(newObserver, parent);
      }

      public override IMoBiCommand GetAddCommand(TObservedBuilder builder, ObserverBuildingBlock buildingBlock)
      {
         return new AddObserverBuilderCommand(buildingBlock, builder);
      }

      protected abstract IEditObserverBuilderPresenter GetEditObserverBuilder();
   }

   public class InteractionTasksForContainerObserverBuilder : InteractionTasksForObserverBuilder<ContainerObserverBuilder>
   {
      public InteractionTasksForContainerObserverBuilder(IInteractionTaskContext interactionTaskContext, IEditTaskFor<ContainerObserverBuilder> editTask) : base(interactionTaskContext, editTask)
      {
      }

      protected override IEditObserverBuilderPresenter GetEditObserverBuilder()
      {
         return ApplicationController.Start<IEditContainerObserverBuilderPresenter>();
      }
   }

   public class InteractionTasksForAmountObserverBuilder : InteractionTasksForObserverBuilder<AmountObserverBuilder>
   {
      public InteractionTasksForAmountObserverBuilder(IInteractionTaskContext interactionTaskContext, IEditTaskFor<AmountObserverBuilder> editTask)
         : base(interactionTaskContext, editTask)
      {
      }

      protected override IEditObserverBuilderPresenter GetEditObserverBuilder()
      {
         return ApplicationController.Start<IEditObserverBuilderPresenter>();
      }
   }
}