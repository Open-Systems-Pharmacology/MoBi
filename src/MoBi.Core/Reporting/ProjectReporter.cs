using System.Collections.Generic;
using System.Linq;
using OSPSuite.TeXReporting.Items;
using MoBi.Core.Domain.Model;
using OSPSuite.Infrastructure.Reporting;
using OSPSuite.Core.Services;

namespace MoBi.Core.Reporting
{
   internal class ProjectReporter : OSPSuiteTeXReporter<MoBiProject>
   {
      private readonly SimulationsReporter _simulationsReporter;
      private readonly ReactionBuildingBlocksReporter _reactionBuildingBlocksReporter;
      private readonly SpatialStructuresReporter _spatialStructuresReporter;
      private readonly SimulationSettingsReporter _simulationSettingsReporter;
      private readonly IDisplayUnitRetriever _displayUnitRetriever;

      public ProjectReporter(SimulationsReporter simulationsReporter, ReactionBuildingBlocksReporter reactionBuildingBlocksReporter, SpatialStructuresReporter spatialStructuresReporter, SimulationSettingsReporter simulationSettingsReporter, IDisplayUnitRetriever displayUnitRetriever)
      {
         _simulationsReporter = simulationsReporter;
         _reactionBuildingBlocksReporter = reactionBuildingBlocksReporter;
         _spatialStructuresReporter = spatialStructuresReporter;
         _simulationSettingsReporter = simulationSettingsReporter;
         _displayUnitRetriever = displayUnitRetriever;
      }

      public override IReadOnlyCollection<object> Report(MoBiProject project, OSPSuiteTracker buildTracker)
      {
         var listToReport = new List<object>();
         listToReport.Add(new Part(Constants.BUILDING_BLOCKS));
         listToReport.AddRange(_spatialStructuresReporter.Report(project.Modules.ToList(), buildTracker));
         listToReport.AddRange(new MoleculeBuildingBlocksReporter().Report(project.IndividualsCollection.ToList(), buildTracker));
         listToReport.AddRange(_reactionBuildingBlocksReporter.Report(project.ExpressionProfileCollection.ToList(), buildTracker));
         listToReport.AddRange(_simulationSettingsReporter.Report(project.SimulationSettings, buildTracker));
         listToReport.AddRange(new ObservedDataReporter().Report(project.AllObservedData.ToList(), buildTracker));
         listToReport.AddRange(_simulationsReporter.Report(project.Simulations.ToList(), buildTracker));
         listToReport.AddRange(new ChartsReporter().Report(project.Charts.ToList(), buildTracker));       
         return listToReport;
      }
   }
}