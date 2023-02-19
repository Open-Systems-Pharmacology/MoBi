using System.Collections.Generic;
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
   internal class ContextMenuForEventGroupBuilder : ContextMenuFor<IEventGroupBuilder>
   {
      private readonly IActiveSubjectRetriever _activeSubujectRetriever;

      public ContextMenuForEventGroupBuilder(IMoBiContext context, IObjectTypeResolver objectTypeResolver, IActiveSubjectRetriever activeSubujectRetriever) : base(context, objectTypeResolver)
      {
         _activeSubujectRetriever = activeSubujectRetriever;
      }

      protected override IMenuBarItem CreateDeleteItemFor(IEventGroupBuilder objectToRemove)
      {
         if (objectToRemove.ParentContainer == null)
         {
            var buildingBlock = _activeSubujectRetriever.Active<IEventGroupBuildingBlock>();
            return CreateMenuButton.WithCaption(AppConstants.MenuNames.Delete)
               .WithIcon(ApplicationIcons.Delete)
               .WithRemoveCommand(buildingBlock, objectToRemove);
         }


         return CreateMenuButton.WithCaption(AppConstants.MenuNames.Delete)
            .WithIcon(ApplicationIcons.Delete)
            .WithCommandFor<RemoveEventBuilderFromEventGroupBuilderUICommand, IEventGroupBuilder>(objectToRemove);
      }

      public override IContextMenu InitializeWith(ObjectBaseDTO dto, IPresenter presenter)
      {
         _allMenuItems = new List<IMenuBarItem>();

         if (dto == null)
         {
            var buildingBlock = _activeSubujectRetriever.Active<IEventGroupBuildingBlock>();
            _allMenuItems.Add(CreateMenuButton.WithCaption(AppConstants.MenuNames.AddNew(ObjectTypes.EventGroupBuilder))
               .WithIcon(ApplicationIcons.Add)
               .WithCommandFor<AddNewCommandFor<IEventGroupBuildingBlock, IEventGroupBuilder>, IEventGroupBuildingBlock>(buildingBlock));

            _allMenuItems.Add(CreateMenuButton.WithCaption(AppConstants.MenuNames.AddExisting(ObjectTypes.EventGroupBuilder))
               .WithCommandFor<AddExistingCommandFor<IEventGroupBuildingBlock, IEventGroupBuilder>, IEventGroupBuildingBlock>(buildingBlock)
               .WithIcon(ApplicationIcons.EventLoad));

            _allMenuItems.Add(CreateMenuButton.WithCaption(AppConstants.MenuNames.AddExistingFromTemplate(ObjectTypes.EventGroupBuilder))
               .WithCommandFor<AddExistingFromTemplateCommandFor<IEventGroupBuildingBlock, IEventGroupBuilder>, IEventGroupBuildingBlock>(buildingBlock));

            if (!buildingBlock.ExistsByName(Constants.APPLICATIONS))
            {
               _allMenuItems.Add(CreateMenuButton.WithCaption(AppConstants.MenuNames.AddNew(Constants.APPLICATIONS))
                  .WithIcon(ApplicationIcons.Applications)
                  .WithCommandFor<AddNewApplicationsEventGroup, IEventGroupBuildingBlock>(buildingBlock)
                  .AsGroupStarter());
            }

            _allMenuItems.Add(CreateMenuButton.WithCaption(AppConstants.MenuNames.AddNew(ObjectTypes.Application))
               .WithIcon(ApplicationIcons.Add)
               .WithCommandFor<AddNewCommandFor<IEventGroupBuildingBlock, IApplicationBuilder>, IEventGroupBuildingBlock>(buildingBlock)
               .AsGroupStarter());

            _allMenuItems.Add(CreateMenuButton.WithCaption(AppConstants.MenuNames.AddExisting(ObjectTypes.Application))
               .WithCommandFor<AddExistingCommandFor<IEventGroupBuildingBlock, IApplicationBuilder>, IEventGroupBuildingBlock>(buildingBlock)
               .WithIcon(ApplicationIcons.PKMLLoad));

            _allMenuItems.Add(CreateMenuButton.WithCaption(AppConstants.MenuNames.AddExistingFromTemplate(ObjectTypes.Application))
               .WithCommandFor<AddExistingFromTemplateCommandFor<IEventGroupBuildingBlock, IApplicationBuilder>, IEventGroupBuildingBlock>(buildingBlock));
         }
         else
         {
            var eventGroupBuilder = _context.Get<IEventGroupBuilder>(dto.Id);
            base.InitializeWith(dto, presenter);
            _allMenuItems.Add(createAddNewChild<IApplicationBuilder>(eventGroupBuilder).WithIcon(ApplicationIcons.Create).AsGroupStarter());
            _allMenuItems.Add(createAddExistingChild<IApplicationBuilder>(eventGroupBuilder).WithIcon(ApplicationIcons.PKMLLoad));
            _allMenuItems.Add(createAddExistingFromTemplateChild<IApplicationBuilder>(eventGroupBuilder).WithIcon(ApplicationIcons.LoadFromTemplate));
            _allMenuItems.Add(createAddNewChild<IEventBuilder>(eventGroupBuilder).WithIcon(ApplicationIcons.EventAdd).AsGroupStarter());
            _allMenuItems.Add(createAddExistingChild<IEventBuilder>(eventGroupBuilder).WithIcon(ApplicationIcons.EventLoad));
            _allMenuItems.Add(createAddExistingFromTemplateChild<IEventBuilder>(eventGroupBuilder).WithIcon(ApplicationIcons.LoadFromTemplate));
            _allMenuItems.Add(createAddNewChild<IEventGroupBuilder>(eventGroupBuilder).WithIcon(ApplicationIcons.Create).AsGroupStarter());
            _allMenuItems.Add(createAddExistingChild<IEventGroupBuilder>(eventGroupBuilder).WithIcon(ApplicationIcons.PKMLLoad));
            _allMenuItems.Add(createAddExistingFromTemplateChild<IEventGroupBuilder>(eventGroupBuilder).WithIcon(ApplicationIcons.LoadFromTemplate));
            _allMenuItems.Add(createAddNewChild<IContainer>(eventGroupBuilder).WithIcon(ApplicationIcons.ContainerAdd).AsGroupStarter());
            _allMenuItems.Add(createAddExistingChild<IContainer>(eventGroupBuilder).WithIcon(ApplicationIcons.ContainerLoad));
            _allMenuItems.Add(createAddExistingFromTemplateChild<IContainer>(eventGroupBuilder).WithIcon(ApplicationIcons.LoadFromTemplate));
         }
         return this;
      }

      private IMenuBarItem createAddExistingChild<T>(IEventGroupBuilder container) where T : class, IEntity
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.AddExisting(_objectTypeResolver.TypeFor<T>()))
            .WithCommandFor<AddExistingCommandFor<IEventGroupBuilder, T>, IEventGroupBuilder>(container);
      }

      private IMenuBarItem createAddExistingFromTemplateChild<T>(IEventGroupBuilder container) where T : class, IEntity
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.AddExistingFromTemplate(_objectTypeResolver.TypeFor<T>()))
            .WithCommandFor<AddExistingFromTemplateCommandFor<IEventGroupBuilder, T>, IEventGroupBuilder>(container);
      }

      private IMenuBarItem createAddNewChild<T>(IEventGroupBuilder container) where T : class, IEntity
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.AddNew(_objectTypeResolver.TypeFor<T>()))
            .WithCommandFor<AddNewCommandFor<IEventGroupBuilder, T>, IEventGroupBuilder>(container);
      }

      protected virtual IMenuBarItem CreateAddNewItemFor(IEventGroupBuilder selectedObject)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.AddNew(ObjectTypes.EventGroupBuilder))
            .WithCommandFor<AddNewCommandFor<IEventGroupBuilder, IEventGroupBuilder>, IEventGroupBuilder>(selectedObject);
      }
   }

   internal class ContextMenuForApplicationBuilder : ContextMenuForEventGroupBuilder
   {
      public ContextMenuForApplicationBuilder(IMoBiContext context, IObjectTypeResolver objectTypeResolver, IActiveSubjectRetriever activeSubjectRetriever) : base(context, objectTypeResolver, activeSubjectRetriever)
      {
      }

      public override IContextMenu InitializeWith(ObjectBaseDTO dto, IPresenter presenter)
      {
         base.InitializeWith(dto, presenter);
         var applicationBuilder = _context.Get<IApplicationBuilder>(dto.Id);
         _allMenuItems.Add(createAddNewPassiveTranportBuilder(applicationBuilder).AsGroupStarter());
         _allMenuItems.Add(createAddExistingPassiveTranportBuilder(applicationBuilder));

         return this;
      }

      private IMenuBarItem createAddNewPassiveTranportBuilder(IApplicationBuilder container)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.AddNew(ObjectTypes.ApplicationTransport))
            .WithCommandFor<AddNewPassiveTransportToApplicationBuilderUICommand, IApplicationBuilder>(container)
            .WithIcon(ApplicationIcons.Add);
      }

      private IMenuBarItem createAddExistingPassiveTranportBuilder(IApplicationBuilder container)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.AddExisting(ObjectTypes.ApplicationTransport))
            .WithCommandFor<AddExistingPassiveTransportToApplicationBuilderUICommand, IApplicationBuilder>(container)
            .WithIcon(ApplicationIcons.PKMLLoad);
      }

      protected override IMenuBarItem CreateDeleteItemFor(IEventGroupBuilder objectToRemove)
      {
         if (objectToRemove.ParentContainer == null)
            return base.CreateDeleteItemFor(objectToRemove);
         else
         {
            return CreateMenuButton.WithCaption(AppConstants.MenuNames.Delete)
               .WithIcon(ApplicationIcons.Delete)
               .WithCommandFor<RemoveEventBuilderFromEventGroupBuilderUICommand, IEventGroupBuilder>(objectToRemove);
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
         contestMenu.InitialisedWith(viewItem as ObjectBaseDTO, editPresenter.Subject as IApplicationBuilder);
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
      IContextMenu InitialisedWith(ObjectBaseDTO dto, IApplicationBuilder parent);
   }

   internal class ContextMenuForApplicationMoleculeBuilder : ContextMenuBase, IContextMenuForApplicationMoleculeBuilder
   {
      private IList<IMenuBarItem> _allMenuItems;
      private readonly IMoBiContext _context;

      public ContextMenuForApplicationMoleculeBuilder(IMoBiContext context)
      {
         _context = context;
      }

      public IContextMenu InitialisedWith(ObjectBaseDTO dto, IApplicationBuilder parent)
      {
         _allMenuItems = new List<IMenuBarItem>();
         _allMenuItems.Add(CreateMenuButton.WithCaption(AppConstants.MenuNames.AddNew(ObjectTypes.ApplicationMoleculeBuilder))
            .WithIcon(ApplicationIcons.Add)
            .WithCommandFor<AddNewApplicationMoleculeBuilderUICommand, IApplicationBuilder>(parent));

         if (dto != null)
         {
            var applicationMoleculeBuilder = _context.Get<IApplicationMoleculeBuilder>(dto.Id);
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