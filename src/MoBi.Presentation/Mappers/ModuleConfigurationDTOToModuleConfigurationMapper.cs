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
         return new ModuleConfiguration(dto.Module, selectedInitialConditions(dto), selectedParameterValues(dto));
      }

      private static ParameterValuesBuildingBlock selectedParameterValues(ModuleConfigurationDTO dto)
      {
         return dto.HasParameterValues ? dto.SelectedParameterValues : null;
      }

      private static InitialConditionsBuildingBlock selectedInitialConditions(ModuleConfigurationDTO dto)
      {
         return dto.HasInitialConditions ? dto.SelectedInitialConditions : null;
      }
   }
}