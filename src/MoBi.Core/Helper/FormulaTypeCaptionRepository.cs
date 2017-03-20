using System;
using System.Collections.Generic;
using MoBi.Assets;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core.Helper
{
   public class FormulaTypeCaptionRepository : Dictionary<Type, string>
   {
      public FormulaTypeCaptionRepository()
      {
         Add(typeof(ConstantFormula),AppConstants.Captions.ConstantFormula);
         Add(typeof(ExplicitFormula),AppConstants.Captions.ExplicitFormula);
         Add(typeof(TableFormula),AppConstants.Captions.TableFormula);
         Add(typeof(BlackBoxFormula),AppConstants.Captions.BlackBoxFormula);
         Add(typeof(TableFormulaWithOffset), AppConstants.Captions.TableFormulaWithOffset);
         Add(typeof (SumFormula), AppConstants.Captions.SumFormula);
      }
   }
}