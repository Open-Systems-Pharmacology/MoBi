using System;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Services;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Helper;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Presentation
{
   public abstract class concern_for_CreateBuildingBlockMergePresenter : ContextSpecification<ICreateBuildingBlockMergePresenter<IBuildingBlock>>
   {
      private IObjectTypeResolver _objectTypeResolver;
      protected ICreateBuildingBlockMergeView _view;
      protected IBuildingBlockMergePresenter<IBuildingBlock> _buildingBlockMergePresenter;
      private IMoBiContext _context;
      protected IDialogCreator _dialogCreator;

      protected override void Context()
      {
         _objectTypeResolver = A.Fake<IObjectTypeResolver>();
         _view = A.Fake<ICreateBuildingBlockMergeView>();
         _buildingBlockMergePresenter = A.Fake<IBuildingBlockMergePresenter<IBuildingBlock>>();
         _context = A.Fake<IMoBiContext>();
         _dialogCreator = A.Fake<IDialogCreator>();
         sut = new CreateBuildingBlockMergePresenter<IBuildingBlock>(_view, _buildingBlockMergePresenter, _context, _dialogCreator, _objectTypeResolver);
      }
   }

   public class When_starting_a_single_building_block_merge : concern_for_CreateBuildingBlockMergePresenter
   {
      private Action<IBuildingBlockMergePresenter, IBuildingBlock> _configuration;

      protected override void Context()
      {
         base.Context();
         _configuration = (presenter, block) => { };
         A.CallTo(_dialogCreator).WithReturnType<string>().Returns("fileName");
      }

      protected override void Because()
      {
         sut.StartSingleMerge(_configuration);
      }

      [Observation]
      public void should_initialize_the_merge_presenter_with_the_available_configuration()
      {
         A.CallTo(() => _buildingBlockMergePresenter.SetMergeConfiguration(_configuration)).MustHaveHappened();
      }

      [Observation]
      public void should_ask_the_user_to_select_a_file_containing_the_building_block_to_merge_and_start_the_merge_process()
      {
         A.CallTo(() => _buildingBlockMergePresenter.LoadMergeConfigurationFromFile("fileName")).MustHaveHappened();
         A.CallTo(() => _buildingBlockMergePresenter.PerformMerge()).MustHaveHappened();
      }
   }

   public class When_starting_a_single_building_block_merge_and_the_user_cancels_the_action : concern_for_CreateBuildingBlockMergePresenter
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(_dialogCreator).WithReturnType<string>().Returns(string.Empty);
      }

      protected override void Because()
      {
         sut.StartSingleMerge((presenter, block) => { });
      }

      [Observation]
      public void should_not_start_the_merge_action()
      {
         A.CallTo(() => _buildingBlockMergePresenter.LoadMergeConfigurationFromFile(A<string>._)).MustNotHaveHappened();
         A.CallTo(() => _buildingBlockMergePresenter.PerformMerge()).MustNotHaveHappened();
      }
   }

   public class When_starting_a_full_building_block_merge : concern_for_CreateBuildingBlockMergePresenter
   {
      private Action<IBuildingBlockMergePresenter, IBuildingBlock> _configuration;

      protected override void Context()
      {
         base.Context();
         _configuration = (presenter, block) => { };
         A.CallTo(() => _view.Canceled).Returns(false);
      }

      protected override void Because()
      {
         sut.StartFullMerge(_configuration,  caption: "toto");
      }

      [Observation]
      public void should_initialize_the_merge_presenter_with_the_available_configuration()
      {
         A.CallTo(() => _buildingBlockMergePresenter.SetMergeConfiguration(_configuration)).MustHaveHappened();
      }

      [Observation]
      public void should_update_the_view_caption_according_to_the_input_given()
      {
         _view.Caption.ShouldBeEqualTo("toto");
      }

      [Observation]
      public void should_start_the_merge_process()
      {
         A.CallTo(() => _buildingBlockMergePresenter.PerformMerge()).MustHaveHappened();
      }
   }

   public class When_starting_a_full_building_block_merge_and_the_user_cancels_the_merge : concern_for_CreateBuildingBlockMergePresenter
   {
      private Action<IBuildingBlockMergePresenter, IBuildingBlock> _configuration;

      protected override void Context()
      {
         base.Context();
         _configuration = (presenter, block) => { };
         A.CallTo(() => _view.Canceled).Returns(true);
      }

      protected override void Because()
      {
         sut.StartFullMerge(_configuration,  caption: "toto");
      }

      [Observation]
      public void should_initialize_the_merge_presenter_with_the_available_configuration()
      {
         A.CallTo(() => _buildingBlockMergePresenter.SetMergeConfiguration(_configuration)).MustHaveHappened();
      }

      [Observation]
      public void should_update_the_view_caption_according_to_the_input_given()
      {
         _view.Caption.ShouldBeEqualTo("toto");
      }

      [Observation]
      public void should_not_start_the_merge_action()
      {
         A.CallTo(() => _buildingBlockMergePresenter.PerformMerge()).MustNotHaveHappened();
      }
   }
}