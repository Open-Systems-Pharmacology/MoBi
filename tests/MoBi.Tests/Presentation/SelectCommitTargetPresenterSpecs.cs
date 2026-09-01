using System.Linq;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation
{
   public class concern_for_SelectCommitTargetPresenter : ContextSpecification<SelectCommitTargetPresenter>
   {
      protected ISelectCommitTargetView _view;
      protected ITemplateResolverTask _templateResolverTask;
      protected MoBiSimulation _simulation;
      protected Module _firstModule;
      protected Module _lastModule;
      protected Module _templateOfFirstModule;
      protected Module _templateOfLastModule;
      protected ModuleConfiguration _firstModuleConfiguration;
      protected ModuleConfiguration _lastModuleConfiguration;
      protected ParameterValuesBuildingBlock _selectedParameterValues;
      protected ParameterValuesBuildingBlock _templateOfSelectedParameterValues;
      protected ParameterValuesBuildingBlock _otherParameterValues;
      protected InitialConditionsBuildingBlock _selectedInitialConditions;
      protected InitialConditionsBuildingBlock _templateOfSelectedInitialConditions;
      protected CommitTargetDTO _boundDTO;

      protected override void Context()
      {
         _view = A.Fake<ISelectCommitTargetView>();
         _templateResolverTask = A.Fake<ITemplateResolverTask>();

         _firstModule = new Module().WithName("first");
         _lastModule = new Module().WithName("last");
         _firstModuleConfiguration = new ModuleConfiguration(_firstModule);
         _selectedParameterValues = new ParameterValuesBuildingBlock().WithName("selected");
         _selectedInitialConditions = new InitialConditionsBuildingBlock().WithName("selected initial conditions");
         _lastModule.Add(_selectedParameterValues);
         _lastModule.Add(_selectedInitialConditions);
         _lastModuleConfiguration = new ModuleConfiguration(_lastModule)
         {
            SelectedParameterValues = _selectedParameterValues,
            SelectedInitialConditions = _selectedInitialConditions
         };

         _simulation = new MoBiSimulation { Configuration = new SimulationConfiguration() };
         _simulation.Configuration.AddModuleConfiguration(_firstModuleConfiguration);
         _simulation.Configuration.AddModuleConfiguration(_lastModuleConfiguration);

         _templateOfFirstModule = new Module().WithName("first");
         _templateOfSelectedParameterValues = new ParameterValuesBuildingBlock().WithName("selected");
         _otherParameterValues = new ParameterValuesBuildingBlock().WithName("other");
         _templateOfSelectedInitialConditions = new InitialConditionsBuildingBlock().WithName("selected initial conditions");
         _templateOfLastModule = new Module
         {
            _templateOfSelectedParameterValues,
            _otherParameterValues,
            _templateOfSelectedInitialConditions
         }.WithName("last");

         A.CallTo(() => _templateResolverTask.TemplateModuleFor(_firstModule)).Returns(_templateOfFirstModule);
         A.CallTo(() => _templateResolverTask.TemplateModuleFor(_lastModule)).Returns(_templateOfLastModule);
         A.CallTo(() => _templateResolverTask.TemplateBuildingBlockFor(_selectedParameterValues)).Returns(_templateOfSelectedParameterValues);
         A.CallTo(() => _templateResolverTask.TemplateBuildingBlockFor(_selectedInitialConditions)).Returns(_templateOfSelectedInitialConditions);

         A.CallTo(() => _view.BindTo(A<CommitTargetDTO>._)).Invokes((CommitTargetDTO dto) => _boundDTO = dto);

         sut = new SelectCommitTargetPresenter(_view, _templateResolverTask);
      }
   }

   public class When_selecting_the_commit_target_and_the_user_accepts_the_defaults : concern_for_SelectCommitTargetPresenter
   {
      private CommitTarget _commitTarget;

      protected override void Because()
      {
         _commitTarget = sut.SelectCommitTargetFor(_simulation);
      }

      [Observation]
      public void the_modules_of_the_simulation_are_available_for_selection()
      {
         sut.AllModules.ShouldOnlyContainInOrder(_firstModule, _lastModule);
      }

      [Observation]
      public void the_template_building_blocks_and_a_create_new_entry_are_available_for_each_type()
      {
         var allParameterValues = sut.AllParameterValuesFor(_lastModule);
         allParameterValues.Count.ShouldBeEqualTo(3);
         allParameterValues[0].ShouldBeEqualTo(_templateOfSelectedParameterValues);
         allParameterValues[1].ShouldBeEqualTo(_otherParameterValues);

         var allInitialConditions = sut.AllInitialConditionsFor(_lastModule);
         allInitialConditions.Count.ShouldBeEqualTo(2);
         allInitialConditions[0].ShouldBeEqualTo(_templateOfSelectedInitialConditions);
      }

      [Observation]
      public void the_last_module_and_the_templates_of_the_used_building_blocks_are_preselected()
      {
         _boundDTO.Module.ShouldBeEqualTo(_lastModule);
         _boundDTO.ParameterValues.ShouldBeEqualTo(_templateOfSelectedParameterValues);
         _boundDTO.InitialConditions.ShouldBeEqualTo(_templateOfSelectedInitialConditions);
      }

      [Observation]
      public void the_commit_target_is_the_last_module_and_the_templates_of_the_used_building_blocks()
      {
         _commitTarget.ModuleConfiguration.ShouldBeEqualTo(_lastModuleConfiguration);
         _commitTarget.ParameterValues.ShouldBeEqualTo(_templateOfSelectedParameterValues);
         _commitTarget.CreateNewParameterValues.ShouldBeFalse();
         _commitTarget.InitialConditions.ShouldBeEqualTo(_templateOfSelectedInitialConditions);
         _commitTarget.CreateNewInitialConditions.ShouldBeFalse();
      }
   }

   public class When_selecting_the_commit_target_and_the_user_selects_the_create_new_entries : concern_for_SelectCommitTargetPresenter
   {
      private CommitTarget _commitTarget;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _view.Display()).Invokes(() =>
         {
            _boundDTO.ParameterValues = sut.AllParameterValuesFor(_lastModule).Last();
            _boundDTO.InitialConditions = sut.AllInitialConditionsFor(_lastModule).Last();
         });
      }

      protected override void Because()
      {
         _commitTarget = sut.SelectCommitTargetFor(_simulation);
      }

      [Observation]
      public void the_commit_target_creates_a_new_building_block_for_each_type()
      {
         _commitTarget.ModuleConfiguration.ShouldBeEqualTo(_lastModuleConfiguration);
         _commitTarget.ParameterValues.ShouldBeNull();
         _commitTarget.CreateNewParameterValues.ShouldBeTrue();
         _commitTarget.InitialConditions.ShouldBeNull();
         _commitTarget.CreateNewInitialConditions.ShouldBeTrue();
      }
   }

   public class When_the_user_switches_to_a_module_without_selected_building_blocks : concern_for_SelectCommitTargetPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.SelectCommitTargetFor(_simulation);
      }

      protected override void Because()
      {
         _boundDTO.Module = _firstModule;
         sut.ModuleChanged();
      }

      [Observation]
      public void the_create_new_entries_are_preselected()
      {
         _boundDTO.ParameterValues.ShouldBeEqualTo(sut.AllParameterValuesFor(_firstModule).Single());
         _boundDTO.InitialConditions.ShouldBeEqualTo(sut.AllInitialConditionsFor(_firstModule).Single());
      }

      [Observation]
      public void the_view_is_bound_again_to_refresh_the_building_block_lists()
      {
         A.CallTo(() => _view.BindTo(_boundDTO)).MustHaveHappenedTwiceExactly();
      }
   }

   public class When_the_template_of_the_used_building_block_cannot_be_resolved : concern_for_SelectCommitTargetPresenter
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _templateResolverTask.TemplateBuildingBlockFor(_selectedParameterValues)).Returns(null);
      }

      protected override void Because()
      {
         sut.SelectCommitTargetFor(_simulation);
      }

      [Observation]
      public void the_create_new_entry_is_preselected_so_that_the_selection_is_never_empty()
      {
         _boundDTO.ParameterValues.ShouldBeEqualTo(sut.AllParameterValuesFor(_lastModule).Last());
      }
   }

   public class When_selecting_the_commit_target_and_the_user_cancels : concern_for_SelectCommitTargetPresenter
   {
      private CommitTarget _commitTarget;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _view.Canceled).Returns(true);
      }

      protected override void Because()
      {
         _commitTarget = sut.SelectCommitTargetFor(_simulation);
      }

      [Observation]
      public void no_commit_target_is_returned()
      {
         _commitTarget.ShouldBeNull();
      }
   }
}
