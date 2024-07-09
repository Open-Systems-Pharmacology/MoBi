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
   public class ContextMenuForParameterValues : ContextMenuBase
   {
      private readonly IEntity _entity;
      private readonly IParameterValuesPresenter _presenter;

      public ContextMenuForParameterValues(IMoBiContext context, IParameterValuesPresenter presenter)
      {
         _presenter = presenter;
      }

      public override IEnumerable<IMenuBarItem> AllMenuItems()
      {
         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.NewParameterValue)
            .WithActionCommand(() => _presenter.AddNewParameterValue())
            .WithIcon(ApplicationIcons.Add);

      }
   }
}