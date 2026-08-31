using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
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
      protected List<ListItemDTO<ParameterValuesBuildingBlock>> _boundParameterValues;
      protected ParameterValuesBuildingBlock _boundSelectedParameterValues;
      protected List<ListItemDTO<InitialConditionsBuildingBlock>> _boundInitialConditions;
      protected InitialConditionsBuildingBlock _boundSelectedInitialConditions;

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

         A.CallTo(() => _view.BindParameterValues(A<IEnumerable<ListItemDTO<ParameterValuesBuildingBlock>>>._, A<ParameterValuesBuildingBlock>._))
            .Invokes((IEnumerable<ListItemDTO<ParameterValuesBuildingBlock>> parameterValues, ParameterValuesBuildingBlock selected) =>
            {
               _boundParameterValues = parameterValues.ToList();
               _boundSelectedParameterValues = selected;
            });

         A.CallTo(() => _view.BindInitialConditions(A<IEnumerable<ListItemDTO<InitialConditionsBuildingBlock>>>._, A<InitialConditionsBuildingBlock>._))
            .Invokes((IEnumerable<ListItemDTO<InitialConditionsBuildingBlock>> initialConditions, InitialConditionsBuildingBlock selected) =>
            {
               _boundInitialConditions = initialConditions.ToList();
               _boundSelectedInitialConditions = selected;
            });

         sut = new SelectCommitTargetPresenter(_view, _templateResolverTask, new ItemToListItemMapper<Module>(), new ItemToListItemMapper<ParameterValuesBuildingBlock>(), new ItemToListItemMapper<InitialConditionsBuildingBlock>());
      }
   }

   public class When_selecting_the_commit_target_and_the_user_accepts_the_defaults : concern_for_SelectCommitTargetPresenter
   {
      private CommitTarget _commitTarget;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _view.Canceled).Returns(false);
         A.CallTo(() => _view.SelectedModule).Returns(_lastModule);
         A.CallTo(() => _view.SelectedParameterValues).ReturnsLazily(() => _boundSelectedParameterValues);
         A.CallTo(() => _view.SelectedInitialConditions).ReturnsLazily(() => _boundSelectedInitialConditions);
      }

      protected override void Because()
      {
         _commitTarget = sut.SelectCommitTargetFor(_simulation);
      }

      [Observation]
      public void the_modules_of_the_simulation_are_bound_with_the_last_module_preselected()
      {
         A.CallTo(() => _view.BindModules(A<IEnumerable<ListItemDTO<Module>>>.That.Matches(x => x.Select(item => item.Item).SequenceEqual(new[] { _firstModule, _lastModule })), _lastModule)).MustHaveHappened();
      }

      [Observation]
      public void the_template_building_blocks_and_a_create_new_entry_are_bound_for_each_type()
      {
         _boundParameterValues.Count.ShouldBeEqualTo(3);
         _boundParameterValues[0].Item.ShouldBeEqualTo(_templateOfSelectedParameterValues);
         _boundParameterValues[1].Item.ShouldBeEqualTo(_otherParameterValues);

         _boundInitialConditions.Count.ShouldBeEqualTo(2);
         _boundInitialConditions[0].Item.ShouldBeEqualTo(_templateOfSelectedInitialConditions);
      }

      [Observation]
      public void the_templates_of_the_used_building_blocks_are_preselected()
      {
         _boundSelectedParameterValues.ShouldBeEqualTo(_templateOfSelectedParameterValues);
         _boundSelectedInitialConditions.ShouldBeEqualTo(_templateOfSelectedInitialConditions);
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

   public class When_selecting_the_commit_target_and_the_user_selects_the_create_new_entry : concern_for_SelectCommitTargetPresenter
   {
      private CommitTarget _commitTarget;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _view.SelectedModule).Returns(_lastModule);
         A.CallTo(() => _view.SelectedParameterValues).ReturnsLazily(() => _boundParameterValues.Last().Item);
         A.CallTo(() => _view.SelectedInitialConditions).ReturnsLazily(() => _boundInitialConditions.Last().Item);
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
         A.CallTo(() => _view.SelectedModule).Returns(_lastModule);
         sut.SelectCommitTargetFor(_simulation);
         A.CallTo(() => _view.SelectedModule).Returns(_firstModule);
      }

      protected override void Because()
      {
         sut.ModuleChanged();
      }

      [Observation]
      public void only_the_create_new_entries_are_bound_and_preselected()
      {
         _boundParameterValues.Count.ShouldBeEqualTo(1);
         _boundSelectedParameterValues.ShouldBeEqualTo(_boundParameterValues.Single().Item);

         _boundInitialConditions.Count.ShouldBeEqualTo(1);
         _boundSelectedInitialConditions.ShouldBeEqualTo(_boundInitialConditions.Single().Item);
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
