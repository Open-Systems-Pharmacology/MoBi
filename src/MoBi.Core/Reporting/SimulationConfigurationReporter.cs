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

         listToReport.AddRange(_spatialStructureReporter.Report(simulationConfiguration.All<SpatialStructure>(), buildTracker));
         listToReport.AddRange(new MoleculeBuildingBlockReporter().Report(simulationConfiguration.All<MoleculeBuildingBlock>(), buildTracker));
         listToReport.AddRange(_reactionBuildingBlockReporter.Report(simulationConfiguration.All<ReactionBuildingBlock>(), buildTracker));
         listToReport.AddRange(new PassiveTransportBuildingBlockReporter().Report(simulationConfiguration.All<PassiveTransportBuildingBlock>(), buildTracker));
         listToReport.AddRange(new ObserverBuildingBlockReporter().Report(simulationConfiguration.All<ObserverBuildingBlock>(), buildTracker));
         listToReport.AddRange(_simulationSettingsReporter.Report(simulationConfiguration.SimulationSettings, buildTracker));
         listToReport.AddRange(new EventGroupBuildingBlockReporter().Report(simulationConfiguration.All<EventGroupBuildingBlock>(), buildTracker));
         listToReport.AddRange(new MoleculeStartValuesBuildingBlockReporter(_displayUnitRetriever).Report(simulationConfiguration.All<MoleculeStartValuesBuildingBlock>(), buildTracker));
         listToReport.AddRange(new ParameterStartValuesBuildingBlockReporter(_displayUnitRetriever).Report(simulationConfiguration.All<ParameterStartValuesBuildingBlock>(), buildTracker));

         return listToReport;
      }
   }
}