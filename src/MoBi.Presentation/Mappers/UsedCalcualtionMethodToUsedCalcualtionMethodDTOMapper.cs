using System.Globalization;
using System.Linq;
using System.Text;
using MoBi.Assets;
using OSPSuite.Utility;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Assets;

namespace MoBi.Presentation.Mappers
{
   public interface IUsedCalcualtionMethodToUsedCalcualtionMethodDTOMapper : IMapper<UsedCalculationMethod, UsedCalculationMethodDTO>
   {
   }

   internal class UsedCalcualtionMethodToUsedCalcualtionMethodDTOMapper : IUsedCalcualtionMethodToUsedCalcualtionMethodDTOMapper
   {
      private readonly ICoreCalculationMethodRepository _calculationMethodsRepository;

      public UsedCalcualtionMethodToUsedCalcualtionMethodDTOMapper(ICoreCalculationMethodRepository calculationMethodsRepository)
      {
         _calculationMethodsRepository = calculationMethodsRepository;
      }

      public UsedCalculationMethodDTO MapFrom(UsedCalculationMethod usedCalculationMethod)
      {
         return new UsedCalculationMethodDTO
         {
            Category = usedCalculationMethod.Category,
            CalculationMethodName = usedCalculationMethod.CalculationMethod,
            Description = getDescriptionFor(usedCalculationMethod)
         };
      }

      private string getDescriptionFor(UsedCalculationMethod usedCalculationMethod)
      {
         var usedMethod = _calculationMethodsRepository.GetAllCalculationMethodsFor(usedCalculationMethod.Category).FirstOrDefault(c => c.Name.Equals(usedCalculationMethod.CalculationMethod));
         if (usedMethod == null)
            return string.Empty;

         var description = new StringBuilder();
         description.AppendLine("Defined Formulas:");

         foreach (var formula in usedMethod.AllOutputFormulas())
         {
            var descriptor = usedMethod.DescriptorFor(formula);
            description.AppendLine($"For Parameter:'{descriptor.ParameterName}'");
            description.AppendLine($"{"Black Box Formula"}:'{formula.Name}'");

            if (formula.IsExplicit())
               description.AppendLine(((ExplicitFormula) formula).FormulaString);
         }

         description.AppendLine("Defined Parameters:");

         foreach (var parameter in usedMethod.AllHelpParameters())
         {
            description.AppendLine($"{ObjectTypes.Parameter}:'{parameter.Name}'");
            addFormualDescription(parameter.Formula, description);
         }

         return description.ToString();
      }

      private static void addFormualDescription(IFormula formula, StringBuilder description)
      {
         if (formula.IsConstant())
            description.AppendLine($"{AppConstants.Captions.Value}: {((ConstantFormula) formula).Value.ToString(CultureInfo.InvariantCulture)}");

         if (formula.IsExplicit())
            description.AppendLine($"{AppConstants.Captions.Formula}:{((ExplicitFormula) formula).FormulaString}");
      }
   }
}