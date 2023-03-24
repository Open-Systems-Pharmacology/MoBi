using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility;

namespace MoBi.Presentation.Mappers
{
   public interface IEventAssignmentBuilderToEventAssignmentDTOMapper : IMapper<IEventAssignmentBuilder, EventAssignmentBuilderDTO>
   {
   }

   internal class EventAssignmentBuilderToEventAssignmentDTOMapper : ObjectBaseToObjectBaseDTOMapperBase, IEventAssignmentBuilderToEventAssignmentDTOMapper
   {
      private readonly IFormulaToFormulaBuilderDTOMapper _formulaDTOMapper;

      public EventAssignmentBuilderToEventAssignmentDTOMapper(IFormulaToFormulaBuilderDTOMapper formulaDTOMapper)
      {
         _formulaDTOMapper = formulaDTOMapper;
      }

      public EventAssignmentBuilderDTO MapFrom(IEventAssignmentBuilder eventAssignmentBuilder)
      {
         var dto = Map(new EventAssignmentBuilderDTO(eventAssignmentBuilder));
         dto.ChangedEntityPath = eventAssignmentBuilder.ObjectPath == null ? string.Empty : eventAssignmentBuilder.ObjectPath.PathAsString;
         dto.NewFormula = _formulaDTOMapper.MapFrom(eventAssignmentBuilder.Formula);
         dto.UseAsValue = eventAssignmentBuilder.UseAsValue;
         return dto;
      }
   }
}