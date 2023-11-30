using OSPSuite.Core.Domain;

namespace MoBi.Presentation.DTO
{
   public class ModuleAndSpatialStructureDTO : ObjectBaseDTO
   {
      public ModuleAndSpatialStructureDTO(Module module) : base(module)
      {
      }

      public SpatialStructureDTO SpatialStructure { get; set; }
   }
}