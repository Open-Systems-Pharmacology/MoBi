using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using OSPSuite.Assets;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Format;
using OSPSuite.Utility.Validation;
using DevExpress.XtraBars.Docking;
using DevExpress.XtraBars.Ribbon;
using MoBi.Assets;
using MoBi.Core;
using MoBi.Core.Domain;
using MoBi.Core.Domain.Model.Diagram;
using MoBi.Presentation.Settings;
using OSPSuite.Core;
using OSPSuite.Core.Comparison;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Diagram.Elements;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Presenters.SensitivityAnalyses;
using OSPSuite.Presentation.Services;
using OSPSuite.Presentation.Settings;
using OSPSuite.UI.Diagram.Elements;

namespace MoBi.UI.Settings
{
   public class UserSettings : ValidatableDTO, IUserSettings
   {
      public int LayoutVersion { get; set; }

      private readonly DockManager _dockManager;
      private readonly RibbonBarManager _ribbonManager;

      public bool CheckDimensions
      {
         set { _validationSettings.CheckDimensions = value; }
         get { return _validationSettings.CheckDimensions; }
      }

      public IList<string> ProjectFiles { get; set; }
      public uint MRUListItemCount { get; set; }
      public ComparerSettings ComparerSettings { get; set; }
      public string IconSizeGeneral { get; set; }
      public string ActiveSkin { get; set; }

      public Scalings DefaultChartYScaling
      {
         get { return ChartOptions.DefaultChartYScaling; }
         set { ChartOptions.DefaultChartYScaling = value; }
      }

      public IconSize IconSizeTreeView { get; set; }
      public IconSize IconSizeTab { get; set; }
      public IconSize IconSizeContextMenu { get; set; }

      public Color ChartBackColor
      {
         get { return ChartOptions.DefaultChartBackColor; }
         set { ChartOptions.DefaultChartBackColor = value; }
      }

      public Color ChartDiagramBackColor
      {
         get { return ChartOptions.DefaultChartDiagramBackColor; }
         set { ChartOptions.DefaultChartDiagramBackColor = value; }
      }

      public bool RenameDependentObjectsDefault { get; set; }
      public IDiagramOptions DiagramOptions { get; set; }
      public IForceLayoutConfiguration ForceLayoutConfigutation { get; set; }
      public ChartOptions ChartOptions { get; set; }
      public string MainViewLayout { get; set; }
      public ObjectPathType ObjectPathType { get; set; }
      public string ParameterDefaultDimension { get; set; }
      public string RibbonLayout { get; set; }
      private readonly DirectoryMapSettings _directoryMapSettings;
      public ISimulationSettings LastSimulationSettings { get; set; }
      public NotificationType VisibleNotification { get; set; }
      public OutputSelections OutputSelections { get; set; }
      public DisplayUnitsManager DisplayUnits { get; set; }
      private readonly INumericFormatterOptions _numericFormatterOptions;
      private readonly ValidationSettings _validationSettings;
      public bool ShowAdvancedParameters { get; set; }
      public bool GroupParameters { get; set; }
      public string ChartEditorLayout { get; set; }
      public int MaximumNumberOfCoresToUse { get; set; }
      public int NumberOfBins { get; set; }
      public int NumberOfIndividualsPerBin { get; set; }
      public JournalPageEditorSettings JournalPageEditorSettings { get; set; }
      public ParameterIdentificationFeedbackEditorSettings ParameterIdentificationFeedbackEditorSettings { get; set; }
      public SensitivityAnalysisFeedbackEditorSettings SensitivityAnalysisFeedbackEditorSettings { get; set; }
      public MergeConflictViewSettings MergeConflictViewSettings { get; set; }


