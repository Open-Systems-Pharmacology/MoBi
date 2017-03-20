using OSPSuite.Presentation.MenuAndBars;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Presentation.Presenters.Nodes;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public class RootContextMenuForParameterIdentification : RootContextMenuFor<IMoBiProject, ParameterIdentification>
   {
      public RootContextMenuForParameterIdentification(IObjectTypeResolver objectTypeResolver, IMoBiContext context) : base(objectTypeResolver, context)
      {
      }

      public override IContextMenu InitializeWith(RootNodeType rootNodeType, IExplorerPresenter presenter)
      {
         var parameterIdentificationRootNode = presenter.NodeByType(rootNodeType);

         _allMenuItems.Add(ParameterIdentificationContextMenuItems.CreateParameterIdentification());
         _allMenuItems.Add(ClassificationCommonContextMenuItems.CreateClassificationUnderMenu(parameterIdentificationRootNode, presenter).AsGroupStarter());
         _allMenuItems.Add(ClassificationCommonContextMenuItems.RemoveClassificationFolderMainMenu(parameterIdentificationRootNode, presenter));
         return this;
      }
   }
}