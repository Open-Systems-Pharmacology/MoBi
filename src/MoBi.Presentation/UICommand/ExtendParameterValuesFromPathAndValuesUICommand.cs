using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.UICommand
{
   public abstract class ExtendParameterValuesFromPathAndValuesUICommand<TBuildingBlock, TParent> : ExtendBuildingBlockFromPathAndValuesUICommand<TBuildingBlock, TParent, ParameterValuesBuildingBlock, ParameterValue> where TBuildingBlock : BuildingBlock where TParent : class
   {
      

      protected ExtendParameterValuesFromPathAndValuesUICommand(
         IInteractionTasksForBuildingBlock<TParent, TBuildingBlock> interactionTasksForSource,
         IParameterValuesTask parameterValuesTask,
         IMoBiContext context) : base(interactionTasksForSource, parameterValuesTask, context)
      {
      }
   }

   public class ExtendParameterValuesFromIndividualUICommand : ExtendParameterValuesFromPathAndValuesUICommand<IndividualBuildingBlock, MoBiProject>
   {
      private readonly IIndividualToParameterValuesMapper _mapper;

      public ExtendParameterValuesFromIndividualUICommand(
         IInteractionTasksForIndividualBuildingBlock interactionTask,
         IIndividualToParameterValuesMapper mapper,
         IParameterValuesTask parameterValuesTask,
         IMoBiContext context) : base(interactionTask, parameterValuesTask, context)
      {
         _mapper = mapper;
      }

      protected override IReadOnlyList<ParameterValue> MapAll(IReadOnlyList<IndividualBuildingBlock> buildingBlocks)
      {
         return buildingBlocks.MapAllUsing(_mapper).SelectMany(x => x).ToList();
      }
   }

   public class ExtendParameterValuesFromExpressionUICommand : ExtendParameterValuesFromPathAndValuesUICommand<ExpressionProfileBuildingBlock, MoBiProject>
   {
      private readonly IExpressionProfileToParameterValuesMapper _mapper;

      public ExtendParameterValuesFromExpressionUICommand(
         IInteractionTasksForExpressionProfileBuildingBlock interactionTask,
         IExpressionProfileToParameterValuesMapper mapper,
         IParameterValuesTask parameterValuesTask,
         IMoBiContext context) : base(interactionTask, parameterValuesTask, context)
      {
         _mapper = mapper;
      }

      protected override IReadOnlyList<ParameterValue> MapAll(IReadOnlyList<ExpressionProfileBuildingBlock> buildingBlocks)
      {
         return buildingBlocks.MapAllUsing(_mapper).SelectMany(x => x).ToList();
      }
   }

   public class ExtendParameterValuesFromParameterValuesUICommand : ExtendParameterValuesFromPathAndValuesUICommand<ParameterValuesBuildingBlock, Module>
   {
      private readonly ICloneManagerForBuildingBlock _cloneManager;

      public ExtendParameterValuesFromParameterValuesUICommand(
         IParameterValuesTask interactionTask,
         ICloneManagerForBuildingBlock cloneManager,
         IParameterValuesTask parameterValuesTask,
         IMoBiContext context) : base(interactionTask, parameterValuesTask, context)
      {
         _cloneManager = cloneManager;
      }

      protected override IReadOnlyList<ParameterValue> MapAll(IReadOnlyList<ParameterValuesBuildingBlock> buildingBlocks)
      {
         return buildingBlocks.SelectMany(x => _cloneManager.Clone(x, new BuildingBlockFormulaCache())).ToList();
      }
   }
}