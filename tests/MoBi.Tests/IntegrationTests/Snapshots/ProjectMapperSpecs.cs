using System;
using System.Linq;
using FakeItEasy;
using MoBi.Core;
using MoBi.Core.Domain.Builder;
using MoBi.Core.Domain.Model;
using MoBi.Core.Serialization.Xml.Services;
using MoBi.Core.Services;
using MoBi.Core.Snapshots.Mappers;
using MoBi.HelpersForTests;
using OSPSuite.Assets.Extensions;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart.ParameterIdentifications;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Qualification;
using OSPSuite.Core.Services;
using OSPSuite.Core.Snapshots.Mappers;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;
using Classification = OSPSuite.Core.Domain.Classification;
using ICoreUserSettings = MoBi.Core.ICoreUserSettings;
using OutputMapping = OSPSuite.Core.Domain.OutputMapping;
using ParameterIdentification = OSPSuite.Core.Domain.ParameterIdentifications.ParameterIdentification;
using SnapshotProject = MoBi.Core.Snapshots.Project;

namespace MoBi.IntegrationTests.Snapshots
{
   internal class concern_for_ProjectMapper : ContextForIntegration<ProjectMapper>
   {
      protected MoBiProject _project;
      private IXmlSerializationService _xmlSerializationService;
      private ICreationMetaDataFactory _creationMetaDataFactory;
      private IClassificationSnapshotTask _classificationSnapshotTask;
      private Classification _moduleClassification;
      private Classification _observedDataClassification;
      private Classification _simulationClassification;
      private Classification _parameterIdentificationClassification;
      private IMoBiContext _context;
      private IOSPSuiteLogger _ospSuiteLogger;
      private ParameterIdentificationMapper _parameterIdentificationMapper;
      private IPKSimStarter _pkSimStarter;
      private ISimulationSettingsFactory _simulationSettingsFactory;
      private SimulationMapper _simulationMapper;
      private ICoreSimulationRunner _coreSimulationRunner;
      private ICoreUserSettings _coreUserSettings;
      protected IParameterValueUpdateManager _parameterValueUpdateManager;
      protected IndividualBuildingBlock _snapshotIndividualBuildingBlock;
      protected ExpressionProfileBuildingBlock _snapshotExpressionProfile;

