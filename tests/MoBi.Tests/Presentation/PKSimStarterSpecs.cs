using System;
using FakeItEasy;
using MoBi.Core;
using MoBi.Core.Domain.Model;
using MoBi.Core.Serialization.Xml.Services;
using MoBi.Core.Services;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;
using OSPSuite.Utility;
using OSPSuite.Utility.Exceptions;

namespace MoBi.Presentation
{
   public abstract class concern_for_PKSimStarter : ContextSpecification<IPKSimStarter>
   {
      protected IMoBiConfiguration _configuration;
      protected IStartableProcessFactory _startableProcessFactory;
      protected IApplicationSettings _applicationSettings;
      protected readonly string _simulationFile = "SimFile.pkml";
      protected ICloneManagerForBuildingBlock _cloneManager;
      protected IXmlSerializationService _serializationService;
      protected IMoBiProjectRetriever _projectRetriever;
      protected IPKSimAssemblyLoader _pkSimLoader;

      protected override void Context()
      {
         _configuration = A.Fake<IMoBiConfiguration>();
         _startableProcessFactory = A.Fake<IStartableProcessFactory>();
         _applicationSettings = A.Fake<IApplicationSettings>();
         _cloneManager = A.Fake<ICloneManagerForBuildingBlock>();
         _serializationService = A.Fake<IXmlSerializationService>();
         _projectRetriever = A.Fake<IMoBiProjectRetriever>();
         _pkSimLoader = A.Fake<IPKSimAssemblyLoader>();
         A.CallTo(() => _configuration.PKSimPath).Returns("path");
         sut = new PKSimStarter(_configuration, _applicationSettings, _startableProcessFactory, _cloneManager, _serializationService, _projectRetriever, _pkSimLoader);
      }
   }

   public class When_creating_an_individual_with_PKSim : concern_for_PKSimStarter
   {
      private IndividualBuildingBlock _deserializedBuildingBlock;
      private IndividualBuildingBlock _clonedBuildingBlock;
      private IBuildingBlock _result;

      protected override void Context()
      {
         base.Context();
         var project = new MoBiProject();
         _deserializedBuildingBlock = new IndividualBuildingBlock();
         _clonedBuildingBlock = new IndividualBuildingBlock();
         A.CallTo(() => _projectRetriever.Current).Returns(project);
         A.CallTo(() => _pkSimLoader.ExecuteMethod("PKSim.Starter.IndividualCreator", "CreateIndividual", A<object[]>._)).Returns("<pkml/>");
         A.CallTo(() => _serializationService.Deserialize<IndividualBuildingBlock>("<pkml/>", project)).Returns(_deserializedBuildingBlock);
         A.CallTo(() => _cloneManager.Clone(_deserializedBuildingBlock)).Returns(_clonedBuildingBlock);
      }

      protected override void Because()
      {
         _result = sut.CreateIndividual();
      }

      [Observation]
      public void should_deserialize_the_pkml_returned_by_PKSim_and_return_a_clone_of_the_building_block()
      {
         _result.ShouldBeEqualTo(_clonedBuildingBlock);
      }
   }

