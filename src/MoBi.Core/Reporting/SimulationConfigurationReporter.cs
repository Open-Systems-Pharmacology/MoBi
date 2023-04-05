using System.Collections.Generic;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Infrastructure.Reporting;
using OSPSuite.Core.Services;

namespace MoBi.Core.Reporting
{
   internal class SimulationConfigurationReporter : OSPSuiteTeXReporter<SimulationConfiguration>
   {
      private readonly ReactionBuildingBlockReporter _reactionBuildingBlockReporter;
      private readonly SpatialStructureReporter _spatialStructureReporter;
      private readonly SimulationSettingReporter _simulationSettingsReporter;
      private readonly IDisplayUnitRetriever _displayUnitRetriever;

      public SimulationConfigurationReporter(ReactionBuildingBlockReporter reactionBuildingBlockReporter,SpatialStructureReporter spatialStructureReporter, SimulationSettingReporter simulationSettingsReporter, IDisplayUnitRetriever displayUnitRetriever)
      {
         _reactionBuildingBlockReporter = reactionBuildingBlockReporter;
         _spatialStructureReporter = spatialStructureReporter;
         _simulationSettingsReporter = simulationSettingsReporter;
         _displayUnitRetriever = displayUnitRetriever;
      }

      public override IReadOnlyCollection<object> Report(SimulationConfiguration simulationConfiguration, OSPSuiteTracker buildTracker)
      {
         var listToReport = new List<object>();

         listToReport.AddRange(_spatialStructureReporter.Report(simulationConfiguration.SpatialStructure, buildTracker));
         listToReport.AddRange(new MoleculeBuildingBlockReporter().Report(simulationConfiguration.Molecules, buildTracker));
         listToReport.AddRange(_reactionBuildingBlockReporter.Report(simulationConfiguration.Reactions, buildTracker));
         listToReport.AddRange(new PassiveTransportBuildingBlockReporter().Report(simulationConfiguration.PassiveTransports, buildTracker));
         listToReport.AddRange(new ObserverBuildingBlockReporter().Report(simulationConfiguration.Observers, buildTracker));
         listToReport.AddRange(_simulationSettingsReporter.Report(simulationConfiguration.SimulationSettings, buildTracker));
         listToReport.AddRange(new EventGroupBuildingBlockReporter().Report(simulationConfiguration.EventGroups, buildTracker));
         listToReport.AddRange(new MoleculeStartValuesBuildingBlockReporter(_displayUnitRetriever).Report(simulationConfiguration.MoleculeStartValues, buildTracker));
         listToReport.AddRange(new ParameterStartValuesBuildingBlockReporter(_displayUnitRetriever).Report(simulationConfiguration.ParameterStartValues, buildTracker));

         return listToReport;
      }
   }
}