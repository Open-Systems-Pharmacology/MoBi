using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter.BasePresenter;
using MoBi.Presentation.Tasks;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Presenter
{
   public interface ITagsPresenter : IEditPresenter<IEntity>, IPresenter<ITagsView>
   {
      void AddNewTag();
      void RemoveTag(TagDTO tagDTO);
      IBuildingBlock BuildingBlock { get; set; }
   }

   public class TagsPresenter : AbstractEditPresenter<ITagsView, ITagsPresenter, IEntity>, ITagsPresenter
   {
      private readonly IEntityTask _entityTask;
      private IEntity _entity;
      private readonly ITagToTagDTOMapper _tagMapper;
      private INotifyList<TagDTO> _tagsDTO;

      public IBuildingBlock BuildingBlock { get; set; }

      public TagsPresenter(ITagsView view, IEntityTask entityTask, ITagToTagDTOMapper tagMapper) : base(view)
      {
         _entityTask = entityTask;
         _tagMapper = tagMapper;
      }

      public void AddNewTag()
      {
         AddCommand(_entityTask.AddNewTagTo(_entity, BuildingBlock));
         rebind();
      }

      public void RemoveTag(TagDTO tagDTO)
      {
         AddCommand(_entityTask.RemoveTagFrom(tagDTO, _entity, BuildingBlock));
         rebind();
      }

      public override void Edit(IEntity entity)
      {
         _entity = entity;
         rebind();
      }

      private void rebind()
      {
         _tagsDTO = _entity.Tags.MapAllUsing(_tagMapper).ToRichList();
         _view.BindTo(_tagsDTO);
      }

      public override object Subject => _entity;
   }
}