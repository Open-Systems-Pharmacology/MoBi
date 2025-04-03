using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.UICommand;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public class ContextMenuFactoryForNeighborhoods : ContextMenuForObjectBaseDTOSpecificationFactory<IContainer>
   {
      public ContextMenuFactoryForNeighborhoods(IMoBiContext context) : base(context)
      {
      }

      public override IContextMenu CreateFor(ObjectBaseDTO objectBaseDTO, IPresenterWithContextMenu<IViewItem> presenter)
      {
         var contextMenu = _context.Resolve<ContextMenuForNeighborhoods>();
         return contextMenu.InitializeWith(objectBaseDTO, presenter);
      }

      protected override bool IsSatisfiedBy(IEntity entity, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return presenter.IsAnImplementationOf<IHierarchicalSpatialStructurePresenter>()
                && entity.IsAnImplementationOf<IContainer>()
                && entity.IsNamed(Constants.NEIGHBORHOODS)
                && entity.ParentContainer == null;
      }
   }

   public class ContextMenuForNeighborhoods : ContextMenuForContainerBase<IContainer>
   {
      public ContextMenuForNeighborhoods(
         IMoBiContext context,
         IObjectTypeResolver objectTypeResolver,
         OSPSuite.Utility.Container.IContainer container) : base(context, objectTypeResolver, container)
      {
      }

      public override IContextMenu InitializeWith(ObjectBaseDTO dto, IPresenter presenter)
      {
         base.InitializeWith(dto, presenter);
         var neighborhoods = _context.Get<IContainer>(dto.Id);
         _allMenuItems.Add(createAddExistingChild(neighborhoods));
         _allMenuItems.Add(CreateAddNewChild<NeighborhoodBuilder>(neighborhoods).WithIcon(ApplicationIcons.Neighborhood).AsGroupStarter());
         return this;
      }

      private IMenuBarItem createAddExistingChild(IContainer neighborhoodsContainer)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.AddExisting(ObjectTypes.Neighborhood))
            .WithIcon(ApplicationIcons.PKMLLoad)
            .WithCommandFor<AddExistingCommandFor<IContainer, NeighborhoodBuilder>, IContainer>(neighborhoodsContainer, _container);
      }
   }
}