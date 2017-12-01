using MoBi.Core;
using OSPSuite.Presentation.DTO;

namespace MoBi.Presentation.DTO
{
   public class ApplicationSettingsDTO : ValidatableDTO
   {
      private readonly IApplicationSettings _applicationSettings;

      public ApplicationSettingsDTO(IApplicationSettings applicationSettings)
      {
         _applicationSettings = applicationSettings;
      }

      public virtual string PKSimPath
      {
         get => _applicationSettings.PKSimPath;
         set
         {
            _applicationSettings.PKSimPath = value;
            OnPropertyChanged();
         }
      }

      public bool UseWatermark
      {
         get => _applicationSettings.UseWatermark.GetValueOrDefault(false);
         set => _applicationSettings.UseWatermark = value;
      }

      public string WatermarkText
      {
         get => _applicationSettings.WatermarkText;
         set => _applicationSettings.WatermarkText = value;
      }

   }
}