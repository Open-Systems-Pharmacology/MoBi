using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility;

namespace MoBi.Presentation
{
   public abstract class concern_for_StartOptions : ContextSpecification<StartOptions>
   {
      protected IList<string> _args;
      private Func<string, bool> _oldFileExists;
      protected string _fileThatExists;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _fileThatExists = "c:\\i_am_alive.txt";
         _oldFileExists = FileHelper.FileExists;
         FileHelper.FileExists = x => string.Equals(x, _fileThatExists);
         sut = new StartOptions();  
      }

      protected override void Context()
      {
         _args = new List<string>();
      }

      protected override void Because()
      {
         sut.InitializeFrom(_args.ToArray());
      }

      public override void GlobalCleanup()
      {
         base.GlobalCleanup();
         FileHelper.FileExists = _oldFileExists;
      }
   }


   public class When_initializing_the_start_options_with_the_required_arguments_to_open_an_existing_simulation_file : concern_for_StartOptions
   {
      protected override void Context()
      {
         base.Context();
         _args.Add("/Simulation");
         _args.Add(_fileThatExists);
      }

      [Observation]
      public void shoud_return_a_valid_state()
      {
         sut.IsValid().ShouldBeTrue();
      }

      [Observation]
      public void should_have_set_the_option_mode_to_simulation()
      {
         sut.StartOptionsMode.ShouldBeEqualTo(StartOptionsMode.Simulation);
      }
   }


   public class When_initializing_the_start_options_with_the_required_arguments_to_open_a_simulation_file_that_does_not_exist : concern_for_StartOptions
   {
      protected override void Context()
      {
         base.Context();
         _args.Add("/Simulation");
         _args.Add("C:\\does_not_exist.xml");
      }

      [Observation]
      public void should_return_an_invalid_state()
      {
         sut.IsValid().ShouldBeFalse();
      }
   }


   public class When_initializing_the_start_options_with_the_arguments_to_open_an_existing_project_file_from_shell : concern_for_StartOptions
   {
      protected override void Context()
      {
         base.Context();
         _args.Add(_fileThatExists);
      }

      [Observation]
      public void shoud_return_a_valid_state()
      {
         sut.IsValid().ShouldBeTrue();
      }

      [Observation]
      public void should_have_set_the_option_mode_to_project()
      {
         sut.StartOptionsMode.ShouldBeEqualTo(StartOptionsMode.Project);
      }

      [Observation]
      public void should_not_start_the_app_in_developer_mode()
      {
         sut.IsDeveloperMode.ShouldBeFalse();
      }
   }


   public class When_initializing_the_start_options_with_the_required_arguments_to_open_an_existing_project_file : concern_for_StartOptions
   {
      protected override void Context()
      {
         base.Context();
         _args.Add("/p");
         _args.Add(_fileThatExists);
         _args.Add("/dev");
      }

      [Observation]
      public void shoud_return_a_valid_state()
      {
         sut.IsValid().ShouldBeTrue();
      }

      [Observation]
      public void should_have_set_the_option_mode_to_project()
      {
         sut.StartOptionsMode.ShouldBeEqualTo(StartOptionsMode.Project);
      }

      [Observation]
      public void should_start_the_app_in_developer_mode()
      {
         sut.IsDeveloperMode.ShouldBeTrue();
      }

   }

   public class When_initializing_the_start_options_with_the_required_arguments_to_open_an_existing_journal_file : concern_for_StartOptions
   {
      protected override void Context()
      {
         base.Context();
         _fileThatExists = "c:\\i_am_alive.sbj";
         FileHelper.FileExists = x => string.Equals(x, _fileThatExists);

         _args.Add("/j");
         _args.Add(_fileThatExists);
      }

      [Observation]
      public void shoud_return_a_valid_state()
      {
         sut.IsValid().ShouldBeTrue();
      }

      [Observation]
      public void should_have_set_the_option_mode_to_journal()
      {
         sut.StartOptionsMode.ShouldBeEqualTo(StartOptionsMode.Journal);
      }
   }

   public class When_initializing_the_start_options_with_no_argument_to_open_an_existing_journal_file : concern_for_StartOptions
   {
      protected override void Context()
      {
         base.Context();
         _fileThatExists = "c:\\i_am_alive.sbj";
         FileHelper.FileExists = x => string.Equals(x, _fileThatExists);

         _args.Add(_fileThatExists);
      }

      [Observation]
      public void shoud_return_a_valid_state()
      {
         sut.IsValid().ShouldBeTrue();
      }

      [Observation]
      public void should_have_set_the_option_mode_to_journal()
      {
         sut.StartOptionsMode.ShouldBeEqualTo(StartOptionsMode.Journal);
      }
   }
}