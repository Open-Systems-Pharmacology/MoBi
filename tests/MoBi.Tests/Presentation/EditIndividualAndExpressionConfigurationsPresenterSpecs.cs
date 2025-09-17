using System.Linq;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Repository;
using MoBi.Core.Services;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Nodes;
using MoBi.Presentation.Presenter;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Presentation.Presenters.Nodes;
using OSPSuite.Utility.Extensions;
using ITreeNodeFactory = MoBi.Presentation.Nodes.ITreeNodeFactory;
using IBuildingBlockRepository = MoBi.Core.Domain.Repository.IBuildingBlockRepository;

namespace MoBi.Presentation
{
   public class concern_for_EditIndividualAndExpressionConfigurationsPresenter : ContextSpecification<EditIndividualAndExpressionConfigurationsPresenter>
   {
      protected IMoBiProjectRetriever _projectRetriever;
      protected IBuildingBlockRepository _buildingBlockRepository;
      protected ITreeNodeFactory _treeNodeFactory;
      protected ISelectedIndividualToIndividualSelectionDTOMapper _selectedIndividualDTOMapper;
      protected IEditIndividualAndExpressionConfigurationsView _view;
      protected IDialogCreator _dialogCreator;

      protected override void Context()
      {
         _dialogCreator = A.Fake<IDialogCreator>();
         _projectRetriever = A.Fake<IMoBiProjectRetriever>();
         _buildingBlockRepository = new BuildingBlockRepository(_projectRetriever);
         _treeNodeFactory = A.Fake<ITreeNodeFactory>();
         _selectedIndividualDTOMapper = A.Fake<ISelectedIndividualToIndividualSelectionDTOMapper>();
         _view = A.Fake<IEditIndividualAndExpressionConfigurationsView>();
         sut = new EditIndividualAndExpressionConfigurationsPresenter(_view, _selectedIndividualDTOMapper, _treeNodeFactory, _buildingBlockRepository, _dialogCreator);
      }
   }

   public class When_moving_expression_nodes_within_the_simulation_configuration : concern_for_EditIndividualAndExpressionConfigurationsPresenter
   {
      private SimulationConfiguration _simulationConfiguration;
      private ExpressionProfileBuildingBlock _expressionProfile1;
      private ExpressionProfileBuildingBlock _expressionProfile2;
      private ITreeNode _treeNode1;
      private ITreeNode _treeNode2;

      protected override void Context()
      {
         base.Context();
         _simulationConfiguration = new SimulationConfiguration();

         _expressionProfile1 = new ExpressionProfileBuildingBlock().WithName("molecule1|species|category");
         _expressionProfile2 = new ExpressionProfileBuildingBlock().WithName("molecule2|species|category");
         _treeNode1 = new ObjectWithIdAndNameNode<ExpressionProfileBuildingBlock>(_expressionProfile1);
         _treeNode2 = new ObjectWithIdAndNameNode<ExpressionProfileBuildingBlock>(_expressionProfile2);

         var moBiProject = new MoBiProject();

         A.CallTo(() => _projectRetriever.Current).Returns(moBiProject);

         moBiProject.AddExpressionProfileBuildingBlock(_expressionProfile1);
         moBiProject.AddExpressionProfileBuildingBlock(_expressionProfile2);

         _simulationConfiguration.AddExpressionProfile(_expressionProfile1);
         _simulationConfiguration.AddExpressionProfile(_expressionProfile2);

         A.CallTo(() => _treeNodeFactory.CreateFor(_expressionProfile1)).Returns(_treeNode1);
         A.CallTo(() => _treeNodeFactory.CreateFor(_expressionProfile2)).Returns(_treeNode2);

         sut.Edit(_simulationConfiguration);
      }

      protected override void Because()
      {
         sut.DropNode(_treeNode2, _treeNode1);
      }

      [Observation]
      public void the_order_of_the_nodes_changes()
      {
         sut.ExpressionProfiles.ElementAt(0).ShouldBeEqualTo(_expressionProfile2);
         sut.ExpressionProfiles.ElementAt(1).ShouldBeEqualTo(_expressionProfile1);
      }
   }

   public class When_removing_an_expression_profile_from_a_simulation_configuration : concern_for_EditIndividualAndExpressionConfigurationsPresenter
   {
      private SimulationConfiguration _simulationConfiguration;
      private ExpressionProfileBuildingBlock _expressionProfile;
      private ITreeNode _treeNode;

      protected override void Context()
      {
         base.Context();
         _simulationConfiguration = new SimulationConfiguration();

         _expressionProfile = new ExpressionProfileBuildingBlock().WithName("molecule|species|category");
         var moBiProject = new MoBiProject();

         A.CallTo(() => _projectRetriever.Current).Returns(moBiProject);

         moBiProject.AddExpressionProfileBuildingBlock(_expressionProfile);
         sut.Edit(_simulationConfiguration);
         _simulationConfiguration.AddExpressionProfile(_expressionProfile);

         _treeNode = new ObjectWithIdAndNameNode<ExpressionProfileBuildingBlock>(_expressionProfile);
         A.CallTo(() => _treeNodeFactory.CreateFor(_expressionProfile)).Returns(_treeNode);
      }

      protected override void Because()
      {
         sut.RemoveSelectedExpressions(new[] { _treeNode });
      }

      [Observation]
      public void adds_unused_node_to_the_view()
      {
         A.CallTo(() => _view.AddUnusedExpression(_treeNode)).MustHaveHappened();
      }

