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
   public class ContextMenuForSensitivityAnalysis : ContextMenuBase
   {
      private IEnumerable<IMenuBarItem> _allMenuItems;

      public override IEnumerable<IMenuBarItem> AllMenuItems()
      {
         return _allMenuItems;
      }

      public IContextMenu InitializeWith(ClassifiableSensitivityAnalysis dto, IPresenter presenter)
      {
         _allMenuItems = SensitivityAnalysisContextMenuItems.ContextMenuItemsFor(dto.SensitivityAnalysis);
         return this;
      }
   }

   internal class ContextMenuSpecificationFactoryForSensitivityAnalysis : IContextMenuSpecificationFactory<IViewItem>
   {
      public IContextMenu CreateFor(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         var sensitivityAnalysisNode = viewItem.DowncastTo<SensitivityAnalysisNode>();
         return new ContextMenuForSensitivityAnalysis().InitializeWith(sensitivityAnalysisNode.Tag, presenter);
      }

      public bool IsSatisfiedBy(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return viewItem.IsAnImplementationOf<SensitivityAnalysisNode>();
      }
   }
}