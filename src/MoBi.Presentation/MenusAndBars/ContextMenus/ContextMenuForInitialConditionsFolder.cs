using System.Collections.Generic;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.UICommand;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility.Extensions;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   internal class ContextMenuForInitialConditionsFolder : ContextMenuForModuleBuildingBlockCollection
   {
      private readonly IContainer _container;

      public ContextMenuForInitialConditionsFolder(IContainer container)
      {
         _container = container;
         _allMenuItems = new List<IMenuBarItem>();
      }

      protected override IMenuBarItem AddNewBuildingBlock(Module module)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.AddNew(ObjectTypes.InitialConditionsBuildingBlock))
            .WithIcon(ApplicationIcons.LoadIconFor(nameof(InitialConditionsBuildingBlock)))
            .WithCommandFor<AddNewInitialConditionsBuildingBlockUICommand, Module>(module, _container);
      }
   }

   public class ContextMenuSpecificationFactoryForModuleInitialConditionsCollection : IContextMenuSpecificationFactory<IViewItem>
   {
      private readonly IContainer _container;

      public ContextMenuSpecificationFactoryForModuleInitialConditionsCollection(IContainer container)
      {
         _container = container;
      }

      public IContextMenu CreateFor(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         var contextMenu = new ContextMenuForInitialConditionsFolder(_container);
         return contextMenu.InitializeWith(viewItem.DowncastTo<ModuleViewItem>(), presenter);
      }

      public bool IsSatisfiedBy(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         if (!(viewItem is ModuleViewItem moduleViewItem))
            return false;

         return moduleViewItem.TargetAsObject.IsAnImplementationOf<IEnumerable<InitialConditionsBuildingBlock>>();
      }
   }
}