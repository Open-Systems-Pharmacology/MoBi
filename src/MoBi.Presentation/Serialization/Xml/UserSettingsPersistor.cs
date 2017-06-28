using System.IO;
using System.Linq;
using System.Xml.Linq;
using MoBi.Core;
using MoBi.Core.Serialization.Services;
using MoBi.Core.Serialization.Xml;
using MoBi.Core.Serialization.Xml.Services;
using MoBi.Presentation.Settings;
using OSPSuite.Utility;

namespace MoBi.Presentation.Serialization.Xml
{
   public class UserSettingsPersistor : IUserSettingsPersistor
   {
      private readonly IMoBiXmlSerializerRepository _serializerRepository;
      private readonly IMoBiConfiguration _configuration;
      private readonly ISerializationContextFactory _serializationContextFactory;
      private readonly IUserSettings _userSettings;

      public UserSettingsPersistor(IUserSettings userSettings, IMoBiXmlSerializerRepository serializerRepository, IMoBiConfiguration configuration, ISerializationContextFactory serializationContextFactory)
      {
         _userSettings = userSettings;
         _serializerRepository = serializerRepository;
         _configuration = configuration;
         _serializationContextFactory = serializationContextFactory;
      }

      public void Save()
      {
         _userSettings.SaveLayout();
         using (var serializationContext = _serializationContextFactory.Create())
         {
            var serializer = _serializerRepository.SerializerFor(_userSettings);
            var xml = serializer.Serialize(_userSettings, serializationContext);
            var doc = XDocument.Load(new StringReader(xml.ToString()));
            doc.Save(_configuration.UserSettingsFilePath);
         }
      }

      public void Load()
      {
         foreach (var filePath in _configuration.UserSettingsFilePaths.Where(FileHelper.FileExists))
         {
            try
            {
               using (var serializationContext = _serializationContextFactory.Create())
               {
                  var xml = XDocument.Load(filePath).Root;
                  var serializer = _serializerRepository.SerializerFor(_userSettings);
                  serializer.Deserialize(_userSettings, xml, serializationContext);
                  return;
               }
            }
            catch
            {
               //continue trying older versions
            }
         }
      }
   }
}