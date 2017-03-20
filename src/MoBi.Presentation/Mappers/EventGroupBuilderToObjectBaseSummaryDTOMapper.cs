using System.Globalization;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Utility;
using MoBi.Core.Helper;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Assets;

namespace MoBi.Presentation.Mappers
{
   public interface IEventGroupBuilderToObjectBaseSummaryDTOMapper : IMapper<IEventGroupBuilder, ObjectBaseSummaryDTO>
   {
   }

   public class EventGroupBuilderToObjectBaseSummaryDTOMapper : IEventGroupBuilderToObjectBaseSummaryDTOMapper
   {
      private readonly IObjectTypeResolver _objectTypeResolver;

      public EventGroupBuilderToObjectBaseSummaryDTOMapper(IObjectTypeResolver objectTypeResolver)
      {
         _objectTypeResolver = objectTypeResolver;
      }

      public ObjectBaseSummaryDTO MapFrom(IEventGroupBuilder eventGroup)
      {
         var dto = new ObjectBaseSummaryDTO
         {
            ApplicationIcon = ApplicationIcons.EventGroup,
            EntityName = eventGroup.Name
         };

         dto.AddToDictionary(AppConstants.Captions.Type, _objectTypeResolver.TypeFor(eventGroup));
         dto.AddToDictionary(AppConstants.Captions.NumberOfChildren, eventGroup.Children.Count().ToString(CultureInfo.InvariantCulture));
         return dto;
      }
   }
}