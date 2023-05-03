using System.Collections.Generic;
using MoBi.Assets;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Utility.Container;
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
            .WithCommand<NewAmountProjectCommand>(_container);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.NewConcentrationProject)
            .WithId(MenuBarItemIds.NewConcentrationProject)
            .WithDescription(ToolTips.FileRibbon.NewConcentrationProjectDescription)
            .WithIcon(ApplicationIcons.ConcentrationProjectNew)
            .WithCommand<NewConcentrationProjectCommand>(_container);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.OpenProject)
            .WithId(MenuBarItemIds.OpenProject)
            .WithDescription(ToolTips.FileRibbon.OpenProjectDescription)
            .WithCommand<OpenProjectCommand>(_container)
            .WithIcon(ApplicationIcons.ProjectOpen)
            .WithShortcut(Keys.Control | Keys.O);

         yield return CreateSubMenu.WithCaption(AppConstants.MenuNames.SaveProject)
            .WithId(MenuBarItemIds.SaveGroup)
            .WithDescription(ToolTips.FileRibbon.SaveProjectDescription)
            .WithIcon(ApplicationIcons.Save);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.SaveProject)
            .WithId(MenuBarItemIds.SaveProject)
            .WithDescription(ToolTips.FileRibbon.SaveProjectDescription)
            .WithCommand<SaveProjectCommand>(_container)
            .WithIcon(ApplicationIcons.Save)
            .WithShortcut(Keys.Control | Keys.S);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.SaveAs)
            .WithId(MenuBarItemIds.SaveProjectAs)
            .WithDescription(ToolTips.FileRibbon.SaveProjectDescription)
            .WithIcon(ApplicationIcons.SaveAs)
            .WithCommand<SaveProjectAsCommand>(_container);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.About)
            .WithId(MenuBarItemIds.About)
            .WithCommand<ShowAboutUICommand>(_container)
            .WithIcon(ApplicationIcons.About)
            .WithDescription(ToolTips.FileRibbon.AboutThisApplication);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.CloseProject)
            .WithId(MenuBarItemIds.CloseProject)
            .WithDescription(ToolTips.FileRibbon.CloseProjectDescription)
            .WithIcon(ApplicationIcons.CloseProject)
            .WithCommand<CloseProjectCommand>(_container);


         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.GarbageCollection)
            .WithId(MenuBarItemIds.GarbageCollection)
            .WithCommand<GarbageCollectionCommand>(_container)
            .ForDeveloper();

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.Options)
            .WithId(MenuBarItemIds.Options)
            .WithCommand<UserSettingsCommand>(_container)
            .WithIcon(ApplicationIcons.UserSettings)
            .WithDescription(ToolTips.ExtrasRibbon.Options)
            .WithShortcut(Keys.Control | Keys.Shift | Keys.O);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.Exit)
            .WithId(MenuBarItemIds.Exit)
            .WithIcon(ApplicationIcons.Exit)
            .WithDescription(ToolTips.FileRibbon.ExitDescription)
            .WithCommand<IExitCommand>(_container)
            .WithShortcut(Keys.Alt | Keys.F4);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.Run)
            .WithId(MenuBarItemIds.Run)
            .WithDescription(ToolTips.SimulationRibbon.RunSimulation)
            .WithCommand<RunActiveSimulationCommand>(_container)
            .WithIcon(ApplicationIcons.Run)
            .WithShortcut(Keys.F5);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.RunWithSettings)
            .WithId(MenuBarItemIds.RunWithSettings)
            .WithDescription(ToolTips.SimulationRibbon.RunWithSettingsDescription)
            .WithCommand<RunActiveSimulationWithSettingsCommand>(_container)
            .WithIcon(ApplicationIcons.ConfigureAndRun);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.ConfigureShortMenu)
            .WithId(MenuBarItemIds.ConfigureActiveSimulation)
            .WithDescription(ToolTips.SimulationRibbon.ConfigureSimulationDescription)
            .WithCommand<ConfigureSimulationUICommand>(_container)
            .WithIcon(ApplicationIcons.SimulationConfigure);
         
         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.CalculateScaleDivisor)
            .WithId(MenuBarItemIds.CalculateScaleFactors)
            .WithDescription(ToolTips.SimulationRibbon.CalculateScaleFactors)
            .WithCommand<CalculateScaleFactorCommand>(_container)
            .WithIcon(ApplicationIcons.ScaleFactor);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.Stop)
            .WithId(MenuBarItemIds.Stop)
            .WithDescription(ToolTips.SimulationRibbon.StopSimulation)
            .WithCommand<StopSimulationCommand>(_container)
            .WithIcon(ApplicationIcons.Stop);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.HistoryView)
            .WithId(MenuBarItemIds.HistoryView)
            .WithDescription(ToolTips.ViewRibbon.ViewsHistoryManager)
            .WithCommand<ShowHistoryCommand>(_container)
            .WithIcon(ApplicationIcons.History)
            .WithShortcut(Keys.Control | Keys.Shift | Keys.H);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.SearchView)
            .WithId(MenuBarItemIds.SearchView)
            .WithDescription(ToolTips.ViewRibbon.ViewSearch)
            .WithIcon(ApplicationIcons.Search)
            .WithCommand<ShowSearchCommand>(_container)
            .WithShortcut(Keys.Control | Keys.Shift | Keys.F);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.NotificationView)
            .WithId(MenuBarItemIds.NotificationView)
            .WithDescription(ToolTips.ViewRibbon.ViewNotification)
            .WithIcon(ApplicationIcons.Warning)
            .WithCommand<ShowNotificationCommand>(_container)
            .WithShortcut(Keys.Control | Keys.Shift | Keys.W);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.ComparisonView)
            .WithId(MenuBarItemIds.ComparisonView)
            .WithDescription(ToolTips.ViewRibbon.ViewComparison)
            .WithIcon(ApplicationIcons.Comparison)
            .WithCommand<ComparisonVisibilityUICommand>(_container)
            .WithShortcut(Keys.Control | Keys.Shift | Keys.N);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.OpenSimulation)
            .WithId(MenuBarItemIds.OpenSimulation)
            .WithDescription(ToolTips.FileRibbon.OpenSimulationDescription)
            .WithCommand<OpenSimulationCommand>(_container)
            .WithIcon(ApplicationIcons.Simulation);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.LoadIntoProject)
            .WithId(MenuBarItemIds.LoadSimulationIntoProject)
            .WithDescription(ToolTips.ImportRibbon.LoadSimulation)
            .WithCommand<LoadProjectUICommand>(_container)
            .WithIcon(ApplicationIcons.Simulation);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.AddObservedData)
            .WithId(MenuBarItemIds.AddObservedData)
            .WithDescription(ToolTips.ImportRibbon.AddObservedData)
            .WithIcon(ApplicationIcons.ObservedData)
            .WithCommand<ImportDataRepositoryUICommand>(_container);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.LoadObservedData)
           .WithId(MenuBarItemIds.LoadObservedData)
           .WithDescription(ToolTips.ImportRibbon.LoadObservedData)
           .WithIcon(ApplicationIcons.PKMLLoad)
           .WithCommand<LoadDataRepositoryUICommand>(_container);
         
         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.ModuleExplorer)
            .WithId(MenuBarItemIds.ModuleExplorerView)
            .WithIcon(ApplicationIcons.BuildingBlockExplorer)
            .WithDescription(ToolTips.ViewRibbon.ViewModules)
            .WithCommand<ShowModuleExplorerCommand>(_container)
            .WithShortcut(Keys.Control | Keys.Shift | Keys.B);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.SimulationExplorer)
            .WithId(MenuBarItemIds.SimulationExplorerView)
            .WithDescription(ToolTips.ViewRibbon.ViewSims)
            .WithIcon(ApplicationIcons.SimulationExplorer)
            .WithCommand<ShowSimulationExplorerCommand>(_container)
            .WithShortcut(Keys.Control | Keys.Shift | Keys.S);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.EditDefaultSimulationSettings)
            .WithId(MenuBarItemIds.EditProjectSimulationSettings)
            .WithIcon(ApplicationIcons.SimulationSettings)
            .WithDescription(ToolTips.SimulationSettingsRibbon.EditDefaultSimulationSettings)
            .WithShortcut(Keys.Control | Keys.Alt | Keys.Shift | Keys.N)
            .WithCommand<EditProjectSimulationSettingsUICommand>(_container);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.SaveProjectSimulationSettings)
            .WithId(MenuBarItemIds.SaveProjectSimulationSettings)
            .WithIcon(ApplicationIcons.PKMLSave)
            .WithDescription(ToolTips.SimulationSettingsRibbon.SaveProjectSimulationSettings)
            .WithCommand<SaveProjectSimulationSettingsUICommand>(_container);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.LoadProjectSimulationSettings)
            .WithId(MenuBarItemIds.LoadProjectSimulationSettings)
            .WithIcon(ApplicationIcons.PKMLLoad)
            .WithDescription(ToolTips.SimulationSettingsRibbon.LoadProjectSimulationSettings)
            .WithCommand<LoadProjectSimulationSettingsUICommand>(_container);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.NewSimulation)
            .WithId(MenuBarItemIds.NewSimulation)
            .WithCommand<NewSimulationCommand>(_container)
            .WithDescription(ToolTips.SimulationRibbon.CreateSimulation)
            .WithIcon(ApplicationIcons.Simulation)
            .WithShortcut(Keys.Control | Keys.Alt | Keys.Shift | Keys.S);

         yield return CreateMenuButton.WithCaption(AppConstants.Captions.AddIndividual)
            .WithId(MenuBarItemIds.NewIndividual)
            .WithDescription(ToolTips.SimulationRibbon.CreateSimulation)
            .WithCommand<AddNewIndividualCommand>(_container)
            .WithDescription(ToolTips.ModelingRibbon.CreateIndividual)
            .WithIcon(ApplicationIcons.Individual);

         yield return CreateMenuButton.WithCaption(AppConstants.Captions.Module)
            .WithId(MenuBarItemIds.NewModule)
            .WithCommand<NewModuleWithBuildingBlocksUICommand>(_container)
            .WithDescription(ToolTips.ModelingRibbon.CreateModule)
            .WithIcon(ApplicationIcons.Module);

         yield return CreateMenuButton.WithCaption(AppConstants.Captions.ExpressionProfile)
            .WithId(MenuBarItemIds.NewExpressionProfile)
            .WithIcon(ApplicationIcons.ExpressionProfile)
            .WithDescription(ToolTips.ModelingRibbon.CreateExpressionProfile);

         yield return CreateMenuButton.WithCaption(AppConstants.Captions.AddMetabolizingEnzyme)
            .WithId(MenuBarItemIds.NewMetabolizingEnzyme)
            .WithCommandFor<AddExpressionProfileBuildingBlock, ExpressionType>(ExpressionTypes.MetabolizingEnzyme, _container)
            .WithIcon(ApplicationIcons.Enzyme);

         yield return CreateMenuButton.WithCaption(AppConstants.Captions.AddTransportProtein)
            .WithId(MenuBarItemIds.NewTransportProtein)
            .WithCommandFor<AddExpressionProfileBuildingBlock, ExpressionType>(ExpressionTypes.TransportProtein, _container)
            .WithIcon(ApplicationIcons.Transporter);

         yield return CreateMenuButton.WithCaption(AppConstants.Captions.AddSpecificBindingPartner)
            .WithId(MenuBarItemIds.NewSpecificBindingPartner)
            .WithCommandFor<AddExpressionProfileBuildingBlock, ExpressionType>(ExpressionTypes.ProteinBindingPartner, _container)
            .WithIcon(ApplicationIcons.SpecificBinding);

         yield return CreateSubMenu.WithCaption(AppConstants.MenuNames.ExportHistory)
            .WithId(MenuBarItemIds.HistoryReportGroup)
            .WithIcon(ApplicationIcons.HistoryExport);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.ExportHistoryToExcel)
            .WithId(MenuBarItemIds.ExportHistoryToExcel)
            .WithDescription(ToolTips.ExportRibbon.CreateReport)
            .WithIcon(ApplicationIcons.Excel)
            .WithCommand<ExportHistoryUICommand>(_container);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.NewMolecule)
            .WithId(MenuBarItemIds.NewMolecule)
            .WithDescription(ToolTips.BuildingBlockMolecule.NewMolecule)
            .WithIcon(ApplicationIcons.MoleculeAdd)
            .WithCommand<AddNewCommandFor<MoleculeBuildingBlock, MoleculeBuilder>>(_container);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.LoadMolecule)
            .WithId(MenuBarItemIds.LoadMolecule)
            .WithDescription(ToolTips.BuildingBlockMolecule.LoadMolecule)
            .WithIcon(ApplicationIcons.MoleculeLoad)
            .WithCommand<AddExistingCommandFor<MoleculeBuildingBlock, MoleculeBuilder>>(_container);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.LoadMoleculeFromTemplate)
           .WithId(MenuBarItemIds.LoadMoleculeFromTemplate)
           .WithDescription(ToolTips.BuildingBlockMolecule.LoadMolecule)
           .WithIcon(ApplicationIcons.LoadFromTemplate)
           .WithCommand<AddExistingFromTemplateCommandFor<MoleculeBuildingBlock, MoleculeBuilder>>(_container);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.AddPKSimMolecule)
            .WithId(MenuBarItemIds.AddPKSimMolecule)
            .WithDescription(ToolTips.BuildingBlockMolecule.AddPKSimMolecule)
            .WithIcon(ApplicationIcons.PKSimMoleculeAdd)
            .WithCommand<AddPKSimMoleculeCommand>(_container);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.NewReaction)
            .WithId(MenuBarItemIds.NewReaction)
            .WithDescription(ToolTips.BuildingBlockReaction.NewReaction)
            .WithIcon(ApplicationIcons.ReactionAdd)
            .WithCommand<AddNewCommandFor<MoBiReactionBuildingBlock, ReactionBuilder>>(_container);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.LoadReaction)
            .WithId(MenuBarItemIds.LoadReaction)
            .WithDescription(ToolTips.BuildingBlockReaction.LoadReaction)
            .WithIcon(ApplicationIcons.ReactionLoad)
            .WithCommand<AddExistingCommandFor<MoBiReactionBuildingBlock, ReactionBuilder>>(_container);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.LoadReactionFromTemplate)
            .WithId(MenuBarItemIds.LoadReactionFromTemplate)
            .WithDescription(ToolTips.BuildingBlockReaction.LoadReaction)
            .WithIcon(ApplicationIcons.LoadFromTemplate)
            .WithCommand<AddExistingFromTemplateCommandFor<MoBiReactionBuildingBlock, ReactionBuilder>>(_container);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.NewTopContainer)
            .WithId(MenuBarItemIds.NewTopContainer)
            .WithDescription(ToolTips.BuildingBlockSpatialStructure.NewTopContainer)
            .WithIcon(ApplicationIcons.ContainerAdd)
            .WithCommand<AddNewTopContainerCommand>(_container);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.LoadTopContainer)
            .WithId(MenuBarItemIds.LoadTopContainer)
            .WithDescription(ToolTips.BuildingBlockSpatialStructure.LoadTopContainer)
            .WithIcon(ApplicationIcons.ContainerLoad)
            .WithCommand<AddExistingTopContainerCommand>(_container);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.LoadTopContainerFromTemplate)
           .WithId(MenuBarItemIds.LoadTopContainerFromTemplate)
           .WithDescription(ToolTips.BuildingBlockSpatialStructure.LoadTopContainer)
           .WithIcon(ApplicationIcons.LoadFromTemplate)
           .WithCommand<AddExistingFromTemplateTopContainerCommand>(_container);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.NewPassiveTransport)
            .WithId(MenuBarItemIds.NewPassiveTransport)
            .WithDescription(ToolTips.BuildingBlockPassiveTransport.NewTransport)
            .WithIcon(ApplicationIcons.Create)
            .WithCommand<AddNewCommandFor<PassiveTransportBuildingBlock, TransportBuilder>>(_container);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.LoadPassiveTransport)
            .WithId(MenuBarItemIds.LoadPassiveTransport)
            .WithIcon(ApplicationIcons.PKMLLoad)
            .WithDescription(ToolTips.BuildingBlockPassiveTransport.LoadTransport)
            .WithCommand<AddExistingCommandFor<PassiveTransportBuildingBlock, TransportBuilder>>(_container);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.LoadPassiveTransportFromTemplate)
           .WithId(MenuBarItemIds.LoadPassiveTransportFromTemplate)
           .WithIcon(ApplicationIcons.LoadFromTemplate)
           .WithDescription(ToolTips.BuildingBlockPassiveTransport.LoadTransport)
           .WithCommand<AddExistingFromTemplateCommandFor<PassiveTransportBuildingBlock, TransportBuilder>>(_container);

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
            .WithCommand<AddNewCommandFor<ObserverBuildingBlock, AmountObserverBuilder>>(_container);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.NewContainerObserver)
            .WithId(MenuBarItemIds.NewContainerObserver)
            .WithDescription(ToolTips.BuildingBlockObserver.NewContainerObs)
            .WithIcon(ApplicationIcons.Create)
            .WithCommand<AddNewCommandFor<ObserverBuildingBlock, ContainerObserverBuilder>>(_container);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.LoadContainerObserver)
            .WithId(MenuBarItemIds.LoadContainerObserver)
            .WithDescription(ToolTips.BuildingBlockObserver.LoadContainerObs)
            .WithIcon(ApplicationIcons.ObserverLoad)
            .WithCommand<AddExistingCommandFor<ObserverBuildingBlock, ContainerObserverBuilder>>(_container);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.LoadContainerObserverFromTemplate)
           .WithId(MenuBarItemIds.LoadContainerObserverFromTemplate)
           .WithDescription(ToolTips.BuildingBlockObserver.LoadContainerObs)
           .WithIcon(ApplicationIcons.LoadFromTemplate)
           .WithCommand<AddExistingFromTemplateCommandFor<ObserverBuildingBlock, ContainerObserverBuilder>>(_container);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.LoadAmountObserver)
            .WithId(MenuBarItemIds.LoadAmountObserver)
            .WithDescription(ToolTips.BuildingBlockObserver.LoadAmountObs)
            .WithIcon(ApplicationIcons.ObserverLoad)
            .WithCommand<AddExistingCommandFor<ObserverBuildingBlock, AmountObserverBuilder>>(_container);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.LoadAmountObserverFromTemplate)
            .WithId(MenuBarItemIds.LoadAmountObserverFromTemplate)
            .WithDescription(ToolTips.BuildingBlockObserver.LoadAmountObs)
            .WithIcon(ApplicationIcons.LoadFromTemplate)
            .WithCommand<AddExistingFromTemplateCommandFor<ObserverBuildingBlock, AmountObserverBuilder>>(_container);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.NewEvent)
            .WithId(MenuBarItemIds.NewEvent)
            .WithDescription(ToolTips.BuildingBlockEventGroup.NewEventGroup)
            .WithIcon(ApplicationIcons.EventAdd)
            .WithCommand<AddNewCommandFor<EventGroupBuildingBlock, EventGroupBuilder>>(_container);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.LoadEvent)
            .WithId(MenuBarItemIds.LoadEvent)
            .WithDescription(ToolTips.BuildingBlockEventGroup.LoadEventGroup)
            .WithIcon(ApplicationIcons.EventLoad)
            .WithCommand<AddExistingCommandFor<EventGroupBuildingBlock, EventGroupBuilder>>(_container);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.LoadEventFromTemplate)
            .WithId(MenuBarItemIds.LoadEventFromTemplate)
            .WithDescription(ToolTips.BuildingBlockEventGroup.LoadEventGroup)
            .WithIcon(ApplicationIcons.LoadFromTemplate)
            .WithCommand<AddExistingFromTemplateCommandFor<EventGroupBuildingBlock, EventGroupBuilder>>(_container);

         yield return CreateMenuButton.WithCaption(AppConstants.RibbonButtonNames.NewMolecule)
            .WithId(MenuBarItemIds.NewReactionMolecule)
            .WithDescription(ToolTips.BuildingBlockReaction.NewMolecule)
            .WithIcon(ApplicationIcons.MoleculeAdd)
            .WithCommand<AddMoleculeNameUICommand>(_container);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.ZoomIn)
            .WithId(MenuBarItemIds.ZoomIn)
            .WithDescription(ToolTips.ZoomIn)
            .WithIcon(ApplicationIcons.ZoomIn)
            .WithCommand<ZoomInCommand>(_container);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.ZoomOut)
            .WithId(MenuBarItemIds.ZoomOut)
            .WithDescription(ToolTips.ZoomOut)
            .WithIcon(ApplicationIcons.ZoomOut)
            .WithCommand<ZoomOutCommand>(_container);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.FitToPage)
            .WithId(MenuBarItemIds.FitToPage)
            .WithDescription(ToolTips.FitToPage)
            .WithIcon(ApplicationIcons.FitToPage)
            .WithCommand<FitToPageCommand>(_container);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.Extend)
            .WithId(MenuBarItemIds.MoleculeStartValuesExtend)
            .WithDescription(ToolTips.Extend)
            .WithIcon(ApplicationIcons.ExtendMoleculeStartValues)
            .WithCommand<MoleculeStartValuesExtendUICommand>(_container);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.Extend)
            .WithId(MenuBarItemIds.ParameterStartValuesExtend)
            .WithDescription(ToolTips.Extend)
            .WithIcon(ApplicationIcons.ExtendParameterStartValues)
            .WithCommand<ParameterStartValuesExtendUICommand>(_container);

         yield return CreateMenuButton
            .WithCaption(AppConstants.MenuNames.NewParameterStartValue)
            .WithId(MenuBarItemIds.NewParameterStartValue)
            .WithIcon(ApplicationIcons.AddParameterStartValues)
            .WithCommand<AddParameterStartValuesUICommand>(_container);

         yield return CreateMenuButton
            .WithCaption(AppConstants.MenuNames.NewMoleculeStartValue)
            .WithId(MenuBarItemIds.NewMoleculeStartValue)
            .WithIcon(ApplicationIcons.AddMoleculeStartValues)
            .WithCommand<AddMoleculeStartValuesUICommand>(_container);
     
         yield return CreateMenuButton
              .WithCaption(AppConstants.MenuNames.ImportSBML)
              .WithId(MenuBarItemIds.ImportSBML)
              .WithDescription(ToolTips.FileRibbon.ImportSBMLDescription)
              .WithIcon(ApplicationIcons.SBML)
              .WithCommand<ImportSbmlUICommand>(_container);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.NewNeighborhood)
            .WithId(MenuBarItemIds.NewNeighborhood)
            .WithDescription(ToolTips.BuildingBlockSpatialStructure.NewNeighborhood)
            .WithIcon(ApplicationIcons.Create)
            .WithCommand<AddNewNeighborhoodCommand>(_container);

         yield return CommonMenuBarButtons.ManageUserDisplayUnits(MenuBarItemIds.ManageUserDisplayUnits, _container);
         yield return CommonMenuBarButtons.ManageProjectDisplayUnits(MenuBarItemIds.ManageProjectDisplayUnits, _container);
         yield return CommonMenuBarButtons.UpdateAllToDisplayUnits(MenuBarItemIds.UpdateAllToDisplayUnits, _container);
         yield return CommonMenuBarButtons.LoadFavoritesFromFile(MenuBarItemIds.LoadFavorites, _container);
         yield return CommonMenuBarButtons.SaveFavoritesToFile(MenuBarItemIds.SaveFavorites, _container);
         yield return CommonMenuBarButtons.ClearHistory(MenuBarItemIds.ClearHistory, _container);
         yield return CommonMenuBarButtons.Help(MenuBarItemIds.Help, _container);

         yield return JournalMenuBarButtons.JournalView(MenuBarItemIds.JournalView, _container);
         yield return JournalMenuBarButtons.CreateJournalPage(MenuBarItemIds.CreateJournalPage, _container);
         yield return JournalMenuBarButtons.SelectJournal(MenuBarItemIds.SelectJournal, _container);
         yield return JournalMenuBarButtons.JournalEditorView(MenuBarItemIds.JournalEditorView, _container);
         yield return CommonMenuBarButtons.JournalDiagramView(MenuBarItemIds.JournalDiagramView, _container);
         yield return JournalMenuBarButtons.SearchJournal(MenuBarItemIds.SearchJournal, _container);
         yield return JournalMenuBarButtons.ExportJournal(MenuBarItemIds.ExportJournal, _container);
         yield return JournalMenuBarButtons.RefreshJournal(MenuBarItemIds.RefreshJournal, _container);

         yield return ParameterIdentificationMenuBarButtons.CreateParameterIdentification(MenuBarItemIds.CreateParameterIdentification, _container);
         yield return ParameterIdentificationMenuBarButtons.RunParameterIdentification(MenuBarItemIds.RunParameterIdentification, _container);
         yield return ParameterIdentificationMenuBarButtons.StopParameterIdentification(MenuBarItemIds.StopParameterIdentification, _container);
         yield return ParameterIdentificationMenuBarButtons.TimeProfileParameterIdentification(MenuBarItemIds.TimeProfileParameterIdentification, _container);
         yield return ParameterIdentificationMenuBarButtons.PredictedVsObservedParameterIdentification(MenuBarItemIds.PredictedVsObservedParameterIdentification, _container);
         yield return ParameterIdentificationMenuBarButtons.ResidualsVsTimeParameterIdentification(MenuBarItemIds.ResidualsVsTimeParameterIdentification, _container);
         yield return ParameterIdentificationMenuBarButtons.ResidualHistogramParameterIdentification(MenuBarItemIds.ResidualHistogramParameterIdentification, _container);
         yield return ParameterIdentificationMenuBarButtons.CorrelationMatrixParameterIdentification(MenuBarItemIds.CorrelationMatrixParameterIdentification, _container);
         yield return ParameterIdentificationMenuBarButtons.CovarianceMatrixParameterIdentification(MenuBarItemIds.CovarianceMatrixParameterIdentification, _container);
         yield return ParameterIdentificationMenuBarButtons.ParameterIdentificationFeedbackView(MenuBarItemIds.ParameterIdentificationFeedbackView, _container);
         yield return ParameterIdentificationMenuBarButtons.TimeProfilePredictionInterval(MenuBarItemIds.TimeProfilePredictionInterval, _container);
         yield return ParameterIdentificationMenuBarButtons.TimeProfileVPCInterval(MenuBarItemIds.TimeProfileVPCInterval, _container);
         yield return ParameterIdentificationMenuBarButtons.TimeProfileConfidenceInterval(MenuBarItemIds.TimeProfileConfidenceInterval, _container);

         yield return SensitivityAnalysisMenuBarButtons.SensitivityAnalysisPKParameterAnalysis(MenuBarItemIds.SensitivityAnalysisPKParameterAnalysis, _container);
         yield return SensitivityAnalysisMenuBarButtons.CreateSensitivityAnalysis(MenuBarItemIds.CreateSensitivityAnalysis, _container);
         yield return SensitivityAnalysisMenuBarButtons.SensitivityAnalysisFeedbackView(MenuBarItemIds.SensitivityAnalysisFeedbackView, _container);
         yield return SensitivityAnalysisMenuBarButtons.RunSensitivityAnalysis(MenuBarItemIds.RunSensitivityAnalysis, _container);
         yield return SensitivityAnalysisMenuBarButtons.StopSensitivityAnalysis(MenuBarItemIds.StopSensitivityAnalysis, _container);

      }
   }
}