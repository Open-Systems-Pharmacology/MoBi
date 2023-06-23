using System.Linq;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Mappers
{
   public interface IEventBuilderToEventBuilderDTOMapper : IMapper<EventBuilder, EventBuilderDTO>
   {
   }

   internal class EventBuilderToEventBuilderDTOMapper : ObjectBaseToObjectBaseDTOMapperBase, IEventBuilderToEventBuilderDTOMapper
   {
      private readonly IParameterToParameterDTOMapper _parameterToDTOParameterMapper;

      private readonly IEventAssignmentBuilderToEventAssignmentDTOMapper _assignmentToDTOAssignmentMapper;

      public EventBuilderToEventBuilderDTOMapper(IParameterToParameterDTOMapper parameterToDTOParameterMapper, IEventAssignmentBuilderToEventAssignmentDTOMapper assignmentToDTOAssignmentMapper)
      {
         _parameterToDTOParameterMapper = parameterToDTOParameterMapper;
         _assignmentToDTOAssignmentMapper = assignmentToDTOAssignmentMapper;
      }

      public EventBuilderDTO MapFrom(EventBuilder eventBuilder)
      {
         var dto = Map(new EventBuilderDTO(eventBuilder));
         dto.OneTime = eventBuilder.OneTime;
         dto.Condition = eventBuilder.Formula != null ? eventBuilder.Formula.Name : string.Empty;
         dto.Parameter = eventBuilder.Parameters.MapAllUsing(_parameterToDTOParameterMapper).Cast<ParameterDTO>();
         dto.Assignments = eventBuilder.Assignments.MapAllUsing(_assignmentToDTOAssignmentMapper);
         return dto;
      }
   }
}