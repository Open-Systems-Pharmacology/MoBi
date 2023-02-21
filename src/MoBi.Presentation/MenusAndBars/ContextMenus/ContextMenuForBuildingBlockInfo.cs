﻿using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.UICommand;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Assets;
using OSPSuite.Utility.Container;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   internal class ContextMenuForBuildingBlockInfo : ContextMenuBase, IContextMenuFor<IBuildingBlockInfo>
   {
      private readonly IContainer _container;
      private readonly List<IMenuBarItem> _allMenuItems;

      public ContextMenuForBuildingBlockInfo(IContainer container)
      {
         _container = container;
         _allMenuItems = new List<IMenuBarItem>();
      }

      public override IEnumerable<IMenuBarItem> AllMenuItems()
      {
         return _allMenuItems;
      }

      public IContextMenu InitializeWith(IObjectBaseDTO dto, IPresenter presenter)
      {
         var buildingBlockInfoViewItem = dto.DowncastTo<BuildingBlockInfoViewItem>();
         var simulation = buildingBlockInfoViewItem.Simulation;
         var buildingBlockInfo = buildingBlockInfoViewItem.BuildingBlockInfo;
         var templateBuildingBlock = buildingBlockInfo.UntypedTemplateBuildingBlock;

         if (buildingBlockInfo.BuildingBlockChanged)
         {
            _allMenuItems.Add(createUpdateItem(templateBuildingBlock, simulation));
            _allMenuItems.Add(createCommitItem(templateBuildingBlock, simulation));
            _allMenuItems.Add(createDiffItem(templateBuildingBlock, simulation));
         }

         return this;
      }

      private IMenuBarItem createDiffItem(IBuildingBlock templateBuildingBlock, IMoBiSimulation simulation)
      {
         var item = CreateMenuButton.WithCaption(MenuNames.Diff)
            .WithCommand<ShowBuildingBlockDiffUICommand>(_container)
            .WithIcon(ApplicationIcons.Comparison);


         ((ShowBuildingBlockDiffUICommand)item.Command).Initialize(templateBuildingBlock, simulation);
         return item;
      }

      private IMenuBarItem createUpdateItem(IBuildingBlock buildingBlock, IMoBiSimulation simulation)
      {
         var item = CreateMenuButton.WithCaption(AppConstants.MenuNames.Update)
            .WithIcon(ApplicationIcons.Update)
            .WithCommand<UpdateSimulationFromBuildingBlockUICommand>(_container);

         ((UpdateSimulationFromBuildingBlockUICommand) item.Command).Initialize(buildingBlock, simulation);
         return item;
      }

      private IMenuBarItem createCommitItem(IBuildingBlock buildingBlock, IMoBiSimulation simulation)
      {
         var item = CreateMenuButton.WithCaption(AppConstants.MenuNames.Commit)
            .WithIcon(ApplicationIcons.Commit)
            .WithCommand<CommitSimulationChangesToBuildingBlockUICommand>(_container);

         ((CommitSimulationChangesToBuildingBlockUICommand) item.Command).Initialize(buildingBlock, simulation);
         return item;
      }
   }

   public class ContextMenuSpecificationFactoryForBuildingBlockForBuildingBlockInfo : IContextMenuSpecificationFactory<IViewItem>
   {
      private readonly IContainer _container;

      public ContextMenuSpecificationFactoryForBuildingBlockForBuildingBlockInfo(IContainer container)
      {
         _container = container;
      }

      public IContextMenu CreateFor(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         var contextMenu = new ContextMenuForBuildingBlockInfo(_container);
         return contextMenu.InitializeWith(viewItem.DowncastTo<BuildingBlockInfoViewItem>(), presenter);
      }

      public bool IsSatisfiedBy(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return viewItem.IsAnImplementationOf<BuildingBlockInfoViewItem>();
      }
   }
}