﻿using System.Collections.Generic;
using MoBi.Assets;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Utility.Extensions;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Assets;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public class ContextMenuForFormulaUsablePathFactory : IContextMenuSpecificationFactory<IViewItem>
   {
      public IContextMenu CreateFor(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return new ContextMenuForFormulaUsablePath(viewItem.DowncastTo<FormulaUsablePathDTO>(), presenter.DowncastTo<IEditExplicitFormulaPresenter>());
      }

      public bool IsSatisfiedBy(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return presenter.IsAnImplementationOf<IEditExplicitFormulaPresenter>() && viewItem.IsAnImplementationOf<FormulaUsablePathDTO>();
      }
   }

   internal class ContextMenuForFormulaUsablePath : ContextMenuBase
   {
      private readonly IList<IMenuBarItem> _allMenuItems;

      public ContextMenuForFormulaUsablePath(FormulaUsablePathDTO formulaUsablePathDTO, IEditExplicitFormulaPresenter editExplicitFormulaPresenter)
      {
         _allMenuItems = new List<IMenuBarItem>
         {
            CreateMenuButton.WithCaption((AppConstants.MenuNames.Delete)).
               WithActionCommand(() => editExplicitFormulaPresenter.RemovePath(formulaUsablePathDTO)).
               WithIcon(ApplicationIcons.Delete)
         };

      }

      public override IEnumerable<IMenuBarItem> AllMenuItems()
      {
         return _allMenuItems;
      }

   }
}