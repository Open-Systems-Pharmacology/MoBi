using System;
using System.Collections.Generic;
using System.Threading;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Services;
using FakeItEasy;
using MoBi.Assets;
using MoBi.Core.Domain.Repository;
using MoBi.Core.Repositories;
using MoBi.Core.Serialization.Services;
using MoBi.Core.Services;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation
{
   public abstract class concern_for_BuildingBlockMergePresenter : ContextSpecification<BuildingBlockMergePresenter<IBuildingBlock>>
   {
      protected ISerializationTask _serializationTask;
      protected IBuildingBlock _targetObject;
      protected IBuildingBlock _mergeObject;
      protected IDialogCreator _dialogCreator;
      protected IBuildingBlockMergeView _buildingBlockMergeView;
      protected IMergeTask _mergeTask;

      protected override void Context()
      {
         _serializationTask = A.Fake<ISerializationTask>();
         _dialogCreator = A.Fake<IDialogCreator>();
         _buildingBlockMergeView = A.Fake<IBuildingBlockMergeView>();
         _mergeTask = A.Fake<IMergeTask>();
         sut = new BuildingBlockMergePresenter<IBuildingBlock>(_buildingBlockMergeView, _dialogCreator, _mergeTask, A.Fake<IBuildingBlockRepository>(), A.Fake<IIconRepository>(), _serializationTask);
         sut.InitializeWith(A.Fake<ICommandCollector>());
         _targetObject = A.Fake<IBuildingBlock>();
         _mergeObject = A.Fake<IBuildingBlock>();

         A.CallTo(() => _targetObject.Name).Returns("target");
         A.CallTo(() => _mergeObject.Name).Returns("merge");

         A.CallTo(() => _serializationTask.Load<IBuildingBlock>(A<string>._, A<bool>._)).Returns(_mergeObject);
         
      }
   }

   public class when_performing_merge_for_multiple_building_blocks_ : concern_for_BuildingBlockMergePresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.AddMappingForAllBuildingBlocks(_mergeObject, true);
      }

      protected override void Because()
      {
         sut.PerformMerge();
      }

      [Observation]
      public void a_call_to_mergetask_merge_building_blocks_should_result()
      {
         A.CallTo(() => _mergeTask.MergeBuildingBlocks(A<IList<IBuildingBlock>>.That.Contains(_mergeObject), A<IList<IBuildingBlock>>._, A<CancellationToken>._)).MustHaveHappened();         
      }
   }

   public class when_performing_merge_on_a_single_building_block : concern_for_BuildingBlockMergePresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.AddMappingForSingleBuildingBlock(_mergeObject, _targetObject);
      }

      protected override void Because()
      {
         sut.PerformMerge();
      }

      [Observation]
      public void a_call_to_mergetask_merge_building_blocks_should_result()
      {
         A.CallTo(() => _mergeTask.MergeBuildingBlocks(A<IList<IBuildingBlock>>.That.Contains(_mergeObject), A<IList<IBuildingBlock>>.That.Contains(_targetObject), A<CancellationToken>._)).MustHaveHappened();
      }
   }

   public class when_adding_pkml_file_to_merge_presenter : concern_for_BuildingBlockMergePresenter
   {
      private string _pkmlFileName;
      private Action<IBuildingBlockMergePresenter, IBuildingBlock> _mergeConfigurationAction;

      protected override void Context()
      {
         base.Context();
         _pkmlFileName = "pkmlfile.pkml";
         _mergeConfigurationAction = A.Fake<Action<IBuildingBlockMergePresenter, IBuildingBlock>>();
         A.CallTo(() => _dialogCreator.AskForFileToOpen(AppConstants.Captions.SourceSimulationFileForMerge, Constants.Filter.PKML_FILE_FILTER, Constants.DirectoryKey.MODEL_PART, null, null)).Returns(_pkmlFileName);
         sut.SetMergeConfiguration(_mergeConfigurationAction);
      }

      protected override void Because()
      {
         sut.LoadMergeConfiguration();
      }

      [Observation]
      public void view_binding_must_have_happened()
      {
         A.CallTo(() => _buildingBlockMergeView.BindTo(A<IEnumerable<BuildingBlockMappingDTO>>.Ignored)).MustHaveHappened();
      }

      [Observation]
      public void merge_configuration_action_is_called()
      {
         A.CallTo(() => _mergeConfigurationAction(sut, _mergeObject)).MustHaveHappened();
      }

      [Observation]
      public void results_in_a_call_to_deserialize_the_file()
      {
         A.CallTo(() => _serializationTask.Load<IBuildingBlock>(_pkmlFileName, true)).MustHaveHappened();
      }
   }
}