      public UserSettings(DockManager dockManager, RibbonBarManager ribbonManager, DirectoryMapSettings directoryMapSettings, IMoBiConfiguration configuration)
      {
         _dockManager = dockManager;
         _ribbonManager = ribbonManager;
         _directoryMapSettings = directoryMapSettings;
         _numericFormatterOptions = NumericFormatterOptions.Instance;
         IconSizeGeneral = IconSizes.Size24x24.Id;
         ActiveSkin = AppConstants.DefaultSkin;
         RenameDependentObjectsDefault = true;
         MRUListItemCount = 5;
         ProjectFiles = new List<string>();
         DiagramOptions = new DiagramOptions();
         ForceLayoutConfigutation = new ForceLayoutConfiguration();
         ChartOptions = new ChartOptions();
         ParameterDefaultDimension = Constants.Dimension.DIMENSIONLESS;
         _directoryMapSettings.AddUsedDirectory(AppConstants.DirectoryKey.LAYOUT, configuration.CurrentUserFolderPath);
         ShowAdvancedParameters = true;
         GroupParameters = false;
         VisibleNotification = NotificationType.Warning | NotificationType.Error;
         _validationSettings = new ValidationSettings { CheckDimensions = true };
         DisplayUnits = new DisplayUnitsManager();
         DefaultChartYScaling = Scalings.Log;
         IconSizeTreeView = IconSizes.Size24x24;
         IconSizeTab = IconSizes.Size16x16;
         IconSizeContextMenu = IconSizes.Size16x16;
         ComparerSettings = new ComparerSettings { CompareHiddenEntities = true };
         JournalPageEditorSettings = new JournalPageEditorSettings();
         ParameterIdentificationFeedbackEditorSettings = new ParameterIdentificationFeedbackEditorSettings();
         SensitivityAnalysisFeedbackEditorSettings = new SensitivityAnalysisFeedbackEditorSettings();
         MergeConflictViewSettings = new MergeConflictViewSettings();
         MaximumNumberOfCoresToUse = Math.Max(Environment.ProcessorCount - 1, 1);
         NumberOfBins = AppConstants.DEFAULT_NUMBER_OF_BINS;
         NumberOfIndividualsPerBin = AppConstants.DEFAULT_NUMBER_OF_INDIVIDUALS_PER_BIN;
         Rules.AddRange(AllRules.All());
      }

      public string DefaultChartEditorLayout
      {
         get { return ChartOptions.DefaultLayoutName; }
         set { ChartOptions.DefaultLayoutName = value; }
      }

      public bool ShowPKSimObserverMessages
      {
         set { _validationSettings.ShowPKSimObserverMessages = value; }
         get { return _validationSettings.ShowPKSimObserverMessages; }
      }

      public bool CheckRules
      {
         get { return _validationSettings.CheckRules; }
         set { _validationSettings.CheckRules = value; }
      }

      public ValidationSettings ValidationSettings
      {
         get { return _validationSettings; }
      }

      public uint DecimalPlace
      {
         get { return _numericFormatterOptions.DecimalPlace; }
         set { _numericFormatterOptions.DecimalPlace = value; }
      }

      public DirectoryMapSettings DirectoryMapSettings
      {
         get { return _directoryMapSettings; }
      }

      public IEnumerable<DirectoryMap> UsedDirectories
      {
         get { return _directoryMapSettings.UsedDirectories; }
      }

      public void SaveLayout()
      {
         LayoutVersion = AppConstants.LayoutVersion;
         var streamMainView = new MemoryStream();
         _dockManager.SaveLayoutToStream(streamMainView);
         MainViewLayout = streamToString(streamMainView);
         var streamRibbon = new MemoryStream();
         _ribbonManager.Ribbon.Toolbar.SaveLayoutToStream(streamRibbon);
         RibbonLayout = streamToString(streamRibbon);
      }

      public void RestoreLayout()
      {
         if (LayoutVersion != AppConstants.LayoutVersion)
            resetLayout();

         if (!MainViewLayout.IsNullOrEmpty())
            _dockManager.RestoreFromStream(streamFromString(MainViewLayout));

         if (!RibbonLayout.IsNullOrEmpty())
            _ribbonManager.RestoreFromStream(streamFromString(RibbonLayout));
      }

      private void resetLayout()
      {
         MainViewLayout = string.Empty;
         RibbonLayout = string.Empty;
      }

      private MemoryStream streamFromString(string stringToConvert)
      {
         return new MemoryStream(stringToConvert.ToByteArray());
      }

      private string streamToString(MemoryStream streamToConvert)
      {
         return streamToConvert.ToArray().ToByteString();
      }

      public bool ShowPKSimDimensionProblemWarnings
      {
         set { _validationSettings.ShowPKSimDimensionProblemWarnings = value; }
         get { return _validationSettings.ShowPKSimDimensionProblemWarnings; }
      }

      public bool ShowCannotCalcErrors
      {
         set { _validationSettings.ShowCannotCalcErrors = value; }
         get { return _validationSettings.ShowCannotCalcErrors; }
      }

      private static class AllRules
      {
         private static IBusinessRule numberOfCoreSmallerThanNumberOfProcessor
         {
            get
            {
               return CreateRule.For<ICoreUserSettings>()
                  .Property(x => x.MaximumNumberOfCoresToUse)
                  .WithRule((x, numCore) => numCore > 0 && numCore <= Environment.ProcessorCount)
                  .WithError(OSPSuite.Assets.Error.NumberOfCoreToUseShouldBeInferiorAsTheNumberOfProcessor(Environment.ProcessorCount));
            }
         }

         public static IEnumerable<IBusinessRule> All()
         {
            yield return numberOfCoreSmallerThanNumberOfProcessor;
         }
      }
   }
}
