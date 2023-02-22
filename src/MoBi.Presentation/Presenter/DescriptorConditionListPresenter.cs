using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Commands;
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
      void UpdateCriteriaTag(DescriptorConditionDTO descriptorConditionDTO, string newTag);
      IObjectBase Subject { get; }
      void RemoveCondition(DescriptorConditionDTO descriptorConditionDTO);
      void NewMatchTagCondition();
      void NewMatchAllCondition();
      void NewNotMatchTagCondition();
      void NewInContainerCondition();
      void NewInParentCondition();
      void NewNotInContainerCondition();
      void ChangeOperator(CriteriaOperator criteriaOperator);
   }

   public interface IDescriptorConditionListPresenter<T> : IDescriptorConditionListPresenter where T : IObjectBase
   {
      void Edit(T taggedObject, Func<T, DescriptorCriteria> descriptorCriteriaRetriever, IBuildingBlock buildingBlock);

      void Edit(T taggedObject, Func<T, DescriptorCriteria> descriptorCriteriaRetriever, Func<T, DescriptorCriteria> descriptorCriteriaCreator, IBuildingBlock buildingBlock);
   }

   public class DescriptorConditionListPresenter<T> : AbstractCommandCollectorPresenter<IDescriptorConditionListView, IDescriptorConditionListPresenter>, IDescriptorConditionListPresenter<T> where T : class, IObjectBase
   {
      private readonly IViewItemContextMenuFactory _viewItemContextMenuFactory;
      private readonly ITagTask _tagTask;
      private readonly IDescriptorCriteriaToDescriptorCriteriaDTOMapper _descriptorCriteriaMapper;
      private readonly IDialogCreator _dialogCreator;
      private readonly ITagVisitor _tagVisitor;
      private DescriptorCriteria _descriptorCriteria;
      private DescriptorCriteriaDTO _descriptorCriteriaDTO;
      private T _taggedObject;
      private Func<T, DescriptorCriteria> _descriptorCriteriaRetriever;
      private Func<T, DescriptorCriteria> _descriptorCriteriaCreator;
      private readonly ContainerDescriptorRootItem _defaultRootItem;

      private IBuildingBlock _buildingBlock;
      public IViewItem ViewRootItem { get; set; }

      public DescriptorConditionListPresenter(
         IDescriptorConditionListView view, 
         IViewItemContextMenuFactory viewItemContextMenuFactory,
         ITagTask tagTask,
         IDescriptorCriteriaToDescriptorCriteriaDTOMapper descriptorCriteriaMapper, 
         IDialogCreator dialogCreator,
         ITagVisitor tagVisitor)
         : base(view)
      {
         _viewItemContextMenuFactory = viewItemContextMenuFactory;
         _tagTask = tagTask;
         _dialogCreator = dialogCreator;
         _tagVisitor = tagVisitor;
         _descriptorCriteriaMapper = descriptorCriteriaMapper;
         _defaultRootItem = new ContainerDescriptorRootItem();
      }

      public void ShowContextMenu(IViewItem objectRequestingPopup, Point popupLocation)
      {
         var contextMenu = _viewItemContextMenuFactory.CreateFor(objectRequestingPopup ?? _defaultRootItem, this);
         contextMenu.Show(_view, popupLocation);
      }

      public void Edit(T taggedObject, Func<T, DescriptorCriteria> descriptorCriteriaRetriever, IBuildingBlock buildingBlock)
      {
         Edit(taggedObject, descriptorCriteriaRetriever, x => null, buildingBlock);
      }

      public void Edit(T taggedObject, Func<T, DescriptorCriteria> descriptorCriteriaRetriever, Func<T, DescriptorCriteria> descriptorCriteriaCreator, IBuildingBlock buildingBlock)
      {
         _taggedObject = taggedObject;
         _buildingBlock = buildingBlock;
         _descriptorCriteriaRetriever = descriptorCriteriaRetriever;
         _descriptorCriteriaCreator = descriptorCriteriaCreator;
         bindToView();
      }

      private void bindToView()
      {
         _descriptorCriteria = _descriptorCriteriaRetriever(_taggedObject);
         _descriptorCriteriaDTO = _descriptorCriteriaMapper.MapFrom(_descriptorCriteria);
         _view.BindTo(_descriptorCriteriaDTO);
         updateCriteriaDescription();
      }

      private void updateCriteriaDescription()
      {
         _view.CriteriaDescription = _descriptorCriteria?.ToString() ?? string.Empty;
      }

      private TagConditionCommandParameters<T> createCommandParameters() => new TagConditionCommandParameters<T>
      {
         TaggedObject = _taggedObject,
         BuildingBlock = _buildingBlock,
         DescriptorCriteriaCreator = _descriptorCriteriaCreator,
         DescriptorCriteriaRetriever = _descriptorCriteriaRetriever,
      };

      public void RemoveCondition(DescriptorConditionDTO dto)
      {
         AddCommand(_tagTask.RemoveTagCondition(dto.Tag, dto.TagType, createCommandParameters()));
      }

      public void UpdateCriteriaTag(DescriptorConditionDTO descriptorConditionDTO, string newTag)
      {
         AddCommand(_tagTask.EditTag(newTag, descriptorConditionDTO.Tag, createCommandParameters()));
         updateCriteriaDescription();
      }

      private void addCondition(TagType tagType)
      {
         var tag = getNewTagName(tagType);
         if (string.IsNullOrEmpty(tag))
            return;

         AddCommand(_tagTask.AddTagCondition(tag, tagType, createCommandParameters()));
      }

      private string getNewTagName(TagType tagType)
      {
         switch (tagType)
         {
            case TagType.MatchAll:
               return AppConstants.MatchAll;
            case TagType.InParent:
               return AppConstants.InParent;
            case TagType.Match:
               return getNewTagName<MatchTagCondition>(AppConstants.Dialog.NewMatchTag);
            case TagType.NotMatch:
               return getNewTagName<NotMatchTagCondition>(AppConstants.Dialog.NewNotMatchTag);
            case TagType.InContainer:
               return getNewTagName<InContainerCondition>(AppConstants.Dialog.NewInContainerTag);
            case TagType.NotInContainer:
               return getNewTagName<NotInContainerCondition>(AppConstants.Dialog.NewNotInContainerTag);
            default:
               return string.Empty;
         }
      }

      private string getNewTagName<TTagCondition>(string caption) where TTagCondition : ITagCondition
      {
         var forbiddenTags = _descriptorCriteria?.OfType<TTagCondition>().Select(x => x.Tag) ?? Enumerable.Empty<string>();
         return _dialogCreator.AskForInput(caption, AppConstants.Captions.Tag, string.Empty, forbiddenTags, getUsedTags());
      }

      private IEnumerable<string> getUsedTags() => _tagVisitor.AllTags();

      public void NewMatchTagCondition() => addCondition(TagType.Match);

      public void NewMatchAllCondition() => addCondition(TagType.MatchAll);

      public void NewNotMatchTagCondition() => addCondition(TagType.NotMatch);

      public void NewInContainerCondition() => addCondition(TagType.InContainer);

      public void NewNotInContainerCondition() => addCondition(TagType.NotInContainer);

      public void NewInParentCondition() => addCondition(TagType.InParent);

      public void ChangeOperator(CriteriaOperator newOperator)
      {
         AddCommand(_tagTask.EditOperator(newOperator, createCommandParameters()));
         updateCriteriaDescription();
      }

      public IObjectBase Subject => _taggedObject;

      public void Handle(AddTagConditionEvent eventToHandle) => handle(eventToHandle);

      public void Handle(RemoveTagConditionEvent eventToHandle) => handle(eventToHandle);

      private void handle(TagConditionEvent tagConditionEvent)
      {
         if (!Equals(tagConditionEvent.TaggedObject, _taggedObject))
            return;

         bindToView();
      }
   }
}