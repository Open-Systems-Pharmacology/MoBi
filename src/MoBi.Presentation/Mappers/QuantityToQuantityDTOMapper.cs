using OSPSuite.Utility;
using MoBi.Core.Domain.Extensions;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;

namespace MoBi.Presentation.Mappers
{
   public interface IQuantityToQuantityDTOMapper : IMapper<IQuantity, QuantityDTO>
   {
   }

   internal class QuantityToQuantityDTOMapper : ObjectBaseToObjectBaseDTOMapperBase, IQuantityToQuantityDTOMapper
   {
      private readonly IFormulaToFormulaBuilderDTOMapper _formulaToDTOFormulaMapper;

      public QuantityToQuantityDTOMapper(IFormulaToFormulaBuilderDTOMapper formulaToDTOFormulaMapper)
      {
         _formulaToDTOFormulaMapper = formulaToDTOFormulaMapper;
      }

      public QuantityDTO MapFrom(IQuantity quantity)
      {
         var dto = Map<QuantityDTO>(quantity);
         setQuantityValueAndFormula(dto, quantity.QuantityToEdit());
         return dto;
      }

      private void setQuantityValueAndFormula(QuantityDTO dto, IQuantity quantity)
      {
         dto.Value = new ValueEditDTO {Dimension = quantity.Dimension, DisplayUnit = quantity.DisplayUnit, KernelValue = quantity.Value};
         dto.Formula = _formulaToDTOFormulaMapper.MapFrom(quantity.Formula);
      }
   }
}