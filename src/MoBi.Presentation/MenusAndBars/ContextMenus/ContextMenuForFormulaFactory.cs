using System.Collections.Generic;
using MoBi.Assets;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Utility.Extensions;
using MoBi.Core;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Assets;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public class ContextMenuForFormulaFactory : IContextMenuSpecificationFactory<IViewItem>
   {
      public IContextMenu CreateFor(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return new ContextMenuForFormula(viewItem.DowncastTo<FormulaBuilderDTO>(), presenter.DowncastTo<IFormulaCachePresenter>());
      }

      public bool IsSatisfiedBy(IViewItem objectRequestingContextMenu, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return objectRequestingContextMenu.IsAnImplementationOf<FormulaBuilderDTO>() &&
                presenter.IsAnImplementationOf<IFormulaCachePresenter>();
      }
   }

   public class ContextMenuForFormula : ContextMenuBase
   {
      private readonly FormulaBuilderDTO _formulaDTO;
      private readonly IFormulaCachePresenter _presenter;

      public ContextMenuForFormula() 
      {
      }

      public ContextMenuForFormula(FormulaBuilderDTO formulaDTO, IFormulaCachePresenter presenter)
      {
         _formulaDTO = formulaDTO;
         _presenter = presenter;
      }

      public override IEnumerable<IMenuBarItem> AllMenuItems()
      {
         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.Rename)
            .WithActionCommand(()=>_presenter.Rename(_formulaDTO))
            .WithIcon(ApplicationIcons.Rename);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.Clone)
            .WithActionCommand(() => _presenter.Clone(_formulaDTO))
            .WithIcon(ApplicationIcons.Clone);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.Delete)
            .WithActionCommand(() => _presenter.Remove(_formulaDTO))
            .WithIcon(ApplicationIcons.Delete)
            .AsGroupStarter();
      }
   }
}