using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Events;
using MoBi.Core.Services;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Presenter
{
   public interface IDescriptorConditionListPresenter :
      IPresenter<IDescriptorConditionListView>,
      IPresenterWithContextMenu<IViewItem>,
      IListener<AddTagConditionEvent>,
      IListener<RemoveTagConditionEvent>
   {
      void UpdateCriteriaTag(IDescriptorConditionDTO descriptorConditionDTO, string newTag);
      IObjectBase Subject { get; }
      void RemoveCondition(IDescriptorConditionDTO descriptorConditionDTO);
      void NewMatchTagCondition();
      void NewMatchAllCondition();
      void NewNotMatchTagCondition();
      void NewInContainerCondition();
   }

   public interface IDescriptorConditionListPresenter<T> : IDescriptorConditionListPresenter where T : IObjectBase
   {
      void Edit(T taggedObject, Func<T, DescriptorCriteria> criteriaRetriever, IBuildingBlock buildingBlock);
   }

   public class DescriptorConditionListPresenter<T> : AbstractCommandCollectorPresenter<IDescriptorConditionListView, IDescriptorConditionListPresenter>, IDescriptorConditionListPresenter<T> where T : class, IObjectBase
   {
      private readonly IViewItemContextMenuFactory _viewItemContextMenuFactory;
      private readonly ITagTask _tagTask;
      private readonly IDescriptorConditionToDescriptorConditionDTOMapper _descriptorConditionMapper;
      private readonly IDialogCreator _dialogCreator;
      private readonly ITagVisitor _tagVisitor;
      private DescriptorCriteria _descriptorCriteria;
      private IReadOnlyList<IDescriptorConditionDTO> _descriptorCriteriaDTO;
      private T _taggedObject;
      private Func<T, DescriptorCriteria> _descriptorCriteriaRetriever;
      private readonly ContainerDescriptorRootItem _defaultRootItem;

      private IBuildingBlock _buildingBlock;
      public IViewItem ViewRootItem { get; set; }

      public DescriptorConditionListPresenter(IDescriptorConditionListView view, IViewItemContextMenuFactory viewItemContextMenuFactory,
         ITagTask tagTask, IDescriptorConditionToDescriptorConditionDTOMapper descriptorConditionMapper, IDialogCreator dialogCreator,
         ITagVisitor tagVisitor)
         : base(view)
      {
         _viewItemContextMenuFactory = viewItemContextMenuFactory;
         _tagTask = tagTask;
         _descriptorConditionMapper = descriptorConditionMapper;
         _dialogCreator = dialogCreator;
         _tagVisitor = tagVisitor;
         _defaultRootItem = new ContainerDescriptorRootItem();
      }

      public void ShowContextMenu(IViewItem objectRequestingPopup, Point popupLocation)
      {
         var contextMenu = _viewItemContextMenuFactory.CreateFor(objectRequestingPopup ?? _defaultRootItem, this);
         contextMenu.Show(_view, popupLocation);
      }

      public void Edit(T taggedObject, Func<T, DescriptorCriteria> descriptorCriteriaRetriever, IBuildingBlock buildingBlock)
      {
         _taggedObject = taggedObject;
         _buildingBlock = buildingBlock;
         _descriptorCriteriaRetriever = descriptorCriteriaRetriever;
         _descriptorCriteria = descriptorCriteriaRetriever(taggedObject);
         bindToView();
      }

      private void bindToView()
      {
         _descriptorCriteriaDTO = _descriptorCriteria.MapAllUsing(_descriptorConditionMapper);
         _view.BindTo(_descriptorCriteriaDTO);
         updateCriteriaDescription();
      }

      private void updateCriteriaDescription()
      {
         _view.CriteriaDescription = _descriptorCriteria.ToString();
      }

      public void RemoveCondition(IDescriptorConditionDTO dto)
      {
         AddCommand(_tagTask.RemoveTagCondition(dto.Tag, dto.TagType, _taggedObject, _buildingBlock, _descriptorCriteriaRetriever));
      }

      public void UpdateCriteriaTag(IDescriptorConditionDTO descriptorConditionDTO, string newTag)
      {
         AddCommand(_tagTask.EditTag(newTag, descriptorConditionDTO.Tag, _taggedObject, _buildingBlock, _descriptorCriteriaRetriever));
         updateCriteriaDescription();
      }

      private void addCondition(TagType tagType)
      {
         var tag = getNewTagName(tagType);
         if (string.IsNullOrEmpty(tag))
            return;

         AddCommand(_tagTask.AddTagCondition(tag, tagType, _taggedObject, _buildingBlock, _descriptorCriteriaRetriever));
      }

      private string getNewTagName(TagType tagType)
      {
         switch (tagType)
         {
            case TagType.MatchAll:
               return AppConstants.MatchAll;
            case TagType.Match:
               return getNewTagName<MatchTagCondition>(AppConstants.Dialog.NewMatchTag);
            case TagType.NotMatch:
               return getNewTagName<NotMatchTagCondition>(AppConstants.Dialog.NewNotMatchTag);
            case TagType.InContainer:
               return getNewTagName<InContainerCondition>(AppConstants.Dialog.NewInContainerTag);
            default:
               return string.Empty;
         }
      }

      private string getNewTagName<TTagCondition>(string caption) where TTagCondition : ITagCondition
      {
         var forbiddenTags = _descriptorCriteria.OfType<TTagCondition>().Select(x => x.Tag);
         return _dialogCreator.AskForInput(caption, AppConstants.Captions.Tag, String.Empty, forbiddenTags, getUsedTags());
      }

      private IEnumerable<string> getUsedTags() => _tagVisitor.AllTags();

      public void NewMatchTagCondition() => addCondition(TagType.Match);

      public void NewMatchAllCondition() => addCondition(TagType.MatchAll);

      public void NewNotMatchTagCondition() => addCondition(TagType.NotMatch);

      public void NewInContainerCondition() => addCondition(TagType.InContainer);

      public IObjectBase Subject => _taggedObject;

      public void Handle(AddTagConditionEvent eventToHandle)
      {
         handle(eventToHandle);
      }

      private void handle(TagConditionEvent tagConditionEvent)
      {
         if (!Equals(tagConditionEvent.TaggedObject, _taggedObject))
            return;

         bindToView();
      }

      public void Handle(RemoveTagConditionEvent eventToHandle)
      {
         handle(eventToHandle);
      }
   }
}