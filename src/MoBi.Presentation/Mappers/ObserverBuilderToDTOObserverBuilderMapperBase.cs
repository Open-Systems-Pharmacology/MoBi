using OSPSuite.Utility;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Mappers
{
   public interface IObserverBuilderToDTOObserverBuilderMapper : IMapper<ObserverBuilder, ObserverBuilderDTO>
   {
   }

   public class ObserverBuilderToDTOObserverBuilderMapper : ObjectBaseToObjectBaseDTOMapperBase, IObserverBuilderToDTOObserverBuilderMapper
   {
      private readonly IFormulaToFormulaBuilderDTOMapper _formulaToDTOFormulaMapper;

      public ObserverBuilderToDTOObserverBuilderMapper(IFormulaToFormulaBuilderDTOMapper formulaToDTOFormulaMapper)
      {
         _formulaToDTOFormulaMapper = formulaToDTOFormulaMapper;
      }

      protected T MapObserverBuilder<T>(T dto, ObserverBuilder observerBuilder) where T : ObserverBuilderDTO
      {
         MapProperties(observerBuilder, dto);
         dto.Dimension = observerBuilder.Dimension;
         if (observerBuilder.Formula != null)
         {
            dto.Monitor = _formulaToDTOFormulaMapper.MapFrom(observerBuilder.Formula);
            dto.MonitorString = observerBuilder.Formula.ToString();
         }
         return dto;
      }

      public ObserverBuilderDTO MapFrom(ObserverBuilder observerBuilder)
      {
         return MapObserverBuilder(new ObserverBuilderDTO(observerBuilder), observerBuilder);
      }
   }
}