using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoBi.Core.Domain.Builder;
using MoBi.Core.Domain.Model;
using MoBi.Core.Serialization.Xml.Services;
using MoBi.Core.Services;
using OSPSuite.Assets.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Qualification;
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

   /// <summary>
   ///    Loads the project from the snapshot. If there are any PK-Sim modules, they are rebuilt through a local installation
   ///    of PK-Sim.
   ///    If any of the PK-Sim building blocks should have markdown exported it will be done during the module rebuild.
   /// </summary>
   /// <param name="snapshot">The MoBi project snapshot</param>
   /// <param name="projectContext">Context for the project load from snapshot</param>
   /// <param name="qualificationConfiguration">
   ///    The configuration containing any building block inputs that should have report
   ///    markdown exported while the module is rebuilt in PK-Sim
   /// </param>
   /// <returns>The project that was created and any input mappings that were exported</returns>
   public async Task<(ModelProject, InputMapping[])> MapToModelAndExportInputs(SnapshotProject snapshot, ProjectContext projectContext, QualificationConfiguration qualificationConfiguration)
   {
      InputMapping[] inputMappings = [];

      return (await mapToModel(snapshot, projectContext, (module, project) => inputMappings = loadModulesAndExportInputsFromPKSimSnapshot(module, project, qualificationConfiguration)), inputMappings);
   }

   private async Task<ModelProject> mapToModel(SnapshotProject projectSnapshot, ProjectContext context, Action<string, ModelProject> rebuildAction)
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
         rebuildAction(x, project);
      });

      projectSnapshot.ExtensionModules?.Each(x => project.AddModule(deserializeFromBase64PKML<Module>(x, project)));

      projectSnapshot.ExpressionProfileBuildingBlocks?.Each(x => project.AddExpressionProfileBuildingBlock(deserializeFromBase64PKML<ExpressionProfileBuildingBlock>(x, project)));

      projectSnapshot.IndividualBuildingBlocks?.Each(x => project.AddIndividualBuildingBlock(deserializeFromBase64PKML<IndividualBuildingBlock>(x, project)));

      projectSnapshot.IndividualBuildingBlockSnapshots?.Each(x => project.AddIndividualBuildingBlock(_pkSimStarter.LoadIndividualFromSnapshot(x)));

      projectSnapshot.ExpressionProfileSnapshots?.Each(x => project.AddExpressionProfileBuildingBlock(_pkSimStarter.LoadExpressionProfileFromSnapshot(x)));

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

   public override async Task<ModelProject> MapToModel(SnapshotProject projectSnapshot, ProjectContext context)
   {
      return await mapToModel(projectSnapshot, context, loadModulesFromPKSimSnapshot);
   }

   private void addSimulations(ModelProject project, MoBiSimulation x)
   {
      AddClassifiableToProject<ClassifiableSimulation, IMoBiSimulation>(project, x, project.AddSimulation, project.Simulations);
   }

   private void loadModulesFromPKSimSnapshot(string snapshot, ModelProject project)
   {
      var module = _pkSimStarter.LoadModuleFromSnapshot(snapshot);

      loadModuleToProject(project, module);
   }

   private static void loadModuleToProject(ModelProject project, Module module)
   {
      project.AddModule(module);
   }

   private InputMapping[] loadModulesAndExportInputsFromPKSimSnapshot(string snapshot, ModelProject project, QualificationConfiguration config)
   {
      var (module, inputMappings) = _pkSimStarter.LoadModuleFromSnapshotAndExportInputs(snapshot, config);

      loadModuleToProject(project, module);

      return inputMappings;
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
      snapshot.ExpressionProfileSnapshots = mapExpressionProfileSnapshots(project);
      snapshot.IndividualBuildingBlockSnapshots = mapIndividualSnapshots(project);
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

   private string[] mapExpressionProfilesBuildingBlocks(ModelProject project) => project.ExpressionProfileCollection.Where(x => !x.HasSnapshot).Select(serializeToBase64PKML).ToArray();
   private string[] mapIndividualBuildingBlocks(ModelProject project) => project.IndividualsCollection.Where(x => !x.HasSnapshot).Select(serializeToBase64PKML).ToArray();

   // TODO snapshot needs to be augmented with the changed parameters after https://github.com/Open-Systems-Pharmacology/MoBi/issues/1560
   private string[] mapExpressionProfileSnapshots(ModelProject project) => project.ExpressionProfileCollection.Where(x => x.HasSnapshot).Select(x => x.Snapshot).ToArray();
   private string[] mapIndividualSnapshots(ModelProject project) => project.IndividualsCollection.Where(x => x.HasSnapshot).Select(x => x.Snapshot).ToArray();

   private static bool shouldUsePKSimSnapshot(Module module)
   {
      return module.IsPKSimModule && module.HasSnapshot;
   }

   private T deserializeFromBase64PKML<T>(string encodedModule, ModelProject project) => _xmlSerializationService.Deserialize<T>(encodedModule.FromBase64String(), project);

   private string serializeToBase64PKML<T>(T elementToSerialize) => _xmlSerializationService.SerializeAsString(elementToSerialize).ToBase64String();
}