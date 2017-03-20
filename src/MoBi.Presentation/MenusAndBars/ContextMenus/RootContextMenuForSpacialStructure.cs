using System.Collections.Generic;
using MoBi.Assets;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Utility.Extensions;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.UICommand;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Assets;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public class RootContextMenuForSpacialStructure : ContextMenuBase
   {
      public override IEnumerable<IMenuBarItem> AllMenuItems()
      {
         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.AddNew("Top Container"))
            .WithIcon(ApplicationIcons.ContainerAdd)
            .WithCommand<AddNewTopContainerCommand>();

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.AddExisting("Top Container"))
            .WithIcon(ApplicationIcons.ContainerLoad)
            .WithCommand<AddExistingTopContainerCommand>();
      }
   }

   public class RootContextMenuForSpacialStructureFactory : IContextMenuSpecificationFactory<IViewItem>
   {
      public IContextMenu CreateFor(IViewItem objectRequestingContextMenu, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return new RootContextMenuForSpacialStructure();
      }

      public bool IsSatisfiedBy(IViewItem objectRequestingContextMenu, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return objectRequestingContextMenu.IsAnImplementationOf<SpatialStructureRootItem>() && presenter.IsAnImplementationOf<IHierarchicalSpatialStructurePresenter>();
      }
   }
}