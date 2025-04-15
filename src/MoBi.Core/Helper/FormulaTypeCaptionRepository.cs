using System;
using MoBi.Assets;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Utility.Collections;

namespace MoBi.Core.Helper
{
   public class FormulaTypeCaptionRepository : Cache<Type, string>
   {
      public FormulaTypeCaptionRepository() : base(onMissingKey:key => null)
      {
         this[typeof(ConstantFormula)] = AppConstants.Captions.ConstantFormula;
         this[typeof(ExplicitFormula)] = AppConstants.Captions.ExplicitFormula;
         this[typeof(TableFormula)] = AppConstants.Captions.TableFormula;
         this[typeof(BlackBoxFormula)] = AppConstants.Captions.BlackBoxFormula;
         this[typeof(TableFormulaWithOffset)] = AppConstants.Captions.TableFormulaWithOffset;
         this[typeof(TableFormulaWithXArgument)] = AppConstants.Captions.TableFormulaWithXArgument;
         this[typeof(SumFormula)] = AppConstants.Captions.SumFormula;
         this[typeof(DistributedTableFormula)] = AppConstants.Captions.DistributedTableFormula;
      }
   }
}