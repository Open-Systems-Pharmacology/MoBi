using MoBi.Assets;
using OSPSuite.Presentation.MenuAndBars;
using MoBi.Core.Domain.Model;
using MoBi.Core.Helper;
using MoBi.Presentation.DTO;
using MoBi.Presentation.UICommand;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Extensions;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Assets;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public class ContextMenuForMergableBuildingBlock<T> : ContextMenuForBuildingBlock<T> where T : class, IBuildingBlock
   {
      public ContextMenuForMergableBuildingBlock(IMoBiContext context, IObjectTypeResolver objectTypeResolver)
         : base(context, objectTypeResolver)
      {
      }

      public override IContextMenu InitializeWith(ObjectBaseDTO dto, IPresenter presenter)
      {
         var buildingBlock = _context.Get<T>(dto.Id);
         base.InitializeWith(dto, presenter);

         _allMenuItems.Add(CreateMenuButton.WithCaption(AppConstants.MenuNames.Merge.WithEllipsis())
            .WithIcon(ApplicationIcons.Merge)
            .WithCommandFor<MergeBuildingBlockUICommand<T>, T>(buildingBlock));

         return this;
      }
   }
}