using DevExpress.LookAndFeel;
using DevExpress.Skins;
using DevExpress.XtraLayout;

namespace MoBi.UI.Extensions
{
   public static class LayoutItemExtensions
   {
      public static void InitializeDisabledColors(this LayoutControl layoutControl)
      {
         InitializeDisabledColors(layoutControl, UserLookAndFeel.Default);
      }

      public static void InitializeDisabledColors(this LayoutControl layoutControl, UserLookAndFeel lookAndFeel)
      {
         var currentSkin = CommonSkins.GetSkin(lookAndFeel);
         var clrText = currentSkin.Colors[CommonColors.WindowText];
         layoutControl.Appearance.ControlDisabled.ForeColor = clrText;
         layoutControl.Appearance.DisabledLayoutItem.ForeColor = clrText;
         layoutControl.Appearance.DisabledLayoutGroupCaption.ForeColor = clrText;
      }
   }
}