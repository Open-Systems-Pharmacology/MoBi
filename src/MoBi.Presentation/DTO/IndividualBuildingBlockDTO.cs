using System.Collections.Generic;
using OSPSuite.Core.Domain;
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

      public IReadOnlyList<IndividualParameterDTO> Parameters { get; set; }
      public ExtendedProperties OriginData => _individualBuildingBlock.OriginData;
      public string PKSimVersion => _individualBuildingBlock.PKSimVersion;
   }
}