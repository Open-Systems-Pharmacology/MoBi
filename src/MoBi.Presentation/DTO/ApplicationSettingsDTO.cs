using MoBi.Core;
using OSPSuite.Presentation.DTO;

namespace MoBi.Presentation.DTO
{
   public class ApplicationSettingsDTO : ValidatableDTO
   {
      private string _pkSimPath;
      private bool _useWatermark;
      private string _watermarkText;

      public ApplicationSettingsDTO(IApplicationSettings applicationSettings)
      {
         _pkSimPath = applicationSettings.PKSimPath;
         _useWatermark = applicationSettings.UseWatermark.GetValueOrDefault(false);
         _watermarkText = applicationSettings.WatermarkText;
      }

      public virtual string PKSimPath
      {
         get => _pkSimPath;
         set
         {
            _pkSimPath = value;
            OnPropertyChanged();
         }
      }

      public bool UseWatermark
      {
         get => _useWatermark;
         set => _useWatermark = value;
      }

      public string WatermarkText
      {
         get => _watermarkText;
         set => _watermarkText = value;
      }

      public void UpdateApplicationSettings(IApplicationSettings applicationSettings)
      {
         applicationSettings.PKSimPath = PKSimPath;
         applicationSettings.UseWatermark = UseWatermark;
         applicationSettings.WatermarkText = WatermarkText;
      }
   }
}