      protected override void Context()
      {
         _context = A.Fake<IMoBiContext>();

         _xmlSerializationService = IoC.Resolve<IXmlSerializationService>();
         _creationMetaDataFactory = IoC.Resolve<ICreationMetaDataFactory>();
         _classificationSnapshotTask = IoC.Resolve<IClassificationSnapshotTask>();
         _parameterIdentificationMapper = IoC.Resolve<ParameterIdentificationMapper>();
         _simulationSettingsFactory = IoC.Resolve<ISimulationSettingsFactory>();
         _ospSuiteLogger = A.Fake<IOSPSuiteLogger>();
         _pkSimStarter = A.Fake<IPKSimStarter>();
         _simulationMapper = IoC.Resolve<SimulationMapper>();
         _coreSimulationRunner = A.Fake<ICoreSimulationRunner>();
         _coreUserSettings = A.Fake<ICoreUserSettings>();
         _parameterValueUpdateManager = A.Fake<IParameterValueUpdateManager>();

         A.CallTo(() => _context.Resolve<ISnapshotMapper>()).ReturnsLazily(x => IoC.Resolve<ISnapshotMapper>());

         _project = new MoBiProject();
         sut = new ProjectMapper(_xmlSerializationService, _creationMetaDataFactory, _classificationSnapshotTask, _context, _ospSuiteLogger, _parameterIdentificationMapper, _simulationMapper, _pkSimStarter, _simulationSettingsFactory, _coreSimulationRunner, _coreUserSettings, _parameterValueUpdateManager);

         var module = new Module().WithId("module").WithName("module");
         _project.AddModule(module);

         var pksimModule = new Module { IsPKSimModule = true, Snapshot = "{ \"JSON\":true }".ToBase64String(), Id = "pksimmodule" };
         _project.AddModule(pksimModule);

         _snapshotIndividualBuildingBlock = new IndividualBuildingBlock
         {
            Snapshot = "{ \"JSON\":true }".ToBase64String(),
            Id = "pksimInd"
         };

         _snapshotExpressionProfile = new ExpressionProfileBuildingBlock
         {
            Type = ExpressionTypes.MetabolizingEnzyme,
            Snapshot = "{ \"JSON\":true }".ToBase64String(),
            Id = "pksimexpression"
         };

         var transferModule = new Module { IsPKSimModule = true };

         A.CallTo(() => _pkSimStarter.LoadModuleFromSnapshot(A<string>._)).Returns(transferModule);
         A.CallTo(() => _pkSimStarter.LoadModuleFromSnapshotAndExportInputs(A<string>._, A<QualificationConfiguration>._)).Returns((transferModule, Array.Empty<InputMapping>()));

         var dataRepository = DomainHelperForSpecs.ObservedData();
         _project.AddObservedData(dataRepository);

         var simulation = new MoBiSimulation().WithId("simulation").WithName("simulation");
         simulation.Configuration = new SimulationConfiguration
         {
            SimulationSettings = _simulationSettingsFactory.CreateDefault()
         };
         simulation.Configuration.AddModuleConfiguration(new ModuleConfiguration(pksimModule));
         _project.AddSimulation(simulation);

         var parameterIdentification = new ParameterIdentification();
         var outputMapping = new OutputMapping
         {
            OutputSelection = new SimulationQuantitySelection(simulation, new QuantitySelection("the|path"))
         };
         parameterIdentification.AddSimulation(simulation);
         parameterIdentification.AddOutputMapping(outputMapping);
         parameterIdentification.Configuration.AlgorithmProperties = new OptimizationAlgorithmProperties("toto");

         var parameterIdentificationAnalysis = new ParameterIdentificationTimeProfileChart();
         parameterIdentification.AddAnalysis(parameterIdentificationAnalysis);
         _project.AddParameterIdentification(parameterIdentification);

         var expressionProfileBuildingBlock = new ExpressionProfileBuildingBlock
         {
            Type = ExpressionTypes.MetabolizingEnzyme
         };

         _project.AddExpressionProfileBuildingBlock(expressionProfileBuildingBlock);
         _project.AddExpressionProfileBuildingBlock(_snapshotExpressionProfile);
         _project.AddIndividualBuildingBlock(new IndividualBuildingBlock());
         _project.AddIndividualBuildingBlock(_snapshotIndividualBuildingBlock);

         _moduleClassification = new Classification { ClassificationType = ClassificationType.Module }.WithName("Module Classification");
         _observedDataClassification = new Classification { ClassificationType = ClassificationType.ObservedData }.WithName("Observed Data Classification");
         _simulationClassification = new Classification { ClassificationType = ClassificationType.Simulation }.WithName("Simulation Classification");
         _parameterIdentificationClassification = new Classification { ClassificationType = ClassificationType.ParameterIdentification }.WithName("Parameter Identification Classification");

         _project.AddClassification(_simulationClassification);
         _project.AddClassification(_moduleClassification);
         _project.AddClassification(_observedDataClassification);
         _project.AddClassification(_parameterIdentificationClassification);

         _project.AddClassifiable(new ClassifiableModule { Subject = module, Parent = _moduleClassification });
         _project.AddClassifiable(new ClassifiableObservedData { Subject = dataRepository, Parent = _observedDataClassification });
         _project.AddClassifiable(new ClassifiableSimulation { Subject = simulation, Parent = _simulationClassification });
         _project.AddClassifiable(new ClassifiableParameterIdentification { Subject = parameterIdentification, Parent = _parameterIdentificationClassification });
      }
   }

   internal class When_mapping_snapshot_to_project_and_exporting_inputs : concern_for_ProjectMapper
   {
      private SnapshotProject _snapshot;
      private MoBiProject _result;

      protected override void Context()
      {
         base.Context();
         _snapshot = sut.MapToSnapshot(_project).Result;
      }

      protected override void Because()
      {
         (_result, _) = sut.MapToModelAndExportInputs(_snapshot, new ProjectContext(new MoBiProject(), runSimulations: false), new QualificationConfiguration()).Result;
      }

      [Observation]
      public void the_project_should_contain_extension_modules_snapshots_for_each_extension_module()
      {
         _result.Modules.Count(x => !x.IsPKSimModule).ShouldBeEqualTo(1);
      }

      [Observation]
      public void the_project_should_contain_expression_snapshots_for_each_expression()
      {
         _result.ExpressionProfileCollection.Count.ShouldBeEqualTo(_snapshot.ExpressionProfileBuildingBlocks.Length + _snapshot.ExpressionProfileSnapshots.Length);
      }

      [Observation]
      public void the_project_should_contain_individual_snapshots_for_each_individual()
      {
         _result.IndividualsCollection.Count.ShouldBeEqualTo(_snapshot.IndividualBuildingBlocks.Length + _snapshot.IndividualBuildingBlockSnapshots.Length);
      }

      [Observation]
      public void the_project_should_have_classifications()
      {
         _result.AllClassificationsByType(ClassificationType.Module).Count.ShouldBeEqualTo(1);
         _result.AllClassificationsByType(ClassificationType.ObservedData).Count.ShouldBeEqualTo(1);
         _result.AllClassificationsByType(ClassificationType.Simulation).Count.ShouldBeEqualTo(1);
         _result.AllClassificationsByType(ClassificationType.ParameterIdentification).Count.ShouldBeEqualTo(1);
      }

