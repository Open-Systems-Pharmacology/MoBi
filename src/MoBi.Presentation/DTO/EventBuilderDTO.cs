using System.Collections.Generic;

namespace MoBi.Presentation.DTO
{
   public class EventBuilderDTO : ObjectBaseDTO
   {
      public bool OneTime { get; set; }
      public string Condition { get; set; }
      public IEnumerable<ParameterDTO> Parameter { get; set; }
      public IEnumerable<EventAssignmentBuilderDTO> Assignments { get; set; }
   }
}