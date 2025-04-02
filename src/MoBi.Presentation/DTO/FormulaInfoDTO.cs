using System;
using MoBi.Assets;
using MoBi.Core.Domain.Extensions;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Validation;

namespace MoBi.Presentation.DTO
{
   public class FormulaInfoDTO : IValidatable
   {
      public string Name { get; set; }
      public Type Type { get; set; }

      public FormulaInfoDTO()
      {
         Rules = new BusinessRuleSet(createNonEmptyNameRule());
      }

      private IBusinessRule createNonEmptyNameRule()
      {
         return CreateRule.For<FormulaInfoDTO>()
            .Property(x => x.Name)
            .WithRule((dto, name) => dto.nameIsValid(name))
            .WithError(AppConstants.Validation.EmptyName);
      }

      private bool nameIsValid(string name)
      {
         if (Type.IsAnImplementationOf<ConstantFormula>() || Type.IsAnImplementationOf<DistributionFormula>())
            return true;

         return name.IsNotEmpty();
      }

      public IBusinessRuleSet Rules { get; }
   }
}