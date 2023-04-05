using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Presentation.Mappers
{
   public interface IConstantFormulaToDTOConstantFormulaMapper
   {
      ConstantFormulaBuilderDTO MapFrom(ConstantFormula constantFormula, Unit displayUnit);
   }

   public class ConstantFormulaToDTOConstantFormulaMapper : ObjectBaseToObjectBaseDTOMapperBase, IConstantFormulaToDTOConstantFormulaMapper
   {
      private readonly IDimensionFactory _dimensionFactory;

      public ConstantFormulaToDTOConstantFormulaMapper(IDimensionFactory dimensionFactory)
      {
         _dimensionFactory = dimensionFactory;
      }

      public ConstantFormulaBuilderDTO MapFrom(ConstantFormula constantFormula, Unit displayUnit)
      {
         var dto = Map(new ConstantFormulaBuilderDTO(constantFormula));
         dto.Dimension = constantFormula.Dimension ?? _dimensionFactory.Dimension(Constants.Dimension.DIMENSIONLESS);
         //set the kernel value after the fact (afer display unit);
         var valueDTO = new ValueEditDTO {Dimension = dto.Dimension, DisplayUnit = displayUnit, KernelValue = constantFormula.Value};
         constantFormula.Changed += o => { valueDTO.KernelValue = constantFormula.Value; };
         dto.Value = valueDTO;
         return dto;
      }
   }
}