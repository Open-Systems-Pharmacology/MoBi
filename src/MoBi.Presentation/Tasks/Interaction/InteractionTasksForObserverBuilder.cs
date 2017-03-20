using System;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Commands;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks.Interaction
{
   //only required for dynamic merging
   public class InteractionTaskForObserverBuilder : IInteractionTasksForBuilder<IObserverBuilder>
   {
      private readonly IInteractionTasksForBuilder<IContainerObserverBuilder> _containerObserverTask;
      private readonly IInteractionTasksForBuilder<IAmountObserverBuilder> _amountObserverBuilderTask;

      public InteractionTaskForObserverBuilder(IInteractionTasksForBuilder<IContainerObserverBuilder> containerObserverTask, IInteractionTasksForBuilder<IAmountObserverBuilder> amountObserverBuilderTask)
      {
         _containerObserverTask = containerObserverTask;
         _amountObserverBuilderTask = amountObserverBuilderTask;
      }

      public IMoBiCommand GetAddCommand(IObserverBuilder builder, IBuildingBlock buildingBlock)
      {
         if (builder.IsAnImplementationOf<IContainerObserverBuilder>())
            return _containerObserverTask.GetAddCommand(builder.DowncastTo<IContainerObserverBuilder>(), buildingBlock);

         return _amountObserverBuilderTask.GetAddCommand(builder.DowncastTo<IAmountObserverBuilder>(), buildingBlock);
      }

      public IMoBiCommand GetRemoveCommand(IObserverBuilder builder, IBuildingBlock buildingBlock)
      {
         if (builder.IsAnImplementationOf<IContainerObserverBuilder>())
            return _containerObserverTask.GetRemoveCommand(builder.DowncastTo<IContainerObserverBuilder>(), buildingBlock);

         return _amountObserverBuilderTask.GetRemoveCommand(builder.DowncastTo<IAmountObserverBuilder>(), buildingBlock);
      }

      public void AddToParent(IObserverBuilder builder, IBuildingBlock buildingBlockWithFormulaCache, IMoBiMacroCommand macroCommand,
         Func<IObserverBuilder, IMoBiCommand> getAddCommand)
      {
         if (builder.IsAnImplementationOf<IContainerObserverBuilder>())
            _containerObserverTask.AddToParent(builder.DowncastTo<IContainerObserverBuilder>(), buildingBlockWithFormulaCache, macroCommand,
               getAddCommand);
         else
            _amountObserverBuilderTask.AddToParent(builder.DowncastTo<IAmountObserverBuilder>(), buildingBlockWithFormulaCache, macroCommand,
               getAddCommand);
      }
   }

   public abstract class InteractionTasksForObserverBuilder<TObservedBuilder> : InteractionTasksForBuilder<TObservedBuilder, IObserverBuildingBlock>
      where TObservedBuilder : class, IObserverBuilder
   {

      protected InteractionTasksForObserverBuilder(IInteractionTaskContext interactionTaskContext, IEditTaskFor<TObservedBuilder> editTask) : base(interactionTaskContext, editTask)
      {
      }


      public override IMoBiCommand GetRemoveCommand(TObservedBuilder observerToRemove, IObserverBuildingBlock parent, IBuildingBlock buildingBlock)
      {
         return new RemoveObserverBuilderCommand(parent, observerToRemove);
      }

      public override IMoBiCommand GetRemoveCommand(TObservedBuilder builder, IObserverBuildingBlock buildingBlock)
      {
         return GetRemoveCommand(builder, buildingBlock, null);
      }

      public override IMoBiCommand GetAddCommand(TObservedBuilder newObserver, IObserverBuildingBlock parent, IBuildingBlock buildingBlock)
      {
         return GetAddCommand(newObserver, parent);
      }

      public override IMoBiCommand GetAddCommand(TObservedBuilder builder, IObserverBuildingBlock buildingBlock)
      {
         return new AddObserverBuilderCommand(buildingBlock, builder);
      }

      protected abstract IEditObserverBuilderPresenter GetEditObserverBuilder();
   }

   public class InteractionTasksForContainerObserverBuilder : InteractionTasksForObserverBuilder<IContainerObserverBuilder>
   {
      public InteractionTasksForContainerObserverBuilder(IInteractionTaskContext interactionTaskContext, IEditTaskFor<IContainerObserverBuilder> editTask) : base(interactionTaskContext, editTask)
      {
      }

      protected override IEditObserverBuilderPresenter GetEditObserverBuilder()
      {
         return ApplicationController.Start<IEditContainerObserverBuilderPresenter>();
      }
   }

   public class InteractionTasksForAmountObserverBuilder : InteractionTasksForObserverBuilder<IAmountObserverBuilder>
   {
      public InteractionTasksForAmountObserverBuilder(IInteractionTaskContext interactionTaskContext, IEditTaskFor<IAmountObserverBuilder> editTask)
         : base(interactionTaskContext, editTask)
      {
      }

      protected override IEditObserverBuilderPresenter GetEditObserverBuilder()
      {
         return ApplicationController.Start<IEditObserverBuilderPresenter>();
      }
   }
}