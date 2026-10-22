using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace MoBi.Presentation
{
   public abstract class concern_for_BuildingBlockTaskRetriever : ContextSpecification<IBuildingBlockTaskRetriever>
   {
      protected IContainer _container;

      protected override void Context()
      {
         _container = A.Fake<IContainer>();
         sut = new BuildingBlockTaskRetriever(_container);
      }
   }

   public class When_retrieving_the_task_for_a_module_building_block : concern_for_BuildingBlockTaskRetriever
   {
      private IInteractionTasksForBuildingBlock<Module, MoleculeBuildingBlock> _moleculeTask;
      private IInteractionTasksForBuildingBlock _result;

      protected override void Context()
      {
         base.Context();
         _moleculeTask = A.Fake<IInteractionTasksForBuildingBlock<Module, MoleculeBuildingBlock>>();
         A.CallTo(() => _container.Resolve<IInteractionTasksForBuildingBlock<Module, MoleculeBuildingBlock>>()).Returns(_moleculeTask);
      }

      protected override void Because()
      {
         _result = sut.TaskFor(new MoleculeBuildingBlock());
      }

      [Observation]
      public void should_return_the_task_registered_for_the_building_block_in_a_module()
      {
         _result.ShouldBeEqualTo(_moleculeTask);
      }
   }

   public class When_retrieving_the_task_for_an_initial_conditions_building_block : concern_for_BuildingBlockTaskRetriever
   {
      private IInitialConditionsTask<InitialConditionsBuildingBlock> _initialConditionsTask;
      private IInteractionTasksForBuildingBlock _result;

      protected override void Context()
      {
         base.Context();
         _initialConditionsTask = A.Fake<IInitialConditionsTask<InitialConditionsBuildingBlock>>();
         A.CallTo(() => _container.Resolve<IInitialConditionsTask<InitialConditionsBuildingBlock>>()).Returns(_initialConditionsTask);
      }

      protected override void Because()
      {
         _result = sut.TaskFor(new InitialConditionsBuildingBlock());
      }

      [Observation]
      public void should_return_the_initial_conditions_task()
      {
         _result.ShouldBeEqualTo(_initialConditionsTask);
      }
   }

   public class When_retrieving_the_task_for_an_individual_building_block : concern_for_BuildingBlockTaskRetriever
   {
      private IInteractionTasksForBuildingBlock<MoBiProject, IndividualBuildingBlock> _individualTask;
      private IInteractionTasksForBuildingBlock _result;

      protected override void Context()
      {
         base.Context();
         _individualTask = A.Fake<IInteractionTasksForBuildingBlock<MoBiProject, IndividualBuildingBlock>>();
         A.CallTo(() => _container.Resolve<IInteractionTasksForBuildingBlock<MoBiProject, IndividualBuildingBlock>>()).Returns(_individualTask);
      }

      protected override void Because()
      {
         _result = sut.TaskFor(new IndividualBuildingBlock());
      }

      [Observation]
      public void should_return_the_task_registered_for_the_building_block_in_the_project()
      {
         _result.ShouldBeEqualTo(_individualTask);
      }
   }

   public class When_retrieving_the_task_for_an_expression_profile_building_block : concern_for_BuildingBlockTaskRetriever
   {
      private IInteractionTasksForBuildingBlock<MoBiProject, ExpressionProfileBuildingBlock> _expressionProfileTask;
      private IInteractionTasksForBuildingBlock _result;

      protected override void Context()
      {
         base.Context();
         _expressionProfileTask = A.Fake<IInteractionTasksForBuildingBlock<MoBiProject, ExpressionProfileBuildingBlock>>();
         A.CallTo(() => _container.Resolve<IInteractionTasksForBuildingBlock<MoBiProject, ExpressionProfileBuildingBlock>>()).Returns(_expressionProfileTask);
      }

      protected override void Because()
      {
         _result = sut.TaskFor(new ExpressionProfileBuildingBlock());
      }

      [Observation]
      public void should_return_the_task_registered_for_the_building_block_in_the_project()
      {
         _result.ShouldBeEqualTo(_expressionProfileTask);
      }
   }
}
