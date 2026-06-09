using MoBi.Assets;
using MoBi.Presentation.DTO;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility;

namespace MoBi.Presentation.Mappers
{
   public interface IObserverBuilderToObjectBaseSummaryDTOMapper : IMapper<ObserverBuilder, ObjectBaseSummaryDTO>
   {

   }

   public class ObserverBuilderToObjectBaseSummaryDTOMapper : IObserverBuilderToObjectBaseSummaryDTOMapper
   {
      private readonly IObjectTypeResolver _objectTypeResolver;

      public ObserverBuilderToObjectBaseSummaryDTOMapper(IObjectTypeResolver objectTypeResolver)
      {
         _objectTypeResolver = objectTypeResolver;
      }

      public ObjectBaseSummaryDTO MapFrom(ObserverBuilder observerBuilder)
      {
         var dto = new ObjectBaseSummaryDTO
         {
            ApplicationIcon = ApplicationIcons.Observer,
            EntityName = observerBuilder.Name
         };

         dto.AddToDictionary(AppConstants.Captions.Type, _objectTypeResolver.TypeFor(observerBuilder));

         return dto;

      }
   }
}