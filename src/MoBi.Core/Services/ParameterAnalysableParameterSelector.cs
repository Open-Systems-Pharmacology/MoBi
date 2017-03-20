using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Core.Services
{
   public class ParameterAnalysableParameterSelector : AbstractParameterAnalysableParameterSelector
   {
      public override bool CanUseParameter(IParameter parameter)
      {
         //in MoBi, you can use any parameter except table parameters and categorial parameters
         return !ParameterIsTable(parameter) 
                && !ParameterIsCategorial(parameter);
      }

      public override ParameterGroupingMode DefaultParameterSelectionMode => ParameterGroupingModes.Simple;
   }
}