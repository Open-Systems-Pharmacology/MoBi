using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class ResetMoleculeValuesToDefaultFromStartValuesInSimulationCommand : ResetQuantityValuesToDefaultFromStartValuesInSimulation<IMoleculeStartValue>
   {
      public ResetMoleculeValuesToDefaultFromStartValuesInSimulationCommand(IMoBiSimulation simulation) : base(simulation, simulation.BuildConfiguration.MoleculeStartValues)
      {
      }

      protected override IQuantity QuantityUsedToFindPathFor(IQuantity quantity)
      {
         return quantity as IMoleculeAmount ?? quantity.ParentContainer as IMoleculeAmount;
      }

      protected override IReadOnlyList<IQuantity> AllQuantitiesToReset()
      {
         return _simulation.Model.Root.GetAllChildren<IMoleculeAmount>()
            .Select(x => x.QuantityToEdit())
            .Where(x => x.IsFixedValue)
            .ToList();
      }
   }
}