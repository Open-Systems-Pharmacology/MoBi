using System.Collections.Generic;
using System.Drawing;
using MoBi.Core;
using MoBi.Core.Domain;
using MoBi.Core.Domain.Model.Diagram;
using MoBi.Presentation.Settings;
using OSPSuite.Assets;
using OSPSuite.Core.Comparison;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Diagram.Elements;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Presenters.SensitivityAnalyses;
using OSPSuite.Presentation.Services;
using OSPSuite.Presentation.Settings;
using OSPSuite.UI.Diagram.Elements;

namespace MoBi.BatchTool.Services
{
   public class BatchUserSettings : IUserSettings
   {
      public string DefaultChartEditorLayout { get; set; }
      public string ActiveSkin { get; set; }
      public Scalings DefaultChartYScaling { get; set; }
      public IconSize IconSizeTreeView { get; set; }
      public IconSize IconSizeTab { get; set; }
      public IconSize IconSizeContextMenu { get; set; }
      public Color ChartBackColor { get; set; }
      public Color ChartDiagramBackColor { get; set; }
      public bool ColorGroupObservedDataFromSameFolder { get; set; }
      public DisplayUnitsManager DisplayUnits { get; set; }
      public IList<string> ProjectFiles { get; set; }
      public uint MRUListItemCount { get; set; }
      public ComparerSettings ComparerSettings { get; set; }
      public IDiagramOptions DiagramOptions { get; set; }
      public string ChartEditorLayout { get; set; }

      public JournalPageEditorSettings JournalPageEditorSettings { get; set; }
      public ParameterIdentificationFeedbackEditorSettings ParameterIdentificationFeedbackEditorSettings { get; set; }
      public SensitivityAnalysisFeedbackEditorSettings SensitivityAnalysisFeedbackEditorSettings { get; set; }
      public MergeConflictViewSettings MergeConflictViewSettings { get; set; }
      public string PKSimPath { get; set; }

      public string IconSizeGeneral { get; set; }
      public bool RenameDependentObjectsDefault { get; set; }
      public bool CheckDimensions { get; set; }
      public IForceLayoutConfiguration ForceLayoutConfigutation { get; set; }
      public ChartOptions ChartOptions { get; set; }
      public string MainViewLayout { get; set; }
      public string RibbonLayout { get; set; }
      public ObjectPathType ObjectPathType { get; set; }
      public string ParameterDefaultDimension { get; set; }

      public void SaveLayout()
      {
      }

      public void RestoreLayout()
      {
      }

      public int LayoutVersion { get; set; }
      public uint DecimalPlace { get; set; }
      public DirectoryMapSettings DirectoryMapSettings { get; private set; }
      public IEnumerable<DirectoryMap> UsedDirectories { get; private set; }
      public SimulationSettings LastSimulationSettings { get; set; }
      public bool ShowAdvancedParameters { get; set; }
      public NotificationType VisibleNotification { get; set; }
      public bool ShowPKSimDimensionProblemWarnings { get; set; }
      public bool ShowCannotCalcErrors { get; set; }
      public bool ShowPKSimObserverMessages { get; set; }
      public bool ShowUnresolvedEndosomeMessagesForInitialConditions { get; set; }
      public bool CheckRules { get; set; }
      public bool CheckCircularReference { get; set; }
      public ValidationSettings ValidationSettings { get; private set; }
      public OutputSelections OutputSelections { get; set; }
      public bool GroupParameters { get; set; }

      public BatchUserSettings()
      {
         ProjectFiles = new List<string>();
         DiagramOptions = new DiagramOptions();
         ForceLayoutConfigutation = new ForceLayoutConfiguration();
         ChartOptions = new ChartOptions();
         DisplayUnits = new DisplayUnitsManager();
         ComparerSettings = new ComparerSettings { CompareHiddenEntities = true };
         JournalPageEditorSettings = new JournalPageEditorSettings();
         ParameterIdentificationFeedbackEditorSettings = new ParameterIdentificationFeedbackEditorSettings();
         MergeConflictViewSettings = new MergeConflictViewSettings();
      }

      public int MaximumNumberOfCoresToUse { get; set; }
      public int NumberOfBins { get; set; }
      public int NumberOfIndividualsPerBin { get; set; }
   }
}