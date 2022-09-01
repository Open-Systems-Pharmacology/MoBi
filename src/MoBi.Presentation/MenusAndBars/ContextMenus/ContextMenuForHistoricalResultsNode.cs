using System.Collections.Generic;
using MoBi.Assets;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Extensions;
using MoBi.Presentation.Nodes;
using MoBi.Presentation.UICommand;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Assets;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public class ContextMenuForHistoricalResultsNode : ContextMenuBase
   {
      private IList<IMenuBarItem> _allMenuItems;

      public override IEnumerable<IMenuBarItem> AllMenuItems()
      {
         return _allMenuItems;
      }

      public IContextMenu InitializeWith(DataRepository dataRepository, IMoBiSimulation simulation)
      {
         _allMenuItems = new List<IMenuBarItem>();

         _allMenuItems.Add(CreateMenuButton.WithCaption(AppConstants.MenuNames.CompareSimulationResults)
            .WithIcon(ApplicationIcons.SimulationComparison)
            .WithCommandFor<ShowDataRepositoryUICommand, DataRepository>(dataRepository));

         if (!Equals(simulation.ResultsDataRepository, dataRepository))
         {
            var persitableCommand = CreateMenuButton.WithCaption(dataRepository.IsPersistable() ? AppConstants.MenuNames.DiscardResults : AppConstants.MenuNames.KeepResults)
               .WithCommandFor<SwitchHistoricalResultPersistanceUICommand, DataRepository>(dataRepository);
            _allMenuItems.Add(persitableCommand);
         }

         var exportAllCommmand = IoC.Resolve<ExportSimulationResultsToExcelCommand>().InitializeWith(simulation, dataRepository);
         _allMenuItems.Add(CreateMenuButton.WithCaption(AppConstants.MenuNames.ExportSimulationResultsToExcel)
            .WithCommand(exportAllCommmand)
            .WithIcon(ApplicationIcons.ObservedData));

         var command = IoC.Resolve<RenameSimulationResultsUICommand>();
         command.Subject = dataRepository;
         command.Simulation = simulation;

         _allMenuItems.Add(CreateMenuButton.WithCaption(AppConstants.MenuNames.Rename)
            .WithIcon(ApplicationIcons.Rename)
            .WithCommand(command));

         _allMenuItems.Add(CreateMenuButton.WithCaption(AppConstants.MenuNames.Delete)
            .WithCommandFor<RemoveDataRepositoryUICommand, DataRepository>(dataRepository)
            .AsGroupStarter()
            .WithIcon(ApplicationIcons.Delete));

         return this;
      }
   }

   internal class ContextMenuFactoryForHistoricalResultsNode : IContextMenuSpecificationFactory<IViewItem>
   {
      public IContextMenu CreateFor(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         var contextMenu = new ContextMenuForHistoricalResultsNode();
         var historicalResultNode = viewItem.DowncastTo<HistoricalResultsNode>();
         var simulationNode = historicalResultNode.ParentNode.DowncastTo<SimulationNode>();
         var simulation = simulationNode.Simulation;

         return contextMenu.InitializeWith(historicalResultNode.Tag, simulation);
      }

      public bool IsSatisfiedBy(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         var historicalResultNode = viewItem as HistoricalResultsNode;
         return historicalResultNode.BelongsToSimulation();
      }
   }


}