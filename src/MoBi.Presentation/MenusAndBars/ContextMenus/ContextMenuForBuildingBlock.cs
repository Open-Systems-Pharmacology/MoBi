using MoBi.Assets;
using OSPSuite.Presentation.MenuAndBars;
using MoBi.Core.Domain.Model;
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
   public class ContextMenuForBuildingBlock<T> : ContextMenuFor<T>, IContextMenuForBuildingBlock<T> where T : class, IBuildingBlock
   {
      public ContextMenuForBuildingBlock(IMoBiContext context, IObjectTypeResolver objectTypeResolver) : base(context, objectTypeResolver)
      {
      }

      public override IContextMenu InitializeWith(IObjectBaseDTO dto, IPresenter presenter)
      {
         var buildingBlock = _context.Get<T>(dto.Id);
         base.InitializeWith(dto, presenter);
         _allMenuItems.Add(AddToJournal(buildingBlock));
         _allMenuItems.Add(createCloneMenuItem(buildingBlock));
         return this;
      }

      protected override IMenuBarItem CreateDeleteItemFor(T objectBase)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.Delete)
            .WithIcon(ApplicationIcons.Delete)
            .WithRemoveCommand(_context.CurrentProject, objectBase);
      }

      private IMenuBarItem createCloneMenuItem(T buildingBlock)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.Clone.WithEllipsis())
            .WithIcon(ApplicationIcons.Clone)
            .WithCommandFor<CloneBuildingBlockUICommand<T>, T>(buildingBlock);
      }
   }
}