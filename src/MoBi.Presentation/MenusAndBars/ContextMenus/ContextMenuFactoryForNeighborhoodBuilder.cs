using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public class ContextMenuFactoryForNeighborhoodBuilder : ContextMenuForObjectBaseDTOSpecificationFactory<INeighborhoodBuilder>
   {
      public ContextMenuFactoryForNeighborhoodBuilder(IMoBiContext context) : base(context)
      {
      }

      public override IContextMenu CreateFor(ObjectBaseDTO objectBaseDTO, IPresenterWithContextMenu<IViewItem> presenter)
      {
         var contextMenu = IoC.Resolve<ContextMenuForNeighborhoodBuilder>();
         return contextMenu.InitializeWith(objectBaseDTO, presenter);
      }
   }
}