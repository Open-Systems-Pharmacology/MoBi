using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using FluentNHibernate.Utils;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Nodes;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Repositories;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Presentation.Services;
using ITreeNodeFactory = MoBi.Presentation.Nodes.ITreeNodeFactory;
using TreeNodeFactory = MoBi.Presentation.Nodes.TreeNodeFactory;

namespace MoBi.Presentation
{
   public class concern_for_EditModuleConfigurationsPresenter : ContextSpecification<EditModuleConfigurationsPresenter>
   {
      protected IModuleConfigurationToModuleConfigurationDTOMapper _moduleConfigurationMapper;
      protected IMoBiProjectRetriever _projectRetriever;
      protected ITreeNodeFactory _treeNodeFactory;
      protected IEditModuleConfigurationsView _view;
      protected MoBiProject _moBiProject;
      private IToolTipPartCreator _tooltipCreator;
      private IObservedDataRepository _observedDataRepository;

      protected override void Context()
      {
         _view = A.Fake<IEditModuleConfigurationsView>();
         _tooltipCreator = A.Fake<IToolTipPartCreator>();
         _observedDataRepository = A.Fake<IObservedDataRepository>();
         _treeNodeFactory = new TreeNodeFactory(_observedDataRepository, _tooltipCreator);
         _projectRetriever = A.Fake<IMoBiProjectRetriever>();
         _moduleConfigurationMapper = new ModuleConfigurationToModuleConfigurationDTOMapper(_projectRetriever);

         _moBiProject = new MoBiProject();
         A.CallTo(() => _projectRetriever.Current).Returns(_moBiProject);
         sut = new EditModuleConfigurationsPresenter(_view, _treeNodeFactory, _projectRetriever, _moduleConfigurationMapper);
      }
   }

   public class When_moving_nodes_in_the_view : concern_for_EditModuleConfigurationsPresenter
   {
      private ITreeNode _treeNode2;
      private ITreeNode _treeNode1;
      private SimulationConfiguration _simulationConfiguration;
      private Module _usedModule;
      private Module _projectModule;
      private Module _projectModule2;
      private Module _usedModule2;

      protected override void Context()
      {
         base.Context();
         _simulationConfiguration = new SimulationConfiguration();
         _usedModule = new Module().WithName("usedModule");
         _projectModule = new Module().WithName("usedModule");
         _usedModule2 = new Module().WithName("usedModule2");
         _projectModule2 = new Module().WithName("usedModule2");
         var moduleConfiguration = new ModuleConfiguration(_usedModule);
         var moduleConfiguration2 = new ModuleConfiguration(_usedModule2);
         _simulationConfiguration.AddModuleConfiguration(moduleConfiguration);
         _simulationConfiguration.AddModuleConfiguration(moduleConfiguration2);
         _moBiProject.AddModule(_projectModule);
         _moBiProject.AddModule(_projectModule2);

         sut.Edit(_simulationConfiguration);
         _treeNode1 = _treeNodeFactory.CreateFor(sut.ModuleConfigurationDTOs[0]);
         _treeNode2 = _treeNodeFactory.CreateFor(sut.ModuleConfigurationDTOs[1]);
      }

      protected override void Because()
      {
         sut.MoveNode(_treeNode2, _treeNode1);
      }

      [Observation]
      public void the_order_of_module_configurations_should_be_update()
      {
         sut.ModuleConfigurationDTOs.ElementAt(0).ShouldBeEqualTo(_treeNode2.TagAsObject);
         sut.ModuleConfigurationDTOs.ElementAt(1).ShouldBeEqualTo(_treeNode1.TagAsObject);
      }
   }

   public abstract class When_selecting_different_nodes_in_the_configuration_tree : concern_for_EditModuleConfigurationsPresenter
   {
      protected SimulationConfiguration _simulationConfiguration;
      protected List<ITreeNode> _moduleConfigurationNodes;
      protected Module _projectModule;
      protected Module _projectModule2;
      protected Module _projectModule3;

      protected override void Context()
      {
         base.Context();
         _simulationConfiguration = new SimulationConfiguration();
         _projectModule = new Module().WithName("module1");
         _projectModule2 = new Module().WithName("module2");
         _projectModule3 = new Module().WithName("module3");
         _moBiProject.AddModule(_projectModule);
         _moBiProject.AddModule(_projectModule2);
         _moBiProject.AddModule(_projectModule3);
         sut.Edit(_simulationConfiguration);
         _view.EnableRemove = true;
         _view.EnableUp = true;
         _view.EnableDown = true;
         _moduleConfigurationNodes = new List<ITreeNode>();
         A.CallTo(() => _view.AddModuleConfigurationNode(A<ITreeNode>._)).Invokes(x => _moduleConfigurationNodes.Add(x.GetArgument<ITreeNode>(0)));
         sut.AddModuleConfiguration(new ModuleNode(_projectModule));
         sut.AddModuleConfiguration(new ModuleNode(_projectModule2));
         sut.AddModuleConfiguration(new ModuleNode(_projectModule3));
      }
   }

