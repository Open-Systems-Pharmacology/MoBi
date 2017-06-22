using MoBi.Assets;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.Extensions;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.Views;

namespace MoBi.UI.Views
{
   public partial class CreatePKSimMoleculeView : BaseModalView, ICreatePKSimMoleculeView
   {
      private ICreatePKSimMoleculePresenter _presenter;
      private readonly GridViewBinder<ParameterDTO> _gridViewBinder;
      private readonly UxComboBoxUnit<ParameterDTO> _comboBoxUnit;
      private readonly ScreenBinder<MoleculeBuilderDTO> _screenBinder;

      public CreatePKSimMoleculeView()
      {
         InitializeComponent();
         _gridViewBinder = new GridViewBinder<ParameterDTO>(gridView);
         _screenBinder = new ScreenBinder<MoleculeBuilderDTO>();
         gridView.AllowsFiltering = false;
         _comboBoxUnit = new UxComboBoxUnit<ParameterDTO>(gridControl);
      }

      public void AttachPresenter(ICreatePKSimMoleculePresenter presenter)
      {
         _presenter = presenter;
      }

      protected override void SetActiveControl()
      {
         ActiveControl = tbName;
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _gridViewBinder.Bind(x => x.Name)
            .AsReadOnly();

         _gridViewBinder.AutoBind(x => x.Value)
            .WithFormat(x => x.ParameterFormatter())
            .WithCaption(AppConstants.Captions.Value)
            .WithEditorConfiguration((activeEditor, parmeterDTO) => _comboBoxUnit.UpdateUnitsFor(activeEditor, parmeterDTO))
            .WithOnValueUpdating((p, e) => setParameterValue(p, e.NewValue))
            .OnChanged += p => NotifyViewChanged();

         _comboBoxUnit.ParameterUnitSet += setParameterUnit;

         gridView.HiddenEditor += (o, e) => { _comboBoxUnit.Visible = false; };

         _screenBinder.Bind(x => x.Name)
            .To(tbName);

         RegisterValidationFor(_screenBinder, NotifyViewChanged);
      }

      private void setParameterUnit(ParameterDTO parameterDTO, Unit unit)
      {
         OnEvent(() => _presenter.SetParameterUnit(parameterDTO, unit));
      }

      private void setParameterValue(ParameterDTO parameterDTO, double newDisplayValue)
      {
         OnEvent(() => _presenter.SetParameterValue(parameterDTO, newDisplayValue));
      }

      public void BindTo(MoleculeBuilderDTO moleculeBuilderDTO)
      {
         _gridViewBinder.BindToSource(moleculeBuilderDTO.Parameters);
         _screenBinder.BindToSource(moleculeBuilderDTO);
      }

      public override bool HasError => _gridViewBinder.HasError || _screenBinder.HasError;

      public override void InitializeResources()
      {
         base.InitializeResources();
         layoutItemName.Text = AppConstants.Captions.Name.FormatForLabel();
         Caption = AppConstants.Captions.CreatePKSimMoleculeFromTemplate;
      }
   }
}