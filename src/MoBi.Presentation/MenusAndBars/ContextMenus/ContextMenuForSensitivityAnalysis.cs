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
   public class ContextMenuForSensitivityAnalysis : ContextMenuBase
   {
      private readonly IContainer _container;

      public ContextMenuForSensitivityAnalysis(OSPSuite.Utility.Container.IContainer container)
      {
         _container = container;
      }

      private IEnumerable<IMenuBarItem> _allMenuItems;

      public override IEnumerable<IMenuBarItem> AllMenuItems()
      {
         return _allMenuItems;
      }

      public IContextMenu InitializeWith(ClassifiableSensitivityAnalysis dto, IPresenter presenter)
      {
         _allMenuItems = SensitivityAnalysisContextMenuItems.ContextMenuItemsFor(dto.SensitivityAnalysis, _container);
         return this;
      }
   }

   internal class ContextMenuSpecificationFactoryForSensitivityAnalysis : IContextMenuSpecificationFactory<IViewItem>
   {
      private readonly IContainer _container;

      public ContextMenuSpecificationFactoryForSensitivityAnalysis(IContainer container)
      {
         _container = container;
      }

      public IContextMenu CreateFor(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         var sensitivityAnalysisNode = viewItem.DowncastTo<SensitivityAnalysisNode>();
         return new ContextMenuForSensitivityAnalysis(_container).InitializeWith(sensitivityAnalysisNode.Tag, presenter);
      }

      public bool IsSatisfiedBy(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return viewItem.IsAnImplementationOf<SensitivityAnalysisNode>();
      }
   }
}