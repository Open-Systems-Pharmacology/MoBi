using OSPSuite.Presentation.MenuAndBars;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.SensitivityAnalyses;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Presentation.Presenters.Nodes;
using OSPSuite.Utility.Container;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public class RootContextMenuForSensitivityAnalysis : RootContextMenuFor<MoBiProject, SensitivityAnalysis>
   {
      public RootContextMenuForSensitivityAnalysis(IObjectTypeResolver objectTypeResolver, IMoBiContext context, IContainer container) : base(objectTypeResolver, context, container)
      {
      }

      public override IContextMenu InitializeWith(RootNodeType rootNodeType, IExplorerPresenter presenter)
      {
         var sensitivityAnalysisRootNode = presenter.NodeByType(rootNodeType);

         _allMenuItems.Add(SensitivityAnalysisContextMenuItems.CreateSensitivityAnalysis(_container));
         _allMenuItems.Add(ClassificationCommonContextMenuItems.CreateClassificationUnderMenu(sensitivityAnalysisRootNode, presenter).AsGroupStarter());
         _allMenuItems.Add(ClassificationCommonContextMenuItems.RemoveClassificationFolderMainMenu(sensitivityAnalysisRootNode, presenter));
         return this;
      }
   }
}