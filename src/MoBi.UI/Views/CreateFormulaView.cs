using DevExpress.XtraEditors;
using DevExpress.XtraLayout.Utils;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.Utility.Extensions;

namespace MoBi.UI.Views
{
   public class EditFormulaInPathAndValuesView : EditFormulaView<IEditFormulaInPathAndValuesPresenter>, IEditFormulaInPathAndValuesView
   {
      public bool IsNamedFormulaView
      {
         set
         {
            layoutItemFormulaName.Visibility = LayoutVisibilityConvertor.FromBoolean(value);
            layoutItemAddFormula.Visibility = LayoutVisibilityConvertor.FromBoolean(value);
         }
      }

      protected override BaseControl FormulaNameControl => tbFormulaName;

      public override void InitializeResources()
      {
         base.InitializeResources();

         btnAddFormula.Click += (o, e) => this.DoWithinExceptionHandler(() => _presenter.AddNewFormula());
         layoutItemFormulaSelect.Visibility = LayoutVisibility.Never;
         layoutItemAddFormula.Visibility = LayoutVisibility.Never;
         layoutItemCloneFormula.Visibility = LayoutVisibility.Never;
         cbFormulaName.Properties.Buttons.Clear();
         tbFormulaName.ReadOnly = true;
      }

      protected override void InitializeNameBinding() =>
         _screenBinder.Bind(x => x.Name)
            .To(tbFormulaName);
   }
}