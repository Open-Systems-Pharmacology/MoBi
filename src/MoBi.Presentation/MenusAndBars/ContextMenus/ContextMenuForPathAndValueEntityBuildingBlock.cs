using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Utility.Container;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public abstract class ContextMenuForPathAndValueEntityBuildingBlock<TBuildingBlock, TPathAndValueEntity> : ContextMenuForModuleBuildingBlock<TBuildingBlock> where TBuildingBlock : PathAndValueEntityBuildingBlock<TPathAndValueEntity> where TPathAndValueEntity : PathAndValueEntity
   {
      protected ContextMenuForPathAndValueEntityBuildingBlock(IMoBiContext context, IObjectTypeResolver objectTypeResolver, IContainer container) : base(context, objectTypeResolver, container)
      {
      }

      public override IContextMenu InitializeWith(ObjectBaseDTO dto, IPresenter presenter)
      {
         var buildingBlock = _context.Get<TBuildingBlock>(dto.Id);
         base.InitializeWith(dto, presenter);
         _allMenuItems.Add(CreateCloneMenuItem(buildingBlock));
         return this;
      }

      protected abstract IMenuBarItem CreateCloneMenuItem(TBuildingBlock buildingBlock);
   }
}