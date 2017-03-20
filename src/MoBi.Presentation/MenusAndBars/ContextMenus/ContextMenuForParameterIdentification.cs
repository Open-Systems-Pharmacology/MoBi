using System.Collections.Generic;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public class ContextMenuForParameterIdentification : ContextMenuBase
   {
      private IEnumerable<IMenuBarItem> _allMenuItems;

      public override IEnumerable<IMenuBarItem> AllMenuItems()
      {
         return _allMenuItems;
      }

      public IContextMenu InitializeWith(ClassifiableParameterIdentification dto, IPresenter presenter)
      {
         _allMenuItems = ParameterIdentificationContextMenuItems.ContextMenuItemsFor(dto.ParameterIdentification);
         return this;
      }
   }

   internal class ContextMenuSpecificationFactoryForParameterIdentification : IContextMenuSpecificationFactory<IViewItem>
   {
      public IContextMenu CreateFor(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         var parameterIdentificationNode = viewItem.DowncastTo<ParameterIdentificationNode>();
         return new ContextMenuForParameterIdentification().InitializeWith(parameterIdentificationNode.Tag, presenter);
      }

      public bool IsSatisfiedBy(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return viewItem.IsAnImplementationOf<ParameterIdentificationNode>();
      }
   }
}