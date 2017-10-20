using System;
using FakeItEasy;
using MoBi.Core;
using MoBi.Core.Services;
using MoBi.Presentation.Tasks;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
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
      protected string _simuationFile = "SimFile.pkml";

      protected override void Context()
      {
         _configuration = A.Fake<IMoBiConfiguration>();
         _startableProcessFactory = A.Fake<IStartableProcessFactory>();
         _applicationSettings = A.Fake<IApplicationSettings>();
         sut = new PKSimStarter(_configuration, _applicationSettings, _startableProcessFactory);
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
         sut.StartPopulationSimulationWithSimulationFile(_simuationFile);
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
         sut.StartPopulationSimulationWithSimulationFile(_simuationFile);
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

      public override void GlobalContext()
      {
         base.GlobalContext();
         _oldFileHelper = FileHelper.FileExists;
         FileHelper.FileExists = s => false;
      }

      [Observation]
      public void should_thrown_an_exception()
      {
         The.Action(() => sut.StartPopulationSimulationWithSimulationFile(_simuationFile)).ShouldThrowAn<OSPSuiteException>();
      }

      public override void GlobalCleanup()
      {
         base.GlobalCleanup();
         FileHelper.FileExists = _oldFileHelper;
      }
   }
}