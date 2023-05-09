using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Mappers
{
   public abstract class PathWithValueToDTOMapper<TInput, TDTO> : ObjectBaseToObjectBaseDTOMapperBase where TDTO : PathAndValueEntityDTO<TInput> where TInput : PathAndValueEntity
   {
      protected readonly IFormulaToValueFormulaDTOMapper _formulaMapper;

      protected PathWithValueToDTOMapper(IFormulaToValueFormulaDTOMapper formulaMapper)
      {
         _formulaMapper = formulaMapper;
      }

      public TDTO MapFrom(TInput inputParameter)
      {
         var dto = DTOFor(inputParameter);
         dto.Formula = _formulaMapper.MapFrom(inputParameter.Formula);
         return dto;
      }

      protected abstract TDTO DTOFor(TInput inputParameter);
   }
}