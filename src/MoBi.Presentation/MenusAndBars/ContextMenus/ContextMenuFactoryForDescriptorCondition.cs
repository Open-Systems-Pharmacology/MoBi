using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public interface IContextMenuFactoryForDescriptorCondition : IContextMenuSpecificationFactory<IViewItem>
   {
   }

   internal class ContextMenuFactoryForDescriptorCondition : IContextMenuFactoryForDescriptorCondition
   {
      public bool IsSatisfiedBy(IViewItem objectRequestingContextMenu, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return presenter.IsAnImplementationOf<IDescriptorConditionListPresenter>() &&
                (objectRequestingContextMenu.IsAnImplementationOf<DescriptorConditionDTO>()
                 || objectRequestingContextMenu.IsAnImplementationOf<IRootViewItem<DescriptorConditionDTO>>());
      }

      public IContextMenu CreateFor(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         var descriptorConditionListPresenter = presenter.DowncastTo<IDescriptorConditionListPresenter>();
         return createContextMenu(viewItem, descriptorConditionListPresenter);
      }

      private IContextMenu createContextMenu(IViewItem viewItem, IDescriptorConditionListPresenter presenter)
      {
         var subject = presenter.Subject;

         var doNotAddAllCondition = (subject.IsAnImplementationOf<SumFormula>() ||
                                     subject.IsAnImplementationOf<ITransportBuilder>() ||
                                     subject.IsAnImplementationOf<IContainerObserverBuilder>());

         return new ContextMenuForDescriptorCondition(presenter, viewItem, allowAddAllCondition: !doNotAddAllCondition);
      }
   }
}