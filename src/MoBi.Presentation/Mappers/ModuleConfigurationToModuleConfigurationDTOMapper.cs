using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;
using OSPSuite.Utility;

namespace MoBi.Presentation.Mappers
{
   public interface IModuleConfigurationToModuleConfigurationDTOMapper : IMapper<ModuleConfiguration, ModuleConfigurationDTO>
   {

   }
   
   public class ModuleConfigurationToModuleConfigurationDTOMapper : IModuleConfigurationToModuleConfigurationDTOMapper
   {
      public ModuleConfigurationDTO MapFrom(ModuleConfiguration moduleConfiguration)
      {
         return new ModuleConfigurationDTO(moduleConfiguration);
      }
   }
}