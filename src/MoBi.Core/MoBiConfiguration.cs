using System;
using System.IO;
using Microsoft.Win32;
using MoBi.Assets;
using OSPSuite.Core;
using OSPSuite.Core.Domain;
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
      public override string ProductNameWithTrademark { get; } = AppConstants.PRODUCT_NAME_WITH_TRADEMARK;
      public override string IconName { get; } = ApplicationIcons.MoBi.IconName;
      protected override string[] LatestVersionWithOtherMajor { get; } = {"7.4", "6.3", "3.6" };
      public override string UserSettingsFileName { get; } = "Settings.xml";
      public override string ApplicationSettingsFileName { get; } = "ApplicationSettings.xml";
      public override string IssueTrackerUrl { get; } = AppConstants.IssueTrackerUrl;
      public string GroupRepositoryFile { get; }
      public string TemplateFolder { get; }
      public string SpaceOrganismUserTemplate { get; }
      public string SpaceOrganismBaseTemplate { get; }
      public string StandardMoleculeTemplateFile { get; }
      public override string WatermarkOptionLocation { get; } = "Utilities -> Options -> Application";
      public override string ApplicationFolderPathName { get; } = AppConstants.SpecialFileNames.APPLICATION_FOLDER_PATH;
      public override int InternalVersion { get; } = ProjectVersions.Current;

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
              return (string) Registry.GetValue($@"HKEY_LOCAL_MACHINE\SOFTWARE\{Constants.RegistryPaths.PKSIM_REG_PATH}{Major}", Constants.RegistryPaths.INSTALL_PATH, null);
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