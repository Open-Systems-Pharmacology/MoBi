using System.Collections.Generic;
using FakeItEasy;
using MoBi.Core.Commands;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation
{
   public abstract class concern_for_TagsPresenter : ContextSpecification<ITagsPresenter>
   {
      protected ITagsView _view;
      protected IEntityTask _entityTask;
      private ITagToTagDTOMapper _tagMapper;
      protected ICommandCollector _commandCollector;
      protected IEntity _parameter;
      protected IBuildingBlock _buildingBlock;

      protected override void Context()
      {
         _view = A.Fake<ITagsView>();
         _entityTask = A.Fake<IEntityTask>();
         _tagMapper = A.Fake<ITagToTagDTOMapper>();
         sut = new TagsPresenter(_view, _entityTask, _tagMapper);

         _buildingBlock = A.Fake<IBuildingBlock>();
         _commandCollector = new MoBiMacroCommand();
         sut.InitializeWith(_commandCollector);
         _parameter = new Parameter();
         sut.BuildingBlock = _buildingBlock;
         sut.Edit(_parameter);
      }
   }

   public class When_adding_a_new_tag_to_an_entity : concern_for_TagsPresenter
   {
      private IMoBiCommand _addCommand;

      protected override void Context()
      {
         base.Context();
         _addCommand = A.Fake<IMoBiCommand>();
         A.CallTo(() => _entityTask.AddNewTagTo(_parameter, _buildingBlock)).Returns(_addCommand);
      }

      protected override void Because()
      {
         sut.AddNewTag();
      }

      [Observation]
      public void should_rebind_to_the_view()
      {
         //twice because binding happens on init
         A.CallTo(() => _view.BindTo(A<IEnumerable<TagDTO>>._)).MustHaveHappenedTwiceExactly();
      }

      [Observation]
      public void should_leverage_the_entity_task_to_add_the_tag_and_store_the_corresponding_command()
      {
         _commandCollector.All().ShouldContain(_addCommand);
      }
   }

   public class When_removing_a_tag_from_an_entity : concern_for_TagsPresenter
   {
      private TagDTO _tagDTO;
      private IMoBiCommand _removeCommand;

      protected override void Context()
      {
         base.Context();
         _tagDTO = new TagDTO("tag");
         _removeCommand = A.Fake<IMoBiCommand>();
         A.CallTo(() => _entityTask.RemoveTagFrom(_tagDTO, _parameter, _buildingBlock)).Returns(_removeCommand);
      }

      protected override void Because()
      {
         sut.RemoveTag(_tagDTO);
      }

      [Observation]
      public void should_leverage_the_entity_task_to_add_the_tag_and_store_the_corresponding_command()
      {
         _commandCollector.All().ShouldContain(_removeCommand);
      }

      [Observation]
      public void should_rebind_to_the_view()
      {
         //twice because binding happens on init
         A.CallTo(() => _view.BindTo(A<IEnumerable<TagDTO>>._)).MustHaveHappenedTwiceExactly();
      }
   }
}