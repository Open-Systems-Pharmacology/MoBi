using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.UICommand;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Extensions;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Utility.Container;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public class ContextMenuForExpressionProfileBuildingBlock : ContextMenuForProjectBuildingBlock<ExpressionProfileBuildingBlock>
   {
      public ContextMenuForExpressionProfileBuildingBlock(IMoBiContext context, IObjectTypeResolver objectTypeResolver, IContainer container) : base(context, objectTypeResolver, container)
      {
      }

      public override IContextMenu InitializeWith(ObjectBaseDTO dto, IPresenter presenter)
      {
         var buildingBlock = _context.Get<ExpressionProfileBuildingBlock>(dto.Id);
         base.InitializeWith(dto, presenter);

         _allMenuItems.Add(CreateMenuButton.WithCaption(AppConstants.MenuNames.ExportToExcel)
            .WithIcon(ApplicationIcons.ExportToExcel)
            .WithCommandFor<ExportExpressionProfilesBuildingBlockToExcelUICommand, ExpressionProfileBuildingBlock>(buildingBlock, _container));

         if (buildingBlock.HasSnapshot)
            _allMenuItems.Add(createSnapshotMenu(buildingBlock));

         return this;
      }

      private IMenuBarItem createSnapshotMenu(ExpressionProfileBuildingBlock buildingBlock)
      {
         return CreateSubMenu.WithCaption(AppConstants.MenuNames.Snapshot)
            .WithItem(createLoadFromItemFor(buildingBlock))
            .WithItem(createExportFromItemFor(buildingBlock));
      }

      private IMenuBarItem createExportFromItemFor(ExpressionProfileBuildingBlock buildingBlock)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.Export.WithEllipsis())
            .WithCommandFor<ExportExpressionProfileBuildingBlockSnapshotUICommand, ExpressionProfileBuildingBlock>(buildingBlock, _container)
            .WithIcon(ApplicationIcons.SnapshotExport);
      }

      private IMenuBarItem createLoadFromItemFor(ExpressionProfileBuildingBlock buildingBlock)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.ReloadExpressionProfile)
            .WithCommandFor<LoadExpressionProfileBuildingBlockFromSnapshotUICommand, ExpressionProfileBuildingBlock>(buildingBlock, _container)
            .WithIcon(ApplicationIcons.PKSim);
      }
   }
}