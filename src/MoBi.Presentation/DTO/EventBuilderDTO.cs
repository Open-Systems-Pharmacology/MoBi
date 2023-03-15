using System.Collections.Generic;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.DTO
{
   public class EventBuilderDTO : ObjectBaseDTO
   {
      public EventBuilderDTO(IEventBuilder eventBuilder) : base(eventBuilder)
      {
      }

      public bool OneTime { get; set; }
      public string Condition { get; set; }
      public IEnumerable<ParameterDTO> Parameter { get; set; }
      public IEnumerable<EventAssignmentBuilderDTO> Assignments { get; set; }
   }
}