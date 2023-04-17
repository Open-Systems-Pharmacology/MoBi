using MoBi.Assets;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Assets;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public interface IContextMenuForAmountObserverBuilder : IContextMenuFor<AmountObserverBuilder>
   {
   }

   public interface IContextMenuForContainerObserverBuilder : IContextMenuFor<ContainerObserverBuilder>
   {
   }

   public class ContextMenuForContainerObserverBuilder : ContextMenuForObserverBuilder<ContainerObserverBuilder>, IContextMenuForContainerObserverBuilder
   {
      public ContextMenuForContainerObserverBuilder(IMoBiContext context,
         IObjectTypeResolver objectTypeResolver,
         IActiveSubjectRetriever activeSubjectRetriever, IContainer container) : base(context, objectTypeResolver, activeSubjectRetriever, container)
      {
      }
   }

   public class ContextMenuForAmountObserverBuilder : ContextMenuForObserverBuilder<AmountObserverBuilder>, IContextMenuForAmountObserverBuilder
   {
      public ContextMenuForAmountObserverBuilder(IMoBiContext context,
         IObjectTypeResolver objectTypeResolver,
         IActiveSubjectRetriever activeSubjectRetriever, IContainer container) : base(context, objectTypeResolver, activeSubjectRetriever, container)
      {
      }
   }

   public class ContextMenuForObserverBuilder<T> : ContextMenuFor<T> where T : class, IObjectBase
   {
      private readonly IActiveSubjectRetriever _activeSubjectRetriever;

      public ContextMenuForObserverBuilder(IMoBiContext context, IObjectTypeResolver objectTypeResolver, IActiveSubjectRetriever activeSubjectRetriever, IContainer container)
         : base(context, objectTypeResolver, container)
      {
         _activeSubjectRetriever = activeSubjectRetriever;
      }

      protected override IMenuBarItem CreateDeleteItemFor(T objectToRemove)
      {
         var buildingBlock = _activeSubjectRetriever.Active<ObserverBuildingBlock>();
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.Delete)
            .WithIcon(ApplicationIcons.Delete)
            .WithRemoveCommand(buildingBlock, objectToRemove);
      }
   }

   internal class ContextMenuSpecificationFactoryForAmountObserver : IContextMenuSpecificationFactory<IViewItem>
   {
      public IContextMenu CreateFor(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         if (viewItem.IsAnImplementationOf<AmountObserverBuilderRootItem>())
         {
            return IoC.Resolve<IRootContextMenuFor<ObserverBuildingBlock, AmountObserverBuilder>>().InitializeWith(presenter);
         }
         return IoC.Resolve<IContextMenuForAmountObserverBuilder>().InitializeWith(viewItem as ObserverBuilderDTO, presenter);
      }

      public bool IsSatisfiedBy(IViewItem objectRequestingContextMenu, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return objectRequestingContextMenu.IsAnImplementationOf<ObserverBuilderDTO>() &&
                presenter.IsAnImplementationOf<IAmountObserverBuilderListPresenter>();
      }
   }

   internal class ContextMenuSpecificationFactoryForContainerObserver : IContextMenuSpecificationFactory<IViewItem>
   {
      public IContextMenu CreateFor(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         if (viewItem.IsAnImplementationOf<AmountObserverBuilderRootItem>())
         {
            return IoC.Resolve<IRootContextMenuFor<ObserverBuildingBlock, ContainerObserverBuilder>>().InitializeWith(presenter);
         }
         return IoC.Resolve<IContextMenuForContainerObserverBuilder>().InitializeWith(viewItem as ObserverBuilderDTO, presenter);
      }

      public bool IsSatisfiedBy(IViewItem objectRequestingContextMenu, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return objectRequestingContextMenu.IsAnImplementationOf<ObserverBuilderDTO>() &&
                presenter.IsAnImplementationOf<IContainerObserverBuilderListPresenter>();
      }
   }
}