using System.Collections.Generic;
using System.Windows.Forms;
using MoBi.Assets;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Utility.Container;
using MoBi.Core.Domain;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.MenusAndBars;
using MoBi.Presentation.UICommand;
using MoBi.Presentation.UICommand.DiagramUICommands;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Presentation.UICommands;
using OSPSuite.Assets;
using Keys = OSPSuite.Presentation.Core.Keys;
using ToolTips = MoBi.Assets.ToolTips;
namespace MoBi.Presentation.Repositories
{
   public class MenuBarItemRepository : OSPSuite.Presentation.Repositories.MenuBarItemRepository
   {
      public MenuBarItemRepository(IContainer container): base(container)
      {
      }

      protected override IEnumerable<IMenuBarItem> AllMenuBarItems()
      {
         yield return CreateSubMenu.WithCaption(AppConstants.MenuNames.NewProject)
            .WithId(MenuBarItemIds.NewProject)
            .WithDescription(ToolTips.FileRibbon.NewProjectDescription)
            .WithIcon(ApplicationIcons.ProjectNew);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.NewAmountProject)
            .WithId(MenuBarItemIds.NewAmountProject)
            .WithDescription(ToolTips.FileRibbon.NewAmountProjectDescription)
            .WithIcon(ApplicationIcons.AmountProjectNew)
            .WithCommand<NewAmountProjectCommand>();

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.NewConcentrationProject)
            .WithId(MenuBarItemIds.NewConcentrationProject)
            .WithDescription(ToolTips.FileRibbon.NewConcentrationProjectDescription)
            .WithIcon(ApplicationIcons.ConcentrationProjectNew)
            .WithCommand<NewConcentrationProjectCommand>();

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.OpenProject)
            .WithId(MenuBarItemIds.OpenProject)
            .WithDescription(ToolTips.FileRibbon.OpenProjectDescription)
            .WithCommand<OpenProjectCommand>()
            .WithIcon(ApplicationIcons.ProjectOpen)
            .WithShortcut(Keys.Control | Keys.O);

         yield return CreateSubMenu.WithCaption(AppConstants.MenuNames.SaveProject)
            .WithId(MenuBarItemIds.SaveGroup)
            .WithDescription(ToolTips.FileRibbon.SaveProjectDescription)
            .WithIcon(ApplicationIcons.Save);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.SaveProject)
            .WithId(MenuBarItemIds.SaveProject)
            .WithDescription(ToolTips.FileRibbon.SaveProjectDescription)
            .WithCommand<SaveProjectCommand>()
            .WithIcon(ApplicationIcons.Save)
            .WithShortcut(Keys.Control | Keys.S);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.SaveAs)
            .WithId(MenuBarItemIds.SaveProjectAs)
            .WithDescription(ToolTips.FileRibbon.SaveProjectDescription)
            .WithIcon(ApplicationIcons.SaveAs)
            .WithCommand<SaveProjectAsCommand>();

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.About)
            .WithId(MenuBarItemIds.About)
            .WithCommand<ShowAboutUICommand>()
            .WithIcon(ApplicationIcons.About)
            .WithDescription(ToolTips.FileRibbon.AboutThisApplication);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.Merge)
            .WithId(MenuBarItemIds.Merge)
            .WithDescription(ToolTips.WorkFlowRibbon.Merge)
            .WithCommand<MergeBuildingBlocksUICommand>()
            .WithIcon(ApplicationIcons.Merge);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.CloseProject)
            .WithId(MenuBarItemIds.CloseProject)
            .WithDescription(ToolTips.FileRibbon.CloseProjectDescription)
            .WithIcon(ApplicationIcons.CloseProject)
            .WithCommand<CloseProjectCommand>();


         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.GarbageCollection)
            .WithId(MenuBarItemIds.GarbageCollection)
            .WithCommand<GarbageCollectionCommand>()
            .ForDeveloper();

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.Options)
            .WithId(MenuBarItemIds.Options)
            .WithCommand<UserSettingsCommand>()
            .WithIcon(ApplicationIcons.UserSettings)
            .WithDescription(ToolTips.ExtrasRibbon.Options)
            .WithShortcut(Keys.Control | Keys.Shift | Keys.O);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.Exit)
            .WithId(MenuBarItemIds.Exit)
            .WithIcon(ApplicationIcons.Exit)
            .WithDescription(ToolTips.FileRibbon.ExitDescription)
            .WithCommand<IExitCommand>()
            .WithShortcut(Keys.Alt | Keys.F4);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.Run)
            .WithId(MenuBarItemIds.Run)
            .WithDescription(ToolTips.SimulationRibbon.RunSimulation)
            .WithCommand<RunActiveSimulationCommand>()
            .WithIcon(ApplicationIcons.Run)
            .WithShortcut(Keys.F5);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.RunWithSettings)
            .WithId(MenuBarItemIds.RunWithSettings)
            .WithDescription(ToolTips.SimulationRibbon.RunWithSettingsDescription)
            .WithCommand<RunActiveSimulationWithSettingsCommand>()
            .WithIcon(ApplicationIcons.ConfigureAndRun);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.ConfigureShortMenu)
            .WithId(MenuBarItemIds.ConfigureActiveSimulation)
            .WithDescription(ToolTips.SimulationRibbon.ConfigureSimulationDescription)
            .WithCommand<ConfigureSimulationUICommand>()
            .WithIcon(ApplicationIcons.SimulationConfigure);
         
         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.CalculateScaleDivisor)
            .WithId(MenuBarItemIds.CalculateScaleFactors)
            .WithDescription(ToolTips.SimulationRibbon.CalculateScaleFactors)
            .WithCommand<CalculateScaleFactorCommand>()
            .WithIcon(ApplicationIcons.ScaleFactor);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.Stop)
            .WithId(MenuBarItemIds.Stop)
            .WithDescription(ToolTips.SimulationRibbon.StopSimulation)
            .WithCommand<StopSimulationCommand>()
            .WithIcon(ApplicationIcons.Stop);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.HistoryView)
            .WithId(MenuBarItemIds.HistoryView)
            .WithDescription(ToolTips.ViewRibbon.ViewsHistoryManager)
            .WithCommand<ShowHistoryCommand>()
            .WithIcon(ApplicationIcons.History)
            .WithShortcut(Keys.Control | Keys.Shift | Keys.H);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.SearchView)
            .WithId(MenuBarItemIds.SearchView)
            .WithDescription(ToolTips.ViewRibbon.ViewSearch)
            .WithIcon(ApplicationIcons.Search)
            .WithCommand<ShowSearchCommand>()
            .WithShortcut(Keys.Control | Keys.Shift | Keys.F);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.NotificationView)
            .WithId(MenuBarItemIds.NotificationView)
            .WithDescription(ToolTips.ViewRibbon.ViewNotification)
            .WithIcon(ApplicationIcons.Warning)
            .WithCommand<ShowNotificationCommand>()
            .WithShortcut(Keys.Control | Keys.Shift | Keys.W);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.ComparisonView)
            .WithId(MenuBarItemIds.ComparisonView)
            .WithDescription(ToolTips.ViewRibbon.ViewComparison)
            .WithIcon(ApplicationIcons.Comparison)
            .WithCommand<ComparisonVisibilityUICommand>()
            .WithShortcut(Keys.Control | Keys.Shift | Keys.N);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.OpenSimulation)
            .WithId(MenuBarItemIds.OpenSimulation)
            .WithDescription(ToolTips.FileRibbon.OpenSimulationDescription)
            .WithCommand<OpenSimulationCommand>()
            .WithIcon(ApplicationIcons.Simulation);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.LoadIntoProject)
            .WithId(MenuBarItemIds.LoadSimulationIntoProject)
            .WithDescription(ToolTips.ImportRibbon.LoadSimulation)
            .WithCommand<LoadProjectUICommand>()
            .WithIcon(ApplicationIcons.Simulation);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.AddObservedData)
            .WithId(MenuBarItemIds.AddObservedData)
            .WithDescription(ToolTips.ImportRibbon.AddObservedData)
            .WithIcon(ApplicationIcons.ObservedData)
            .WithCommand<ImportDataRepositoryUICommand>();

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.LoadObservedData)
           .WithId(MenuBarItemIds.LoadObservedData)
           .WithDescription(ToolTips.ImportRibbon.LoadObservedData)
           .WithIcon(ApplicationIcons.PKMLLoad)
           .WithCommand<LoadDataRepositoryUICommand>();

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.BuildingBlockExplorer)
            .WithId(MenuBarItemIds.BuildingBlockExplorerView)
            .WithIcon(ApplicationIcons.BuildingBlockExplorer)
            .WithDescription(ToolTips.ViewRibbon.ViewBBs)
            .WithCommand<ShowBuildingBlockExplorerCommand>()
            .WithShortcut(Keys.Control | Keys.Shift | Keys.B);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.SimulationExplorer)
            .WithId(MenuBarItemIds.SimulationExplorerView)
            .WithDescription(ToolTips.ViewRibbon.ViewSims)
            .WithIcon(ApplicationIcons.SimulationExplorer)
            .WithCommand<ShowSimulationExplorerCommand>()
            .WithShortcut(Keys.Control | Keys.Shift | Keys.S);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.NewMoleculeBuildingBlock)
            .WithId(MenuBarItemIds.NewMoleculesBB)
            .WithIcon(ApplicationIcons.Molecule)
            .WithDescription(ToolTips.ModellingRibbon.CreateMoleculesBB)
            .WithShortcut(Keys.Control | Keys.Alt | Keys.Shift | Keys.M)
            .WithCommand<AddNewBuildingBlockCommand<IMoleculeBuildingBlock>>();

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.NewReactionBuildingBlock)
            .WithId(MenuBarItemIds.NewReactionBB)
            .WithDescription(ToolTips.ModellingRibbon.CreateReactionsBB)
            .WithIcon(ApplicationIcons.Reaction)
            .WithShortcut(Keys.Control | Keys.Alt | Keys.Shift | Keys.R)
            .WithCommand<AddNewBuildingBlockCommand<IMoBiReactionBuildingBlock>>();

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.NewSpatialStructure)
            .WithId(MenuBarItemIds.NewSpatialStructure)
            .WithIcon(ApplicationIcons.SpatialStructure)
            .WithDescription(ToolTips.ModellingRibbon.CreateSpatStructuresBB)
            .WithShortcut(Keys.Control | Keys.Alt | Keys.Shift | Keys.P)
            .WithCommand<AddNewBuildingBlockCommand<IMoBiSpatialStructure>>();

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.NewPassiveTransportBuildingBlock)
            .WithId(MenuBarItemIds.NewPassiveTransportBB)
            .WithIcon(ApplicationIcons.PassiveTransport)
            .WithDescription(ToolTips.ModellingRibbon.CreatePassiveTansportsBB)
            .WithShortcut(Keys.Control | Keys.Alt | Keys.Shift | Keys.T)
            .WithCommand<AddNewBuildingBlockCommand<IPassiveTransportBuildingBlock>>();

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.NewEventsBuildingBlock)
            .WithId(MenuBarItemIds.NewEventBB)
            .WithIcon(ApplicationIcons.Event)
            .WithDescription(ToolTips.ModellingRibbon.CreateEventGroupsBB)
            .WithShortcut(Keys.Control | Keys.Alt | Keys.Shift | Keys.E)
            .WithCommand<AddNewBuildingBlockCommand<IEventGroupBuildingBlock>>();

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.NewSimulationSettingsBuildingBlock)
            .WithId(MenuBarItemIds.NewSimulationSettingsBB)
            .WithIcon(ApplicationIcons.SimulationSettings)
            .WithDescription(ToolTips.ModellingRibbon.CreateSimulationSettingsBB)
            .WithShortcut(Keys.Control | Keys.Alt | Keys.Shift | Keys.N)
            .WithCommand<AddNewBuildingBlockCommand<ISimulationSettings>>();

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.NewObserverBuildingBlock)
            .WithId(MenuBarItemIds.NewObserverBB)
            .WithIcon(ApplicationIcons.Observer)
            .WithDescription(ToolTips.ModellingRibbon.CreateObserversBB)
            .WithShortcut(Keys.Control | Keys.Alt | Keys.Shift | Keys.O)
            .WithCommand<AddNewBuildingBlockCommand<IObserverBuildingBlock>>();

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.NewSimulation)
            .WithId(MenuBarItemIds.NewSimulation)
            .WithCommand<NewSimulationCommand>()
            .WithDescription(ToolTips.SimulationRibbon.CreateSimulation)
            .WithIcon(ApplicationIcons.Simulation)
            .WithShortcut(Keys.Control | Keys.Alt | Keys.Shift | Keys.S);

         yield return CreateSubMenu.WithCaption(AppConstants.MenuNames.ExportHistory)
            .WithId(MenuBarItemIds.HistoryReportGroup)
            .WithIcon(ApplicationIcons.HistoryExport);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.ExportHistoryToExcel)
            .WithId(MenuBarItemIds.ExportHistoryToExcel)
            .WithDescription(ToolTips.ExportRibbon.CreateReport)
            .WithIcon(ApplicationIcons.Excel)
            .WithCommand<ExportHistoryUICommand>();

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.NewMolecule)
            .WithId(MenuBarItemIds.NewMolecule)
            .WithDescription(ToolTips.BuildingBlockMolecule.NewMolecule)
            .WithIcon(ApplicationIcons.MoleculeAdd)
            .WithCommand<AddNewCommandFor<IMoleculeBuildingBlock, IMoleculeBuilder>>();

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.LoadMolecule)
            .WithId(MenuBarItemIds.LoadMolecule)
            .WithDescription(ToolTips.BuildingBlockMolecule.LoadMolecule)
            .WithIcon(ApplicationIcons.MoleculeLoad)
            .WithCommand<AddExistingCommandFor<IMoleculeBuildingBlock, IMoleculeBuilder>>();

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.LoadExpressionProfile)
            .WithId(MenuBarItemIds.LoadExpressionProfile)
            .WithDescription(ToolTips.BuildingBlockExpressionProfile.LoadExpressionProfile)
            .WithIcon(ApplicationIcons.PKMLLoad)
            //.WithCommand<LoadExpressionProfileUICommand>();
            .WithCommand<AddExistingCommandFor<IMoBiProject, ExpressionProfileBuildingBlock>>();

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.LoadMoleculeFromTemplate)
           .WithId(MenuBarItemIds.LoadMoleculeFromTemplate)
           .WithDescription(ToolTips.BuildingBlockMolecule.LoadMolecule)
           .WithIcon(ApplicationIcons.LoadFromTemplate)
           .WithCommand<AddExistingFromTemplateCommandFor<IMoleculeBuildingBlock, IMoleculeBuilder>>();

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.AddPKSimMolecule)
            .WithId(MenuBarItemIds.AddPKSimMolecule)
            .WithDescription(ToolTips.BuildingBlockMolecule.AddPKSimMolecule)
            .WithIcon(ApplicationIcons.PKSimMoleculeAdd)
            .WithCommand<AddPKSimMoleculeCommand>();

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.NewReaction)
            .WithId(MenuBarItemIds.NewReaction)
            .WithDescription(ToolTips.BuildingBlockReaction.NewReaction)
            .WithIcon(ApplicationIcons.ReactionAdd)
            .WithCommand<AddNewCommandFor<IMoBiReactionBuildingBlock, IReactionBuilder>>();

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.LoadReaction)
            .WithId(MenuBarItemIds.LoadReaction)
            .WithDescription(ToolTips.BuildingBlockReaction.LoadReaction)
            .WithIcon(ApplicationIcons.ReactionLoad)
            .WithCommand<AddExistingCommandFor<IMoBiReactionBuildingBlock, IReactionBuilder>>();

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.LoadReactionFromTemplate)
            .WithId(MenuBarItemIds.LoadReactionFromTemplate)
            .WithDescription(ToolTips.BuildingBlockReaction.LoadReaction)
            .WithIcon(ApplicationIcons.LoadFromTemplate)
            .WithCommand<AddExistingFromTemplateCommandFor<IMoBiReactionBuildingBlock, IReactionBuilder>>();

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.NewTopContainer)
            .WithId(MenuBarItemIds.NewTopContainer)
            .WithDescription(ToolTips.BuildingBlockSpatialStructure.NewTopContainer)
            .WithIcon(ApplicationIcons.ContainerAdd)
            .WithCommand<AddNewTopContainerCommand>();

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.LoadTopContainer)
            .WithId(MenuBarItemIds.LoadTopContainer)
            .WithDescription(ToolTips.BuildingBlockSpatialStructure.LoadTopContainer)
            .WithIcon(ApplicationIcons.ContainerLoad)
            .WithCommand<AddExistingTopContainerCommand>();

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.LoadTopContainerFromTemplate)
           .WithId(MenuBarItemIds.LoadTopContainerFromTemplate)
           .WithDescription(ToolTips.BuildingBlockSpatialStructure.LoadTopContainer)
           .WithIcon(ApplicationIcons.LoadFromTemplate)
           .WithCommand<AddExistingFromTemplateTopContainerCommand>();

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.NewPassiveTransport)
            .WithId(MenuBarItemIds.NewPassiveTransport)
            .WithDescription(ToolTips.BuildingBlockPassiveTransport.NewTransport)
            .WithIcon(ApplicationIcons.Create)
            .WithCommand<AddNewCommandFor<IPassiveTransportBuildingBlock, ITransportBuilder>>();

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.LoadPassiveTransport)
            .WithId(MenuBarItemIds.LoadPassiveTransport)
            .WithIcon(ApplicationIcons.PKMLLoad)
            .WithDescription(ToolTips.BuildingBlockPassiveTransport.LoadTransport)
            .WithCommand<AddExistingCommandFor<IPassiveTransportBuildingBlock, ITransportBuilder>>();

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.LoadPassiveTransportFromTemplate)
           .WithId(MenuBarItemIds.LoadPassiveTransportFromTemplate)
           .WithIcon(ApplicationIcons.LoadFromTemplate)
           .WithDescription(ToolTips.BuildingBlockPassiveTransport.LoadTransport)
           .WithCommand<AddExistingFromTemplateCommandFor<IPassiveTransportBuildingBlock, ITransportBuilder>>();

         yield return CreateSubMenu.WithCaption(AppConstants.MenuNames.NewObserver)
            .WithId(MenuBarItemIds.NewObserverGroup)
            .WithDescription(ToolTips.BuildingBlockObserver.NewObserver)
            .WithIcon(ApplicationIcons.Create);

         yield return CreateSubMenu.WithCaption(AppConstants.MenuNames.LoadObserver)
            .WithId(MenuBarItemIds.LoadObserverGroup)
            .WithDescription(ToolTips.BuildingBlockObserver.LoadObserver)
            .WithIcon(ApplicationIcons.ObserverLoad);

         yield return CreateSubMenu.WithCaption(AppConstants.MenuNames.LoadObserverFromTemplate)
           .WithId(MenuBarItemIds.LoadObserverFromTemplateGroup)
           .WithDescription(ToolTips.BuildingBlockObserver.LoadObserver)
           .WithIcon(ApplicationIcons.LoadFromTemplate);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.NewAmountObserver)
            .WithId(MenuBarItemIds.NewAmountObserver)
            .WithIcon(ApplicationIcons.Create)
            .WithDescription(ToolTips.BuildingBlockObserver.NewAmountObs)
            .WithCommand<AddNewCommandFor<IObserverBuildingBlock, IAmountObserverBuilder>>();

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.NewContainerObserver)
            .WithId(MenuBarItemIds.NewContainerObserver)
            .WithDescription(ToolTips.BuildingBlockObserver.NewContainerObs)
            .WithIcon(ApplicationIcons.Create)
            .WithCommand<AddNewCommandFor<IObserverBuildingBlock, IContainerObserverBuilder>>();

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.LoadContainerObserver)
            .WithId(MenuBarItemIds.LoadContainerObserver)
            .WithDescription(ToolTips.BuildingBlockObserver.LoadContainerObs)
            .WithIcon(ApplicationIcons.ObserverLoad)
            .WithCommand<AddExistingCommandFor<IObserverBuildingBlock, IContainerObserverBuilder>>();

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.LoadContainerObserverFromTemplate)
           .WithId(MenuBarItemIds.LoadContainerObserverFromTemplate)
           .WithDescription(ToolTips.BuildingBlockObserver.LoadContainerObs)
           .WithIcon(ApplicationIcons.LoadFromTemplate)
           .WithCommand<AddExistingFromTemplateCommandFor<IObserverBuildingBlock, IContainerObserverBuilder>>();

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.LoadAmountObserver)
            .WithId(MenuBarItemIds.LoadAmountObserver)
            .WithDescription(ToolTips.BuildingBlockObserver.LoadAmountObs)
            .WithIcon(ApplicationIcons.ObserverLoad)
            .WithCommand<AddExistingCommandFor<IObserverBuildingBlock, IAmountObserverBuilder>>();

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.LoadAmountObserverFromTemplate)
            .WithId(MenuBarItemIds.LoadAmountObserverFromTemplate)
            .WithDescription(ToolTips.BuildingBlockObserver.LoadAmountObs)
            .WithIcon(ApplicationIcons.LoadFromTemplate)
            .WithCommand<AddExistingFromTemplateCommandFor<IObserverBuildingBlock, IAmountObserverBuilder>>();

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.NewEvent)
            .WithId(MenuBarItemIds.NewEvent)
            .WithDescription(ToolTips.BuildingBlockEventGroup.NewEventGroup)
            .WithIcon(ApplicationIcons.EventAdd)
            .WithCommand<AddNewCommandFor<IEventGroupBuildingBlock, IEventGroupBuilder>>();

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.LoadEvent)
            .WithId(MenuBarItemIds.LoadEvent)
            .WithDescription(ToolTips.BuildingBlockEventGroup.LoadEventGroup)
            .WithIcon(ApplicationIcons.EventLoad)
            .WithCommand<AddExistingCommandFor<IEventGroupBuildingBlock, IEventGroupBuilder>>();

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.LoadEventFromTemplate)
            .WithId(MenuBarItemIds.LoadEventFromTemplate)
            .WithDescription(ToolTips.BuildingBlockEventGroup.LoadEventGroup)
            .WithIcon(ApplicationIcons.LoadFromTemplate)
            .WithCommand<AddExistingFromTemplateCommandFor<IEventGroupBuildingBlock, IEventGroupBuilder>>();

         yield return CreateMenuButton.WithCaption(AppConstants.RibbonButtonNames.NewMolecule)
            .WithId(MenuBarItemIds.NewReactionMolecule)
            .WithDescription(ToolTips.BuildingBlockReaction.NewMolecule)
            .WithIcon(ApplicationIcons.MoleculeAdd)
            .WithCommand<AddMoleculeNameUICommand>();

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.ZoomIn)
            .WithId(MenuBarItemIds.ZoomIn)
            .WithDescription(ToolTips.ZoomIn)
            .WithIcon(ApplicationIcons.ZoomIn)
            .WithCommand<ZoomInCommand>();

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.ZoomOut)
            .WithId(MenuBarItemIds.ZoomOut)
            .WithDescription(ToolTips.ZoomOut)
            .WithIcon(ApplicationIcons.ZoomOut)
            .WithCommand<ZoomOutCommand>();

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.FitToPage)
            .WithId(MenuBarItemIds.FitToPage)
            .WithDescription(ToolTips.FitToPage)
            .WithIcon(ApplicationIcons.FitToPage)
            .WithCommand<FitToPageCommand>();

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.Extend)
            .WithId(MenuBarItemIds.MoleculeStartValuesExtend)
            .WithDescription(ToolTips.Extend)
            .WithIcon(ApplicationIcons.ExtendMoleculeStartValues)
            .WithCommand<MoleculeStartValuesExtendUICommand>();

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.Extend)
            .WithId(MenuBarItemIds.ParameterStartValuesExtend)
            .WithDescription(ToolTips.Extend)
            .WithIcon(ApplicationIcons.ExtendParameterStartValues)
            .WithCommand<ParameterStartValuesExtendUICommand>();

         yield return CreateMenuButton
            .WithCaption(AppConstants.MenuNames.NewParameterStartValue)
            .WithId(MenuBarItemIds.NewParameterStartValue)
            .WithIcon(ApplicationIcons.AddParameterStartValues)
            .WithCommand<AddParameterStartValuesUICommand>();

         yield return CreateMenuButton
            .WithCaption(AppConstants.MenuNames.NewMoleculeStartValue)
            .WithId(MenuBarItemIds.NewMoleculeStartValue)
            .WithIcon(ApplicationIcons.AddMoleculeStartValues)
            .WithCommand<AddMoleculeStartValuesUICommand>();
     
         yield return CreateMenuButton
              .WithCaption(AppConstants.MenuNames.ImportSBML)
              .WithId(MenuBarItemIds.ImportSBML)
              .WithDescription(ToolTips.FileRibbon.ImportSBMLDescription)
              .WithIcon(ApplicationIcons.SBML)
              .WithCommand<ImportSbmlUICommand>();

         yield return CommonMenuBarButtons.ManageUserDisplayUnits(MenuBarItemIds.ManageUserDisplayUnits);
         yield return CommonMenuBarButtons.ManageProjectDisplayUnits(MenuBarItemIds.ManageProjectDisplayUnits);
         yield return CommonMenuBarButtons.UpdateAllToDisplayUnits(MenuBarItemIds.UpdateAllToDisplayUnits);
         yield return CommonMenuBarButtons.LoadFavoritesFromFile(MenuBarItemIds.LoadFavorites);
         yield return CommonMenuBarButtons.SaveFavoritesToFile(MenuBarItemIds.SaveFavorites);
         yield return CommonMenuBarButtons.ClearHistory(MenuBarItemIds.ClearHistory);
         yield return CommonMenuBarButtons.Help(MenuBarItemIds.Help);

         yield return JournalMenuBarButtons.JournalView(MenuBarItemIds.JournalView);
         yield return JournalMenuBarButtons.CreateJournalPage(MenuBarItemIds.CreateJournalPage);
         yield return JournalMenuBarButtons.SelectJournal(MenuBarItemIds.SelectJournal);
         yield return JournalMenuBarButtons.JournalEditorView(MenuBarItemIds.JournalEditorView);
         yield return CommonMenuBarButtons.JournalDiagramView(MenuBarItemIds.JournalDiagramView);
         yield return JournalMenuBarButtons.SearchJournal(MenuBarItemIds.SearchJournal);
         yield return JournalMenuBarButtons.ExportJournal(MenuBarItemIds.ExportJournal);
         yield return JournalMenuBarButtons.RefreshJournal(MenuBarItemIds.RefreshJournal);

         yield return ParameterIdentificationMenuBarButtons.CreateParameterIdentification(MenuBarItemIds.CreateParameterIdentification);
         yield return ParameterIdentificationMenuBarButtons.RunParameterIdentification(MenuBarItemIds.RunParameterIdentification);
         yield return ParameterIdentificationMenuBarButtons.StopParameterIdentification(MenuBarItemIds.StopParameterIdentification);
         yield return ParameterIdentificationMenuBarButtons.TimeProfileParameterIdentification(MenuBarItemIds.TimeProfileParameterIdentification);
         yield return ParameterIdentificationMenuBarButtons.PredictedVsObservedParameterIdentification(MenuBarItemIds.PredictedVsObservedParameterIdentification);
         yield return ParameterIdentificationMenuBarButtons.ResidualsVsTimeParameterIdentification(MenuBarItemIds.ResidualsVsTimeParameterIdentification);
         yield return ParameterIdentificationMenuBarButtons.ResidualHistogramParameterIdentification(MenuBarItemIds.ResidualHistogramParameterIdentification);
         yield return ParameterIdentificationMenuBarButtons.CorrelationMatrixParameterIdentification(MenuBarItemIds.CorrelationMatrixParameterIdentification);
         yield return ParameterIdentificationMenuBarButtons.CovarianceMatrixParameterIdentification(MenuBarItemIds.CovarianceMatrixParameterIdentification);
         yield return ParameterIdentificationMenuBarButtons.ParameterIdentificationFeedbackView(MenuBarItemIds.ParameterIdentificationFeedbackView);
         yield return ParameterIdentificationMenuBarButtons.TimeProfilePredictionInterval(MenuBarItemIds.TimeProfilePredictionInterval);
         yield return ParameterIdentificationMenuBarButtons.TimeProfileVPCInterval(MenuBarItemIds.TimeProfileVPCInterval);
         yield return ParameterIdentificationMenuBarButtons.TimeProfileConfidenceInterval(MenuBarItemIds.TimeProfileConfidenceInterval);

         yield return SensitivityAnalysisMenuBarButtons.SensitivityAnalysisPKParameterAnalysis(MenuBarItemIds.SensitivityAnalysisPKParameterAnalysis);
         yield return SensitivityAnalysisMenuBarButtons.CreateSensitivityAnalysis(MenuBarItemIds.CreateSensitivityAnalysis);
         yield return SensitivityAnalysisMenuBarButtons.SensitivityAnalysisFeedbackView(MenuBarItemIds.SensitivityAnalysisFeedbackView);
         yield return SensitivityAnalysisMenuBarButtons.RunSensitivityAnalysis(MenuBarItemIds.RunSensitivityAnalysis);
         yield return SensitivityAnalysisMenuBarButtons.StopSensitivityAnalysis(MenuBarItemIds.StopSensitivityAnalysis);

      }
   }
}