using System.Collections.Generic;
using System.Linq;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility;

namespace MoBi.Presentation.Mappers;

public interface ISimulationConfigurationToMoleculeUsedCalculationMethodsDTOMapper : IMapper<SimulationConfiguration, IReadOnlyList<MoleculeUsedCalculationMethodsDTO>>
{
}

public class SimulationConfigurationToMoleculeUsedCalculationMethodsDTOMapper : MoleculeUsedCalculationMethodsDTOMapper<SimulationConfiguration>, ISimulationConfigurationToMoleculeUsedCalculationMethodsDTOMapper
{
   protected override IReadOnlyCollection<UsedCalculationMethod> MoleculeUsedCalculationMethodsFor(SimulationConfiguration simulationConfiguration, MoleculeBuilder m)
   {
      return simulationConfiguration.CalculationMethodOverridesFor(m.Name).UsedCalculationMethods;
   }

   protected override List<MoleculeBuilder> AllMolecules(SimulationConfiguration simulationConfiguration) =>
      simulationConfiguration.ModuleConfigurations.Select(x => x.BuildingBlock<MoleculeBuildingBlock>()).Where(x => x != null).SelectMany(x => x).ToList();
}