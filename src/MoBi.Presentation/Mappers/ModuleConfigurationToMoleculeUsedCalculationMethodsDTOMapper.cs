using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility;
using System.Collections.Generic;
using System.Linq;

namespace MoBi.Presentation.Mappers;

public interface IModuleConfigurationToMoleculeUsedCalculationMethodsDTOMapper : IMapper<ModuleConfiguration, IReadOnlyList<MoleculeUsedCalculationMethodsDTO>>
{
}

public class ModuleConfigurationToMoleculeUsedCalculationMethodsDTOMapper : MoleculeUsedCalculationMethodsDTOMapper<ModuleConfiguration>, IModuleConfigurationToMoleculeUsedCalculationMethodsDTOMapper
{
   protected override IReadOnlyList<MoleculeBuilder> AllMolecules(ModuleConfiguration configuration)
   {
      return configuration.Module.Molecules?.ToList();
   }

   protected override IReadOnlyCollection<UsedCalculationMethod> MoleculeUsedCalculationMethodsFor(ModuleConfiguration configuration, MoleculeBuilder m)
   {
      return configuration.Module.Molecules?.FindByName(m.Name).UsedCalculationMethods.ToList();
   }
}