using System;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Commands;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks.Interaction
{
   public interface IInteractionTasksForBuilder<TBuilder>
   {
      IMoBiCommand GetAddCommand(TBuilder builder, IBuildingBlock buildingBlock);

      void AddToParent(TBuilder builder, IBuildingBlock buildingBlockWithFormulaCache, IMoBiMacroCommand macroCommand,
         Func<TBuilder, IMoBiCommand> getAddCommand);

      IMoBiCommand GetRemoveCommand(TBuilder builder, IBuildingBlock buildingBlock);
   }

   public abstract class InteractionTasksForBuilder<TBuilder, TBuildingBlock> : InteractionTasksForChildren<TBuildingBlock, TBuilder>, IInteractionTasksForBuilder<TBuilder>
      where TBuilder : class, IObjectBase where TBuildingBlock : class, IObjectBase

   {
      protected InteractionTasksForBuilder(IInteractionTaskContext interactionTaskContext, IEditTaskFor<TBuilder> editTask) : base(interactionTaskContext, editTask)
      {
      }

      public abstract IMoBiCommand GetAddCommand(TBuilder builder, TBuildingBlock buildingBlock);
      public abstract IMoBiCommand GetRemoveCommand(TBuilder builder, TBuildingBlock buildingBlock);

      public override IMoBiCommand GetAddCommand(TBuilder builder, TBuildingBlock parent, IBuildingBlock buildingBlock)
      {
         return GetAddCommand(builder, buildingBlock.DowncastTo<TBuildingBlock>());
      }

      public IMoBiCommand GetAddCommand(TBuilder builder, IBuildingBlock buildingBlock)
      {
         return GetAddCommand(builder, buildingBlock.DowncastTo<TBuildingBlock>());
      }

      public void AddToParent(TBuilder builder, IBuildingBlock buildingBlockWithFormulaCache, IMoBiMacroCommand macroCommand,
         Func<TBuilder, IMoBiCommand> getAddCommand)
      {
         AddTo(builder, buildingBlockWithFormulaCache.DowncastTo<TBuildingBlock>(), buildingBlockWithFormulaCache);
      }

      public IMoBiCommand GetRemoveCommand(TBuilder builder, IBuildingBlock buildingBlock)
      {
         return GetRemoveCommand(builder, buildingBlock.DowncastTo<TBuildingBlock>());
      }
   }
}