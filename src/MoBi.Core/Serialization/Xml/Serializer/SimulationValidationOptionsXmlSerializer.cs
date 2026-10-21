using MoBi.Core.Domain.Model;
using OSPSuite.Core.Serialization.Xml;

namespace MoBi.Core.Serialization.Xml.Serializer
{
   public class SimulationValidationOptionsXmlSerializer : OSPSuiteXmlSerializer<SimulationValidationOptions>
   {
      public override void PerformMapping()
      {
         Map(x => x.CheckDimensions);
         Map(x => x.ShowPKSimDimensionProblemWarnings);
         Map(x => x.ShowCannotCalcErrors);
         Map(x => x.ShowPKSimObserverMessages);
         Map(x => x.CheckRules);
         Map(x => x.CheckCircularReference);
         Map(x => x.ShowUnresolvedEndosomesWarningsForInitialConditions);
         Map(x => x.WarnForNonFiniteQuantities);
      }
   }
}
