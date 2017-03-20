using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class ResetParamterValuesToDefaultFromStartValuesInSimulationCommand : ResetQuantityValuesToDefaultFromStartValuesInSimulation<IParameterStartValue>
   {
      public ResetParamterValuesToDefaultFromStartValuesInSimulationCommand(IMoBiSimulation simulation)
         : base(simulation, simulation.BuildConfiguration.ParameterStartValues)
      {
      }

      protected override IQuantity QuantityUsedToFindPathFor(IQuantity quantity)
      {
         return quantity;
      }

      protected override IReadOnlyList<IQuantity> AllQuantitiesToReset()
      {
         return _simulation.Model.Root.GetAllChildren<IParameter>()
            .Where(x => x.IsFixedValue)
            .ToList();
      }
   }
}