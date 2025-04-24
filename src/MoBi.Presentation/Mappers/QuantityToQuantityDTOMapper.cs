using MoBi.Core.Domain.Extensions;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Core.Domain;
using OSPSuite.Utility;

namespace MoBi.Presentation.Mappers
{
   public interface IQuantityToQuantityDTOMapper : IMapper<IQuantity, QuantityDTO>
   {
      QuantityDTO MapFrom(IQuantity quantity, TrackableSimulation trackableSimulation);
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
         var dto = Map(new QuantityDTO(quantity));
         setQuantityValueAndFormula(dto, quantity.QuantityToEdit());
         return dto;
      }

      public QuantityDTO MapFrom(IQuantity quantity, TrackableSimulation trackableSimulation)
      {
         var dto = MapFrom(quantity);
         dto.SourceReference = trackableSimulation?.SourceFor(quantity);
         return dto;
      }

      private void setQuantityValueAndFormula(QuantityDTO dto, IQuantity quantity)
      {
         dto.Value = new ValueEditDTO { Dimension = quantity.Dimension, DisplayUnit = quantity.DisplayUnit, KernelValue = quantity.Value };
         dto.Formula = _formulaToDTOFormulaMapper.MapFrom(quantity.Formula);
      }
   }
}