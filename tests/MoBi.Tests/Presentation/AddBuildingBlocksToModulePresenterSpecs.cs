using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Domain;

namespace MoBi.Presentation
{
   public class concern_for_AddBuildingBlocksToModulePresenter : ContextSpecification<AddBuildingBlocksToModulePresenter>
   {
      protected IAddBuildingBlocksToModuleDTOToModuleMapper _mapper;
      protected IAddBuildingBlocksToModuleView _view;
      protected MoBiProject _project;
      protected Module _existingModule;

      protected override void Context()
      {
         _project = new MoBiProject();
         _existingModule = new Module();
         _view = A.Fake<IAddBuildingBlocksToModuleView>();
         _mapper = A.Fake<IAddBuildingBlocksToModuleDTOToModuleMapper>();

         sut = new AddBuildingBlocksToModulePresenter(_view, _mapper);
      }
   }
/*
   public class When_adding_building_blocks_to_a_module_and_the_view_is_not_canceled : concern_for_AddBuildingBlocksToModulePresenter
   {
      private Module _moduleWithNewBuildingBlocks;
      private Module _result;

      protected override void Context()
      {
         base.Context();
         _moduleWithNewBuildingBlocks = new Module();
         A.CallTo(() => _view.Canceled).Returns(false);
         A.CallTo(() => _mapper.MapFrom(A<AddBuildingBlocksToModuleDTO>._)).Returns(_moduleWithNewBuildingBlocks);
      }

      protected override void Because()
      {
         _result = sut.AddBuildingBlocksToModule(_existingModule);
      }

      [Observation]
      public void the_module_mapper_should_create_the_module()
      {
         _result.ShouldBeEqualTo(_moduleWithNewBuildingBlocks);
      }
   }

   public class When_adding_building_blocks_to_a_module_and_the_view_is_canceled : concern_for_AddBuildingBlocksToModulePresenter
   {
      private Module _result;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _view.Canceled).Returns(true);
      }

      protected override void Because()
      {
         _result = sut.AddBuildingBlocksToModule(_existingModule);
      }

      [Observation]
      public void the_module_with_the_new_building_blocks_should_be_empty()
      {
         _result.ShouldBeNull();
      }
   }
*/
}