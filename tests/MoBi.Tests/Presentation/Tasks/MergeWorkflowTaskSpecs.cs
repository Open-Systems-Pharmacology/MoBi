using System;
using FakeItEasy;
using OSPSuite.BDDHelper;
using MoBi.Core.Domain.Model;
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

   public class When_starting_a_simulation_merge_workflow : concern_for_MergeWorkflowTask
   {
      private ICreateBuildingBlockMergePresenter<IMoBiBuildConfiguration> _createBuildingBlockMergePresenter;
      private Action<IBuildingBlockMergePresenter, IMoBiBuildConfiguration> _configuration;
      private IBuildingBlockMergePresenter _buildingBlockMergePresenter;
      private IMoBiBuildConfiguration _buildConfiguration;

      protected override void Context()
      {
         base.Context();
         _createBuildingBlockMergePresenter= A.Fake<ICreateBuildingBlockMergePresenter<IMoBiBuildConfiguration>>();
         A.CallTo(() => _applicationController.Start<ICreateBuildingBlockMergePresenter<IMoBiBuildConfiguration>>()).Returns(_createBuildingBlockMergePresenter);
         A.CallTo(() => _createBuildingBlockMergePresenter.StartFullMerge(A<Action<IBuildingBlockMergePresenter, IMoBiBuildConfiguration>>._,A<string>._))
            .Invokes(x => _configuration = x.GetArgument<Action<IBuildingBlockMergePresenter, IMoBiBuildConfiguration>>(0));
         _buildingBlockMergePresenter = A.Fake<IBuildingBlockMergePresenter>();
         _buildConfiguration = A.Fake<IMoBiBuildConfiguration>();

      }
      protected override void Because()
      {
         sut.StartSimulationMerge();
         _configuration(_buildingBlockMergePresenter, _buildConfiguration);
      }

      [Observation]
      public void should_add_merge_capability_merged_by_default_for_molecules()
      {
         A.CallTo(() => _buildingBlockMergePresenter.AddMappingForAllBuildingBlocks(_buildConfiguration.Molecules, true)).MustHaveHappened();
      }

      [Observation]
      public void should_add_merge_capability_merged_by_default_for_reactions()
      {
         A.CallTo(() => _buildingBlockMergePresenter.AddMappingForAllBuildingBlocks(_buildConfiguration.Reactions, true)).MustHaveHappened();
      }

      [Observation]
      public void should_add_merge_capability_merged_by_default_for_passive_transports()
      {
         A.CallTo(() => _buildingBlockMergePresenter.AddMappingForAllBuildingBlocks(_buildConfiguration.PassiveTransports, true)).MustHaveHappened();
      }

      [Observation]
      public void should_add_merge_capability_merged_by_default_for_molecule_start_values()
      {
         A.CallTo(() => _buildingBlockMergePresenter.AddMappingForAllBuildingBlocks(_buildConfiguration.MoleculeStartValues, true)).MustHaveHappened();
      }
      [Observation]
      public void should_add_merge_capability_merged_by_default_for_parameter_start_values()
      {
         A.CallTo(() => _buildingBlockMergePresenter.AddMappingForAllBuildingBlocks(_buildConfiguration.ParameterStartValues, true)).MustHaveHappened();
      }

      [Observation]
      public void should_add_merge_capability_not_merged_by_default_for_observers()
      {
         A.CallTo(() => _buildingBlockMergePresenter.AddMappingForAllBuildingBlocks(_buildConfiguration.Observers, false)).MustHaveHappened();
      }

      [Observation]
      public void should_add_merge_capability_not_merged_by_default_for_events()
      {
         A.CallTo(() => _buildingBlockMergePresenter.AddMappingForAllBuildingBlocks(_buildConfiguration.EventGroups, false)).MustHaveHappened();
      }


      [Observation]
      public void should_not_add_merge_capability_for_spatial_structure()
      {
         A.CallTo(() => _buildingBlockMergePresenter.AddMappingForAllBuildingBlocks(_buildConfiguration.SpatialStructure, A<bool>._)).MustNotHaveHappened();
      }

      [Observation]
      public void should_not_add_merge_capability_for_simulation_settings()
      {
         A.CallTo(() => _buildingBlockMergePresenter.AddMappingForAllBuildingBlocks(_buildConfiguration.SimulationSettings, A<bool>._)).MustNotHaveHappened();
      }
   }
}	