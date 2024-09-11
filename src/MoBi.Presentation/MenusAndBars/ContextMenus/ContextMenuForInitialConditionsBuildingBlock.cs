using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Tasks.Interaction;
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
   public class ContextMenuForInitialConditionsBuildingBlock : ContextMenuForPathAndValueEntityBuildingBlock<InitialConditionsBuildingBlock, InitialCondition>
   {
      public ContextMenuForInitialConditionsBuildingBlock(IMoBiContext context, IObjectTypeResolver objectTypeResolver, IContainer container) : base(context, objectTypeResolver, container)
      {
      }

      public override IContextMenu InitializeWith(ObjectBaseDTO dto, IPresenter presenter)
      {
         var buildingBlock = _context.Get<InitialConditionsBuildingBlock>(dto.Id);
         base.InitializeWith(dto, presenter);

         _allMenuItems.Add(CreateMenuButton.WithCaption(AppConstants.MenuNames.Import)
            .WithIcon(ApplicationIcons.InitialConditionsImport)
            .WithCommandFor<ImportInitialConditionsUICommand, IBuildingBlock>(buildingBlock, _container));

         _allMenuItems.Add(CreateMenuButton.WithCaption(AppConstants.MenuNames.ExportToExcel)
            .WithIcon(ApplicationIcons.ExportToExcel)
            .WithCommandFor<ExportInitialConditionsBuildingBlockToExcelUICommand, InitialConditionsBuildingBlock>(buildingBlock, _container));

         _allMenuItems.Add(createExtendBuildingBlockCommand(buildingBlock));
         return this;
      }

      protected override IMenuBarItem CreateCloneMenuItem(InitialConditionsBuildingBlock buildingBlock)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.Clone.WithEllipsis())
            .WithIcon(ApplicationIcons.Clone)
            .WithCommandFor<ClonePathAndValueEntityBuildingBlockUICommand<InitialConditionsBuildingBlock, InitialCondition, IInitialConditionsTask<InitialConditionsBuildingBlock>>, InitialConditionsBuildingBlock>(buildingBlock, _container);
      }

      private IMenuBarItem createExtendBuildingBlockCommand(InitialConditionsBuildingBlock buildingBlock)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.ExtendFrom(ObjectTypes.InitialConditionsBuildingBlock))
            .WithIcon(ApplicationIcons.LoadIconFor(nameof(InitialConditionsBuildingBlock)))
            .WithCommandFor<ExtendInitialConditionsFromInitialConditionsUICommand, InitialConditionsBuildingBlock>(buildingBlock, _container);
      }
   }
}