using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.UICommands;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.UICommand
{
   public abstract class ExtendBuildingBlockFromPathAndValuesUICommand<TSource, TSourceParent, TTarget, TTargetBuilder> : ObjectUICommand<TTarget> where TSource : BuildingBlock where TTarget : PathAndValueEntityBuildingBlock<TTargetBuilder> where TTargetBuilder : PathAndValueEntity where TSourceParent : class
   {
      private readonly IInteractionTasksForBuildingBlock<TSourceParent, TSource> _interactionTasksForSource;
      private readonly IInteractionTasksForExtendablePathAndValueEntity<TTarget, TTargetBuilder> _taskForExtendingTarget;
      private readonly IMoBiContext _context;

      protected ExtendBuildingBlockFromPathAndValuesUICommand(
         IInteractionTasksForBuildingBlock<TSourceParent, TSource> interactionTasksForSource,
         IInteractionTasksForExtendablePathAndValueEntity<TTarget, TTargetBuilder> taskForExtendingTarget,
         IMoBiContext context)
      {
         _interactionTasksForSource = interactionTasksForSource;
         _taskForExtendingTarget = taskForExtendingTarget;
         _context = context;
      }

      protected override void PerformExecute()
      {
         var buildingBlocks = _interactionTasksForSource.LoadFromPKML();
         _context.AddToHistory(_taskForExtendingTarget.Extend(MapAll(buildingBlocks), Subject));
      }

      protected abstract IReadOnlyList<TTargetBuilder> MapAll(IReadOnlyList<TSource> buildingBlocks);
   }

   public abstract class ExtendParameterValuesFromPathAndValuesUICommand<TBuildingBlock> : ExtendBuildingBlockFromPathAndValuesUICommand<TBuildingBlock, MoBiProject, ParameterValuesBuildingBlock, ParameterValue> where TBuildingBlock : BuildingBlock
   {
      private readonly IMapper<TBuildingBlock, ParameterValuesBuildingBlock> _mapper;

      protected ExtendParameterValuesFromPathAndValuesUICommand(
         IInteractionTasksForBuildingBlock<MoBiProject, TBuildingBlock> interactionTasksForSource,
         IMapper<TBuildingBlock, ParameterValuesBuildingBlock> mapper,
         IParameterValuesTask parameterValuesTask,
         IMoBiContext context) : base(interactionTasksForSource, parameterValuesTask, context)
      {
         _mapper = mapper;
      }

      protected override IReadOnlyList<ParameterValue> MapAll(IReadOnlyList<TBuildingBlock> buildingBlocks)
      {
         return buildingBlocks.MapAllUsing(_mapper).SelectMany(x => x).ToList();
      }
   }

   public class ExtendParameterValuesFromIndividualUICommand : ExtendParameterValuesFromPathAndValuesUICommand<IndividualBuildingBlock>
   {
      public ExtendParameterValuesFromIndividualUICommand(
         IInteractionTasksForIndividualBuildingBlock interactionTask,
         IIndividualToParameterValuesMapper mapper,
         IParameterValuesTask parameterValuesTask,
         IMoBiContext context) : base(interactionTask, mapper, parameterValuesTask, context)
      {
      }
   }

   public class ExtendParameterValuesFromExpressionUICommand : ExtendParameterValuesFromPathAndValuesUICommand<ExpressionProfileBuildingBlock>
   {
      public ExtendParameterValuesFromExpressionUICommand(
         IInteractionTasksForExpressionProfileBuildingBlock interactionTask,
         IExpressionProfileToParameterValuesMapper mapper,
         IParameterValuesTask parameterValuesTask,
         IMoBiContext context) : base(interactionTask, mapper, parameterValuesTask, context)
      {
      }
   }

   public class ExtendInitialConditionsFromInitialConditionsUICommand : ExtendBuildingBlockFromPathAndValuesUICommand<InitialConditionsBuildingBlock, Module, InitialConditionsBuildingBlock, InitialCondition>
   {
      public ExtendInitialConditionsFromInitialConditionsUICommand(IInitialConditionsTask<InitialConditionsBuildingBlock> interactionTasksForSource, IMoBiContext context) : base(interactionTasksForSource, interactionTasksForSource, context)
      {
      }

      protected override IReadOnlyList<InitialCondition> MapAll(IReadOnlyList<InitialConditionsBuildingBlock> buildingBlocks)
      {
         return buildingBlocks.SelectMany(x => x).ToList();
      }
   }
}