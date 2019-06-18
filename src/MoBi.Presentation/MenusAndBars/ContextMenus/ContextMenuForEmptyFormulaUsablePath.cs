using System.Collections.Generic;
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
   public class ContextMenuForEmptyFormulaUsablePathFactory : IContextMenuSpecificationFactory<IViewItem>
   {
      public IContextMenu CreateFor(IViewItem objectRequestingContextMenu, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return new ContextMenuForEmptyFormulaUsablePath(presenter.DowncastTo<IEditFormulaPathListPresenter>());
      }

      public bool IsSatisfiedBy(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return presenter.IsAnImplementationOf<IEditFormulaPathListPresenter>() && viewItem.IsAnImplementationOf<EmptyFormulaUsableDTO>();
      }
   }

   public class ContextMenuForEmptyFormulaUsablePath : ContextMenuBase
   {
      private readonly IList<IMenuBarItem> _allMenuItems;

      public ContextMenuForEmptyFormulaUsablePath(IEditFormulaPathListPresenter editFormulaPathListPresenter)
      {
         _allMenuItems = new List<IMenuBarItem>{
            CreateMenuButton.WithCaption(AppConstants.MenuNames.AddNew("Reference")).
               WithActionCommand(editFormulaPathListPresenter.CreateNewPath).
               WithIcon(ApplicationIcons.Add)};
      }

      public override IEnumerable<IMenuBarItem> AllMenuItems()
      {
         return _allMenuItems;
      }
   }
}