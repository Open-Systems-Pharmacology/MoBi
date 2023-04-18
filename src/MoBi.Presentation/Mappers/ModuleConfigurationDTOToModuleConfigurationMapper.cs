using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility;

namespace MoBi.Presentation.Mappers
{
   public interface IModuleConfigurationDTOToModuleConfigurationMapper : IMapper<ModuleConfigurationDTO, ModuleConfiguration>
   {
   }
   
   public class ModuleConfigurationDTOToModuleConfigurationMapper : IModuleConfigurationDTOToModuleConfigurationMapper
   {
      private readonly ICloneManagerForBuildingBlock _cloneManager;

      public ModuleConfigurationDTOToModuleConfigurationMapper(ICloneManagerForBuildingBlock cloneManager)
      {
         _cloneManager = cloneManager;
      }
      
      public ModuleConfiguration MapFrom(ModuleConfigurationDTO dto)
      {
         var clonedModule = _cloneManager.Clone(dto.Module);
         return new ModuleConfiguration(clonedModule, selectedMoleculeStartValues(dto, clonedModule), selectedParameterStartValues(dto, clonedModule));
      }

      private static ParameterStartValuesBuildingBlock selectedParameterStartValues(ModuleConfigurationDTO dto, Module targetModule)
      {
         return dto.HasParameterStartValues ? targetModule.ParameterStartValuesCollection.FindByName(dto.SelectedParameterStartValues.Name) : null;
      }

      private static MoleculeStartValuesBuildingBlock selectedMoleculeStartValues(ModuleConfigurationDTO dto, Module targetModule)
      {
         return dto.HasMoleculeStartValues ? targetModule.MoleculeStartValuesCollection.FindByName(dto.SelectedMoleculeStartValues.Name) : null;
      }
   }
}