using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using System.Collections.Generic;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
    public class ContextMenuForSimulationEntities : ContextMenuBase
   {
      private readonly IMoBiContext _context;
      private IEntity _entity;
      private readonly IEntityPathResolver _entityPathResolver;
      private readonly ClipboardTask _clipboardTask;

      public ContextMenuForSimulationEntities(IMoBiContext context, IEntityPathResolver entityPathResolver, ClipboardTask clipboardTask)
      {
         _context = context;
         _entityPathResolver = entityPathResolver;
         _clipboardTask = clipboardTask;
      }

      public IContextMenu InitializeWith(ObjectBaseDTO dto, IPresenter presenter)
      {

         _entity = _context.Get<IEntity>(dto.Id);
         return this;
      }

      public override IEnumerable<IMenuBarItem> AllMenuItems()
      {
         return new List<IMenuBarItem>() { CreateMenuButton.WithCaption("Copy path").WithActionCommand(copyPathToClipBoard).WithIcon(ApplicationIcons.Copy) };
      }

      private void copyPathToClipBoard()
      {
         _clipboardTask.PutTextToClipboard(_entityPathResolver.FullPathFor(_entity));
      }
   }
}