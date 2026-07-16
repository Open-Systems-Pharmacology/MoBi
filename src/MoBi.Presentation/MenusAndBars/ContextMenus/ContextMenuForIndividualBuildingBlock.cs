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
   public class ContextMenuForIndividualBuildingBlock : ContextMenuForProjectBuildingBlock<IndividualBuildingBlock>
   {
      public ContextMenuForIndividualBuildingBlock(IMoBiContext context, IObjectTypeResolver objectTypeResolver, IContainer container) : base(context, objectTypeResolver, container)
      {
      }

      public override IContextMenu InitializeWith(ObjectBaseDTO dto, IPresenter presenter)
      {
         base.InitializeWith(dto, presenter);
         var buildingBlock = _context.Get<IndividualBuildingBlock>(dto.Id);

         if (buildingBlock.HasSnapshot)
            _allMenuItems.Add(createSnapshotMenu(buildingBlock));
         return this;
      }

      private IMenuBarItem createSnapshotMenu(IndividualBuildingBlock buildingBlock)
      {
         return CreateSubMenu.WithCaption(AppConstants.MenuNames.Snapshot)
            .WithItem(createLoadFromItemFor(buildingBlock))
            .WithItem(createExportFromItemFor(buildingBlock));
      }

      private IMenuBarItem createExportFromItemFor(IndividualBuildingBlock buildingBlock)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.Export.WithEllipsis())
            .WithCommandFor<ExportIndividualBuildingBlockSnapshotUICommand, IndividualBuildingBlock>(buildingBlock, _container)
            .WithIcon(ApplicationIcons.SnapshotExport);
      }

      private IMenuBarItem createLoadFromItemFor(IndividualBuildingBlock buildingBlock)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.ReloadIndividual)
            .WithCommandFor<LoadIndividualBuildingBlockFromSnapshotUICommand, IndividualBuildingBlock>(buildingBlock, _container)
            .WithIcon(ApplicationIcons.PKSim);
      }
   }
}