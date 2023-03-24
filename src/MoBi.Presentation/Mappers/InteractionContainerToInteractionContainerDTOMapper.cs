using System.Linq;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Mappers
{
   public interface IInteractionContainerToInteractionContainerDTOMapper : IMapper<InteractionContainer, InteractionContainerDTO>
   {
   }
   class InteractionContainerToInteractionContainerDTOMapper : ObjectBaseToObjectBaseDTOMapperBase, IInteractionContainerToInteractionContainerDTOMapper
   {
      private readonly IParameterToParameterDTOMapper _parameterToParameterDTOMapper;

      public InteractionContainerToInteractionContainerDTOMapper(IParameterToParameterDTOMapper parameterToParameterDTOMapper)
      {
         _parameterToParameterDTOMapper = parameterToParameterDTOMapper;
      }

      public InteractionContainerDTO MapFrom(InteractionContainer interactionContainer)
      {
         var dto = Map(new InteractionContainerDTO(interactionContainer));
         dto.Parameters = interactionContainer.GetChildren<IParameter>().MapAllUsing(_parameterToParameterDTOMapper).Cast<ParameterDTO>();
         return dto;
      }
   }
}