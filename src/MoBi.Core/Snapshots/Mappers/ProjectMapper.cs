using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoBi.Core.Domain.Builder;
using MoBi.Core.Domain.Model;
using MoBi.Core.Serialization.Xml.Services;
using MoBi.Core.Services;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Services;
using OSPSuite.Core.Snapshots;
using OSPSuite.Core.Snapshots.Mappers;
using OSPSuite.Utility.Extensions;
using ModelDataRepository = OSPSuite.Core.Domain.Data.DataRepository;
using ModelParameterIdentification = OSPSuite.Core.Domain.ParameterIdentifications.ParameterIdentification;
using ModelProject = MoBi.Core.Domain.Model.MoBiProject;
using SnapshotProject = MoBi.Core.Snapshots.Project;

namespace MoBi.Core.Snapshots.Mappers;

public class ProjectMapper : ProjectMapper<ModelProject, SnapshotProject, ProjectContext>
{
   private readonly IXmlSerializationService _xmlSerializationService;
   private readonly SimulationMapper _simulationMapper;
   private readonly IPKSimStarter _pkSimStarter;
   private readonly ISimulationSettingsFactory _simulationSettingsFactory;

   public ProjectMapper(IXmlSerializationService xmlSerializationService,
      ICreationMetaDataFactory creationMetaDataFactory,
      IClassificationSnapshotTask classificationSnapshotTask,
      IMoBiContext context,
      IOSPSuiteLogger logger,
      ParameterIdentificationMapper parameterIdentificationMapper,
      SimulationMapper simulationMapper,
      IPKSimStarter pkSimStarter,
      ISimulationSettingsFactory simulationSettingsFactory) : base(creationMetaDataFactory, logger, context, classificationSnapshotTask, parameterIdentificationMapper)
   {
      _xmlSerializationService = xmlSerializationService;
      _simulationMapper = simulationMapper;
      _pkSimStarter = pkSimStarter;
      _simulationSettingsFactory = simulationSettingsFactory;
   }

   public override async Task<ModelProject> MapToModel(SnapshotProject projectSnapshot, ProjectContext context)
   {
      var project = context.MoBiProject();

      project.Name = projectSnapshot.Name;
      project.Description = projectSnapshot.Description;
      project.Creation = _creationMetaDataFactory.Create();
      project.SimulationSettings = _simulationSettingsFactory.CreateDefault();

      project.ReactionDimensionMode = projectSnapshot.ReactionDimensionMode;

      projectSnapshot.PKSimModules?.Each((x, i) =>
      {
         _logger.AddInfo($"Loading PK-Sim module from project snapshot ({i + 1}/{projectSnapshot.PKSimModules.Length})...", projectSnapshot.Name);
         loadProjectContentFromPKSimSnapshot(x, project);
      });

      projectSnapshot.ExtensionModules?.Each(x => project.AddModule(deserializeFromBase64PKML<Module>(x, project)));

      projectSnapshot.ExpressionProfileBuildingBlocks?.Each(x => project.AddExpressionProfileBuildingBlock(deserializeFromBase64PKML<ExpressionProfileBuildingBlock>(x, project)));

      projectSnapshot.IndividualBuildingBlocks?.Each(x => project.AddIndividualBuildingBlock(deserializeFromBase64PKML<IndividualBuildingBlock>(x, project)));

      var snapshotContext = new SnapshotContext(project, SnapshotVersions.FindByMoBiProjectVersion(projectSnapshot.Version));

      var observedData = await ObservedDataFrom(projectSnapshot.ObservedData, snapshotContext);
      observedData?.Each(repository => AddObservedDataToProject(project, repository));

      var parameterIdentifications = await AllParameterIdentificationsFrom(projectSnapshot.ParameterIdentifications, snapshotContext);
      parameterIdentifications?.Each(pi => AddParameterIdentificationToProject(project, pi));

      var simulationContext = new SimulationContext(context.RunSimulations, snapshotContext)
      {
         NumberOfSimulationsToLoad = projectSnapshot.Simulations.Length,
         NumberOfSimulationsLoaded = 0
      };

      foreach (var simulationSnapshot in projectSnapshot.Simulations)
      {
         try
         {
            var simulation = await _simulationMapper.MapToModel(simulationSnapshot, simulationContext);
            addSimulations(project, simulation);
            simulationContext.NumberOfSimulationsLoaded++;
         }
         catch (Exception e)
         {
            _logger.AddException(e);
         }
      }

      await updateProjectClassifications(projectSnapshot, snapshotContext);

      return project;
   }

   private void addSimulations(ModelProject project, MoBiSimulation x)
   {
      AddClassifiableToProject<ClassifiableSimulation, IMoBiSimulation>(project, x, project.AddSimulation, project.Simulations);
   }

   private void loadProjectContentFromPKSimSnapshot(string snapshot, ModelProject project)
   {
      var simulationTransfer = _pkSimStarter.LoadSimulationTransferFromSnapshot(snapshot);

      var simulationConfiguration = simulationTransfer.Simulation.Configuration;

      simulationConfiguration.ModuleConfigurations.Each(x => project.AddModule(x.Module));
      simulationConfiguration.ExpressionProfiles.Each(project.AddExpressionProfileBuildingBlock);
      project.AddIndividualBuildingBlock(simulationConfiguration.Individual);
   }

