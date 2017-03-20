using OSPSuite.Presentation.MenuAndBars;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.SensitivityAnalyses;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Presentation.Presenters.Nodes;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public class RootContextMenuForSensitivityAnalysis : RootContextMenuFor<IMoBiProject, SensitivityAnalysis>
   {
      public RootContextMenuForSensitivityAnalysis(IObjectTypeResolver objectTypeResolver, IMoBiContext context) : base(objectTypeResolver, context)
      {
      }

      public override IContextMenu InitializeWith(RootNodeType rootNodeType, IExplorerPresenter presenter)
      {
         var sensitivityAnalysisRootNode = presenter.NodeByType(rootNodeType);

         _allMenuItems.Add(SensitivityAnalysisContextMenuItems.CreateSensitivityAnalysis());
         _allMenuItems.Add(ClassificationCommonContextMenuItems.CreateClassificationUnderMenu(sensitivityAnalysisRootNode, presenter).AsGroupStarter());
         _allMenuItems.Add(ClassificationCommonContextMenuItems.RemoveClassificationFolderMainMenu(sensitivityAnalysisRootNode, presenter));
         return this;
      }
   }
}