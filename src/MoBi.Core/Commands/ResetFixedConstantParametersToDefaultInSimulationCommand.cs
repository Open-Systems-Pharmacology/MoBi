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
      private IAffectedBuildingBlockRetriever _affectedBuildingBlockRetriever;

      public ResetFixedConstantParametersToDefaultInSimulationCommand(IMoBiSimulation simulation, TBuildingBlock buildingBlockFromSimulation)
      {
         _simulation = simulation;
         _buildingBlockFromSimulation = buildingBlockFromSimulation;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         _formulaTask = context.Resolve<IMoBiFormulaTask>();
         _affectedBuildingBlockRetriever = context.Resolve<IAffectedBuildingBlockRetriever>();

         var fixedParameters = fixedConstantParametersFromBuildingBlock().ToList();
         resetFixedParametersInSimulation(fixedParameters);

         context.PublishEvent(new ParameterChangedEvent(fixedParameters));
      }

      private void resetFixedParametersInSimulation(IEnumerable<IParameter> fixedParameters)
      {
         fixedParameters.Each(resetQuantity);
      }

      private IEnumerable<IParameter> fixedConstantParametersFromBuildingBlock()
      {
         return _simulation.Model.Root.GetAllContainersAndSelf<IContainer>()
            .SelectMany(container => container.AllParameters(parameterIsConstantFixedAndFromBuildingBlock));
      }

      private bool parameterIsConstantFixedAndFromBuildingBlock(IParameter parameter)
      {
         return parameter.IsFixedValue && parameter.Formula.IsConstant() && isFromBuildingBlock(parameter, _buildingBlockFromSimulation);
      }

      private bool isFromBuildingBlock(IParameter x, TBuildingBlock affectedBuildingBlock)
      {
         return Equals(_affectedBuildingBlockRetriever.RetrieveFor(x, _simulation).UntypedBuildingBlock, affectedBuildingBlock);
      }

      private void resetQuantity(IQuantity fixedQuantity)
      {
         fixedQuantity.Formula = _formulaTask.CreateNewFormula<ConstantFormula>(fixedQuantity.Dimension).WithValue(fixedQuantity.Value);
         fixedQuantity.IsFixedValue = false;
      }

      protected override void ClearReferences()
      {
         _simulation = null;
         _formulaTask = null;
         _affectedBuildingBlockRetriever = null;
         _buildingBlockFromSimulation = null;
      }
   }
}