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
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart.ParameterIdentifications;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Qualification;
using OSPSuite.Core.Serialization.Exchange;
using OSPSuite.Core.Services;
using OSPSuite.Core.Snapshots.Mappers;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;
using Classification = OSPSuite.Core.Domain.Classification;
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
      protected SimulationTransfer _simulationTransfer;
      private SimulationMapper _simulationMapper;

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

         A.CallTo(() => _context.Resolve<ISnapshotMapper>()).ReturnsLazily(x => IoC.Resolve<ISnapshotMapper>());


         _project = new MoBiProject();
         sut = new ProjectMapper(_xmlSerializationService, _creationMetaDataFactory, _classificationSnapshotTask, _context, _ospSuiteLogger, _parameterIdentificationMapper, _simulationMapper, _pkSimStarter, _simulationSettingsFactory);

         var module = new Module().WithId("module").WithName("module");
         _project.AddModule(module);

         var pksimModule = new Module { IsPKSimModule = true, Snapshot = "snapshot", Id = "pksimmodule" };
         _project.AddModule(pksimModule);

         var snapshotIndividualBuildingBlock = new IndividualBuildingBlock
         {
            SnapshotOriginModuleId = pksimModule.Id,
            Id = "pksimInd"
         };

         var snapshotExpressionProfile = new ExpressionProfileBuildingBlock
         {
            Type = ExpressionTypes.MetabolizingEnzyme,
            SnapshotOriginModuleId = pksimModule.Id,
            Id = "pksimexpression"
         };

         _simulationTransfer = new SimulationTransfer();
         var moduleConfiguration = new ModuleConfiguration(new Module { IsPKSimModule = true }, new InitialConditionsBuildingBlock(), new ParameterValuesBuildingBlock());
         _simulationTransfer.Simulation = new MoBiSimulation
         {
            Configuration = new SimulationConfiguration
            {
               Individual = snapshotIndividualBuildingBlock
            }
         };

         _simulationTransfer.Simulation.Configuration.AddExpressionProfile(snapshotExpressionProfile);
         _simulationTransfer.Simulation.Configuration.AddModuleConfiguration(moduleConfiguration);

         A.CallTo(() => _pkSimStarter.LoadSimulationTransferFromSnapshot(pksimModule.Snapshot)).Returns(_simulationTransfer);
         A.CallTo(() => _pkSimStarter.LoadSimulationTransferFromSnapshotAndExportInputs(pksimModule.Snapshot, A<QualificationConfiguration>._)).Returns((_simulationTransfer, Array.Empty<InputMapping>()));

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
         _project.AddExpressionProfileBuildingBlock(snapshotExpressionProfile);
         _project.AddIndividualBuildingBlock(new IndividualBuildingBlock());
         _project.AddIndividualBuildingBlock(snapshotIndividualBuildingBlock);

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
         _result.ExpressionProfileCollection.Count.ShouldBeEqualTo(_snapshot.ExpressionProfileBuildingBlocks.Length + _simulationTransfer.Simulation.Configuration.ExpressionProfiles.Count);
      }

      [Observation]
      public void the_project_should_contain_individual_snapshots_for_each_individual()
      {
         _result.IndividualsCollection.Count.ShouldBeEqualTo(_snapshot.IndividualBuildingBlocks.Length + (_simulationTransfer.Simulation.Configuration.Individual == null ? 0 : 1));
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
      public void the_project_should_contain_expression_snapshots_for_each_expression()
      {
         _result.ExpressionProfileCollection.Count.ShouldBeEqualTo(_snapshot.ExpressionProfileBuildingBlocks.Length + _simulationTransfer.Simulation.Configuration.ExpressionProfiles.Count);
      }

      [Observation]
      public void the_project_should_contain_individual_snapshots_for_each_individual()
      {
         _result.IndividualsCollection.Count.ShouldBeEqualTo(_snapshot.IndividualBuildingBlocks.Length + (_simulationTransfer.Simulation.Configuration.Individual == null ? 0 : 1));
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
   }
}