using System;
using FakeItEasy;
using OSPSuite.BDDHelper;
using MoBi.Core.Helper;
using MoBi.Presentation.Presenter;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks
{
   public abstract class concern_for_MergeWorkflowTask : ContextSpecification<IMergeWorkflowTask>
   {
      protected IMoBiApplicationController _applicationController;

      protected override void Context()
      {
         _applicationController= A.Fake<IMoBiApplicationController>();
         sut = new MergeWorkflowTask(_applicationController,new ObjectTypeResolver());
      }
   }

   public class When_starting_a_single_building_block_merge_workflow : concern_for_MergeWorkflowTask
   {
      private IBuildingBlock _targetBuildingBlock;
      private ICreateBuildingBlockMergePresenter<IBuildingBlock> _createBuildingBlockMergePresenter;
      private Action<IBuildingBlockMergePresenter, IBuildingBlock> _configuration;

      protected override void Context()
      {
         base.Context();
         _targetBuildingBlock= A.Fake<IBuildingBlock>();
         _createBuildingBlockMergePresenter= A.Fake<ICreateBuildingBlockMergePresenter<IBuildingBlock>>();
         A.CallTo(() => _applicationController.Start<ICreateBuildingBlockMergePresenter<IBuildingBlock>>()).Returns(_createBuildingBlockMergePresenter);
         A.CallTo(() => _createBuildingBlockMergePresenter.StartSingleMerge(A<Action<IBuildingBlockMergePresenter, IBuildingBlock>>._))
            .Invokes(x => _configuration = x.GetArgument<Action<IBuildingBlockMergePresenter, IBuildingBlock>>(0));
      }

      protected override void Because()
      {
         sut.StartSingleBuildingBlockMerge(_targetBuildingBlock);
      }

      [Observation]
      public void should_retrieve_the_presenter_for_single_merge_and_start_the_single_merge_use_case()
      {
         A.CallTo(() => _createBuildingBlockMergePresenter.StartSingleMerge(A<Action<IBuildingBlockMergePresenter,IBuildingBlock>>._)).MustHaveHappened();
      }

      [Observation]
      public void the_configuration_should_initialize_the_merge_for_a_single_presenter_only()
      {
         var buildingBlockMergePresenter= A.Fake<IBuildingBlockMergePresenter>();
         var bb= A.Fake<IBuildingBlock>();
         _configuration(buildingBlockMergePresenter, bb);
         A.CallTo(() => buildingBlockMergePresenter.AddMappingForSingleBuildingBlock(bb,_targetBuildingBlock)).MustHaveHappened();
      }
   }
}	