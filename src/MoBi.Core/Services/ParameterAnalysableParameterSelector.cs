using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Core.Services
{
   public class ParameterAnalysableParameterSelector : AbstractParameterAnalysableParameterSelector
   {
      public override bool CanUseParameter(IParameter parameter)
      {
         return parameter.CanBeVaried
                && !ParameterIsTable(parameter)
                && !ParameterIsCategorial(parameter);
      }

      public override ParameterGroupingMode DefaultParameterSelectionMode => ParameterGroupingModes.Simple;
   }
}