      [Observation]
      public void the_project_should_have_classifiables()
      {
         _result.AllClassifiablesByType<ClassifiableModule>().Count.ShouldBeEqualTo(1);
         _result.AllClassifiablesByType<ClassifiableObservedData>().Count.ShouldBeEqualTo(1);
         // _result.AllClassifiablesByType<ClassifiableSimulation>().Count.ShouldBeEqualTo(1);
         _result.AllClassifiablesByType<ClassifiableParameterIdentification>().Count.ShouldBeEqualTo(1);
      }

      [Observation]
      public void the_parameter_identification_should_contain_analyses()
      {
         _result.AllParameterIdentifications.FirstOrDefault().Analyses.Count().ShouldBeEqualTo(1);
         (_result.AllParameterIdentifications.FirstOrDefault().Analyses.First() is ParameterIdentificationTimeProfileChart).ShouldBeTrue();
      }
   }

   internal class When_mapping_snapshot_to_project_and_exporting_inputs_with_PKSimModule_on_inputs : concern_for_ProjectMapper
   {
      private SnapshotProject _snapshot;
      private QualificationConfiguration _configuration;
      private Input _inputWithPKSimModule;
      private Input _inputWithoutPKSimModule;
      private Input _inputWithEmptyPKSimModule;

      protected override void Context()
      {
         base.Context();
         _snapshot = sut.MapToSnapshot(_project).Result;

         _inputWithPKSimModule = new Input
         {
            Name = "Amikacin",
            Project = "MoBi_Project_Id",
            PKSimModule = "PKSim Module Name",
            Type = PKSimBuildingBlockType.Compound
         };
         _inputWithoutPKSimModule = new Input
         {
            Name = "OtherCompound",
            Project = "MoBi_Project_Id",
            PKSimModule = null,
            Type = PKSimBuildingBlockType.Compound
         };
         _inputWithEmptyPKSimModule = new Input
         {
            Name = "YetAnother",
            Project = "MoBi_Project_Id",
            PKSimModule = "",
            Type = PKSimBuildingBlockType.Compound
         };
         _configuration = new QualificationConfiguration
         {
            Inputs = new[] { _inputWithPKSimModule, _inputWithoutPKSimModule, _inputWithEmptyPKSimModule }
         };
      }

      protected override void Because()
      {
         sut.MapToModelAndExportInputs(_snapshot, new ProjectContext(new MoBiProject(), runSimulations: false), _configuration).Wait();
      }

      [Observation]
      public void the_input_with_PKSimModule_set_should_have_its_Project_rewritten_to_the_PKSimModule_value()
      {
         _inputWithPKSimModule.Project.ShouldBeEqualTo("PKSim Module Name");
      }

      [Observation]
      public void the_input_with_null_PKSimModule_should_have_its_Project_preserved()
      {
         _inputWithoutPKSimModule.Project.ShouldBeEqualTo("MoBi_Project_Id");
      }

      [Observation]
      public void the_input_with_empty_PKSimModule_should_have_its_Project_preserved()
      {
         _inputWithEmptyPKSimModule.Project.ShouldBeEqualTo("MoBi_Project_Id");
      }
   }

   internal class When_mapping_snapshot_to_project : concern_for_ProjectMapper
   {
      private SnapshotProject _snapshot;
      private MoBiProject _result;

      protected override void Context()
      {
         base.Context();
         _snapshot = sut.MapToSnapshot(_project).Result;
      }

      protected override void Because()
      {
         _result = sut.MapToModel(_snapshot, new ProjectContext(new MoBiProject(), runSimulations: false)).Result;
      }

      [Observation]
      public void the_project_should_contain_extension_modules_snapshots_for_each_extension_module()
      {
         _result.Modules.Count(x => !x.IsPKSimModule).ShouldBeEqualTo(1);
      }

      [Observation]
      public void the_project_should_contain_expression_snapshots_for_each_expression_in_the_project()
      {
         _result.ExpressionProfileCollection.Count.ShouldBeEqualTo(_snapshot.ExpressionProfileSnapshots.Length + _snapshot.ExpressionProfileBuildingBlocks.Length);
      }

      [Observation]
      public void the_project_should_contain_individual_snapshots_for_each_individual_in_the_project()
      {
         _result.IndividualsCollection.Count.ShouldBeEqualTo(_snapshot.IndividualBuildingBlockSnapshots.Length + _snapshot.IndividualBuildingBlocks.Length);
      }

