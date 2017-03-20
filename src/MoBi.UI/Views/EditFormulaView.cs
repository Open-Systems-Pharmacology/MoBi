using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.UI.Extensions;
using OSPSuite.Assets;
using OSPSuite.Utility.Extensions;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout.Utils;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Controls;
using ToolTips = MoBi.Assets.ToolTips;

namespace MoBi.UI.Views
{
   public partial class EditFormulaView : BaseUserControl, IEditFormulaView
   {
      private IEditFormulaPresenter _presenter;
      private readonly ScreenBinder<FormulaInfoDTO> _screenBinder;
      public bool Init { get; set; }

      public EditFormulaView()
      {
         InitializeComponent();
         _screenBinder = new ScreenBinder<FormulaInfoDTO> {BindingMode = BindingMode.TwoWay};
         splitFormula.CollapsePanel = SplitCollapsePanel.Panel2;
      }

      public override void InitializeBinding()
      {
         _screenBinder.Bind(x => x.Name)
            .To(cbExplicitFormulaName)
            .WithValues(dto => _presenter.DisplayFormulaNames())
            .Changed += () => OnEvent(() => _presenter.NamedFormulaSelectionChanged());

         _screenBinder.Bind(x => x.Type)
            .To(cbFormulaType)
            .WithValues(dto => _presenter.AllFormulaTypes())
            .AndDisplays(type => _presenter.DisplayFor(type))
            .Changed += formulaTypeSelectionChanged;


         btnAddFormula.Click += (o, e) => this.DoWithinExceptionHandler(() => _presenter.AddNewFormula());

         RegisterValidationFor(_screenBinder, NotifyViewChanged);
      }

      public void AttachPresenter(IEditFormulaPresenter presenter)
      {
         _presenter = presenter;
      }

      public void SetEditFormualInstanceView(IView subView)
      {
         splitFormula.Panel1.Clear();
         splitFormula.Panel1.FillWith(subView);
      }

      public void ClearFormulaView()
      {
         splitFormula.Panel1.Clear();
      }

      public void BindTo(FormulaInfoDTO dtoFormulaInfo)
      {
         _screenBinder.BindToSource(dtoFormulaInfo);
         layoutItemFormulaType.Visibility = LayoutVisibilityConvertor.FromBoolean((cbFormulaType.Properties.Items.Count > 1));
      }

      public bool IsComplexFormulaView
      {
         set { splitFormula.PanelVisibility = value ? SplitPanelVisibility.Both : SplitPanelVisibility.Panel1; }
      }

      public bool IsNamedFormulaView
      {
         set
         {
            layoutItemExplicitFormulaName.Visibility = LayoutVisibilityConvertor.FromBoolean(value);
            layoutItemAddFormula.Visibility = layoutItemExplicitFormulaName.Visibility;
         }
      }

      public void SetReferenceView(ISelectReferenceView view)
      {
         splitFormula.Panel2.FillWith(view);
         ((XtraUserControl) view).Visible = false;
         ((XtraUserControl) view).Visible = true;
      }

      private void formulaTypeSelectionChanged()
      {
         this.DoWithinExceptionHandler(() => _presenter.FormulaSelectionChanged(cbExplicitFormulaName.Text));
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         cbFormulaType.ToolTip = ToolTips.Formula.FormulaType;
         cbFormulaType.AllowHtmlTextInToolTip = DefaultBoolean.True;
         cbExplicitFormulaName.ToolTip = ToolTips.Formula.FormulaName;
         layoutItemExplicitFormulaName.Text = AppConstants.Captions.FormulaName.FormatForLabel();
         layoutItemFormulaType.Text = AppConstants.Captions.FormulaType.FormatForLabel();
         btnAddFormula.Text = AppConstants.Captions.AddFormula;
         btnAddFormula.Image = ApplicationIcons.Add.ToImage(IconSizes.Size16x16);
         btnAddFormula.ImageLocation = ImageLocation.MiddleLeft;
         btnAddFormula.ToolTip = ToolTips.Formula.FormulaName;
         layoutItemCloneFormula.Visibility = LayoutVisibility.Never;
         layoutItemCloneFormula.AdjustButtonSize();
         layoutItemAddFormula.AdjustButtonSize();
      }

      public override bool HasError
      {
         get { return base.HasError || _screenBinder.HasError; }
      }
   }
}