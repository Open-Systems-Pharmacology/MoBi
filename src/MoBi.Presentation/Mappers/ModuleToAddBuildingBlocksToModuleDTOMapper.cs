using MoBi.Assets;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility;

namespace MoBi.Presentation.Mappers
{
   public interface IModuleToAddBuildingBlocksToModuleDTOMapper : IMapper<Module, AddBuildingBlocksToModuleDTO>
   {

   }
   
   public class ModuleToAddBuildingBlocksToModuleDTOMapper : IModuleToAddBuildingBlocksToModuleDTOMapper
   {
      private readonly IContainerTask _containerTask;

      public ModuleToAddBuildingBlocksToModuleDTOMapper(IContainerTask containerTask)
      {
         _containerTask = containerTask;
      }
      
      public AddBuildingBlocksToModuleDTO MapFrom(Module module)
      {
         var addBuildingBlocksToModuleDTO = new AddBuildingBlocksToModuleDTO(module)
         {
            InitialConditionsName = _containerTask.CreateUniqueName(module.InitialConditionsCollection.AllNames(), AppConstants.DefaultNames.InitialConditions, canUseBaseName: true),
            ParameterValuesName = _containerTask.CreateUniqueName(module.ParameterValuesCollection.AllNames(), AppConstants.DefaultNames.ParameterValues, canUseBaseName: true)
         };

         addBuildingBlocksToModuleDTO.AddUsedInitialConditionsNames(module.InitialConditionsCollection.AllNames());
         addBuildingBlocksToModuleDTO.AddUsedParameterValuesNames(module.ParameterValuesCollection.AllNames());
         
         return addBuildingBlocksToModuleDTO;
      }
   }
}