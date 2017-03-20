using System.Collections.Generic;

namespace MoBi.Presentation.DTO
{
   public class EventGroupBuilderDTO : ObjectBaseDTO
   {
      public IEnumerable<ApplicationBuilderDTO> Applications { set; get; }
      public IEnumerable<ParameterDTO> Parameters { set; get; }
      public IEnumerable<EventBuilderDTO> Events { get; set; }
      public IEnumerable<EventGroupBuilderDTO> EventGroups { get; set; }
      public IEnumerable<ContainerDTO> ChildContainer { get; set; }
   }
}