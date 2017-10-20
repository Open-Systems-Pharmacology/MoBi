using System.Collections.Generic;
using MoBi.Core;
using MoBi.Core.Serialization.Services;
using MoBi.Core.Serialization.Xml.Services;
using MoBi.Presentation.Settings;

namespace MoBi.Presentation.Serialization.Xml
{
   public class UserSettingsPersistor : SettingsPersistor<IUserSettings>
   {
      private readonly IUserSettings _userSettings;

      public UserSettingsPersistor(IMoBiXmlSerializerRepository serializerRepository, IMoBiConfiguration configuration, ISerializationContextFactory serializationContextFactory, IUserSettings userSettings)
         : base(serializerRepository, configuration, serializationContextFactory, userSettings)
      {
         _userSettings = userSettings;
      }

      public override void Save()
      {
         _userSettings.SaveLayout();
         base.Save();
      }

      protected override string SettingsFilePath => _configuration.UserSettingsFilePath;

      public override IEnumerable<string> AllSettingsFilePath => _configuration.UserSettingsFilePaths;
   }
}