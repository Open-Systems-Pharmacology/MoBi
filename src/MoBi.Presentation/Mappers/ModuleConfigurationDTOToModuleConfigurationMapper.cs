using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility;

namespace MoBi.Presentation.Mappers
{
   public interface IModuleConfigurationDTOToModuleConfigurationMapper : IMapper<ModuleConfigurationDTO, ModuleConfiguration>
   {
   }
   
   public class ModuleConfigurationDTOToModuleConfigurationMapper : IModuleConfigurationDTOToModuleConfigurationMapper
   {

      public ModuleConfigurationDTOToModuleConfigurationMapper()
      {

      }
      
      public ModuleConfiguration MapFrom(ModuleConfigurationDTO dto)
      {
         return new ModuleConfiguration(dto.Module, selectedMoleculeStartValues(dto), selectedParameterStartValues(dto));
      }

      private static ParameterStartValuesBuildingBlock selectedParameterStartValues(ModuleConfigurationDTO dto)
      {
         return dto.HasParameterStartValues ? dto.SelectedParameterStartValues : null;
      }

      private static MoleculeStartValuesBuildingBlock selectedMoleculeStartValues(ModuleConfigurationDTO dto)
      {
         return dto.HasMoleculeStartValues ? dto.SelectedMoleculeStartValues : null;
      }
   }
}