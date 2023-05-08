using MoBi.Assets;
using OSPSuite.Presentation.MenuAndBars;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.UICommand;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Assets;
using OSPSuite.Utility.Container;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Extensions;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public class ContextMenuForParameterValuesBuildingBlock : ContextMenuForStartValuesBuildingBlock<ParameterValuesBuildingBlock, ParameterValue>
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

         return this;
      }

      protected override IMenuBarItem CreateCloneMenuItem(ParameterValuesBuildingBlock buildingBlock)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.Clone.WithEllipsis())
            .WithIcon(ApplicationIcons.Clone)
            .WithCommandFor<CloneStartValueBuildingBlockUICommand<ParameterValuesBuildingBlock, ParameterValue, IParameterValuesTask>, ParameterValuesBuildingBlock>(buildingBlock, _container);
      }
   }
}