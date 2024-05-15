using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;

namespace MoBi.Presentation
{
   public class concern_for_CloneBuildingBlocksToModulePresenter : ContextSpecification<CloneBuildingBlocksToModulePresenter>
   {
      private IMoBiProjectRetriever _projectRetriever;
      protected ICloneBuildingBlocksToModuleView _view;
      protected Module _clonedModule;

      protected override void Context()
      {
         _view = A.Fake<ICloneBuildingBlocksToModuleView>();
         _projectRetriever = A.Fake<IMoBiProjectRetriever>();
         _clonedModule = new Module
         {
            new MoBiSpatialStructure()
         };
         sut = new CloneBuildingBlocksToModulePresenter(_view, _projectRetriever);
      }
   }

   public class When_the_view_is_not_canceled : concern_for_CloneBuildingBlocksToModulePresenter
   {
      private bool _result;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _view.Canceled).Returns(false);
         A.CallTo(() => _view.BindTo(A<CloneBuildingBlocksToModuleDTO>.Ignored)).Invokes((CloneBuildingBlocksToModuleDTO dto) =>
         {
            dto.WithSpatialStructure = false;
            dto.Name = "a new name";
            dto.DefaultMergeBehavior = MergeBehavior.Extend;
         });
      }

      protected override void Because()
      {
         _result = sut.SelectClonedBuildingBlocks(_clonedModule);
      }

      [Observation]
      public void the_cloned_module_should_be_renamed()
      {
         _clonedModule.Name.ShouldBeEqualTo("a new name");
      }

      [Observation]
      public void the_cloned_module_should_retain_the_default_merge_behavior()
      {
         _clonedModule.DefaultMergeBehavior.ShouldBeEqualTo(MergeBehavior.Extend);
      }

      [Observation]
      public void the_cloned_module_should_have_the_deselected_building_blocks_removed()
      {
         _clonedModule.SpatialStructure.ShouldBeNull();
      }

      [Observation]
      public void the_presenter_returns_false()
      {
         _result.ShouldBeTrue();
      }
   }

   public class When_the_view_is_canceled : concern_for_CloneBuildingBlocksToModulePresenter
   {
      private bool _result;

      protected override void Context()
      {
         base.Context();
         
         A.CallTo(() => _view.Canceled).Returns(true);
      }

      protected override void Because()
      {
         _result = sut.SelectClonedBuildingBlocks(_clonedModule);
      }

      [Observation]
      public void the_cloned_module_should_not_have_removed_the_building_block()
      {
         _clonedModule.SpatialStructure.ShouldNotBeNull();
      }

      [Observation]
      public void the_presenter_returns_false()
      {
         _result.ShouldBeFalse();
      }
   }
}
