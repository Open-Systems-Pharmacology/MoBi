using System;
using System.IO;
using Microsoft.Win32;
using MoBi.Assets;
using OSPSuite.Utility;
using OSPSuite.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Infrastructure.Configuration;
using OSPSuite.Assets;

namespace MoBi.Core
{
   public interface IMoBiConfiguration : IApplicationConfiguration
   {
      bool IsToken { get; set; }
      string AllUsersFolderPath { get; }
      string SpaceOrganismUserTemplate { get; }
      string SpaceOrganismBaseTemplate { get; }
      string UserSettings { get; }
      string CurrentUserFolderPath { get; }
      string AllUsersFile(string fileName);
      string CurrentUserFile(string fileName);
      string PKSimPath { get; }
      string TemplateFile(string fileName);
      string TemplateFolder { get; }
      string CalculationMethodRepositoryFile { get; set; }
      string GroupRepositoryFile { get; }
      string StandardMoleculeTemplateFile { get; }
      string DimensionFactoryFile { get; set; }
   }

   public class MoBiConfiguration : OSPSuiteConfiguration, IMoBiConfiguration
   {
      private static readonly string[] LATEST_VERSION_WITH_OTHER_MAJOR = {"3.6"};
      public bool IsToken { get; set; }
      public string CalculationMethodRepositoryFile { get; set; }
      public string DimensionFactoryFile { get; set; }

      public MoBiConfiguration()
      {
         CalculationMethodRepositoryFile = AllUsersFile(AppConstants.SpecialFileNames.CalculationMethodRepositoryFileName);
         DimensionFactoryFile = AllUsersFile(AppConstants.SpecialFileNames.DimensionFactoryFileName);
         PKParametersFilePath = AllUsersFile(AppConstants.SpecialFileNames.PKParametersFileName);
      }

      protected override string ApplicationFolderPathWithRevision(string revision)
      {
         return Path.Combine(AppConstants.SpecialFileNames.ApplicationFolderPath, revision);
      }

      protected override string[] LatestVersionWithOtherMajor => LATEST_VERSION_WITH_OTHER_MAJOR;

      public override string ProductName { get; } = AppConstants.ProductName;

      public override Origin Product { get; } = Origins.MoBi;
   
      public override string ProductNameWithTrademark { get; } = AppConstants.ProductName;

      public override ApplicationIcon Icon { get; } = ApplicationIcons.MoBi;

      public override string UserSettingsFileName { get; } = "Settings.xml";

      public string CurrentUserFolderPath => dataFolderFor(EnvironmentHelper.UserApplicationDataFolder());

      public string AllUsersFolderPath => dataFolderFor(EnvironmentHelper.ApplicationDataFolder());

      private string dataFolderFor(string topPath) => Path.Combine(topPath, AppConstants.SpecialFileNames.ApplicationFolderPath, MajorVersion);

      public override string TEXTemplateFolderPath => Path.Combine(AllUsersFolderPath, "TEXTemplates");

      public string AllUsersFile(string fileName) => Path.Combine(AllUsersFolderPath, fileName);

      public string CurrentUserFile(string fileName) => Path.Combine(CurrentUserFolderPath, fileName);

      public string PKSimPath
      {
         get
         {
            try
            {
              return (string) Registry.GetValue($@"HKEY_LOCAL_MACHINE\SOFTWARE\{AppConstants.PKSimRegPath}{MajorVersion}", "InstallPath", null);
            }
            catch (Exception)
            {
               return string.Empty;
            }
         }
      }

      public string StandardMoleculeTemplateFile => TemplateFile("Standard Molecule.pkml");

      public string GroupRepositoryFile => AllUsersFile(AppConstants.SpecialFileNames.GroupRepositoryFileName);

      public string TemplateFile(string fileName)
      {
         return Path.Combine(TemplateFolder, fileName);
      }

      public string TemplateFolder => Path.Combine(AllUsersFolderPath, AppConstants.SpecialFileNames.TemplatesFolder);

      public override string ChartLayoutTemplateFolderPath => AllUsersFile("Layouts");

      public string SpaceOrganismUserTemplate => CurrentUserFile("SpaceOrganismTemplate.mbdt");

      public string SpaceOrganismBaseTemplate => AllUsersFile("SpaceOrganismTemplate.mbdt");

      public string UserSettings => CurrentUserFile(UserSettingsFileName);

      public override string IssueTrackerUrl { get; } = AppConstants.IssueTrackerUrl;
   }
}