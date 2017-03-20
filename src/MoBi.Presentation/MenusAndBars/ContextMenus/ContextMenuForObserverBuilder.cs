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

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public interface IContextMenuForAmountObserverBuilder : IContextMenuFor<IAmountObserverBuilder>
   {
   }

   public interface IContextMenuForContainerObserverBuilder : IContextMenuFor<IContainerObserverBuilder>
   {
   }

   public class ContextMenuForContainerObserverBuilder : ContextMenuForObserverBuilder<IContainerObserverBuilder>, IContextMenuForContainerObserverBuilder
   {
      public ContextMenuForContainerObserverBuilder(
         IMoBiContext context,
         IObjectTypeResolver objectTypeResolver,
         IActiveSubjectRetriever activeSubjectRetriever) : base(context, objectTypeResolver, activeSubjectRetriever)
      {
      }
   }

   public class ContextMenuForAmountObserverBuilder : ContextMenuForObserverBuilder<IAmountObserverBuilder>, IContextMenuForAmountObserverBuilder
   {
      public ContextMenuForAmountObserverBuilder(
         IMoBiContext context,
         IObjectTypeResolver objectTypeResolver,
         IActiveSubjectRetriever activeSubjectRetriever) : base(context, objectTypeResolver, activeSubjectRetriever)
      {
      }
   }

   public class ContextMenuForObserverBuilder<T> : ContextMenuFor<T> where T : class, IObjectBase
   {
      private readonly IActiveSubjectRetriever _activeSubjectRetriever;

      public ContextMenuForObserverBuilder(IMoBiContext context, IObjectTypeResolver objectTypeResolver, IActiveSubjectRetriever activeSubjectRetriever)
         : base(context, objectTypeResolver)
      {
         _activeSubjectRetriever = activeSubjectRetriever;
      }

      protected override IMenuBarItem CreateDeleteItemFor(T objectToRemove)
      {
         var buildingBlock = _activeSubjectRetriever.Active<IObserverBuildingBlock>();
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
            return IoC.Resolve<IRootContextMenuFor<IObserverBuildingBlock, IAmountObserverBuilder>>().InitializeWith(presenter);
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
            return IoC.Resolve<IRootContextMenuFor<IObserverBuildingBlock, IContainerObserverBuilder>>().InitializeWith(presenter);
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