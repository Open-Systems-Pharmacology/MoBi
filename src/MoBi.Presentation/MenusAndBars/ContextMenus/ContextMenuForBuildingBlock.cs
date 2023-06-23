using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Utility.Container;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public abstract class ContextMenuForBuildingBlock<T> : ContextMenuFor<T>, IContextMenuForBuildingBlock<T> where T : class, IBuildingBlock
   {
      protected ContextMenuForBuildingBlock(IMoBiContext context, IObjectTypeResolver objectTypeResolver, IContainer container) : base(context, objectTypeResolver, container)
      {
      }

      public override IContextMenu InitializeWith(ObjectBaseDTO dto, IPresenter presenter)
      {
         var buildingBlock = _context.Get<T>(dto.Id);
         base.InitializeWith(dto, presenter);
         _allMenuItems.Add(AddToJournal(buildingBlock));
         return this;
      }
   }
}