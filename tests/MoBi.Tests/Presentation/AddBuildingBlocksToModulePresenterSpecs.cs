using System.Collections.Generic;
using FakeItEasy;
using MoBi.Core.Domain.Model;
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
   public class concern_for_AddBuildingBlocksToModulePresenter : ContextSpecification<AddBuildingBlocksToModulePresenter>
   {
      protected IAddBuildingBlocksToModuleDTOToBuildingBlocksListMapper _dtoToBuildingBlocksMapper;
      protected IAddBuildingBlocksToModuleView _view;
      protected MoBiProject _project;
      protected Module _existingModule;
      protected IReadOnlyList<IBuildingBlock> _listOfNewBuildingBlocks;
      private IModuleToAddBuildingBlocksToModuleDTOMapper _moduleToDTOMapper;

      protected override void Context()
      {
         _project = new MoBiProject();
         _existingModule = new Module();
         _view = A.Fake<IAddBuildingBlocksToModuleView>();
         _dtoToBuildingBlocksMapper = A.Fake<IAddBuildingBlocksToModuleDTOToBuildingBlocksListMapper>();
         _moduleToDTOMapper = A.Fake<IModuleToAddBuildingBlocksToModuleDTOMapper>();
         
         _listOfNewBuildingBlocks = new List<IBuildingBlock>()
         {
            new EventGroupBuildingBlock(),
            new ReactionBuildingBlock()
         };
         sut = new AddBuildingBlocksToModulePresenter(_view, _dtoToBuildingBlocksMapper, _moduleToDTOMapper);
      }
   }

   public class When_adding_building_blocks_to_a_module_and_the_view_is_not_canceled : concern_for_AddBuildingBlocksToModulePresenter
   {
      private IReadOnlyList<IBuildingBlock> _result;

      protected override void Context()
      {
         base.Context();
         _listOfNewBuildingBlocks = new List<IBuildingBlock>();
         A.CallTo(() => _view.Canceled).Returns(false);
         A.CallTo(() => _dtoToBuildingBlocksMapper.MapFrom(A<AddBuildingBlocksToModuleDTO>._)).Returns(_listOfNewBuildingBlocks);
      }

      protected override void Because()
      {
         _result = sut.AddBuildingBlocksToModule(_existingModule);
      }

      [Observation]
      public void the_module_mapper_should_create_the_module()
      {
         _result.ShouldBeEqualTo(_listOfNewBuildingBlocks);
      }
   }

   public class When_adding_building_blocks_to_a_module_and_the_view_is_canceled : concern_for_AddBuildingBlocksToModulePresenter
   {
      private IReadOnlyList<IBuildingBlock> _result;

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
         _result.ShouldBeEmpty();
      }
   }
}