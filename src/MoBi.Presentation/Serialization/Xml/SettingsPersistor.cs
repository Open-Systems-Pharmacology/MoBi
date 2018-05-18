using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using MoBi.Core;
using MoBi.Core.Serialization.Services;
using MoBi.Core.Serialization.Xml.Services;
using OSPSuite.Utility;

namespace MoBi.Presentation.Serialization.Xml
{
   public interface ISettingsPersistor<TSettings>
   {
      void Save();
      void Load();
   }

   public abstract class SettingsPersistor<TSettings> : ISettingsPersistor<TSettings>
   {
      private readonly IMoBiXmlSerializerRepository _serializerRepository;
      protected readonly IMoBiConfiguration _configuration;
      private readonly ISerializationContextFactory _serializationContextFactory;
      private readonly TSettings _settings;

      protected SettingsPersistor(IMoBiXmlSerializerRepository serializerRepository, IMoBiConfiguration configuration, ISerializationContextFactory serializationContextFactory, TSettings settings)
      {
         _serializerRepository = serializerRepository;
         _configuration = configuration;
         _serializationContextFactory = serializationContextFactory;
         _settings = settings;
      }

      public virtual void Save()
      {
         using (var serializationContext = _serializationContextFactory.Create())
         {
            var serializer = _serializerRepository.SerializerFor(_settings);
            var xml = serializer.Serialize(_settings, serializationContext);
            saveSerializedSettingsToFile(xml);
         }
      }

      private void saveSerializedSettingsToFile(XElement xml)
      {
         ensureSettingsFileDirectoryExists();
         var doc = XDocument.Load(new StringReader(xml.ToString()));
         doc.Save(SettingsFilePath);
      }

      private void ensureSettingsFileDirectoryExists()
      {
         var directory = Path.GetDirectoryName(SettingsFilePath);
         DirectoryHelper.CreateDirectory(directory);
      }

      protected abstract string SettingsFilePath { get; }

      public void Load()
      {
         foreach (var filePath in AllSettingsFilePath.Where(FileHelper.FileExists))
         {
            try
            {
               using (var serializationContext = _serializationContextFactory.Create())
               {
                  var xml = XDocument.Load(filePath).Root;
                  var serializer = _serializerRepository.SerializerFor(_settings);
                  serializer.Deserialize(_settings, xml, serializationContext);
                  return;
               }
            }
            catch
            {
               //continue trying older versions
            }
         }
      }

      public abstract IEnumerable<string> AllSettingsFilePath { get; }
   }
}