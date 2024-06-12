using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using System.Collections.Generic;
using MoBi.Assets;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public class ContextMenuForSimulationEntities : ContextMenuBase
   {
      private readonly IEntity _entity;
      private readonly IHierarchicalSimulationPresenter _presenter;

      public ContextMenuForSimulationEntities(ObjectBaseDTO dto, IMoBiContext context, IHierarchicalSimulationPresenter presenter)
      {
         _presenter = presenter;
         _entity = context.Get<IEntity>(dto.Id);
      }

      public override IEnumerable<IMenuBarItem> AllMenuItems()
      {
         yield return CreateMenuButton.WithCaption(AppConstants.Captions.CopyPath)
            .WithActionCommand(() => _presenter.CopyCurrentPathToClipBoard(_entity))
            .WithIcon(ApplicationIcons.Copy);
      }
   }
}