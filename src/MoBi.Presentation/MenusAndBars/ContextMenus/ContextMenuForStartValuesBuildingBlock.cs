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
   public class ContextMenuForStartValuesBuildingBlock<TBuildingBlock, TStartValue> : ContextMenuForModuleBuildingBlock<TBuildingBlock> where TBuildingBlock : StartValueBuildingBlock<TStartValue> where TStartValue : PathAndValueEntity, IStartValue
   {
      public ContextMenuForStartValuesBuildingBlock(IMoBiContext context, IObjectTypeResolver objectTypeResolver, IContainer container) : base(context, objectTypeResolver, container)
      {
      }

      public override IContextMenu InitializeWith(ObjectBaseDTO dto, IPresenter presenter)
      {
         var buildingBlock = _context.Get<TBuildingBlock>(dto.Id);
         base.InitializeWith(dto, presenter);
         _allMenuItems.Add(createCloneMenuItem(buildingBlock));
         return this;
      }

      private IMenuBarItem createCloneMenuItem(TBuildingBlock buildingBlock)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.Clone.WithEllipsis())
            .WithIcon(ApplicationIcons.Clone)
            /*.WithCommandFor<CloneModuleBuildingBlockUICommand<TBuildingBlock, TStartValue>, TBuildingBlock>(buildingBlock, _container)*/;
      }
   }
}