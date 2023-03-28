using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Events;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Utility.Extensions;

namespace MoBi.Core.Commands
{
   public class ResetFixedConstantParametersToDefaultInSimulationCommand<TBuildingBlock> : MoBiCommand where TBuildingBlock : class
   {
      private IMoBiSimulation _simulation;
      private TBuildingBlock _buildingBlockFromSimulation;
      private IMoBiFormulaTask _formulaTask;

      public ResetFixedConstantParametersToDefaultInSimulationCommand(IMoBiSimulation simulation, TBuildingBlock buildingBlockFromSimulation)
      {
         _simulation = simulation;
         _buildingBlockFromSimulation = buildingBlockFromSimulation;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         _formulaTask = context.Resolve<IMoBiFormulaTask>();

         var fixedParameters = fixedConstantParametersFromBuildingBlock();
         resetFixedParametersInSimulation(fixedParameters);

         context.PublishEvent(new ParameterChangedEvent(fixedParameters));
      }

      private void resetFixedParametersInSimulation(IEnumerable<IParameter> fixedParameters)
      {
         fixedParameters.Each(resetQuantity);
      }

      private IReadOnlyList<IParameter> fixedConstantParametersFromBuildingBlock()
      {
         return _simulation.Model.Root.GetAllContainersAndSelf<IContainer>()
            .SelectMany(container => container.AllParameters(parameterIsConstantFixedAndFromBuildingBlock)).ToList();
      }

      private bool parameterIsConstantFixedAndFromBuildingBlock(IParameter parameter)
      {
         return parameter.IsFixedValue && parameter.Formula.IsConstant() && isFromBuildingBlock(parameter);
      }

      private bool isFromBuildingBlock(IParameter parameter)
      {
         //TODO SIMULATION_CONFIGURATION
         return false;
         // var buildingBlockInfo = _affectedBuildingBlockRetriever.RetrieveFor(paramter, _simulation);
         // return buildingBlockInfo != null && Equals(buildingBlockInfo.UntypedBuildingBlock, _buildingBlockFromSimulation);
      }

      private void resetQuantity(IQuantity quantity)
      {
         quantity.Formula = _formulaTask.CreateNewFormula<ConstantFormula>(quantity.Dimension).WithValue(quantity.Value);
         quantity.IsFixedValue = false;
      }

      protected override void ClearReferences()
      {
         _simulation = null;
         _formulaTask = null;
         _buildingBlockFromSimulation = null;
      }
   }
}