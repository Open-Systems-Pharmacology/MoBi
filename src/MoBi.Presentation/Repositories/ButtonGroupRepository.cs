using System.Collections.Generic;
using MoBi.Assets;
using OSPSuite.Presentation.MenuAndBars;
using MoBi.Presentation.MenusAndBars;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Repositories;
using OSPSuite.Assets;

namespace MoBi.Presentation.Repositories
{
   public class ButtonGroupRepository : OSPSuite.Presentation.Repositories.ButtonGroupRepository
   {
      public ButtonGroupRepository(IMenuBarItemRepository menuBarItemRepository) : base(menuBarItemRepository)
      {
      }

      protected override IEnumerable<IButtonGroup> AllButtonGroups()
      {
         yield return applicationButtonGroup;
         yield return buildingBlocksButtonGroup;
         yield return toolsButtonGroup;
         yield return simulationButtonGroup;
         yield return viewButtonGroup;
         yield return importButtonGroup;
         yield return exportButtonGroup;
         yield return addMoleculeButtonGroup;
         yield return addReactionButtonGroup;
         yield return editDiagramButtonGroup;
         yield return addOrganismButtonGroup;
         yield return addPassiveTransportButtonGroup;
         yield return addObserverButtonGroup;
         yield return addEventButtonGroup;
         yield return addMoleculeStartValueButtonGroup;
         yield return addParameterStartValueButtonGroup;
         yield return workflowButtonGroup;
         yield return displayUnitsButtonGroup;
         yield return journalButtonGroup;
         yield return favoritesButtonGroup;
         yield return runParameterIdentificationButtonGroup;
         yield return parameterIdentificationAnalysisButtonGroup;
         yield return parameterIdentificationButtonGroup;
         yield return parameterIdentificationConfidenceIntervalButtonGroup;
         yield return parameterSensitivityButtonGroup;
         yield return runSensitivityAnalysisButtonGroup;
         yield return senstivityAnalysisButtonGroup;
      }

