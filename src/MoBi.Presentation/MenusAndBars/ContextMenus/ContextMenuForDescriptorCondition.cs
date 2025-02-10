using System.Collections.Generic;
using MoBi.Assets;
using OSPSuite.Presentation.MenuAndBars;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Core;
using OSPSuite.Assets;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public class ContextMenuForDescriptorCondition : ContextMenuBase
   {
      protected readonly IDescriptorConditionListPresenter _presenter;
      private readonly IViewItem _viewItem;
      private readonly bool _allowAddAllCondition;

      public ContextMenuForDescriptorCondition(IDescriptorConditionListPresenter presenter, IViewItem viewItem, bool allowAddAllCondition = false)
      {
         _presenter = presenter;
         _viewItem = viewItem;
         _allowAddAllCondition = allowAddAllCondition;
      }

      protected virtual IMenuBarItem CreateRemoveCommand(DescriptorConditionDTO dto)
      {
         return CreateMenuButton.WithCaption(AppConstants.Captions.RemoveCondition)
            .WithIcon(ApplicationIcons.Delete)
            .WithActionCommand(() => _presenter.RemoveCondition(dto));
      }

      protected virtual IMenuBarItem CreateAddNewMatchCondition()
      {
         return CreateMenuButton.WithCaption(AppConstants.Captions.NewMatchTagCondition)
            .WithActionCommand(() => _presenter.NewMatchTagCondition());
      }

      protected virtual IMenuBarItem CreateAddAllCondition()
      {
         return CreateMenuButton.WithCaption(AppConstants.Captions.AddMatchAllCondition)
            .WithActionCommand(() => _presenter.NewMatchAllCondition());
      }

      protected virtual IMenuBarItem CreateAddInParentCondition()
      {
         return CreateMenuButton.WithCaption(AppConstants.Captions.AddInParentCondition)
            .WithActionCommand(() => _presenter.NewInParentCondition());
      }

      protected virtual IMenuBarItem CreateAddInChildrenCondition()
      {
         return CreateMenuButton.WithCaption(AppConstants.Captions.AddInChildrenCondition)
            .WithActionCommand(() => _presenter.NewInChildrenCondition());
      }

      protected virtual IMenuBarItem CreateAddNewNotMatchTagCondition()
      {
         return CreateMenuButton.WithCaption(AppConstants.Captions.NewNotMatchTagCondition)
            .WithActionCommand(() => _presenter.NewNotMatchTagCondition());
      }

      protected virtual IMenuBarItem CreateAddNewInContainerCondition()
      {
         return CreateMenuButton.WithCaption(AppConstants.Captions.NewInContainerCondition)
            .WithActionCommand(() => _presenter.NewInContainerCondition());
      }


      protected virtual IMenuBarItem CreateAddNewNotInContainerCondition()
      {
         return CreateMenuButton.WithCaption(AppConstants.Captions.NewNotInContainerCondition)
            .WithActionCommand(() => _presenter.NewNotInContainerCondition());
      }


      public override IEnumerable<IMenuBarItem> AllMenuItems()
      {
         var allItems = new List<IMenuBarItem>
         {
            CreateAddNewMatchCondition(),
            CreateAddNewNotMatchTagCondition(),
            CreateAddNewInContainerCondition().AsGroupStarter(),
            CreateAddNewNotInContainerCondition(),
            CreateAddInParentCondition().AsGroupStarter(),
            CreateAddInChildrenCondition()
         };

         if (_allowAddAllCondition)
            allItems.Add(CreateAddAllCondition().AsGroupStarter());

         var dto = _viewItem as DescriptorConditionDTO;
         if (dto != null)
            allItems.Add(CreateRemoveCommand(dto).AsGroupStarter());

         return allItems;
      }
   }
}