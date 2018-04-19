using System;
using System.IO;
using Microsoft.Win32;
using MoBi.Assets;
using OSPSuite.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Infrastructure.Configuration;
using OSPSuite.Assets;

namespace MoBi.Core
{
   public interface IMoBiConfiguration : IApplicationConfiguration
   {
      string SpaceOrganismUserTemplate { get; }
      string SpaceOrganismBaseTemplate { get; }
      string PKSimPath { get; }
      string TemplateFolder { get; }
      string CalculationMethodRepositoryFile { get; set; }
      string GroupRepositoryFile { get; }
      string StandardMoleculeTemplateFile { get; }
   }

   public class MoBiConfiguration : OSPSuiteConfiguration, IMoBiConfiguration
   {
      public string CalculationMethodRepositoryFile { get; set; }
      public override string ProductName { get; } = AppConstants.PRODUCT_NAME;
      public override Origin Product { get; } = Origins.MoBi;
      public override string ProductNameWithTrademark { get; } = AppConstants.PRODUCT_NAME;
      public override ApplicationIcon Icon { get; } = ApplicationIcons.MoBi;
      protected override string[] LatestVersionWithOtherMajor { get; } = { "3.6", "6.3" };
      public override string UserSettingsFileName { get; } = "Settings.xml";
      public override string ApplicationSettingsFileName { get; } = "ApplicationSettings.xml";
      public override string IssueTrackerUrl { get; } = AppConstants.IssueTrackerUrl;
      public string GroupRepositoryFile { get; }
      public string TemplateFolder { get; }
      public string SpaceOrganismUserTemplate { get; }
      public string SpaceOrganismBaseTemplate { get; }
      public string StandardMoleculeTemplateFile { get; }
      public override string WatermarkOptionLocation { get; } = "Utilties -> Options -> Application";
      public override string ApplicationFolderPathName { get; } = AppConstants.SpecialFileNames.APPLICATION_FOLDER_PATH;

      public MoBiConfiguration()
      {
         CalculationMethodRepositoryFile = LocalOrAllUsersPathForFile(AppConstants.SpecialFileNames.CALCULATION_METHOD_REPOSITORY_FILE_NAME);
         GroupRepositoryFile = LocalOrAllUsersPathForFile(AppConstants.SpecialFileNames.GROUP_REPOSITORY_FILE_NAME);
         TemplateFolder =  LocalOrAllUsersPathForFolder(AppConstants.SpecialFileNames.TEMPLATES_FOLDER, AppConstants.SpecialFileNames.TEMPLATES_FOLDER);
         SpaceOrganismUserTemplate = CurrentUserFile(AppConstants.SpecialFileNames.SPATIAL_STRUCTURE_TEMPLATE);
         SpaceOrganismBaseTemplate = LocalOrAllUsersPathForFile(AppConstants.SpecialFileNames.SPATIAL_STRUCTURE_TEMPLATE);
         StandardMoleculeTemplateFile = templateFile(AppConstants.SpecialFileNames.STANDARD_MOLECULE);
         
      }

      public string PKSimPath
      {
         get
         {
            try
            {
              return (string) Registry.GetValue($@"HKEY_LOCAL_MACHINE\SOFTWARE\{Constants.RegistryPaths.PKSIM_REG_PATH}{MajorVersion}", Constants.RegistryPaths.INSTALL_PATH, null);
            }
            catch (Exception)
            {
               return string.Empty;
            }
         }
      }

      private string templateFile(string fileName) => Path.Combine(TemplateFolder, fileName);
   }
}