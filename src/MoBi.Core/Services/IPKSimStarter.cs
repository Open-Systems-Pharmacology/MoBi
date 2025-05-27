using OSPSuite.Core.Domain.Builder;
using System.Collections.Generic;
using OSPSuite.Core.Serialization.Exchange;

namespace MoBi.Core.Services
{
   public interface IPKSimStarter
   {
      void StartPopulationSimulationWithSimulationFile(string simulationFilePath);
      void StartWithWorkingJournalFile(string journalFilePath);
      IBuildingBlock CreateProfileExpression(ExpressionType expressionType);
      IBuildingBlock CreateIndividual();
      IReadOnlyList<ExpressionParameterValueUpdate> UpdateExpressionProfileFromDatabase(ExpressionProfileBuildingBlock expressionProfile);
      SimulationTransfer RecreateSimulationTransfer(string snapshot);
   }
}