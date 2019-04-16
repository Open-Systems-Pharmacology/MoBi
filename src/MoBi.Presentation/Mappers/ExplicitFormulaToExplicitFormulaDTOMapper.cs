using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Presentation.Mappers
{
   public interface IExplicitFormulaToExplicitFormulaDTOMapper
   {
      ExplicitFormulaBuilderDTO MapFrom(ExplicitFormula explicitFormula, IUsingFormula usingFormula);
   }

   public class ExplicitFormulaToExplicitFormulaDTOMapper : ObjectBaseToObjectBaseDTOMapperBase, IExplicitFormulaToExplicitFormulaDTOMapper
   {
      private readonly IFormulaUsablePathToFormulaUsablePathDTOMapper _formulaUsablePathDTOMapper;

      public ExplicitFormulaToExplicitFormulaDTOMapper(IFormulaUsablePathToFormulaUsablePathDTOMapper formulaUsablePathDTOMapper)
      {
         _formulaUsablePathDTOMapper = formulaUsablePathDTOMapper;
      }

      public ExplicitFormulaBuilderDTO MapFrom(ExplicitFormula explicitFormula, IUsingFormula usingFormula)
      {
         var dto = Map<ExplicitFormulaBuilderDTO>(explicitFormula);
         dto.FormulaString = explicitFormula.FormulaString;
         dto.Dimension = explicitFormula.Dimension;
         dto.ObjectPaths = _formulaUsablePathDTOMapper.MapFrom(explicitFormula, usingFormula);
         return dto;
      }
   }
}