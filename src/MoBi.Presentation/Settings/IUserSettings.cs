using System.Collections.Generic;
using MoBi.Core;
using MoBi.Core.Domain;
using MoBi.Core.Domain.Model.Diagram;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation;
using OSPSuite.Presentation.Diagram.Elements;
using OSPSuite.Presentation.Services;
using IMoBiCoreUserSettings = MoBi.Core.ICoreUserSettings;

namespace MoBi.Presentation.Settings
{
   public interface IUserSettings : IPresentationUserSettings, IMoBiCoreUserSettings
   {
      string IconSizeGeneral { get; set; }
      bool RenameDependentObjectsDefault { get; set; }
      IForceLayoutConfiguration ForceLayoutConfigutation { get; set; }
      ChartOptions ChartOptions { get; set; }
      string MainViewLayout { get; set; }
      string RibbonLayout { get; set; }
      ObjectPathType ObjectPathType { get; set; }
      string ParameterDefaultDimension { get; set; }
      void SaveLayout();
      void RestoreLayout();
      int LayoutVersion { get; set; }

      /// <summary>
      ///    Number of decimal after the comma
      /// </summary>
      uint DecimalPlace { get; set; }

      /// <summary>
      ///    Directory map for the current user
      /// </summary>
      DirectoryMapSettings DirectoryMapSettings { get; }

      /// <summary>
      ///    Key Value pair containing for a given key (ObservedData, Project etc..) the last selected folder by the user
      /// </summary>
      IEnumerable<DirectoryMap> UsedDirectories { get; }

      bool ShowAdvancedParameters { get; set; }

      NotificationType VisibleNotification { get; set; }
      bool ShowPKSimObserverMessages { get; set; }
      bool ShowUnresolvedEndosomeMessagesForInitialConditions { get; set; }
      bool CheckCircularReference { get; set; }
      bool GroupParameters { get; set; }

      ValidationSettings ValidationSettings { get; }
      OutputSelections OutputSelections { get; set; }
      MergeConflictViewSettings MergeConflictViewSettings { get; set; }
   }
}