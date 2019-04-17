using System.Collections.Generic;
using MoBi.Assets;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.UICommand;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Extensions;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Presentation.UICommands;
using OSPSuite.Assets;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public class ContextMenuForSimulation : ContextMenuBase, IContextMenuFor<IMoBiSimulation>
   {
      private readonly IMoBiContext _context;
      private IList<IMenuBarItem> _allMenuItems;

      public ContextMenuForSimulation(IMoBiContext context)
      {
         _context = context;
      }

      public override IEnumerable<IMenuBarItem> AllMenuItems()
      {
         return _allMenuItems;
      }

      public IContextMenu InitializeWith(IObjectBaseDTO dto, IPresenter presenter)
      {
         var simulation = dto.DowncastTo<SimulationViewItem>().Simulation;

         //a simulation might not be registered yet which is required to execute some of the action
         _context.Register(simulation);
         _allMenuItems = new List<IMenuBarItem>
         {
            createEditItem(simulation),
            createRenameItem(simulation),

            createConfigure(simulation),
            
//            createNewSimulationMenuBarItem(),
//            createAddExistingSimulationMenuBarItem(),

            createRunItem(simulation),
            createParameterIdentificationItem(simulation),
            createSensitivityAnalysisItem(simulation),

            createStartPopulationSimulation(simulation),
            createSaveItem(simulation),
            createReportItemFor(simulation),
            createAddToJournal(simulation),
            exportSimulationResultsToExcel(simulation),
            createExportForMatlabItem(simulation),
            createMatlabODEExportItem(simulation),
            createDebugReportItem(simulation),
            createExportModelPartsItem(simulation),

            createImportReactionParameters(simulation),
            createDeleteItem(simulation),
            createDeleteAllResultsItem(simulation),
         };

         return this;
      }

      private IMenuBarItem createSensitivityAnalysisItem(IMoBiSimulation simulation)
      {
         return SensitivityAnalysisContextMenuItems.CreateSensitivityAnalysisFor(simulation);
      }

      private IMenuBarItem createImportReactionParameters(IMoBiSimulation simulation)
      {
         return CreateMenuButton.WithCaption(AppConstants.Captions.ImportSimulationParameters.WithEllipsis())
            .WithIcon(ApplicationIcons.ParameterStartValuesImport)
            .WithCommandFor<ImportSimulationParameterValuesUICommand, IMoBiSimulation>(simulation)
            .AsGroupStarter();
      }

      private IMenuBarItem createExportModelPartsItem(IMoBiSimulation simulation)
      {
         return CreateMenuButton.WithCaption(AppConstants.Captions.ExportModelAsTables)
            .WithIcon(ApplicationIcons.ObservedData)
            .WithCommandFor<ExportModelPartsToExcelUICommand, IMoBiSimulation>(simulation);
      }

      private IMenuBarItem createRunItem(IMoBiSimulation simulation)
      {
         return
            CreateMenuButton.WithCaption(AppConstants.MenuNames.Run)
               .WithIcon(ApplicationIcons.Run)
               .WithCommandFor<RunSimulationCommand, IMoBiSimulation>(simulation)
               .AsGroupStarter();
      }

      private IMenuBarItem createMatlabODEExportItem(IMoBiSimulation simulation)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.MatlabDifferentialSystemExport)
            .WithCommandFor<ExportMatlabODEUICommand, IMoBiSimulation>(simulation)
            .WithIcon(ApplicationIcons.Matlab);
      }

      private IMenuBarItem createParameterIdentificationItem(IMoBiSimulation simulation)
      {
         return ParameterIdentificationContextMenuItems.CreateParameterIdentificationFor(new[] { simulation });
      }

      private IMenuBarItem createDebugReportItem(IMoBiSimulation simulation)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.SimulationReport)
            .WithIcon(ApplicationIcons.Report)
            .WithCommandFor<CreateSimulationReportUICommand, IMoBiSimulation>(simulation);
      }

      private IMenuBarItem createExportForMatlabItem(IMoBiSimulation simulation)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.ExportSimModelXml)
            .WithCommandFor<ExportSimulationToSimModelXml, IMoBiSimulation>(simulation)
            .WithIcon(ApplicationIcons.Matlab);
      }

      private IMenuBarItem createStartPopulationSimulation(IMoBiSimulation simulation)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.StartPopulationSimualtion)
            .AsGroupStarter()
            .WithIcon(ApplicationIcons.PopulationSimulation)
            .WithCommandFor<SendSimulationToPKSimUICommand, IMoBiSimulation>(simulation)
            .AsGroupStarter();
      }

      private IMenuBarItem createSaveItem(IMoBiSimulation simulation)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.SaveToPkmlFormat)
            .WithCommandFor<SaveUICommandFor<IMoBiSimulation>, IMoBiSimulation>(simulation)
            .WithIcon(ApplicationIcons.PKMLSave);
      }

      private IMenuBarItem createDeleteItem(IMoBiSimulation simulation)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.Delete)
            .WithCommandFor<RemoveSimulationUICommand, IMoBiSimulation>(simulation)
            .WithIcon(ApplicationIcons.Delete)
            .AsGroupStarter();
      }

      private IMenuBarItem createDeleteAllResultsItem(IMoBiSimulation simulation)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.DeleteAllResults)
            .WithCommandFor<DeleteAllResultsInSimulationUICommand, IMoBiSimulation>(simulation);
      }

      private IMenuBarItem createRenameItem(IMoBiSimulation simulation)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.Rename)
            .WithCommandFor<RenameSimulationUICommand, IMoBiSimulation>(simulation).WithIcon(ApplicationIcons.Rename);
      }

      private IMenuBarItem createReportItemFor(IMoBiSimulation simulation)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.ExportToPDF)
            .WithCommandFor<ExportToPDFCommand<IMoBiSimulation>, IMoBiSimulation>(simulation)
            .WithIcon(ApplicationIcons.ExportToPDF);
      }

      private IMenuBarItem createEditItem(IMoBiSimulation simulation)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.Edit)
            .WithIcon(ApplicationIcons.Edit)
            .WithCommandFor<EditSimulationUICommand, IMoBiSimulation>(simulation);
      }

      private IMenuBarItem createConfigure(IMoBiSimulation simulation)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.Configure)
            .WithIcon(ApplicationIcons.SimulationConfigure)
            .WithCommandFor<ConfigureSimulationUICommand, IMoBiSimulation>(simulation)
            .AsGroupStarter();
      }

      private IMenuBarItem createAddExistingSimulationMenuBarItem()
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.AddExisting(ObjectTypes.Simulation))
            .WithCommand<LoadProjectUICommand>()
            .WithIcon(ApplicationIcons.SimulationLoad);
      }

      private IMenuBarItem createNewSimulationMenuBarItem()
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.AddNew(ObjectTypes.Simulation))
            .WithIcon(ApplicationIcons.Simulation)
            .WithCommand<NewSimulationCommand>()
            .AsGroupStarter();
      }

      private IMenuBarItem exportSimulationResultsToExcel(IMoBiSimulation simulation)
      {
         //create sub menu containing all compounds
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.ExportSimulationResultsToExcel)
            .WithCommandFor<ExportSimulationResultsToExcelCommand, IMoBiSimulation>(simulation)
            .WithIcon(ApplicationIcons.ObservedData);
      }

      private IMenuBarItem createAddToJournal(IMoBiSimulation simulation)
      {
         return ObjectBaseCommonContextMenuItems.AddToJournal(simulation);
      }
   }

   internal class ContextMenuSpecificationFactoryForSimulation : IContextMenuSpecificationFactory<IViewItem>
   {
      private readonly IMoBiContext _context;

      public ContextMenuSpecificationFactoryForSimulation(IMoBiContext context)
      {
         _context = context;
      }

      public IContextMenu CreateFor(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return new ContextMenuForSimulation(_context).InitializeWith(viewItem.DowncastTo<SimulationViewItem>(), presenter);
      }

      public bool IsSatisfiedBy(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return viewItem.IsAnImplementationOf<SimulationViewItem>();
      }
   }
}