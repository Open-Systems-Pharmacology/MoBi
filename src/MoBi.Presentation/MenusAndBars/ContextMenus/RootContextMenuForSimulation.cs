using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.UICommand;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Presentation.Presenters.Nodes;
using OSPSuite.Utility.Container;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public class RootContextMenuForSimulation : RootContextMenuFor<MoBiProject, IMoBiSimulation>
   {
      public RootContextMenuForSimulation(IObjectTypeResolver objectTypeResolver, IMoBiContext context, IContainer container) : base(objectTypeResolver, context, container)
      {
      }

      public override IContextMenu InitializeWith(RootNodeType rootNodeType, IExplorerPresenter presenter)
      {
         var simulationFolderNode = presenter.NodeByType(rootNodeType);
         _allMenuItems.Add(createNewSimulationMenuBarItem());
         _allMenuItems.Add(createAddExistingSimulationMenuBarItem());
         _allMenuItems.Add(ClassificationCommonContextMenuItems.CreateClassificationUnderMenu(simulationFolderNode, presenter).AsGroupStarter());
         _allMenuItems.Add(SimulationClassificationCommonContextMenuItems.RemoveSimulationFolderMainMenu(simulationFolderNode, presenter).AsGroupStarter());
         _allMenuItems.Add(deleteAllSimulationResults().AsGroupStarter());
         return this;
      }

      private IMenuBarItem deleteAllSimulationResults()
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.DeleteAllResults)
            .WithCommand<DeleteAllResultsInAllSimulationsUICommand>(_container);
      }

      private IMenuBarItem createAddExistingSimulationMenuBarItem()
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.AddExisting(ObjectTypes.Simulation))
            .WithCommand<LoadProjectUICommand>(_container)
            .WithIcon(ApplicationIcons.SimulationLoad);
      }

      private IMenuBarItem createNewSimulationMenuBarItem()
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.AddNew(ObjectTypes.Simulation))
            .WithIcon(ApplicationIcons.Simulation)
            .WithCommand<NewSimulationCommand>(_container);
      }
   }
}