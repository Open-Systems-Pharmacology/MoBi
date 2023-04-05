using System.Linq;
using MoBi.Core.Domain.Extensions;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;
using IContainer = OSPSuite.Core.Domain.IContainer;

namespace MoBi.Presentation.Mappers
{
   public interface IEventGroupBuilderToEventGroupBuilderDTOMapper : IMapper<IEventGroupBuilder, EventGroupBuilderDTO>
   {
   }

   internal class EventGroupBuilderToEventGroupBuilderDTOMapper : ObjectBaseToObjectBaseDTOMapperBase, IEventGroupBuilderToEventGroupBuilderDTOMapper
   {
      private readonly IParameterToParameterDTOMapper _parameterDTOMapper;
      private readonly IEventBuilderToEventBuilderDTOMapper _eventBuilderDTOMapper;
      private readonly IContainerToContainerDTOMapper _containerDTOMapper;

      /// <summary>
      ///    field has to be initialised in different way in derived Types
      /// </summary>
      protected IApplicationBuilderToApplicationBuilderDTOMapper _applicationBuilderToDTOApplicationBuilderMapper;

      public EventGroupBuilderToEventGroupBuilderDTOMapper(IParameterToParameterDTOMapper parameterDTOMapper,
         IEventBuilderToEventBuilderDTOMapper eventBuilderDTOMapper,
         IContainerToContainerDTOMapper containerDTOMapper)
      {
         _parameterDTOMapper = parameterDTOMapper;
         _containerDTOMapper = containerDTOMapper;
         _eventBuilderDTOMapper = eventBuilderDTOMapper;
      }

      public EventGroupBuilderDTO MapFrom(IEventGroupBuilder eventGroupBuilder)
      {
         //TODO -This should be refactor to avoid usage of IoC
         _applicationBuilderToDTOApplicationBuilderMapper = IoC.Resolve<IApplicationBuilderToApplicationBuilderDTOMapper>();
         return MapEventGroupProperties(eventGroupBuilder, new EventGroupBuilderDTO(eventGroupBuilder));
      }

      protected T MapEventGroupProperties<T>(IEventGroupBuilder input, T dto) where T : EventGroupBuilderDTO
      {
         MapProperties(input, dto);
         dto.Parameters = input.GetChildrenSortedByName<IParameter>().MapAllUsing(_parameterDTOMapper).Cast<ParameterDTO>();
         dto.Events = input.Events.OrderBy(x => x.Name).MapAllUsing(_eventBuilderDTOMapper);
         dto.EventGroups = input.GetChildrenSortedByName<IEventGroupBuilder>(child => !child.IsAnImplementationOf<IApplicationBuilder>()).MapAllUsing(this);
         dto.Applications = input.GetChildrenSortedByName<IApplicationBuilder>().MapAllUsing(_applicationBuilderToDTOApplicationBuilderMapper);
         dto.ChildContainer = input.GetChildrenSortedByName<IContainer>(isPureContainer).MapAllUsing(_containerDTOMapper);
         return dto;
      }

      private bool isPureContainer(IContainer container)
      {
         return !container.IsAnImplementationOf<IEventGroupBuilder>() &&
                !container.IsAnImplementationOf<IEventBuilder>() &&
                !container.IsAnImplementationOf<ITransportBuilder>();
      }
   }
}