      [Observation]
      public void the_project_should_have_classifications()
      {
         _result.AllClassificationsByType(ClassificationType.Module).Count.ShouldBeEqualTo(1);
         _result.AllClassificationsByType(ClassificationType.ObservedData).Count.ShouldBeEqualTo(1);
         _result.AllClassificationsByType(ClassificationType.Simulation).Count.ShouldBeEqualTo(1);
         _result.AllClassificationsByType(ClassificationType.ParameterIdentification).Count.ShouldBeEqualTo(1);
      }

      [Observation]
      public void the_project_should_have_classifiables()
      {
         _result.AllClassifiablesByType<ClassifiableModule>().Count.ShouldBeEqualTo(1);
         _result.AllClassifiablesByType<ClassifiableObservedData>().Count.ShouldBeEqualTo(1);
         // _result.AllClassifiablesByType<ClassifiableSimulation>().Count.ShouldBeEqualTo(1);
         _result.AllClassifiablesByType<ClassifiableParameterIdentification>().Count.ShouldBeEqualTo(1);
      }

      [Observation]
      public void the_parameter_identification_should_contain_analyses()
      {
         _result.AllParameterIdentifications.FirstOrDefault().Analyses.Count().ShouldBeEqualTo(1);
         (_result.AllParameterIdentifications.FirstOrDefault().Analyses.First() is ParameterIdentificationTimeProfileChart).ShouldBeTrue();
      }
   }

   internal class When_mapping_project_to_snapshot_and_pksim_module_does_not_have_snapshot : concern_for_ProjectMapper
   {
      private SnapshotProject _snapshot;

      protected override void Context()
      {
         base.Context();
         _project.Modules.Where(x => x.IsPKSimModule).Each(x => x.Snapshot = string.Empty);
      }

      protected override void Because()
      {
         _snapshot = sut.MapToSnapshot(_project).Result;
      }

      [Observation]
      public void the_snapshot_should_contain_extension_modules_snapshots_for_each_extension_module_and_pksim_module_without_a_snapshot()
      {
         _snapshot.ExtensionModules.Length.ShouldBeEqualTo(2);
      }

      [Observation]
      public void the_snapshot_should_not_contain_pksim_modules()
      {
         _snapshot.PKSimModules.Length.ShouldBeEqualTo(0);
      }
   }

   internal class When_mapping_project_to_snapshot : concern_for_ProjectMapper
   {
      private SnapshotProject _snapshot;
      private IndividualParameter _individualParameter;
      private ExpressionParameter _expressionParameter;

      protected override void Context()
      {
         base.Context();
         _individualParameter = new IndividualParameter { Value = 1.0, InitialValue = 2.0 };
         _snapshotIndividualBuildingBlock.Add(_individualParameter);
         _expressionParameter = new ExpressionParameter { Value = 3.0, InitialValue = 4.0 };
         _snapshotExpressionProfile.Add(_expressionParameter);
      }

      protected override void Because()
      {
         _snapshot = sut.MapToSnapshot(_project).Result;
      }

      [Observation]
      public void the_snapshot_should_contain_extension_modules_snapshots_for_each_extension_module()
      {
         _snapshot.ExtensionModules.Length.ShouldBeEqualTo(1);
      }

      [Observation]
      public void the_snapshot_should_contain_pksim_modules_snapshots_for_each_pksim_module()
      {
         _snapshot.PKSimModules.Length.ShouldBeEqualTo(1);
      }

      [Observation]
      public void the_snapshot_should_contain_expression_snapshots_for_each_expression_that_is_not_a_snapshot_expression()
      {
         _snapshot.ExpressionProfileBuildingBlocks.Length.ShouldBeEqualTo(1);
      }

      [Observation]
      public void the_snapshot_should_contain_individual_snapshots_for_each_individual_that_is_not_a_snapshot_individual()
      {
         _snapshot.IndividualBuildingBlocks.Length.ShouldBeEqualTo(1);
      }

      [Observation]
      public void the_snapshot_version_should_be_project_latest()
      {
         _snapshot.Version.ShouldBeEqualTo(ProjectVersions.Current);
      }

      [Observation]
      public void there_should_be_classifications_for_each_type()
      {
         _snapshot.ModuleClassifications.Length.ShouldBeEqualTo(1);
         _snapshot.ObservedDataClassifications.Length.ShouldBeEqualTo(1);
         _snapshot.SimulationClassifications.Length.ShouldBeEqualTo(1);
         _snapshot.ParameterIdentificationClassifications.Length.ShouldBeEqualTo(1);
      }

      [Observation]
      public void the_updated_value_manager_should_be_called_to_update_values_in_building_blocks()
      {
         A.CallTo(() => _parameterValueUpdateManager.MapFrom(_individualParameter)).MustHaveHappened();
         A.CallTo(() => _parameterValueUpdateManager.MapFrom(_expressionParameter)).MustHaveHappened();
      }
   }
}