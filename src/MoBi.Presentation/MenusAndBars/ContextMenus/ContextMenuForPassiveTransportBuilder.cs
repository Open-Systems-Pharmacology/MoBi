using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.UICommand;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Assets;
using OSPSuite.Utility.Container;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public abstract class ContextMenuSpecificationFactoryForTransportBuilderBase<TPresenter> : IContextMenuSpecificationFactory<IViewItem>
      where TPresenter : IPresenter
   {
      protected readonly IMoBiContext _context;

      protected ContextMenuSpecificationFactoryForTransportBuilderBase(IMoBiContext context)
      {
         _context = context;
      }

      public IContextMenu CreateFor(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return CreateFor(viewItem.DowncastTo<TransportBuilderDTO>(), presenter.DowncastTo<TPresenter>());
      }

      public abstract IContextMenu CreateFor(TransportBuilderDTO transportBuilderDTO, TPresenter presenter);

      public bool IsSatisfiedBy(IViewItem objectRequestingContextMenu, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return objectRequestingContextMenu.IsAnImplementationOf<TransportBuilderDTO>() && presenter.IsAnImplementationOf<TPresenter>();
      }
   }

   public class ContextMenuSpecificationFactoryForPassiveTransportBuilder : ContextMenuSpecificationFactoryForTransportBuilderBase<IEditPassiveTransportBuildingBlockPresenter>
   {
      private readonly IContainer _container;

      public ContextMenuSpecificationFactoryForPassiveTransportBuilder(IMoBiContext context, IContainer container)
         : base(context)
      {
         _container = container;
      }

      public override IContextMenu CreateFor(TransportBuilderDTO transportBuilderDTO, IEditPassiveTransportBuildingBlockPresenter presenter)
      {
         return new ContextMenuForPassiveTransportBuilder(_context, _container).InitializeWith(transportBuilderDTO, presenter);
      }
   }

   public class ContextMenuSpecificationFactoryForPassiveTransportBuilderAtEventGroup : ContextMenuSpecificationFactoryForTransportBuilderBase<IEventGroupListPresenter>
   {
      private readonly IContainer _container;

      public ContextMenuSpecificationFactoryForPassiveTransportBuilderAtEventGroup(IMoBiContext context, IContainer container) : base(context)
      {
         _container = container;
      }

      public override IContextMenu CreateFor(TransportBuilderDTO transportBuilderDTO, IEventGroupListPresenter presenter)
      {
         return new ContextMenuForTransportBuilderAtEventGroup(_context, _container).InitializeWith(transportBuilderDTO, presenter);
      }
   }

   public class ContextMenuSpecificationFactoryForTransportBuilderAtMoleculeBuilder : ContextMenuSpecificationFactoryForTransportBuilderBase<IMoleculeListPresenter>
   {
      private readonly IContainer _container;

      public ContextMenuSpecificationFactoryForTransportBuilderAtMoleculeBuilder(IMoBiContext context, IContainer container)
         : base(context)
      {
         _container = container;
      }

      public override IContextMenu CreateFor(TransportBuilderDTO transportBuilderDTO, IMoleculeListPresenter presenter)
      {
         return new ContextMenuForTransportBuilderAtMoleculeBuilder(_context, _container).InitializeWith(transportBuilderDTO, presenter);
      }
   }

   public abstract class ContextMenuForTransportBuilderBase : ContextMenuBase
   {
      private IList<IMenuBarItem> _allMenuItems;
      protected readonly IMoBiContext _context;
      private readonly IContainer _container;

      protected ContextMenuForTransportBuilderBase(IMoBiContext context, IContainer container)
      {
         _context = context;
         _container = container;
      }

      public override IEnumerable<IMenuBarItem> AllMenuItems()
      {
         return _allMenuItems;
      }

      public IContextMenu InitializeWith(TransportBuilderDTO dto, IPresenter presenter)
      {
         var transportBuilder = _context.Get<ITransportBuilder>(dto.Id);
         _allMenuItems = new List<IMenuBarItem>
         {
            createEditItemFor(transportBuilder),
            CreateRenameItemFor(transportBuilder),
            CreateRemoveItemFor(transportBuilder),
            createSaveItemFor(transportBuilder),
         };
         return this;
      }

      private IMenuBarItem createSaveItemFor(ITransportBuilder transportBuilder)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.SaveAsPKML)
            .WithIcon(ApplicationIcons.PKMLSave)
            .WithCommandFor<SaveUICommandFor<ITransportBuilder>, ITransportBuilder>(transportBuilder, _container);
      }

      private IMenuBarItem createEditItemFor(ITransportBuilder transportBuilder)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.Edit)
            .WithCommandFor<EditCommandFor<ITransportBuilder>, ITransportBuilder>(transportBuilder, _container)
            .WithIcon(ApplicationIcons.Edit);
      }

      protected abstract IMenuBarItem CreateRemoveItemFor(ITransportBuilder transportBuilder);

      protected IMenuBarItem CreateRenameItemFor(ITransportBuilder transportBuilder)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.Rename)
            .WithCommandFor<RenameObjectCommand<ITransportBuilder>, ITransportBuilder>(transportBuilder, _container)
            .WithIcon(ApplicationIcons.Rename);
      }
   }

   internal class ContextMenuForTransportBuilderAtMoleculeBuilder : ContextMenuForTransportBuilderBase
   {
      public ContextMenuForTransportBuilderAtMoleculeBuilder(IMoBiContext context, IContainer container) : base(context, container)
      {
      }

      protected override IMenuBarItem CreateRemoveItemFor(ITransportBuilder transportBuilder)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.Delete)
            .WithRemoveCommand(transportBuilder.ParentContainer.DowncastTo<TransporterMoleculeContainer>(), transportBuilder)
            .WithIcon(ApplicationIcons.Delete);
      }
   }

   public class ContextMenuForPassiveTransportBuilder : ContextMenuForTransportBuilderBase
   {
      public ContextMenuForPassiveTransportBuilder(IMoBiContext context, IContainer container) : base(context, container)
      {
      }

      protected override IMenuBarItem CreateRemoveItemFor(ITransportBuilder transportBuilder)
      {
         var buildingblock = _context.CurrentProject.PassiveTransportCollection.FirstOrDefault(x => x.Contains(transportBuilder));
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.Delete)
            .WithRemoveCommand(buildingblock, transportBuilder)
            .WithIcon(ApplicationIcons.Delete);
      }
   }

   internal class ContextMenuForTransportBuilderAtEventGroup : ContextMenuForTransportBuilderBase
   {
      public ContextMenuForTransportBuilderAtEventGroup(IMoBiContext context, IContainer container) : base(context, container)
      {
      }

      protected override IMenuBarItem CreateRemoveItemFor(ITransportBuilder transportBuilder)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.Delete)
            .WithRemoveCommand(transportBuilder.ParentContainer.DowncastTo<IApplicationBuilder>(), transportBuilder)
            .WithIcon(ApplicationIcons.Delete);
      }
   }
}