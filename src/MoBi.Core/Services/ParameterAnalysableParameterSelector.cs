using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Core.Services
{
   public class ParameterAnalysableParameterSelector : AbstractParameterAnalysableParameterSelector
   {
      public override bool CanUseParameter(IParameter parameter)
      {
         return parameter.CanBeVaried
                && !parameterIsSubParameter(parameter)
                && !ParameterIsTable(parameter)
                && !ParameterIsCategorial(parameter);
      }

      private bool parameterIsSubParameter(IParameter parameter) => parameter.ParentContainer is DistributedParameter;

      public override ParameterGroupingMode DefaultParameterSelectionMode => ParameterGroupingModes.Simple;
   }
}