   private Task updateProjectClassifications(SnapshotProject snapshot, SnapshotContext snapshotContext)
   {
      var project = snapshotContext.MoBiProject();
      var tasks = new[]
      {
         _classificationSnapshotTask.UpdateProjectClassifications<ClassifiableObservedData, ModelDataRepository>(
            snapshot.ObservedDataClassifications, snapshotContext, project.AllObservedData),

         _classificationSnapshotTask.UpdateProjectClassifications<ClassifiableSimulation, IMoBiSimulation>(snapshot.SimulationClassifications,
            snapshotContext, project.Simulations),

         _classificationSnapshotTask.UpdateProjectClassifications<ClassifiableModule, Module>(
            snapshot.ModuleClassifications, snapshotContext, project.Modules),

         _classificationSnapshotTask.UpdateProjectClassifications<ClassifiableParameterIdentification, ModelParameterIdentification>(
            snapshot.ParameterIdentificationClassifications, snapshotContext, project.AllParameterIdentifications),
      };

      return Task.WhenAll(tasks);
   }

   public override async Task<SnapshotProject> MapToSnapshot(ModelProject project)
   {
      var snapshot = await SnapshotFrom(project, x =>
      {
         x.Version = ProjectVersions.Current;
         x.Description = SnapshotValueFor(project.Description);
      });

      snapshot.PKSimModules = mapPKSimModules(project);
      snapshot.ExtensionModules = mapExtensionModules(project);
      snapshot.ExpressionProfileBuildingBlocks = mapExpressionProfilesBuildingBlocks(project);
      snapshot.IndividualBuildingBlocks = mapIndividualBuildingBlocks(project);
      snapshot.ObservedData = await MapObservedDataToSnapshots(project.AllObservedData);
      snapshot.ParameterIdentifications = await MapParameterIdentificationToSnapshots(project.AllParameterIdentifications);

      snapshot.ObservedDataClassifications = await MapClassifications<ClassifiableObservedData>(project);
      snapshot.SimulationClassifications = await MapClassifications<ClassifiableSimulation>(project);
      snapshot.ParameterIdentificationClassifications = await MapClassifications<ClassifiableParameterIdentification>(project);
      snapshot.ModuleClassifications = await MapClassifications<ClassifiableModule>(project);
      snapshot.Simulations = mapSimulations(simulationsFromProject(project), project);
      snapshot.ReactionDimensionMode = project.ReactionDimensionMode;

      return snapshot;
   }

   private static IReadOnlyList<MoBiSimulation> simulationsFromProject(ModelProject project) => project.Simulations.OfType<MoBiSimulation>().ToList();

   private Simulation[] mapSimulations(IReadOnlyList<MoBiSimulation> projectSimulations, ModelProject project) => _simulationMapper.MapToSnapshots(projectSimulations, project).Result;

   private string[] mapPKSimModules(ModelProject project) => project.Modules.Where(shouldUsePKSimSnapshot).Select(x => x.Snapshot).ToArray();
   private string[] mapExtensionModules(ModelProject project) => project.Modules.Where(x => !shouldUsePKSimSnapshot(x)).Select(serializeToBase64PKML).ToArray();
   private string[] mapExpressionProfilesBuildingBlocks(ModelProject project) => project.ExpressionProfileCollection.Where(x => !isFromSnapshot(x, project)).Select(serializeToBase64PKML).ToArray();
   private string[] mapIndividualBuildingBlocks(ModelProject project) => project.IndividualsCollection.Where(x => !isFromSnapshot(x, project)).Select(serializeToBase64PKML).ToArray();

   private static bool isFromSnapshot(ExpressionProfileBuildingBlock expressionProfileBuildingBlock, ModelProject project) => 
      projectHasPKSimModule(expressionProfileBuildingBlock.SnapshotOriginModuleId, project);

   private static bool projectHasPKSimModule(string snapshotOriginModuleId, ModelProject project)
   {
      if (snapshotOriginModuleId == null)
         return false;

      var module = project.Modules.FindById(snapshotOriginModuleId);

      return shouldUsePKSimSnapshot(module);
   }

   private static bool shouldUsePKSimSnapshot(Module module)
   {
      return module.IsPKSimModule && module.HasSnapshot;
   }

   private static bool isFromSnapshot(IndividualBuildingBlock individualBuildingBlock, ModelProject project) => 
      projectHasPKSimModule(individualBuildingBlock.SnapshotOriginModuleId, project);

   private T deserializeFromBase64PKML<T>(string encodedModule, ModelProject project) => _xmlSerializationService.Deserialize<T>(fromBase64String(encodedModule), project);

   private string serializeToBase64PKML<T>(T elementToSerialize) => toBase64String(_xmlSerializationService.SerializeAsString(elementToSerialize));

   private string fromBase64String(string encodedElement) => Encoding.UTF8.GetString(Convert.FromBase64String(encodedElement));

   private static string toBase64String(string serializeAsString) => Convert.ToBase64String(Encoding.UTF8.GetBytes(serializeAsString));
}