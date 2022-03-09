﻿using System;
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
      void UpdateCriteriaTag(IDescriptorConditionDTO descriptorConditionDTO, string newTag);
      IObjectBase Subject { get; }
      void RemoveCondition(IDescriptorConditionDTO descriptorConditionDTO);
      void NewMatchTagCondition();
      void NewMatchAllCondition();
      void NewNotMatchTagCondition();
      void NewInContainerCondition();
      void NewNotInContainerCondition();
   }

   public interface IDescriptorConditionListPresenter<T> : IDescriptorConditionListPresenter where T : IObjectBase
   {
      void Edit(T taggedObject, Func<T, DescriptorCriteria> criteriaRetriever, IBuildingBlock buildingBlock);

      /// <summary>
      ///    When criteria is not defined in the object, it can be useful to allow the caller to specify a way to set a criteria
      /// </summary>
      Func<T, DescriptorCriteria> DescriptorCriteriaCreator { get; set; }
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

      //by default we set a dummy implementation
      public Func<T, DescriptorCriteria> DescriptorCriteriaCreator { get; set; } = x => null;

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

      private TagConditionCommandParameters<T> createCommandParameters() => new TagConditionCommandParameters<T>
      {
         TaggedObject = _taggedObject,
         BuildingBlock = _buildingBlock,
         DescriptorCriteriaCreator = DescriptorCriteriaCreator,
         DescriptorCriteriaRetriever = _descriptorCriteriaRetriever,
      };

      public void RemoveCondition(IDescriptorConditionDTO dto)
      {
         AddCommand(_tagTask.RemoveTagCondition(dto.Tag, dto.TagType, createCommandParameters()));
      }

      public void UpdateCriteriaTag(IDescriptorConditionDTO descriptorConditionDTO, string newTag)
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
         var forbiddenTags = _descriptorCriteria.OfType<TTagCondition>().Select(x => x.Tag);
         return _dialogCreator.AskForInput(caption, AppConstants.Captions.Tag, string.Empty, forbiddenTags, getUsedTags());
      }

      private IEnumerable<string> getUsedTags() => _tagVisitor.AllTags();

      public void NewMatchTagCondition() => addCondition(TagType.Match);

      public void NewMatchAllCondition() => addCondition(TagType.MatchAll);

      public void NewNotMatchTagCondition() => addCondition(TagType.NotMatch);

      public void NewInContainerCondition() => addCondition(TagType.InContainer);

      public void NewNotInContainerCondition() => addCondition(TagType.NotInContainer);

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