      [Observation]
      public void removes_used_node_from_the_view()
      {
         A.CallTo(() => _view.RemoveUsedExpression(_treeNode)).MustHaveHappened();
      }

      [Observation]
      public void the_expression_profile_should_be_added_to_the_simulation_configuration()
      {
         sut.ExpressionProfiles.ShouldNotContain(_expressionProfile);
      }
   }

   public class When_adding_a_new_expression_profiles_to_a_simulation_configuration : concern_for_EditIndividualAndExpressionConfigurationsPresenter
   {
      private SimulationConfiguration _simulationConfiguration;
      private ExpressionProfileBuildingBlock _expressionProfile;
      private ITreeNode _treeNode;
      private ExpressionProfileBuildingBlock _expressionProfile2;
      private ITreeNode _treeNode2;

      protected override void Context()
      {
         base.Context();
         _simulationConfiguration = new SimulationConfiguration();

         _expressionProfile = new ExpressionProfileBuildingBlock().WithName("molecule|species|category").WithId("1");
         _expressionProfile.Type = ExpressionTypes.MetabolizingEnzyme;

         _expressionProfile2 = new ExpressionProfileBuildingBlock().WithName("molecule|another species|another category").WithId("2");
         _expressionProfile2.Type = ExpressionTypes.MetabolizingEnzyme;
         var moBiProject = new MoBiProject();

         A.CallTo(() => _projectRetriever.Current).Returns(moBiProject);

         moBiProject.AddExpressionProfileBuildingBlock(_expressionProfile);
         moBiProject.AddExpressionProfileBuildingBlock(_expressionProfile2);
         sut.Edit(_simulationConfiguration);

         _treeNode = new BuildingBlockNode(_expressionProfile);
         _treeNode2 = new BuildingBlockNode(_expressionProfile2);

         A.CallTo(() => _treeNodeFactory.CreateFor(A<IBuildingBlock>._)).ReturnsLazily((IBuildingBlock buildingBlock) => ReferenceEquals(buildingBlock, _expressionProfile) ? _treeNode : _treeNode2);
      }

      protected override void Because()
      {
         sut.AddSelectedExpressions(new[] { _treeNode, _treeNode2 });
      }

      [Observation]
      public void the_dialog_creator_should_inform_the_user_about_the_expressions_that_could_not_be_added()
      {
         A.CallTo(() => _dialogCreator.MessageBoxInfo(A<string>.That.Contains("molecule"))).MustHaveHappened();
      }

      [Observation]
      public void the_node_that_cannot_be_selected_is_not_added_to_the_view()
      {
         A.CallTo(() => _view.AddUsedExpression(_treeNode2)).MustNotHaveHappened();
      }

      [Observation]
      public void adds_used_node_to_the_view()
      {
         A.CallTo(() => _view.AddUsedExpression(_treeNode)).MustHaveHappened();
      }

      [Observation]
      public void removes_unused_node_from_the_view()
      {
         A.CallTo(() => _view.RemoveUnusedExpression(_treeNode)).MustHaveHappened();
      }

      [Observation]
      public void the_expression_profile_should_be_added_to_the_simulation_configuration()
      {
         sut.ExpressionProfiles.ShouldContain(_expressionProfile);
      }

      [Observation]
      public void the_expression_profile_that_could_not_be_selected_should_not_be_added_to_the_simulation_configuration()
      {
         sut.ExpressionProfiles.ShouldNotContain(_expressionProfile2);
      }
   }

   public class When_editing_a_simulation_configuration_expressions_and_individuals : concern_for_EditIndividualAndExpressionConfigurationsPresenter
   {
      private SimulationConfiguration _simulationConfiguration;
      private ExpressionProfileBuildingBlock _expressionProfile1;
      private ExpressionProfileBuildingBlock _expressionProfile2;

      protected override void Context()
      {
         base.Context();
         var moBiProject = new MoBiProject();
         A.CallTo(() => _projectRetriever.Current).Returns(moBiProject);
         _simulationConfiguration = new SimulationConfiguration
         {
            Individual = new IndividualBuildingBlock()
         };
         _simulationConfiguration.AddExpressionProfile(new ExpressionProfileBuildingBlock().WithName("molecule1|species|category"));
         _simulationConfiguration.AddExpressionProfile(new ExpressionProfileBuildingBlock().WithName("molecule2|species|category"));

         _expressionProfile1 = new ExpressionProfileBuildingBlock().WithName("molecule1|species|category");
         moBiProject.AddExpressionProfileBuildingBlock(_expressionProfile1);
         _expressionProfile2 = new ExpressionProfileBuildingBlock().WithName("molecule2|species|category");
         moBiProject.AddExpressionProfileBuildingBlock(_expressionProfile2);
      }

      protected override void Because()
      {
         sut.Edit(_simulationConfiguration);
      }

      [Observation]
      public void the_individual_dto_mapper_is_used_to_create_dto()
      {
         A.CallTo(() => _selectedIndividualDTOMapper.MapFrom(_simulationConfiguration.Individual)).MustHaveHappened();
      }

      [Observation]
      public void selected_expressions_should_not_include_expressions_from_the_simulation_configuration()
      {
         _simulationConfiguration.ExpressionProfiles.Each(x => sut.ExpressionProfiles.ShouldNotContain(x));
         sut.ExpressionProfiles.ShouldContain(_expressionProfile1, _expressionProfile2);
      }
   }
}