using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.UICommand;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   internal class ContextMenuSpecificationFactoryForInteractionContainer : IContextMenuSpecificationFactory<IViewItem>
   {
      public IContextMenu CreateFor(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         //As long as Interaction Container is "just" a "container" we can reuse the container in event group here.(allows no sub-container)
         return IoC.Resolve<IContextMenuForContainerInMoleculeBuildingBlock>().InitializeWith(viewItem as ContainerDTO, presenter);
      }

      public bool IsSatisfiedBy(IViewItem objectRequestingContextMenu, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return objectRequestingContextMenu.IsAnImplementationOf<InteractionContainerDTO>();
      }
   }

   internal interface IContextMenuForContainerInMoleculeBuildingBlock : IContextMenuFor<InteractionContainer>
   {
   }

   class ContextMenuForContainerInMoleculeBuildingBlock : ContextMenuForContainerBase<InteractionContainer>, IContextMenuForContainerInMoleculeBuildingBlock
   {
      public ContextMenuForContainerInMoleculeBuildingBlock(IMoBiContext context, IObjectTypeResolver objectTypeResolver) : base(context, objectTypeResolver)
      {
      }

      protected override IMenuBarItem CreateDeleteItemFor(InteractionContainer interactionContainer)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.Delete)
            .WithIcon(ApplicationIcons.Delete)
            .WithCommandFor<RemoveCommandForContainerAtMolecule, InteractionContainer>(interactionContainer);
      }
   }
}