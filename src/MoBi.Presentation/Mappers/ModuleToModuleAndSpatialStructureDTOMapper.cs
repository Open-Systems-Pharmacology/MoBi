using MoBi.Presentation.DTO;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Utility;

namespace MoBi.Presentation.Mappers
{
   public interface IModuleToModuleAndSpatialStructureDTOMapper : IMapper<Module, ModuleAndSpatialStructureDTO>
   {
   }

   public class ModuleToModuleAndSpatialStructureDTOMapper : ObjectBaseToObjectBaseDTOMapperBase, IModuleToModuleAndSpatialStructureDTOMapper
   {
      private readonly ISpatialStructureToSpatialStructureDTOMapper _spatialStructureToSpatialStructureDTOMapper;

      public ModuleToModuleAndSpatialStructureDTOMapper(ISpatialStructureToSpatialStructureDTOMapper spatialStructureToSpatialStructureDTOMapper)
      {
         _spatialStructureToSpatialStructureDTOMapper = spatialStructureToSpatialStructureDTOMapper;
      }

      public ModuleAndSpatialStructureDTO MapFrom(Module module)
      {
         var dto = Map(new ModuleAndSpatialStructureDTO(module));
         dto.SpatialStructure = _spatialStructureToSpatialStructureDTOMapper.MapFrom(module.SpatialStructure);
         dto.Name = module.Name;
         dto.Icon = ApplicationIcons.IconByName(module.Icon);

         return dto;
      }
   }
}