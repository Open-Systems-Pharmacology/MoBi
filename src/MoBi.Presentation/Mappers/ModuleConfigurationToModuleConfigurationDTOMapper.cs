using System.Linq;
using MoBi.Core.Services;
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
      private readonly IMoBiProjectRetriever _projectRetriever;

      public ModuleConfigurationToModuleConfigurationDTOMapper(IMoBiProjectRetriever projectRetriever)
      {
         _projectRetriever = projectRetriever;
      }

      public ModuleConfigurationDTO MapFrom(ModuleConfiguration moduleConfiguration)
      {
         return new ModuleConfigurationDTO(createProjectModuleConfiguration(moduleConfiguration));
      }

      private ModuleConfiguration createProjectModuleConfiguration(ModuleConfiguration moduleConfiguration)
      {
         // If the module configuration already contains project building blocks then use it
         var module = _projectRetriever.Current.Modules.First(x => Equals(x.Name, moduleConfiguration.Module.Name));
         if (module.Equals(moduleConfiguration.Module))
            return moduleConfiguration;

         // Otherwise create a new module configuration with the same name and the building blocks from the project
         return new ModuleConfiguration(module,
            module.MoleculeStartValuesCollection.FindByName(moduleConfiguration.SelectedMoleculeStartValues?.Name),
            module.ParameterStartValuesCollection.FindByName(moduleConfiguration.SelectedParameterStartValues?.Name));
      }
   }
}