      private IButtonGroup parameterIdentificationButtonGroup
      {
         get
         {
            return CreateButtonGroup.WithCaption(Ribbons.ParameterIdentification)
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.CreateParameterIdentification)))
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.ParameterIdentificationFeedbackView)))
               .WithId(ButtonGroupIds.ParameterIdentification);
         }
      }

      private IButtonGroup parameterIdentificationAnalysisButtonGroup
      {
         get
         {
            return CreateButtonGroup.WithCaption(Ribbons.ParameterIdentificationAnalyses)
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.TimeProfileParameterIdentification)))
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.PredictedVsObservedParameterIdentification)))
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.ResidualsVsTimeParameterIdentifcation)))
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.ResidualHistogramParameterIdentification)))
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.CovarianceMatrixParameterIdentification)))
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.CorrelationMatrixParameterIdentification)))
               .WithId(ButtonGroupIds.ParameterIdentificationAnalyses);
         }
      }

      private IButtonGroup parameterIdentificationConfidenceIntervalButtonGroup
      {
         get
         {
            return CreateButtonGroup.WithCaption(Ribbons.ParameterIdentificationConfidenceInterval)
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.TimeProfileConfidenceInterval)))
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.TimeProfileVPCInterval)))
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.TimeProfilePredictionInterval)))
               .WithId(ButtonGroupIds.ParameterIdentificationConfidenceInterval);
         }
      }

      private IButtonGroup runParameterIdentificationButtonGroup
      {
         get
         {
            return CreateButtonGroup.WithCaption(Ribbons.ParameterIdentification)
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.RunParameterIdentification)))
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.StopParameterIdentification)))
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.ParameterIdentificationFeedbackView)))
               .WithId(ButtonGroupIds.RunParameterIdentification);
         }
      }

      private IButtonGroup applicationButtonGroup
      {
         get
         {
            return CreateButtonGroup.WithCaption(AppConstants.BarNames.File)
               .WithButton(
                  CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.NewProject))
                     .WithSubItem(_menuBarItemRepository.Find(MenuBarItemIds.NewAmountProject))
                     .WithSubItem(_menuBarItemRepository.Find(MenuBarItemIds.NewConcentrationProject))
                     .WithStyle(ItemStyle.Large)
               )
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.OpenProject)))
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.OpenSimulation)))
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.ImportSBML)))
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.CloseProject)))
               .WithButton(
                  CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.SaveGroup))
                     .WithSubItem(_menuBarItemRepository.Find(MenuBarItemIds.SaveProject))
                     .WithSubItem(_menuBarItemRepository.Find(MenuBarItemIds.SaveProjectAs))
                     .WithStyle(ItemStyle.Large)
                     .AsGroupStarter()
               )
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.SelectJournal)))
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.About)).AsGroupStarter())
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.Exit)).AsGroupStarter())
               .WithId(ButtonGroupIds.File);
         }
      }

      private IButtonGroup displayUnitsButtonGroup
      {
         get
         {
            return CreateButtonGroup.WithCaption(AppConstants.BarNames.DisplayUnits)
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.ManageUserDisplayUnits)))
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.ManageProjectDisplayUnits)))
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.UpdateAllToDisplayUnits)))
               .WithId(ButtonGroupIds.DisplayUnits);
         }
      }

      private IButtonGroup addMoleculeButtonGroup
      {
         get
         {
            return CreateButtonGroup.WithCaption(AppConstants.BarNames.Add)
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.NewMolecule)).WithCaption(AppConstants.RibbonButtonNames.New))
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.LoadMolecule)).WithCaption(AppConstants.RibbonButtonNames.Load))
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.LoadMoleculeFromTemplate)).WithCaption(AppConstants.RibbonButtonNames.LoadFromTemplate))
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.AddPKSimMolecule)).WithCaption(AppConstants.RibbonButtonNames.AddPKSimMolecule))
               .WithId(ButtonGroupIds.AddMolecule);
         }
      }

      private IButtonGroup addReactionButtonGroup
      {
         get
         {
            return CreateButtonGroup.WithCaption(AppConstants.BarNames.Add)
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.NewReaction)).WithCaption(AppConstants.RibbonButtonNames.New))
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.LoadReaction)).WithCaption(AppConstants.RibbonButtonNames.Load))
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.LoadReactionFromTemplate)).WithCaption(AppConstants.RibbonButtonNames.LoadFromTemplate))
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.NewReactionMolecule)))
               .WithId(ButtonGroupIds.AddReaction);
         }
      }

      private IButtonGroup addOrganismButtonGroup
      {
         get
         {
            return CreateButtonGroup.WithCaption(AppConstants.BarNames.Add)
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.NewTopContainer)).WithCaption(AppConstants.RibbonButtonNames.New))
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.LoadTopContainer)).WithCaption(AppConstants.RibbonButtonNames.Load))
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.LoadTopContainerFromTemplate)).WithCaption(AppConstants.RibbonButtonNames.LoadFromTemplate))
               .WithId(ButtonGroupIds.AddOrganism);
         }
      }

      private IButtonGroup addPassiveTransportButtonGroup
      {
         get
         {
            return CreateButtonGroup.WithCaption(AppConstants.BarNames.Add)
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.NewPassiveTransport)).WithCaption(AppConstants.RibbonButtonNames.New))
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.LoadPassiveTransport)).WithCaption(AppConstants.RibbonButtonNames.Load))
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.LoadPassiveTransportFromTemplate)).WithCaption(AppConstants.RibbonButtonNames.LoadFromTemplate))
               .WithId(ButtonGroupIds.AddPassiveTransport);
         }
      }

      private IButtonGroup addObserverButtonGroup
      {
         get
         {
            return CreateButtonGroup.WithCaption(AppConstants.BarNames.Add)
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.NewObserverGroup))
                  .WithCaption(AppConstants.RibbonButtonNames.New)
                  .WithSubItem(_menuBarItemRepository.Find(MenuBarItemIds.NewAmountObserver))
                  .WithSubItem(_menuBarItemRepository.Find(MenuBarItemIds.NewContainerObserver))
                  .WithStyle(ItemStyle.Large))
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.LoadObserverGroup))
                  .WithCaption(AppConstants.RibbonButtonNames.Load)
                  .WithSubItem(_menuBarItemRepository.Find(MenuBarItemIds.LoadAmountObserver))
                  .WithSubItem(_menuBarItemRepository.Find(MenuBarItemIds.LoadContainerObserver))
                  .WithStyle(ItemStyle.Large))
            .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.LoadObserverFromTemplateGroup))
                  .WithCaption(AppConstants.RibbonButtonNames.LoadFromTemplate)
                  .WithSubItem(_menuBarItemRepository.Find(MenuBarItemIds.LoadAmountObserverFromTemplate))
                  .WithSubItem(_menuBarItemRepository.Find(MenuBarItemIds.LoadContainerObserverFromTemplate))
                  .WithStyle(ItemStyle.Large))
               .WithId(ButtonGroupIds.AddObserver);
         }
      }

      private IButtonGroup addEventButtonGroup
      {
         get
         {
            return CreateButtonGroup.WithCaption(AppConstants.BarNames.Add)
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.NewEvent)).WithCaption(AppConstants.RibbonButtonNames.New))
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.LoadEvent)).WithCaption(AppConstants.RibbonButtonNames.Load))
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.LoadEventFromTemplate)).WithCaption(AppConstants.RibbonButtonNames.LoadFromTemplate))
               .WithId(ButtonGroupIds.AddEvent);
         }
      }

      private IButtonGroup editDiagramButtonGroup
      {
         get
         {
            return CreateButtonGroup.WithCaption(AppConstants.BarNames.Edit)
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.ZoomIn)))
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.ZoomOut)))
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.FitToPage)))
               .WithId(ButtonGroupIds.EditDiagram);
         }
      }

      private IButtonGroup addMoleculeStartValueButtonGroup
      {
         get
         {
            return CreateButtonGroup.WithCaption(AppConstants.BarNames.Edit)
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.MoleculeStartValuesExtend)))
               .WithId(ButtonGroupIds.EditMoleculeStartValues)
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.NewMoleculeStartValue)));
         }
      }

      private IButtonGroup importButtonGroup
      {
         get
         {
            return CreateButtonGroup.WithCaption(AppConstants.BarNames.Import)
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.LoadSimulationIntoProject)).WithCaption(AppConstants.MenuNames.LoadIntoProject))
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.AddObservedData)).WithCaption(AppConstants.RibbonButtonNames.ObservedData))
               .WithId(ButtonGroupIds.Import);
         }
      }

      private IButtonGroup exportButtonGroup
      {
         get
         {
            return CreateButtonGroup.WithCaption(AppConstants.BarNames.ExportProject)
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.HistoryReportGroup))
                  .WithSubItem(_menuBarItemRepository.Find(MenuBarItemIds.ExportHistoryToExcel))
                  .WithSubItem(_menuBarItemRepository.Find(MenuBarItemIds.ExportHistoryToPDF)))
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.ExportProjectToPDF)))
               .WithId(ButtonGroupIds.Export);
         }
      }

      private IButtonGroup buildingBlocksButtonGroup
      {
         get
         {
            return CreateButtonGroup.WithCaption(AppConstants.BarNames.BuildingBlocks)
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.NewSpatialStructure)).WithCaption(AppConstants.RibbonButtonNames.SpatialStructure))
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.NewMoleculesBB)).WithCaption(AppConstants.RibbonButtonNames.Molecules))
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.NewReactionBB)).WithCaption(AppConstants.RibbonButtonNames.Reactions))
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.NewPassiveTransportBB)).WithCaption(AppConstants.RibbonButtonNames.PassiveTransport))
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.NewObserverBB)).WithCaption(AppConstants.RibbonButtonNames.Observer))
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.NewEventBB)).WithCaption(AppConstants.RibbonButtonNames.Events))
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.NewSimulationSettingsBB)).WithCaption(AppConstants.RibbonButtonNames.SimulationSettings))
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.NewSimulation)).WithCaption(AppConstants.RibbonButtonNames.Simulation))
               .WithId(ButtonGroupIds.BuildingBlocks);
         }
      }

      private IButtonGroup toolsButtonGroup
      {
         get
         {
            return CreateButtonGroup.WithCaption(AppConstants.BarNames.Tools)
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.GarbageCollection)))
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.Options)))
               .WithId(ButtonGroupIds.Tools);
         }
      }

      private IButtonGroup viewButtonGroup
      {
         get
         {
            return
               CreateButtonGroup.WithCaption(AppConstants.BarNames.Views)
                  .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.BuildingBlockExplorerView)))
                  .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.SimulationExplorerView)))
                  .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.HistoryView)))
                  .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.ComparisonView)))
                  .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.JournalView)))
                  .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.JournalDiagramView)))
                  .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.SearchView)))
                  .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.NotificationView)))
                  .WithId(ButtonGroupIds.View);
         }
      }

      private IButtonGroup simulationButtonGroup
      {
         get
         {
            return CreateButtonGroup.WithCaption(AppConstants.BarNames.Simulation)
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.Run)))
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.RunWithSettings)))
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.Stop)))
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.CalculateScaleFactors)))
               .WithId(ButtonGroupIds.Simulation);
         }
      }

      private IButtonGroup workflowButtonGroup
      {
         get
         {
            return CreateButtonGroup.WithCaption(AppConstants.BarNames.Workflows)
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.Merge)))
               .WithId(ButtonGroupIds.Workflows);
         }
      }

      private IButtonGroup addParameterStartValueButtonGroup
      {
         get
         {
            return CreateButtonGroup.WithCaption(AppConstants.BarNames.Edit)
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.ParameterStartValuesExtend)))
               .WithId(ButtonGroupIds.EditParameterStartValues)
               .WithButton((CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.NewParameterStartValue))));
         }
      }

      private IButtonGroup journalButtonGroup
      {
         get
         {
            return CreateButtonGroup.WithCaption(AppConstants.BarNames.Journal)
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.CreateJournalPage)))
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.JournalEditorView)))
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.SearchJournal)))
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.ExportJournal)))
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.RefreshJournal)))
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.SelectJournal)))
               .WithId(ButtonGroupIds.Journal);
         }
      }


      private IButtonGroup favoritesButtonGroup
      {
         get
         {
            return CreateButtonGroup.WithCaption(AppConstants.BarNames.Favorites)
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.LoadFavorites)))
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.SaveFavorites)))
               .WithId(ButtonGroupIds.Favorites);
         }
      }

      private IButtonGroup parameterSensitivityButtonGroup
      {
         get
         {
            return CreateButtonGroup.WithCaption(Ribbons.SensitivityAnalysis)
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.CreateSensitivityAnalysis)))
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.SensitivityAnalysisFeedbackView)))
               .WithId(ButtonGroupIds.SensitivityAnalysis);
         }
      }

      private IButtonGroup runSensitivityAnalysisButtonGroup
      {
         get
         {
            return CreateButtonGroup.WithCaption(Ribbons.SensitivityAnalysis)
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.RunSensitivityAnalysis)))
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.StopSensitivityAnalysis)))
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.SensitivityAnalysisFeedbackView)))
               .WithId(ButtonGroupIds.RunSensitivityAnalysis);
         }
      }

      private IButtonGroup senstivityAnalysisButtonGroup
      {
         get
         {
            return CreateButtonGroup.WithCaption(Ribbons.ParameterSensitivityAnalyses)
               .WithButton(CreateRibbonButton.From(_menuBarItemRepository.Find(MenuBarItemIds.SensitivityAnalysisPKParameterAnalysis)))
               .WithId(ButtonGroupIds.SensitivityAnalysisPKParameterAnalyses);
         }
      }
   }
}