using DevExpress.XtraEditors;
using DevExpress.XtraLayout.Utils;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;

namespace MoBi.UI.Views
{
   public class EditFormulaInPathAndValues : EditFormulaView<IEditFormulaInPathAndValuesPresenter>, IEditFormulaInPathAndValues
   {
      public bool IsNamedFormulaView
      {
         set => layoutItemFormulaName.Visibility = LayoutVisibilityConvertor.FromBoolean(value);
      }

      protected override BaseControl FormulaNameControl => tbFormulaName;

      public override void InitializeResources()
      {
         base.InitializeResources();

         layoutItemFormulaSelect.Visibility = LayoutVisibility.Never;
         layoutItemAddFormula.Visibility = LayoutVisibility.Never;
         layoutItemCloneFormula.Visibility = LayoutVisibility.Never;
         cbFormulaName.Properties.Buttons.Clear();
         tbFormulaName.Enabled = false;
      }

      protected override void InitializeNameBinding() =>
         _screenBinder.Bind(x => x.Name)
            .To(tbFormulaName);
   }
}