using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using System.Collections.Generic;

namespace MoBi.Core.Services
{
   public interface IPKSimStarter
   {
      void StartPopulationSimulationWithSimulationFile(string simulationFilePath);
      void StartWithWorkingJournalFile(string journalFilePath);
      IBuildingBlock CreateProfileExpression(ExpressionType expressionType);
      IBuildingBlock CreateIndividual();
      List<ExpressionParameterValueUpdate> UpdateExpressionProfileFromDatabase(ExpressionProfileBuildingBlock expressionProfile);
   }
}