using System.Linq;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Presentation.Mappers
{
   public interface IFormulaToFormulaBuilderDTOMapper : IMapper<IFormula, FormulaBuilderDTO>
   {
   }

   public class FormulaToFormulaBuilderDTOMapper : ObjectBaseToObjectBaseDTOMapperBase, IFormulaToFormulaBuilderDTOMapper
   {
      private readonly IFormulaUsablePathToFormulaUsablePathDTOMapper _formulaUsablePathMapper;
      private readonly IObjectTypeResolver _resolver;

      public FormulaToFormulaBuilderDTOMapper(IFormulaUsablePathToFormulaUsablePathDTOMapper formulaUsablePathMapper, IObjectTypeResolver resolver)
      {
         _formulaUsablePathMapper = formulaUsablePathMapper;
         _resolver = resolver;
      }

      public FormulaBuilderDTO MapFrom(IFormula formula)
      {
         if (formula == null)
            return FormulaBuilderDTO.NULL;

         var dto = Map<FormulaBuilderDTO>(formula);
         if (formula.IsConstant())
            dto.FormulaString = ((ConstantFormula) formula).Value.ConvertedTo<string>();

         else if (formula.IsExplicit())
            dto.FormulaString = ((ExplicitFormula) formula).FormulaString;

         dto.FormulaType = _resolver.TypeFor(formula);
         dto.Dimension = formula.Dimension;
         dto.ObjectPaths = _formulaUsablePathMapper.MapFrom(formula);
         return dto;
      }
   }
}