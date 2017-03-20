using System.Collections.Generic;
using OSPSuite.Presentation.Core;

namespace MoBi.Presentation.MenusAndBars
{
   public static class MenuBarItemIds
   {
      private static readonly List<MenuBarItemId> _allMenuBarItemIds = new List<MenuBarItemId>();

      public static MenuBarItemId NewProject = createMenuBarItemId("NewProject");
      public static MenuBarItemId NewAmountProject = createMenuBarItemId("NewAmountProject");
      public static MenuBarItemId NewConcentrationProject = createMenuBarItemId("NewConcentrationProject");
      public static MenuBarItemId OpenProject = createMenuBarItemId("OpenProject");
      public static MenuBarItemId SaveProject = createMenuBarItemId("SaveProject");
      public static MenuBarItemId SaveProjectAs = createMenuBarItemId("SaveProjectAs");
      public static MenuBarItemId CloseProject = createMenuBarItemId("CloseProject");
      public static MenuBarItemId About = createMenuBarItemId("About");
      public static MenuBarItemId HistoryView = createMenuBarItemId("HistoryView");
      public static MenuBarItemId BuildingBlockExplorerView = createMenuBarItemId("BuildingBlockExplorerView");
      public static MenuBarItemId Options = createMenuBarItemId("Options");
      public static MenuBarItemId GarbageCollection = createMenuBarItemId("GarbageCollection");
      public static MenuBarItemId Exit = createMenuBarItemId("Exit");
      public static MenuBarItemId NewSimulation = createMenuBarItemId("NewSimulation");
      public static MenuBarItemId Run = createMenuBarItemId("Run");
      public static MenuBarItemId Stop = createMenuBarItemId("Stop");
      public static MenuBarItemId OpenSimulation = createMenuBarItemId("OpenSimulation");
      public static MenuBarItemId LoadSimulationIntoProject = createMenuBarItemId("LoadSimulationIntoProject");
      public static MenuBarItemId AddObservedData = createMenuBarItemId("AddObservedData");
      public static MenuBarItemId LoadObservedData = createMenuBarItemId("LoadObservedData");
      public static MenuBarItemId SaveGroup = createMenuBarItemId("SaveGroup");
      public static MenuBarItemId NewMoleculesBB = createMenuBarItemId("NewMoleculesBB");
      public static MenuBarItemId NewReactionBB = createMenuBarItemId("NewReactionBB");
      public static MenuBarItemId NewSpatialStructure = createMenuBarItemId("NewSpatialStructure");
      public static MenuBarItemId NewPassiveTransportBB = createMenuBarItemId("NewPassiveTransportBB");
      public static MenuBarItemId NewEventBB = createMenuBarItemId("NewEventBB");
      public static MenuBarItemId NewObserverBB = createMenuBarItemId("NewObserverBB");
      public static MenuBarItemId NewSimulationSettingsBB = createMenuBarItemId("NewSimulationSettingsBB");
      public static MenuBarItemId ExportHistoryToExcel = createMenuBarItemId("ExportHistoryToExcel");
      public static MenuBarItemId SimulationExplorerView = createMenuBarItemId("SimulationExplorerView");
      public static MenuBarItemId NewMolecule = createMenuBarItemId("NewMolecule");
      public static MenuBarItemId LoadMolecule = createMenuBarItemId("LoadMolecule");
      public static MenuBarItemId LoadMoleculeFromTemplate = createMenuBarItemId("LoadMoleculeFromTemplate");
      public static MenuBarItemId AddPKSimMolecule = createMenuBarItemId("AddPKSimMolecule");
      public static MenuBarItemId NewReaction = createMenuBarItemId("NewReaction");
      public static MenuBarItemId LoadReaction = createMenuBarItemId("LoadReaction");
      public static MenuBarItemId LoadReactionFromTemplate = createMenuBarItemId("LoadReactionFromTemplate");
      public static MenuBarItemId NewTopContainer = createMenuBarItemId("NewTopContainer");
      public static MenuBarItemId LoadTopContainer = createMenuBarItemId("LoadTopContainer");
      public static MenuBarItemId LoadTopContainerFromTemplate = createMenuBarItemId("LoadTopContainerFromTemplate");
      public static MenuBarItemId NewPassiveTransport = createMenuBarItemId("NewPassiveTransport");
      public static MenuBarItemId LoadPassiveTransport = createMenuBarItemId("LoadPassiveTransport");
      public static MenuBarItemId LoadPassiveTransportFromTemplate = createMenuBarItemId("LoadPassiveTransportFromTemplate");
      public static MenuBarItemId NewObserverGroup = createMenuBarItemId("NewObserverGroup");
      public static MenuBarItemId NewAmountObserver = createMenuBarItemId("NewAmountObserver");
      public static MenuBarItemId NewContainerObserver = createMenuBarItemId("NewContainerObserver");
      public static MenuBarItemId LoadObserverGroup = createMenuBarItemId("LoadObserverGroup");
      public static MenuBarItemId LoadAmountObserver = createMenuBarItemId("LoadAmountObserver");
      public static MenuBarItemId LoadContainerObserver = createMenuBarItemId("LoadContainerObserver");
      public static MenuBarItemId LoadObserverFromTemplateGroup = createMenuBarItemId("LoadObserverFromTemplateGroup");
      public static MenuBarItemId LoadAmountObserverFromTemplate = createMenuBarItemId("LoadAmountObserverFromTemplate");
      public static MenuBarItemId LoadContainerObserverFromTemplate = createMenuBarItemId("LoadContainerObserverFromTemplate");
      public static MenuBarItemId NewEvent = createMenuBarItemId("NewEvent");
      public static MenuBarItemId LoadEvent = createMenuBarItemId("LoadEvent");
      public static MenuBarItemId LoadEventFromTemplate = createMenuBarItemId("LoadEventFromTemplate");
      public static MenuBarItemId NewReactionMolecule = createMenuBarItemId("NewReactionMolecule");
      public static MenuBarItemId ZoomIn = createMenuBarItemId("ZoomIn");
      public static MenuBarItemId ZoomOut = createMenuBarItemId("ZoomOut");
      public static MenuBarItemId FitToPage = createMenuBarItemId("FitToPage");
      public static MenuBarItemId MoleculeStartValuesExtend = createMenuBarItemId("MoleculeStartValuesExtend");
      public static MenuBarItemId ParameterStartValuesExtend = createMenuBarItemId("ParameterStartValuesExtend");
      public static MenuBarItemId Merge = createMenuBarItemId("Merge");
      public static MenuBarItemId Help = createMenuBarItemId("Help");
      public static MenuBarItemId SearchView = createMenuBarItemId("SearchView");
      public static MenuBarItemId ExportProjectToPDF = createMenuBarItemId("ExportProjectToPDF");
      public static MenuBarItemId HistoryReportGroup = createMenuBarItemId("HistoryReportGroup");
      public static MenuBarItemId ExportHistoryToPDF = createMenuBarItemId("ExportHistoryToPDF");
      public static MenuBarItemId NotificationView = createMenuBarItemId("NotificationView");
      public static MenuBarItemId ComparisonView = createMenuBarItemId("ComparisonView");
      public static MenuBarItemId RunWithSettings = createMenuBarItemId("RunWithSettings");
      public static MenuBarItemId ManageProjectDisplayUnits = createMenuBarItemId("ManageProjectDisplayUnits");
      public static MenuBarItemId ManageUserDisplayUnits = createMenuBarItemId("ManageUserDisplayUnits");
      public static MenuBarItemId UpdateAllToDisplayUnits = createMenuBarItemId("UpdateAllToDisplayUnits");
      public static MenuBarItemId CalculateScaleFactors = createMenuBarItemId("CalculateScaleFactors");
      public static MenuBarItemId NewParameterStartValue = createMenuBarItemId("NewParameterStartValue");
      public static MenuBarItemId NewMoleculeStartValue = createMenuBarItemId("NewMoleculeStartValue");
      public static MenuBarItemId CreateJournalPage = createMenuBarItemId("CreateJournalPage");
      public static MenuBarItemId JournalEditorView = createMenuBarItemId("JournalEditorView");
      public static MenuBarItemId JournalView = createMenuBarItemId("JournalView");
      public static MenuBarItemId SelectJournal = createMenuBarItemId("SelectJournal");
      public static MenuBarItemId JournalDiagramView = createMenuBarItemId("JournalDiagramView");
      public static MenuBarItemId SearchJournal = createMenuBarItemId("SearchJournal");
      public static MenuBarItemId LoadFavorites = createMenuBarItemId("LoadFavorites");
      public static MenuBarItemId SaveFavorites = createMenuBarItemId("SaveFavorites");
      public static MenuBarItemId ExportJournal = createMenuBarItemId("ExportJournal");
      public static MenuBarItemId RefreshJournal = createMenuBarItemId("RefreshJournal");
      public static MenuBarItemId ImportSBML = createMenuBarItemId("ImportSBML");
      public static MenuBarItemId RunParameterIdentification = createMenuBarItemId("RunParameterIdentification");
      public static MenuBarItemId StopParameterIdentification = createMenuBarItemId("StopParameterIdentification");
      public static MenuBarItemId TimeProfileParameterIdentification = createMenuBarItemId("TimeProfileParameterIdentification");
      public static MenuBarItemId PredictedVsObservedParameterIdentification = createMenuBarItemId("PredictedVsObservedParameterIdentification");
      public static MenuBarItemId ResidualsVsTimeParameterIdentifcation = createMenuBarItemId("ResidualsVsTimeParameterIdentifcation");
      public static MenuBarItemId ResidualHistogramParameterIdentification = createMenuBarItemId("ResidualHistogramParameterIdentification");
      public static MenuBarItemId ParameterIdentificationFeedbackView = createMenuBarItemId("ParameterIdentificationFeedbackView");
      public static MenuBarItemId CorrelationMatrixParameterIdentification = createMenuBarItemId("CorrelationMatrixParameterIdentification");
      public static MenuBarItemId CovarianceMatrixParameterIdentification = createMenuBarItemId("CovarianceMatrixParameterIdentification");
      public static MenuBarItemId CreateParameterIdentification = createMenuBarItemId("CreateParameterIdentification");
      public static MenuBarItemId TimeProfileConfidenceInterval = createMenuBarItemId("TimeProfileConfidenceInterval");
      public static MenuBarItemId TimeProfilePredictionInterval = createMenuBarItemId("TimeProfilePredictionInterval");
      public static MenuBarItemId TimeProfileVPCInterval = createMenuBarItemId("TimeProfileVPCInterval");
      public static MenuBarItemId CreateSensitivityAnalysis = createMenuBarItemId("CreateSensitivityAnalysis");
      public static MenuBarItemId RunSensitivityAnalysis = createMenuBarItemId("RunSensitivityAnalysis");
      public static MenuBarItemId StopSensitivityAnalysis = createMenuBarItemId("StopSensitivityAnalysis");
      public static MenuBarItemId SensitivityAnalysisPKParameterAnalysis = createMenuBarItemId("SensitivityAnalysisPKParameterAnalysis");
      public static MenuBarItemId SensitivityAnalysisFeedbackView = createMenuBarItemId("SensitivityAnalysisFeedbackView");

      private static MenuBarItemId createMenuBarItemId(string name)
      {
         var menuBarItemId = new MenuBarItemId(name, _allMenuBarItemIds.Count);
         _allMenuBarItemIds.Add(menuBarItemId);
         return menuBarItemId;
      }
   }
}