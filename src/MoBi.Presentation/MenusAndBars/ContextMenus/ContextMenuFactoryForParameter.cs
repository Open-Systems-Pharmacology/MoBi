using OSPSuite.Utility.Container;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public class ContextMenuFactoryForParameter : ContextMenuForObjectBaseDTOSpecificationFactory<IParameter>
   {
      public ContextMenuFactoryForParameter(IMoBiContext context) : base(context)
      {
      }

      public override IContextMenu CreateFor(IObjectBaseDTO objectBaseDTO, IPresenterWithContextMenu<IViewItem> presenter)
      {
         var menu = IoC.Resolve<IContextMenuFor<IParameter>>();
         return menu.InitializeWith(objectBaseDTO, presenter);
      }
   }
}