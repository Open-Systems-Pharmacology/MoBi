using System;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core.Serialization.Converter.v3_1
{
   public interface IModelUpdaterTo3_1_3
   {
      void UpdateModel(IModel model);
   }

   internal class ModelUpdaterTo3_1_3 : IModelUpdaterTo3_1_3
   {
      public void UpdateModel(IModel model)
      {
         var distributedParamtersToChange = model.Root.GetAllChildren<IDistributedParameter>(dp => dp.Formula.IsAnImplementationOf<LogNormalDistributionFormula>());
         distributedParamtersToChange.Each(UpdateDistributedParameterTo3_1_3);
      }

      public void UpdateDistributedParameterTo3_1_3(IDistributedParameter distributedParameter)
      {
         var geoSDParameter = distributedParameter.GetSingleChildByName<IParameter>(Constants.Distribution.GEOMETRIC_DEVIATION);
         var constantFormula = ((ConstantFormula) geoSDParameter.Formula);
         constantFormula.Value = Math.Exp(constantFormula.Value);
      }
   }
}