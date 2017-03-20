using MoBi.Assets;
using OSPSuite.Assets;

namespace MoBi.Presentation.Presenter
{
   public class SelectOption
   {
      public string Caption { get; private set; }
      public ApplicationIcon Icon { get; private set; }

      private SelectOption(string caption, ApplicationIcon icon)
      {
         Caption = caption;
         Icon = icon;
      }

      public static SelectOption AllPresent = new SelectOption(AppConstants.Captions.AllPresent, ApplicationIcons.CheckAll);
      public static SelectOption AllNotPresent = new SelectOption(AppConstants.Captions.AllNotPresent, ApplicationIcons.UncheckAll);
      public static SelectOption SelectedPresent = new SelectOption(AppConstants.Captions.SelectedPresent, ApplicationIcons.CheckSelected);
      public static SelectOption SelectedNotPresent = new SelectOption(AppConstants.Captions.SelectedNotPresent, ApplicationIcons.UncheckSelected);

      public static SelectOption AllNegativeValuesAllowed = new SelectOption(AppConstants.Captions.AllNegativeValuesAllowed, ApplicationIcons.CheckAll);
      public static SelectOption AllNegativeValuesNotAllowed = new SelectOption(AppConstants.Captions.AllNegativeValuesNotAllowed, ApplicationIcons.UncheckAll);
      public static SelectOption SelectedNegativeValuesAllowed = new SelectOption(AppConstants.Captions.SelectedNegativeValuesAllowed, ApplicationIcons.CheckSelected);
      public static SelectOption SelectedNegativeValuesNotAllowed = new SelectOption(AppConstants.Captions.SelectedNegativeValuesNotAllowed, ApplicationIcons.UncheckSelected);

      public static SelectOption RefreshAll = new SelectOption(AppConstants.Captions.RefreshAll, ApplicationIcons.RefreshAll);
      public static SelectOption RefreshSelected = new SelectOption(AppConstants.Captions.RefreshSelected, ApplicationIcons.RefreshSelected);

      public static SelectOption DeleteSourceNotDefined = new SelectOption(AppConstants.Captions.DeleteSourceNotDefined, ApplicationIcons.DeleteSourceNotDefined);
      public static SelectOption DeleteSelected = new SelectOption(AppConstants.Captions.DeleteSelected, ApplicationIcons.DeleteSelected);
   }
}