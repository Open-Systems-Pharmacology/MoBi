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
   public class ContextMenuForParameterValuesBuildingBlock : ContextMenuForPathAndValueEntityBuildingBlock<ParameterValuesBuildingBlock, ParameterValue>
   {
      public ContextMenuForParameterValuesBuildingBlock(IMoBiContext context, IObjectTypeResolver objectTypeResolver, IContainer container) : base(context, objectTypeResolver, container)
      {
      }

      public override IContextMenu InitializeWith(ObjectBaseDTO dto, IPresenter presenter)
      {
         var buildingBlock = _context.Get<ParameterValuesBuildingBlock>(dto.Id);
         base.InitializeWith(dto, presenter);
         _allMenuItems.Add(CreateMenuButton.WithCaption(AppConstants.MenuNames.Import)
            .WithIcon(ApplicationIcons.ParameterValuesImport)
            .WithCommandFor<ImportParameterValuesUICommand, IBuildingBlock>(buildingBlock, _container));

         _allMenuItems.Add(CreateMenuButton.WithCaption(AppConstants.MenuNames.ExportToExcel)
            .WithIcon(ApplicationIcons.ExportToExcel)
            .WithCommandFor<ExportParameterValuesBuildingBlockToExcelUICommand, ParameterValuesBuildingBlock>(buildingBlock, _container));

         _allMenuItems.Add(createExtendParameterValueBuildingBlockFromExpression(buildingBlock));
         _allMenuItems.Add(createExtendParameterValueBuildingBlockFromIndividual(buildingBlock));

         return this;
      }

      protected override IMenuBarItem CreateCloneMenuItem(ParameterValuesBuildingBlock buildingBlock)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.Clone.WithEllipsis())
            .WithIcon(ApplicationIcons.Clone)
            .WithCommandFor<ClonePathAndValueEntityBuildingBlockUICommand<ParameterValuesBuildingBlock, ParameterValue, IParameterValuesTask>, ParameterValuesBuildingBlock>(buildingBlock, _container);
      }

      private IMenuBarItem createExtendParameterValueBuildingBlockFromExpression(ParameterValuesBuildingBlock buildingBlock)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.ExtendFrom(ObjectTypes.ExpressionProfileBuildingBlock))
            .WithIcon(ApplicationIcons.LoadIconFor(nameof(ExpressionProfileBuildingBlock)))
            .WithCommandFor<ExtendParameterValuesFromExpressionUICommand, ParameterValuesBuildingBlock>(buildingBlock, _container);
      }

      private IMenuBarItem createExtendParameterValueBuildingBlockFromIndividual(ParameterValuesBuildingBlock buildingBlock)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.ExtendFrom(_objectTypeResolver.TypeFor<IndividualBuildingBlock>()))
            .WithIcon(ApplicationIcons.LoadIconFor(nameof(IndividualBuildingBlock)))
            .WithCommandFor<ExtendParameterValuesFromIndividualUICommand, ParameterValuesBuildingBlock>(buildingBlock, _container);
      }
   }
}