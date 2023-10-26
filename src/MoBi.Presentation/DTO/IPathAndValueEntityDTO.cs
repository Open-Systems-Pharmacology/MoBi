using OSPSuite.Core.Domain;
using OSPSuite.Presentation.DTO;

namespace MoBi.Presentation.DTO
{
   public interface IPathAndValueEntityDTO : 
      IWithDisplayUnitDTO, 
      IWithValueOrigin, 
      IWithName
   {
      ValueFormulaDTO Formula { get; set; }
   }
}