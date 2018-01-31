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

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public class ContextMenuForParameterInList : ContextMenuBase
   {
      private readonly ParameterDTO _parameterDTO;
      private readonly IEditParameterListPresenter _presenter;

      public ContextMenuForParameterInList(ParameterDTO parameterDTO, IEditParameterListPresenter presenter)
      {
         _parameterDTO = parameterDTO;
         _presenter = presenter;
      }

      public override IEnumerable<IMenuBarItem> AllMenuItems()
      {
         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.GoTo)
            .WithActionCommand(() => _presenter.GoTo(_parameterDTO))
            .WithIcon(ApplicationIcons.GoTo);
      }
   }

   public class ContextMenuForParameterInListFactory : IContextMenuSpecificationFactory<IViewItem>
   {
      public IContextMenu CreateFor(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return new ContextMenuForParameterInList(viewItem.DowncastTo<ParameterDTO>(), presenter.DowncastTo<IEditParameterListPresenter>());
      }

      public bool IsSatisfiedBy(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return viewItem.IsAnImplementationOf<ParameterDTO>() &&
                presenter.IsAnImplementationOf<IEditParameterListPresenter>();
      }
   }
}