   public class When_moving_a_top_node_up : When_selecting_different_nodes_in_the_configuration_tree
   {
      protected override void Because()
      {
         sut.MoveUp(_moduleConfigurationNodes.First());
      }

      [Observation]
      public void the_view_should_resort_the_nodes()
      {
         A.CallTo(() => _view.SortSelectedModules()).MustNotHaveHappened();
      }
   }

   public class When_moving_a_bottom_node_down : When_selecting_different_nodes_in_the_configuration_tree
   {
      protected override void Because()
      {
         sut.MoveDown(_moduleConfigurationNodes.Last());
      }

      [Observation]
      public void the_view_should_resort_the_nodes()
      {
         A.CallTo(() => _view.SortSelectedModules()).MustNotHaveHappened();
      }
   }

   public class When_moving_a_middle_node_down : When_selecting_different_nodes_in_the_configuration_tree
   {
      protected override void Because()
      {
         sut.MoveDown(_moduleConfigurationNodes[1]);
      }

      [Observation]
      public void the_view_should_resort_the_nodes()
      {
         A.CallTo(() => _view.SortSelectedModules()).MustHaveHappened();
      }

      [Observation]
      public void appropriate_buttons_should_be_disabled_in_the_view()
      {
         // the moved node is now in the first position
         _view.EnableUp.ShouldBeTrue();
         _view.EnableDown.ShouldBeFalse();
      }
   }

   public class When_moving_a_middle_node_up : When_selecting_different_nodes_in_the_configuration_tree
   {
      protected override void Because()
      {
         sut.MoveUp(_moduleConfigurationNodes[1]);
      }

      [Observation]
      public void the_view_should_resort_the_nodes()
      {
         A.CallTo(() => _view.SortSelectedModules()).MustHaveHappened();
      }

      [Observation]
      public void appropriate_buttons_should_be_disabled_in_the_view()
      {
         // the moved node is now in the first position
         _view.EnableUp.ShouldBeFalse();
         _view.EnableDown.ShouldBeTrue();
      }
   }


   public class When_selecting_a_non_terminal_node : When_selecting_different_nodes_in_the_configuration_tree
   {
      protected override void Because()
      {
         sut.SelectedModuleConfigurationNodeChanged(_moduleConfigurationNodes[1]);
      }

      [Observation]
      public void appropriate_buttons_should_be_disabled_in_the_view()
      {
         _view.EnableRemove.ShouldBeTrue();
         _view.EnableUp.ShouldBeTrue();
         _view.EnableDown.ShouldBeTrue();
      }
   }

   public class When_selecting_the_last_configuration_node : When_selecting_different_nodes_in_the_configuration_tree
   {
      protected override void Because()
      {
         sut.SelectedModuleConfigurationNodeChanged(_moduleConfigurationNodes.Last());
      }

      [Observation]
      public void appropriate_buttons_should_be_disabled_in_the_view()
      {
         _view.EnableRemove.ShouldBeTrue();
         _view.EnableUp.ShouldBeTrue();
         _view.EnableDown.ShouldBeFalse();
      }
   }

   public class When_selecting_the_first_configuration_node : When_selecting_different_nodes_in_the_configuration_tree
   {
      protected override void Because()
      {
         sut.SelectedModuleConfigurationNodeChanged(_moduleConfigurationNodes.First());
      }

      [Observation]
      public void appropriate_buttons_should_be_disabled_in_the_view()
      {
         _view.EnableRemove.ShouldBeTrue();
         _view.EnableUp.ShouldBeFalse();
         _view.EnableDown.ShouldBeTrue();
      }
   }

   public class When_selecting_a_non_configuration_node : concern_for_EditModuleConfigurationsPresenter
   {
      private ITreeNode _treeNode;
      private ModuleConfigurationDTO _moduleConfigurationDTO;
      private SimulationConfiguration _simulationConfiguration;