   public class When_creating_an_individual_with_PKSim_was_cancelled : concern_for_PKSimStarter
   {
      private IBuildingBlock _result;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _pkSimLoader.ExecuteMethod("PKSim.Starter.IndividualCreator", "CreateIndividual", A<object[]>._)).Returns(null);
      }

      protected override void Because()
      {
         _result = sut.CreateIndividual();
      }

      [Observation]
      public void should_return_null()
      {
         _result.ShouldBeNull();
      }
   }

   public class When_exporting_a_simulation_file_to_PKSim_and_the_application_was_installed_using_the_setup_but_overridden : concern_for_PKSimStarter
   {
      private readonly string _pkSimConfigPath = "PKSimConfigPath";
      private Func<string, bool> _oldFileHelper;
      private readonly string _pkSimUserSettingsPath = "PKSimUserSettingsPath";

      public override void GlobalContext()
      {
         base.GlobalContext();
         _oldFileHelper = FileHelper.FileExists;
         FileHelper.FileExists = s => s == _pkSimConfigPath || s == _pkSimUserSettingsPath;
      }

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _configuration.PKSimPath).Returns(_pkSimConfigPath);
         A.CallTo(() => _applicationSettings.PKSimPath).Returns(_pkSimUserSettingsPath);
      }

      protected override void Because()
      {
         sut.StartPopulationSimulationWithSimulationFile(_simulationFile);
      }

      [Observation]
      public void should_start_override_PKSim_with_the_simulation_file()
      {
         A.CallTo(() => _startableProcessFactory.CreateStartableProcess(_pkSimUserSettingsPath, A<string[]>._)).MustHaveHappened();
      }

      public override void GlobalCleanup()
      {
         base.GlobalCleanup();
         FileHelper.FileExists = _oldFileHelper;
      }
   }

   public class When_exporting_a_simulation_file_to_PKSim_and_the_application_was_installed_using_the_setup : concern_for_PKSimStarter
   {
      private readonly string _pkSimConfigPath = "PKSimConfigPath";
      private Func<string, bool> _oldFileHelper;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _oldFileHelper = FileHelper.FileExists;
         FileHelper.FileExists = s => s == _pkSimConfigPath;
      }

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _configuration.PKSimPath).Returns(_pkSimConfigPath);
      }

      protected override void Because()
      {
         sut.StartPopulationSimulationWithSimulationFile(_simulationFile);
      }

      [Observation]
      public void should_start_PKSim_with_the_simulation_file()
      {
         A.CallTo(() => _startableProcessFactory.CreateStartableProcess(_pkSimConfigPath, A<string[]>._)).MustHaveHappened();
      }

      public override void GlobalCleanup()
      {
         base.GlobalCleanup();
         FileHelper.FileExists = _oldFileHelper;
      }
   }

   public class When_exporting_a_simulation_file_to_PKSim_and_the_application_was_installed_using_a_portable_setup_and_mobi_executable_path_can_be_found_on_system_using_the_application_settings : concern_for_PKSimStarter
   {
      private readonly string _pkSimUserSettingsPath = "PKSimUserSettingsPath";
      private Func<string, bool> _oldFileHelper;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _oldFileHelper = FileHelper.FileExists;
         FileHelper.FileExists = s => s == _pkSimUserSettingsPath;
      }

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _applicationSettings.PKSimPath).Returns(_pkSimUserSettingsPath);
      }

      protected override void Because()
      {
         sut.StartPopulationSimulationWithSimulationFile(_simulationFile);
      }

      [Observation]
      public void should_start_mobi_with_the_simulation_file()
      {
         A.CallTo(() => _startableProcessFactory.CreateStartableProcess(_pkSimUserSettingsPath, A<string[]>._)).MustHaveHappened();
      }

      public override void GlobalCleanup()
      {
         base.GlobalCleanup();
         FileHelper.FileExists = _oldFileHelper;
      }
   }

   public class When_exporting_a_simulation_file_to_PKSim_and_pksim_is_not_found_on_the_system : concern_for_PKSimStarter
   {
      private Func<string, bool> _oldFileHelper;
      private readonly string _pkSimUserSettingsPath = "PKSimUserSettingsPath";
      
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _configuration.PKSimPath).Returns(_pkSimUserSettingsPath);
         _oldFileHelper = FileHelper.FileExists;
         FileHelper.FileExists = s => false;
      }

      [Observation]
      public void should_thrown_an_exception()
      {
         The.Action(() => sut.StartPopulationSimulationWithSimulationFile(_simulationFile)).ShouldThrowAn<OSPSuiteException>();
      }

      public override void Cleanup()
      {
         base.GlobalCleanup();
         FileHelper.FileExists = _oldFileHelper;
      }
   }
}