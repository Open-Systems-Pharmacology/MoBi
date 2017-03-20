using System.Collections.Generic;

namespace MoBi.Presentation.DTO
{
   public class ObserverBuildingBlockDTO :ObjectBaseDTO
   {
      public IEnumerable<ObserverBuilderDTO> ContainerObserverBuilders { get; set; }
      public IEnumerable<ObserverBuilderDTO> AmountObserverBuilders { get; set; }
   }
}