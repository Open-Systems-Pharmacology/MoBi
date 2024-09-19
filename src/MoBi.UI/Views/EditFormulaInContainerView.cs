using DevExpress.XtraEditors;
using DevExpress.XtraLayout.Utils;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.Utility.Extensions;

namespace MoBi.UI.Views
{
   public class EditFormulaInContainerView : EditFormulaView<IEditFormulaInContainerPresenter>, IEditFormulaInContainerView
   {
      public bool IsNamedFormulaView
      {
         set
         {
            layoutItemFormulaSelect.Visibility = LayoutVisibilityConvertor.FromBoolean(value);
            layoutItemAddFormula.Visibility = layoutItemFormulaSelect.Visibility;
         }
      }

      protected override BaseControl FormulaNameControl => cbFormulaName;

      public override void InitializeResources()
      {
         base.InitializeResources();
         layoutItemFormulaName.Visibility = LayoutVisibility.Never;
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         btnAddFormula.Click += (o, e) => this.DoWithinExceptionHandler(() => _presenter.AddNewFormula());
      }

      protected override void InitializeNameBinding()
      {
         _screenBinder.Bind(x => x.Name)
            .To(cbFormulaName)
            .WithValues(dto => _presenter.DisplayFormulaNames())
            .Changed += () => OnEvent(() => _presenter.NamedFormulaSelectionChanged());
      }
   }
}