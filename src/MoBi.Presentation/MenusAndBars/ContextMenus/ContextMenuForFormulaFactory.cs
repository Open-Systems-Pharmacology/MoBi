using System.Collections.Generic;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Assets;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Utility.Extensions;
using static MoBi.Assets.AppConstants.Captions;

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
         return hasContextMenuEntriesFor(objectRequestingContextMenu) &&
                presenter.IsAnImplementationOf<IFormulaCachePresenter>();
      }

      private static bool hasContextMenuEntriesFor(IViewItem objectRequestingContextMenu)
      {
         return objectRequestingContextMenu == null || objectRequestingContextMenu.IsAnImplementationOf<FormulaBuilderDTO>();
      }
   }

   public class ContextMenuForFormula : ContextMenuBase
   {
      private readonly FormulaBuilderDTO _formulaDTO;
      private readonly IFormulaCachePresenter _presenter;

      public ContextMenuForFormula(FormulaBuilderDTO formulaDTO, IFormulaCachePresenter presenter)
      {
         _formulaDTO = formulaDTO;
         _presenter = presenter;
      }

      public override IEnumerable<IMenuBarItem> AllMenuItems()
      {
         var formulaSelected = _formulaDTO != null;
         if (formulaSelected)
         {
            yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.Rename)
               .WithActionCommand(() => _presenter.Rename(_formulaDTO))
               .WithIcon(ApplicationIcons.Rename);

            yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.Clone)
               .WithActionCommand(() => _presenter.Clone(_formulaDTO))
               .WithIcon(ApplicationIcons.Clone);

            yield return CreateMenuButton.WithCaption(CopyFormula)
               .WithActionCommand(() => _presenter.Copy(_formulaDTO))
               .WithIcon(ApplicationIcons.Copy);
         }

         if (_presenter.FormulasExistOnClipBoard())
         {
            yield return CreateMenuButton.WithCaption(PasteFormula)
               .WithActionCommand(() => _presenter.Paste(_formulaDTO))
               .WithIcon(ApplicationIcons.Paste);
         }

         if (formulaSelected)
         {
            yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.Delete)
               .WithActionCommand(() => _presenter.Remove(_formulaDTO))
               .WithIcon(ApplicationIcons.Delete)
               .AsGroupStarter();
         }
      }
   }
}