using System.Collections.Generic;
using MoBi.Core.Services;
using MoBi.Presentation.DTO;
using MoBi.Presentation.UICommand;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public class ContextMenuForSimulationBuildingBlock :  ContextMenuBase
   {
      private readonly IContainer _container;
      private readonly IList<IMenuBarItem> _allMenuItems;
      private readonly ITemplateResolverTask _templateResolverTask;

      public ContextMenuForSimulationBuildingBlock(IContainer container)
      {
         _container = container;
         _templateResolverTask = _container.Resolve<ITemplateResolverTask>();
         _allMenuItems = new List<IMenuBarItem>();
      }

      public IContextMenu InitializeWith(SimulationBuildingBlockViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         var buildingBlockViewItem = viewItem.DowncastTo<SimulationBuildingBlockViewItem>();

         var simulationBuildingBlock = buildingBlockViewItem.BuildingBlock;
         var projectBuildingBlock = _templateResolverTask.TemplateBuildingBlockFor(simulationBuildingBlock);
         if (projectBuildingBlock.Version != simulationBuildingBlock.Version)
         {
            _allMenuItems.Add(createDiffItem(simulationBuildingBlock));
         }
         

         return this;
      }

      private IMenuBarItem createDiffItem(IBuildingBlock simulationBuildingBlock)
      {
         var item = CreateMenuButton.WithCaption(MenuNames.Diff)
            .WithCommand(_container.Resolve<ShowBuildingBlockDiffUICommand>().Initialize(simulationBuildingBlock))
            .WithIcon(ApplicationIcons.Comparison);
         
         return item;
      }

      public override IEnumerable<IMenuBarItem> AllMenuItems()
      {
         return _allMenuItems;
      }
   }

   public class ContextMenuSpecificationFactoryForSimulationBuildingBlockViewItem : IContextMenuSpecificationFactory<IViewItem>
   {
      private readonly IContainer _container;

      public ContextMenuSpecificationFactoryForSimulationBuildingBlockViewItem(IContainer container)
      {
         _container = container;
      }

      public IContextMenu CreateFor(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         var contextMenu = new ContextMenuForSimulationBuildingBlock(_container);
         return contextMenu.InitializeWith(viewItem.DowncastTo<SimulationBuildingBlockViewItem>(), presenter);
      }

      public bool IsSatisfiedBy(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return viewItem is SimulationBuildingBlockViewItem;
      }
   }
}