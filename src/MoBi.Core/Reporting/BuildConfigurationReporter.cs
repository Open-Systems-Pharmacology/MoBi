using System.Collections.Generic;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Infrastructure.Reporting;
using OSPSuite.Core.Services;

namespace MoBi.Core.Reporting
{
   internal class BuildConfigurationReporter : OSPSuiteTeXReporter<IBuildConfiguration>
   {
      private readonly ReactionBuildingBlockReporter _reactionBuildingBlockReporter;
      private readonly SpatialStructureReporter _spatialStructureReporter;
      private readonly SimulationSettingReporter _simulationSettingsReporter;
      private readonly IDisplayUnitRetriever _displayUnitRetriever;

      public BuildConfigurationReporter(ReactionBuildingBlockReporter reactionBuildingBlockReporter,SpatialStructureReporter spatialStructureReporter, SimulationSettingReporter simulationSettingsReporter, IDisplayUnitRetriever displayUnitRetriever)
      {
         _reactionBuildingBlockReporter = reactionBuildingBlockReporter;
         _spatialStructureReporter = spatialStructureReporter;
         _simulationSettingsReporter = simulationSettingsReporter;
         _displayUnitRetriever = displayUnitRetriever;
      }

      public override IReadOnlyCollection<object> Report(IBuildConfiguration buildConfiguration, OSPSuiteTracker buildTracker)
      {
         var listToReport = new List<object>();

         listToReport.AddRange(_spatialStructureReporter.Report(buildConfiguration.SpatialStructure, buildTracker));
         listToReport.AddRange(new MoleculeBuildingBlockReporter().Report(buildConfiguration.Molecules, buildTracker));
         listToReport.AddRange(_reactionBuildingBlockReporter.Report(buildConfiguration.Reactions, buildTracker));
         listToReport.AddRange(new PassiveTransportBuildingBlockReporter().Report(buildConfiguration.PassiveTransports, buildTracker));
         listToReport.AddRange(new ObserverBuildingBlockReporter().Report(buildConfiguration.Observers, buildTracker));
         listToReport.AddRange(_simulationSettingsReporter.Report(buildConfiguration.SimulationSettings, buildTracker));
         listToReport.AddRange(new EventGroupBuildingBlockReporter().Report(buildConfiguration.EventGroups, buildTracker));
         listToReport.AddRange(new MoleculeStartValuesBuildingBlockReporter(_displayUnitRetriever).Report(buildConfiguration.MoleculeStartValues, buildTracker));
         listToReport.AddRange(new ParameterStartValuesBuildingBlockReporter(_displayUnitRetriever).Report(buildConfiguration.ParameterStartValues, buildTracker));

         return listToReport;
      }
   }
}