      protected override void Context()
      {
         base.Context();
         _simulationConfiguration = new SimulationConfiguration();
         _moduleConfigurationDTO = new ModuleConfigurationDTO(new ModuleConfiguration(new Module { new SpatialStructure() }));
         _treeNode = _treeNodeFactory.CreateFor(_moduleConfigurationDTO);
         sut.Edit(_simulationConfiguration);
         _view.EnableRemove = true;
         _view.EnableUp = true;
         _view.EnableDown = true;
      }

      protected override void Because()
      {
         sut.SelectedModuleConfigurationNodeChanged(_treeNode.Children.First());
      }

      [Observation]
      public void appropriate_buttons_should_be_disabled_in_the_view()
      {
         _view.EnableRemove.ShouldBeFalse();
         _view.EnableUp.ShouldBeFalse();
         _view.EnableDown.ShouldBeFalse();
      }
   }

   public class When_changing_the_selected_module_configuration : concern_for_EditModuleConfigurationsPresenter
   {
      private ITreeNode _treeNode;
      private ModuleConfigurationDTO _moduleConfigurationDTO;
      private SimulationConfiguration _simulationConfiguration;

      protected override void Context()
      {
         base.Context();
         _simulationConfiguration = new SimulationConfiguration();
         _moduleConfigurationDTO = new ModuleConfigurationDTO(new ModuleConfiguration(new Module()));
         _treeNode = _treeNodeFactory.CreateFor(_moduleConfigurationDTO);
         sut.Edit(_simulationConfiguration);
      }

      protected override void Because()
      {
         sut.SelectedModuleConfigurationNodeChanged(_treeNode);
      }

      [Observation]
      public void the_view_should_update_binding()
      {
         A.CallTo(() => _view.UnbindModuleConfiguration()).MustHaveHappened();
         A.CallTo(() => _view.BindModuleConfiguration(_moduleConfigurationDTO)).MustHaveHappened();
      }
   }

   public class When_updating_the_start_values_from_the_view : concern_for_EditModuleConfigurationsPresenter
   {
      private SimulationConfiguration _simulationConfiguration;
      private Module _usedModule;
      private Module _projectModule;
      private ITreeNode _treeNode;

      protected override void Context()
      {
         base.Context();
         _simulationConfiguration = new SimulationConfiguration();
         _usedModule = new Module().WithName("usedModule").WithId("3");

         _projectModule = new Module().WithName("usedModule").WithId("4");
         _projectModule.Add(new InitialConditionsBuildingBlock().WithId("1"));
         _projectModule.Add(new ParameterValuesBuildingBlock().WithId("2"));
         var moduleConfiguration = new ModuleConfiguration(_usedModule);
         _simulationConfiguration.AddModuleConfiguration(moduleConfiguration);
         _moBiProject.AddModule(_projectModule);

         sut.Edit(_simulationConfiguration);

         var moduleConfigurationDTO = sut.ModuleConfigurationDTOs.First();
         _treeNode = _treeNodeFactory.CreateFor(moduleConfigurationDTO);
         moduleConfigurationDTO.SelectedInitialConditions = moduleConfigurationDTO.ModuleConfiguration.Module.InitialConditionsCollection.First();
         moduleConfigurationDTO.SelectedParameterValues = moduleConfigurationDTO.ModuleConfiguration.Module.ParameterValuesCollection.First();
      }

      protected override void Because()
      {
         sut.UpdateStartValuesFor(_treeNode);
      }

      [Observation]
      public void the_view_inserts_new_node_for_each_building_block()
      {
         A.CallTo(() => _view.AddNodeToSelectedModuleConfigurations(A<ITreeNode>.That.Matches(x => (x.Id == "1")))).MustHaveHappened();
         A.CallTo(() => _view.AddNodeToSelectedModuleConfigurations(A<ITreeNode>.That.Matches(x => (x.Id == "2")))).MustHaveHappened();
      }

      [Observation]
      public void selected_start_values_come_from_the_selected_module()
      {
         _projectModule.InitialConditionsCollection.Each(x => sut.InitialConditionsCollectionFor(_treeNode).ShouldContain(x));
         _projectModule.ParameterValuesCollection.Each(x => sut.ParameterValuesCollectionFor(_treeNode).ShouldContain(x));
      }
   }

   public class When_removing_a_module_configuration_from_the_simulation_configuration : concern_for_EditModuleConfigurationsPresenter
   {
      private SimulationConfiguration _simulationConfiguration;
      private Module _usedModule;
      private Module _projectModule;
      private ITreeNode _treeNode;

