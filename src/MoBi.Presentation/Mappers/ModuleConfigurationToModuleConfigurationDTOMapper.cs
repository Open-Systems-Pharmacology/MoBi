using System.Linq;
using MoBi.Core.Domain.Model;
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
      private readonly IMoBiContext _context;

      public ModuleConfigurationToModuleConfigurationDTOMapper(IMoBiContext context)
      {
         _context = context;
      }

      public ModuleConfigurationDTO MapFrom(ModuleConfiguration moduleConfiguration)
      {
         return new ModuleConfigurationDTO(createProjectModuleConfiguration(moduleConfiguration));
      }

      private ModuleConfiguration createProjectModuleConfiguration(ModuleConfiguration moduleConfiguration)
      {
         // If the module configuration already contains project building blocks then use it
         var module = _context.CurrentProject.Modules.First(x => Equals(x.Name, moduleConfiguration.Module.Name));
         if (module.Equals(moduleConfiguration.Module))
            return moduleConfiguration;

         // Otherwise create a new module configuration with the same name and the building blocks from the project
         return new ModuleConfiguration(module,
            moduleConfiguration.SelectedMoleculeStartValues == null ? null : module.MoleculeStartValuesCollection.FindByName(moduleConfiguration.SelectedMoleculeStartValues?.Name),
            moduleConfiguration.SelectedParameterStartValues == null ? null : module.ParameterStartValuesCollection.FindByName(moduleConfiguration.SelectedParameterStartValues.Name));
      }
   }
}