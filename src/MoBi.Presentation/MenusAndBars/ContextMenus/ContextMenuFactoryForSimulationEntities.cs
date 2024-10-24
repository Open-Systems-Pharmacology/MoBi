﻿using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public class ContextMenuFactoryForSimulationEntities : IContextMenuSpecificationFactory<IViewItem>
   {
      private readonly IMoBiContext _context;

      public ContextMenuFactoryForSimulationEntities(IMoBiContext context)
      {
         _context = context;
      }

      public IContextMenu CreateFor(IViewItem objectRequestingContextMenu, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return new ContextMenuForSimulationEntities(
            objectRequestingContextMenu.DowncastTo<ObjectBaseDTO>(),
            _context,
            presenter.DowncastTo<IHierarchicalSimulationPresenter>()
         );
      }

      public bool IsSatisfiedBy(IViewItem objectRequestingContextMenu, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return presenter.IsAnImplementationOf<HierarchicalSimulationPresenter>()
                && objectRequestingContextMenu.IsAnImplementationOf<ObjectBaseDTO>();
      }
   }
}