using System.Collections.Generic;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.DTO
{
   public class IndividualBuildingBlockDTO
   {
      private readonly IndividualBuildingBlock _individualBuildingBlock;

      public IndividualBuildingBlockDTO(IndividualBuildingBlock individualBuildingBlock)
      {
         _individualBuildingBlock = individualBuildingBlock;
      }

      public IReadOnlyList<IndividualParameterDTO> ParameterDTOs { get; set; }
      public OriginDataDTO OriginDataDTO { get; set; }
   }
}