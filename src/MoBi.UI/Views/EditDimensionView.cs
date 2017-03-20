using System;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.UI.Controls;

namespace MoBi.UI.Views
{
   public partial class EditDimensionView : BaseUserControl, IEditDimensionView
   {
      private GridViewBinder<Unit> _gridBinder;
      private IEditDimensionPresenter _presenter;
      private ScreenBinder<DimensionDTO> _screenBinder;

      public EditDimensionView()
      {
         InitializeComponent();
      }

      public void BindToSource(DimensionDTO dto)
      {
         _screenBinder.BindToSource(dto);
         _gridBinder.BindToSource(dto.Units);
      }

      public void AttachPresenter(IEditDimensionPresenter presenter)
      {
         _presenter = presenter;
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _screenBinder = new ScreenBinder<DimensionDTO>();
         _screenBinder.Bind(item => item.Name).To(txtName);
         _screenBinder.Bind(item => item.BaseUnit).To(txtBaseUnit);
         _screenBinder.Bind(item => item.Length).To(seLength);
         _screenBinder.Bind(item => item.Mass).To(seMass);
         _screenBinder.Bind(item => item.Time).To(seTime);
         _screenBinder.Bind(item => item.ElectricCurrent).To(seElectricCurrent);
         _screenBinder.Bind(item => item.Temperature).To(seTemperature);
         _screenBinder.Bind(item => item.Amount).To(seAmount);
         _screenBinder.Bind(item => item.LuminousIntensity).To(seLuminousIntensity);
         _gridBinder = new GridViewBinder<Unit>(grdViewUnits);
         _gridBinder.Bind(item => item.Name).WithCaption("Name").AsReadOnly();
         _gridBinder.Bind(item => item.Factor).WithCaption("Factor");
         _gridBinder.Bind(item => item.Offset).WithCaption("Offset");
         RegisterValidationFor(_screenBinder, NotifyViewChanged);
      }

      private void btAddUnit_Click(object sender, EventArgs e)
      {
         _presenter.AddUnit();
      }

      private void btRemoveUnit_Click(object sender, EventArgs e)
      {
         _presenter.RemoveUnit(_gridBinder.FocusedElement);
      }

      public void Activate()
      {
         ActiveControl = txtName;
      }
   }
}