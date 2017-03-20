using System;
using MoBi.Assets;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Validation;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Presentation.DTO
{
   public class FormulaInfoDTO : IValidatable
   {
      private readonly IBusinessRuleSet _rules;
      public string Name { get; set; }
      public Type Type { get; set; }
      
      public FormulaInfoDTO()
      {
         _rules = new BusinessRuleSet(createNonEmptyNameRule());
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
         if(Type.IsAnImplementationOf<ConstantFormula>()||Type.IsAnImplementationOf<DistributionFormula>())
            return true;

         return name.IsNotEmpty();
      }

      
      public IBusinessRuleSet Rules
      {
         get { return _rules; }
      }
   }
}