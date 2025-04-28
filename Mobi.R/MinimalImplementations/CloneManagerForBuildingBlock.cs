using System;
using System.Collections.Generic;
using System.Text;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;

namespace MoBi.R.MinimalImplementations
{
   public class CloneManagerForBuildingBlock : ICloneManagerForBuildingBlock
   {
      public IFormulaCache FormulaCache { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

      T ICloneManagerForBuildingBlock.Clone<T>(T objectToClone, IFormulaCache formulaCache)
      {
         throw new NotImplementedException();
      }

      T ICloneManager.Clone<T>(T objectToClone)
      {
         throw new NotImplementedException();
      }
   }
}
