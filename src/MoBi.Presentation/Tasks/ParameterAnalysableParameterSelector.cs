using MoBi.Presentation.Settings;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Presentation.Tasks
{
   public class ParameterAnalysableParameterSelector : AbstractParameterAnalysableParameterSelector
   {
      private readonly IUserSettings _userSettings;

      public ParameterAnalysableParameterSelector(IUserSettings userSettings)
      {
         _userSettings = userSettings;
      }

      public override bool CanUseParameter(IParameter parameter)
      {
         return parameter.CanBeVaried
                && !parameterIsSubParameter(parameter)
                && !ParameterIsTable(parameter)
                && !ParameterIsCategorial(parameter);
      }

      private bool parameterIsSubParameter(IParameter parameter) => parameter.ParentContainer is DistributedParameter;

      public override ParameterGroupingModeForParameterAnalyzable DefaultParameterSelectionMode => ParameterGroupingModesForParameterAnalyzable.ById(_userSettings.DefaultParameterGroupingModeForPIAndSA);
   }
}