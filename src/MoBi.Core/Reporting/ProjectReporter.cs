using System.Collections.Generic;
using System.Linq;
using OSPSuite.TeXReporting.Items;
using MoBi.Core.Domain.Model;
using OSPSuite.Infrastructure.Reporting;
using OSPSuite.Core.Services;
using MoBi.Core.Domain.Repository;

namespace MoBi.Core.Reporting
{
   internal class ProjectReporter : OSPSuiteTeXReporter<MoBiProject>
   {
      private readonly SimulationsReporter _simulationsReporter;
      private readonly ReactionBuildingBlocksReporter _reactionBuildingBlocksReporter;
      private readonly SpatialStructuresReporter _spatialStructuresReporter;
      private readonly SimulationSettingsReporter _simulationSettingsReporter;
      private readonly IBuildingBlockRepository _buildingBlockRepository;
      private IDisplayUnitRetriever _displayUnitRetriever;

      public ProjectReporter(SimulationsReporter simulationsReporter, 
         ReactionBuildingBlocksReporter reactionBuildingBlocksReporter, 
         SpatialStructuresReporter spatialStructuresReporter, 
         SimulationSettingsReporter simulationSettingsReporter,
         IDisplayUnitRetriever displayUnitRetriever,
         IBuildingBlockRepository buildingBlockRepository)
      {
         _simulationsReporter = simulationsReporter;
         _reactionBuildingBlocksReporter = reactionBuildingBlocksReporter;
         _spatialStructuresReporter = spatialStructuresReporter;
         _simulationSettingsReporter = simulationSettingsReporter;
         _displayUnitRetriever = displayUnitRetriever;
         _buildingBlockRepository = buildingBlockRepository;
      }

      public override IReadOnlyCollection<object> Report(MoBiProject project, OSPSuiteTracker buildTracker)
      {
         var listToReport = new List<object>();
         listToReport.Add(new Part(Constants.BUILDING_BLOCKS));
         listToReport.AddRange(_spatialStructuresReporter.Report(_buildingBlockRepository.SpatialStructureCollection.ToList(), buildTracker));
         listToReport.AddRange(new MoleculeBuildingBlocksReporter().Report(_buildingBlockRepository.MoleculeBlockCollection.ToList(), buildTracker));
         listToReport.AddRange(_reactionBuildingBlocksReporter.Report(_buildingBlockRepository.ReactionBlockCollection.ToList(), buildTracker));
         listToReport.AddRange(new PassiveTransportBuildingBlocksReporter().Report(_buildingBlockRepository.PassiveTransportCollection.ToList(), buildTracker));
         listToReport.AddRange(new ObserverBuildingBlocksReporter().Report(_buildingBlockRepository.ObserverBlockCollection.ToList(), buildTracker));
         listToReport.AddRange(_simulationSettingsReporter.Report(project.SimulationSettings, buildTracker));
         listToReport.AddRange(new EventGroupBuildingBlocksReporter().Report(_buildingBlockRepository.EventBlockCollection.ToList(), buildTracker));
         listToReport.AddRange(new InitialConditionsBuildingBlocksReporter(_displayUnitRetriever).Report(_buildingBlockRepository.InitialConditionBlockCollection.ToList(), buildTracker));
         listToReport.AddRange(new ParameterValuesBuildingBlocksReporter(_displayUnitRetriever).Report(_buildingBlockRepository.ParametersValueBlockCollection.ToList(), buildTracker));
         listToReport.AddRange(new ObservedDataReporter().Report(project.AllObservedData.ToList(), buildTracker));
         listToReport.AddRange(_simulationsReporter.Report(project.Simulations.ToList(), buildTracker));
         listToReport.AddRange(new ChartsReporter().Report(project.Charts.ToList(), buildTracker));       
         return listToReport;
      }
   }
}