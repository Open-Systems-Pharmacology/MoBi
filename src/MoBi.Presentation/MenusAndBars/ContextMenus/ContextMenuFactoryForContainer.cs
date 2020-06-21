using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;
using IContainer = OSPSuite.Core.Domain.IContainer;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public class ContextMenuFactoryForContainer : ContextMenuForObjectBaseDTOSpecificationFactory<IContainer>
   {
      public ContextMenuFactoryForContainer(IMoBiContext context) : base(context)
      {
      }

      public override IContextMenu CreateFor(IObjectBaseDTO objectBaseDTO, IPresenterWithContextMenu<IViewItem> presenter)
      {
         var contextMenu = IoC.Resolve<IContextMenuForContainer>();
         return contextMenu.InitializeWith(objectBaseDTO, presenter);
      }

      protected override bool IsSatisfiedBy(IEntity entity, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return presenter.IsAnImplementationOf<IHierarchicalSpatialStructurePresenter>()
                && entity.IsAnImplementationOf<IContainer>()
                && !entity.IsAnImplementationOf<INeighborhoodBuilder>();
      }
   }
}