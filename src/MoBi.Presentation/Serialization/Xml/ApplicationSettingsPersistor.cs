using System.Collections.Generic;
using MoBi.Core;
using MoBi.Core.Serialization.Services;
using MoBi.Core.Serialization.Xml.Services;

namespace MoBi.Presentation.Serialization.Xml
{
   public class ApplicationSettingsPersistor : SettingsPersistor<IApplicationSettings>
   {
      public ApplicationSettingsPersistor(IMoBiXmlSerializerRepository serializerRepository, IMoBiConfiguration configuration, ISerializationContextFactory serializationContextFactory, IApplicationSettings settings) :
         base(serializerRepository, configuration, serializationContextFactory, settings)
      {
      }

      protected override string SettingsFilePath => _configuration.ApplicationSettingsFilePath;
      public override IEnumerable<string> AllSettingsFilePath => _configuration.ApplicationSettingsFilePaths;
   }
}