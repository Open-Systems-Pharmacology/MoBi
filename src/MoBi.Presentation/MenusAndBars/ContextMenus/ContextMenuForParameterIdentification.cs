using System.Collections.Generic;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public class ContextMenuForParameterIdentification : ContextMenuBase
   {
      private readonly IContainer _container;

      public ContextMenuForParameterIdentification(IContainer container)
      {
         _container = container;
      }
      
      private IEnumerable<IMenuBarItem> _allMenuItems;

      public override IEnumerable<IMenuBarItem> AllMenuItems()
      {
         return _allMenuItems;
      }

      public IContextMenu InitializeWith(ClassifiableParameterIdentification dto, IPresenter presenter)
      {
         _allMenuItems = ParameterIdentificationContextMenuItems.ContextMenuItemsFor(dto.ParameterIdentification, _container);
         return this;
      }
   }

   internal class ContextMenuSpecificationFactoryForParameterIdentification : IContextMenuSpecificationFactory<IViewItem>
   {
      private readonly IContainer _container;

      public ContextMenuSpecificationFactoryForParameterIdentification(IContainer container)
      {
         _container = container;
      }

      public IContextMenu CreateFor(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         var parameterIdentificationNode = viewItem.DowncastTo<ParameterIdentificationNode>();
         return new ContextMenuForParameterIdentification(_container).InitializeWith(parameterIdentificationNode.Tag, presenter);
      }

      public bool IsSatisfiedBy(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return viewItem.IsAnImplementationOf<ParameterIdentificationNode>();
      }
   }
}