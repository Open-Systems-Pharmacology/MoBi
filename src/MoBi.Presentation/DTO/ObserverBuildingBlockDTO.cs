using System.Collections.Generic;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.DTO
{
   public class ObserverBuildingBlockDTO : ObjectBaseDTO
   {
      public ObserverBuildingBlockDTO(ObserverBuildingBlock observerBuildingBlock) : base(observerBuildingBlock)
      {
      }

      public IEnumerable<ObserverBuilderDTO> ContainerObserverBuilders { get; set; }
      public IEnumerable<ObserverBuilderDTO> AmountObserverBuilders { get; set; }
   }
}