namespace MoBi.Core;

public interface ICoreUserSettings : OSPSuite.Core.ICoreUserSettings
{
   bool CheckDimensions { get; set; }
   bool ShowPKSimDimensionProblemWarnings { get; set; }
   bool ShowCannotCalcErrors { get; set; }
   bool CheckRules { get; set; }
}