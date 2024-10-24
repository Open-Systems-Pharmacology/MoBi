﻿using System.Collections.Generic;
using MoBi.Assets;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.UICommand;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Assets;
using OSPSuite.Assets.Extensions;
using IContainer = OSPSuite.Core.Domain.IContainer;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   internal class ContextMenuForEventGroupBuilder : ContextMenuFor<EventGroupBuilder>
   {
      private readonly IActiveSubjectRetriever _activeSubujectRetriever;

      public ContextMenuForEventGroupBuilder(IMoBiContext context, IObjectTypeResolver objectTypeResolver, IActiveSubjectRetriever activeSubujectRetriever, OSPSuite.Utility.Container.IContainer container) : base(context, objectTypeResolver, container)
      {
         _activeSubujectRetriever = activeSubujectRetriever;
      }

      protected override IMenuBarItem CreateDeleteItemFor(EventGroupBuilder objectToRemove)
      {
         if (objectToRemove.ParentContainer == null)
         {
            var buildingBlock = _activeSubujectRetriever.Active<EventGroupBuildingBlock>();
            return CreateMenuButton.WithCaption(AppConstants.MenuNames.Delete)
               .WithIcon(ApplicationIcons.Delete)
               .WithRemoveCommand(buildingBlock, objectToRemove);
         }


         return CreateMenuButton.WithCaption(AppConstants.MenuNames.Delete)
            .WithIcon(ApplicationIcons.Delete)
            .WithCommandFor<RemoveEventBuilderFromEventGroupBuilderUICommand, EventGroupBuilder>(objectToRemove, _container);
      }

      public override IContextMenu InitializeWith(ObjectBaseDTO dto, IPresenter presenter)
      {
         _allMenuItems = new List<IMenuBarItem>();

         if (dto == null)
         {
            var buildingBlock = _activeSubujectRetriever.Active<EventGroupBuildingBlock>();
            _allMenuItems.Add(CreateMenuButton.WithCaption(AppConstants.MenuNames.AddNew(ObjectTypes.EventGroupBuilder))
               .WithIcon(ApplicationIcons.Add)
               .WithCommandFor<AddNewCommandFor<EventGroupBuildingBlock, EventGroupBuilder>, EventGroupBuildingBlock>(buildingBlock, _container));

            _allMenuItems.Add(CreateMenuButton.WithCaption(AppConstants.MenuNames.AddExisting(ObjectTypes.EventGroupBuilder))
               .WithCommandFor<AddExistingCommandFor<EventGroupBuildingBlock, EventGroupBuilder>, EventGroupBuildingBlock>(buildingBlock, _container)
               .WithIcon(ApplicationIcons.EventLoad));

            _allMenuItems.Add(CreateMenuButton.WithCaption(AppConstants.MenuNames.AddExistingFromTemplate(ObjectTypes.EventGroupBuilder))
               .WithCommandFor<AddExistingFromTemplateCommandFor<EventGroupBuildingBlock, EventGroupBuilder>, EventGroupBuildingBlock>(buildingBlock, _container));

            _allMenuItems.Add(CreateMenuButton.WithCaption(AppConstants.MenuNames.AddNew(ObjectTypes.Application))
               .WithIcon(ApplicationIcons.Add)
               .WithCommandFor<AddNewCommandFor<EventGroupBuildingBlock, ApplicationBuilder>, EventGroupBuildingBlock>(buildingBlock, _container)
               .AsGroupStarter());

            _allMenuItems.Add(CreateMenuButton.WithCaption(AppConstants.MenuNames.AddExisting(ObjectTypes.Application))
               .WithCommandFor<AddExistingCommandFor<EventGroupBuildingBlock, ApplicationBuilder>, EventGroupBuildingBlock>(buildingBlock, _container)
               .WithIcon(ApplicationIcons.PKMLLoad));

            _allMenuItems.Add(CreateMenuButton.WithCaption(AppConstants.MenuNames.AddExistingFromTemplate(ObjectTypes.Application))
               .WithCommandFor<AddExistingFromTemplateCommandFor<EventGroupBuildingBlock, ApplicationBuilder>, EventGroupBuildingBlock>(buildingBlock, _container));
         }
         else
         {
            var eventGroupBuilder = _context.Get<EventGroupBuilder>(dto.Id);
            base.InitializeWith(dto, presenter);
            _allMenuItems.Add(createAddNewChild<ApplicationBuilder>(eventGroupBuilder).WithIcon(ApplicationIcons.Create).AsGroupStarter());
            _allMenuItems.Add(createAddExistingChild<ApplicationBuilder>(eventGroupBuilder).WithIcon(ApplicationIcons.PKMLLoad));
            _allMenuItems.Add(createAddExistingFromTemplateChild<ApplicationBuilder>(eventGroupBuilder).WithIcon(ApplicationIcons.LoadFromTemplate));
            _allMenuItems.Add(createAddNewChild<EventBuilder>(eventGroupBuilder).WithIcon(ApplicationIcons.EventAdd).AsGroupStarter());
            _allMenuItems.Add(createAddExistingChild<EventBuilder>(eventGroupBuilder).WithIcon(ApplicationIcons.EventLoad));
            _allMenuItems.Add(createAddExistingFromTemplateChild<EventBuilder>(eventGroupBuilder).WithIcon(ApplicationIcons.LoadFromTemplate));
            _allMenuItems.Add(createAddNewChild<EventGroupBuilder>(eventGroupBuilder).WithIcon(ApplicationIcons.Create).AsGroupStarter());
            _allMenuItems.Add(createAddExistingChild<EventGroupBuilder>(eventGroupBuilder).WithIcon(ApplicationIcons.PKMLLoad));
            _allMenuItems.Add(createAddExistingFromTemplateChild<EventGroupBuilder>(eventGroupBuilder).WithIcon(ApplicationIcons.LoadFromTemplate));
            _allMenuItems.Add(createAddNewChild<IContainer>(eventGroupBuilder).WithIcon(ApplicationIcons.ContainerAdd).AsGroupStarter());
            _allMenuItems.Add(createAddExistingChild<IContainer>(eventGroupBuilder).WithIcon(ApplicationIcons.ContainerLoad));
            _allMenuItems.Add(createAddExistingFromTemplateChild<IContainer>(eventGroupBuilder).WithIcon(ApplicationIcons.LoadFromTemplate));
         }
         return this;
      }

      private IMenuBarItem createAddExistingChild<T>(EventGroupBuilder container) where T : class, IEntity
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.AddExisting(_objectTypeResolver.TypeFor<T>()))
            .WithCommandFor<AddExistingCommandFor<EventGroupBuilder, T>, EventGroupBuilder>(container, _container);
      }

      private IMenuBarItem createAddExistingFromTemplateChild<T>(EventGroupBuilder container) where T : class, IEntity
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.AddExistingFromTemplate(_objectTypeResolver.TypeFor<T>()))
            .WithCommandFor<AddExistingFromTemplateCommandFor<EventGroupBuilder, T>, EventGroupBuilder>(container, _container);
      }

      private IMenuBarItem createAddNewChild<T>(EventGroupBuilder container) where T : class, IEntity
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.AddNew(_objectTypeResolver.TypeFor<T>()))
            .WithCommandFor<AddNewCommandFor<EventGroupBuilder, T>, EventGroupBuilder>(container, _container);
      }

      protected virtual IMenuBarItem CreateAddNewItemFor(EventGroupBuilder selectedObject)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.AddNew(ObjectTypes.EventGroupBuilder))
            .WithCommandFor<AddNewCommandFor<EventGroupBuilder, EventGroupBuilder>, EventGroupBuilder>(selectedObject, _container);
      }
   }

   internal class ContextMenuForApplicationBuilder : ContextMenuForEventGroupBuilder
   {
      public ContextMenuForApplicationBuilder(IMoBiContext context, IObjectTypeResolver objectTypeResolver, IActiveSubjectRetriever activeSubjectRetriever, OSPSuite.Utility.Container.IContainer container) : base(context, objectTypeResolver, activeSubjectRetriever, container)
      {
      }

      public override IContextMenu InitializeWith(ObjectBaseDTO dto, IPresenter presenter)
      {
         base.InitializeWith(dto, presenter);
         var applicationBuilder = _context.Get<ApplicationBuilder>(dto.Id);
         _allMenuItems.Add(createAddNewPassiveTranportBuilder(applicationBuilder).AsGroupStarter());
         _allMenuItems.Add(createAddExistingPassiveTranportBuilder(applicationBuilder));

         return this;
      }

      private IMenuBarItem createAddNewPassiveTranportBuilder(ApplicationBuilder container)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.AddNew(ObjectTypes.ApplicationTransport))
            .WithCommandFor<AddNewPassiveTransportToApplicationBuilderUICommand, ApplicationBuilder>(container, _container)
            .WithIcon(ApplicationIcons.Add);
      }

      private IMenuBarItem createAddExistingPassiveTranportBuilder(ApplicationBuilder container)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.AddExisting(ObjectTypes.ApplicationTransport))
            .WithCommandFor<AddExistingPassiveTransportToApplicationBuilderUICommand, ApplicationBuilder>(container, _container)
            .WithIcon(ApplicationIcons.PKMLLoad);
      }

      protected override IMenuBarItem CreateDeleteItemFor(EventGroupBuilder objectToRemove)
      {
         if (objectToRemove.ParentContainer == null)
            return base.CreateDeleteItemFor(objectToRemove);
         else
         {
            return CreateMenuButton.WithCaption(AppConstants.MenuNames.Delete)
               .WithIcon(ApplicationIcons.Delete)
               .WithCommandFor<RemoveEventBuilderFromEventGroupBuilderUICommand, EventGroupBuilder>(objectToRemove, _container);
         }
      }
   }

   internal interface IContextMenuSpecificationFactoryForApplicationMoleculeBuilder : IContextMenuSpecificationFactory<IViewItem>
   {
   }

   internal class ContextMenuSpecificationFactoryForApplicationMoleculeBuilder : IContextMenuSpecificationFactoryForApplicationMoleculeBuilder
   {
      public IContextMenu CreateFor(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         var contestMenu = IoC.Resolve<IContextMenuForApplicationMoleculeBuilder>();
         var editPresenter = presenter.DowncastTo<IEditApplicationBuilderPresenter>();
         contestMenu.InitialisedWith(viewItem as ObjectBaseDTO, editPresenter.Subject as ApplicationBuilder);
         return contestMenu;
      }

      public bool IsSatisfiedBy(IViewItem objectRequestingContextMenu, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return (objectRequestingContextMenu == null && presenter.IsAnImplementationOf<IEditApplicationBuilderPresenter>())
                || objectRequestingContextMenu.IsAnImplementationOf<ApplicationMoleculeBuilderDTO>();
      }
   }

   internal interface IContextMenuForApplicationMoleculeBuilder : IContextMenu
   {
      IContextMenu InitialisedWith(ObjectBaseDTO dto, ApplicationBuilder parent);
   }

   internal class ContextMenuForApplicationMoleculeBuilder : ContextMenuBase, IContextMenuForApplicationMoleculeBuilder
   {
      private IList<IMenuBarItem> _allMenuItems;
      private readonly IMoBiContext _context;
      private readonly OSPSuite.Utility.Container.IContainer _container;

      public ContextMenuForApplicationMoleculeBuilder(IMoBiContext context, OSPSuite.Utility.Container.IContainer container)
      {
         _context = context;
         _container = container;
      }

      public IContextMenu InitialisedWith(ObjectBaseDTO dto, ApplicationBuilder parent)
      {
         _allMenuItems = new List<IMenuBarItem>();
         _allMenuItems.Add(CreateMenuButton.WithCaption(AppConstants.MenuNames.AddNew(ObjectTypes.ApplicationMoleculeBuilder))
            .WithIcon(ApplicationIcons.Add)
            .WithCommandFor<AddNewApplicationMoleculeBuilderUICommand, ApplicationBuilder>(parent, _container));

         if (dto != null)
         {
            var applicationMoleculeBuilder = _context.Get<ApplicationMoleculeBuilder>(dto.Id);
            var removeMenuItem = CreateMenuButton.WithCaption(AppConstants.MenuNames.Delete)
               .WithIcon(ApplicationIcons.Delete);

            removeMenuItem.Command = IoC.Resolve<RemoveApplicationMoleculeBuilderUICommand>().Initialze(applicationMoleculeBuilder, parent);
            _allMenuItems.Add(removeMenuItem);
         }
         return this;
      }

      public override IEnumerable<IMenuBarItem> AllMenuItems()
      {
         return _allMenuItems;
      }
   }

   internal class ContextMenuSpecificationFactoryForEventBuilder : IContextMenuSpecificationFactory<IViewItem>
   {
      public IContextMenu CreateFor(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return IoC.Resolve<IContextMenuForEventBuilder>().InitializeWith(viewItem as EventBuilderDTO, null);
      }

      public bool IsSatisfiedBy(IViewItem objectRequestingContextMenu, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return objectRequestingContextMenu.IsAnImplementationOf<EventBuilderDTO>();
      }
   }
}