      protected override void Context()
      {
         base.Context();
         _simulationConfiguration = new SimulationConfiguration();
         _usedModule = new Module().WithName("usedModule");
         _projectModule = new Module().WithName("usedModule");
         var moduleConfiguration = new ModuleConfiguration(_usedModule);
         _simulationConfiguration.AddModuleConfiguration(moduleConfiguration);
         _moBiProject.AddModule(_projectModule);

         sut.Edit(_simulationConfiguration);
         _treeNode = _treeNodeFactory.CreateFor(sut.ModuleConfigurationDTOs.First());
      }

      protected override void Because()
      {
         sut.RemoveModuleConfiguration(_treeNode);
      }

      [Observation]
      public void the_module_configuration_is_removed_from_the_list()
      {
         sut.ModuleConfigurationDTOs.ShouldBeEmpty();
      }

      [Observation]
      public void the_module_is_added_to_the_unused_tree()
      {
         A.CallTo(() => _view.AddModuleNode(A<ITreeNode>.That.Matches(x => x.TagAsObject.Equals(_projectModule)))).MustHaveHappened();
      }
   }

   public class When_adding_a_module_configuration_to_the_simulation_configuration : concern_for_EditModuleConfigurationsPresenter
   {
      private SimulationConfiguration _simulationConfiguration;
      private Module _addedModule;
      private Module _usedModule;
      private Module _projectModule;
      private ITreeNode _treeNode;

      protected override void Context()
      {
         base.Context();
         _simulationConfiguration = new SimulationConfiguration();
         _usedModule = new Module().WithName("usedModule");
         _projectModule = new Module().WithName("usedModule");
         var moduleConfiguration = new ModuleConfiguration(_usedModule);
         _simulationConfiguration.AddModuleConfiguration(moduleConfiguration);
         _addedModule = new Module().WithName("unusedModule");
         _moBiProject.AddModule(_addedModule);
         _moBiProject.AddModule(_projectModule);

         _treeNode = _treeNodeFactory.CreateFor(_addedModule);

         sut.Edit(_simulationConfiguration);
      }

      protected override void Because()
      {
         sut.AddModuleConfiguration(_treeNode);
      }

      [Observation]
      public void the_module_configuration_is_added_to_the_list()
      {
         sut.ModuleConfigurationDTOs.Where(x => x.Module.Equals(_addedModule)).ShouldNotBeEmpty();
      }

      [Observation]
      public void the_new_module_is_added_to_the_view()
      {
         A.CallTo(() => _view.AddModuleConfigurationNode(A<ITreeNode>.That.Matches(x => (x.TagAsObject as ModuleConfigurationDTO).Module.Equals(_addedModule)))).MustHaveHappened();
      }

      [Observation]
      public void the_module_is_removed_from_the_unused_tree()
      {
         A.CallTo(() => _view.RemoveNodeFromSelectionView(_treeNode)).MustHaveHappened();
      }
   }

   public class When_editing_a_simulation_configuration_module_configurations : concern_for_EditModuleConfigurationsPresenter
   {
      private SimulationConfiguration _simulationConfiguration;
      private Module _unusedModule;
      private Module _usedModule;
      private Module _projectModule;

      protected override void Context()
      {
         base.Context();
         _simulationConfiguration = new SimulationConfiguration();
         _usedModule = new Module().WithName("usedModule");
         _projectModule = new Module().WithName("usedModule");
         var moduleConfiguration = new ModuleConfiguration(_usedModule);
         _simulationConfiguration.AddModuleConfiguration(moduleConfiguration);
         _unusedModule = new Module().WithName("unusedModule");
         _moBiProject.AddModule(_unusedModule);
         _moBiProject.AddModule(_projectModule);
      }

      protected override void Because()
      {
         sut.Edit(_simulationConfiguration);
      }

      [Observation]
      public void adds_the_unused_modules_to_the_view()
      {
         A.CallTo(() => _view.AddModuleNode(A<ITreeNode>.That.Matches(x => x.TagAsObject.Equals(_unusedModule)))).MustHaveHappened();
      }

      [Observation]
      public void adds_the_used_module_configuration_to_the_view()
      {
         A.CallTo(() => _view.AddModuleConfigurationNode(A<ITreeNode>.That.Matches(x => (x.TagAsObject as ModuleConfigurationDTO).Module.Equals(_projectModule)))).MustHaveHappened();
      }

      [Observation]
      public void should_use_the_project_modules_to_create_dtos()
      {
         sut.ModuleConfigurationDTOs.Select(x => x.Module).ShouldContain(_projectModule);
      }
   }
}