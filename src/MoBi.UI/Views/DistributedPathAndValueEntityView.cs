using DevExpress.XtraEditors;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.UI.Controls;
using OSPSuite.Utility.Format;

namespace MoBi.UI.Views
{
   public abstract partial class DistributedPathAndValueEntityView<TPresenter, TPathAndValueEntity, TPathAndValueDTO, TBuildingBlock> : BaseUserControl 
      where TPresenter : IDistributedPathAndValueEntityPresenter<TPathAndValueDTO, TBuildingBlock>
      where TPathAndValueDTO : PathAndValueEntityDTO<TPathAndValueEntity, TPathAndValueDTO> where TPathAndValueEntity : PathAndValueEntity
   {
      private GridViewBinder<TPathAndValueDTO> _gridViewBinder;
      private ScreenBinder<TPathAndValueDTO> _screenBinder;
      private readonly UxComboBoxUnit<TPathAndValueDTO> _unitControl;
      private TPresenter _presenter;

      protected DistributedPathAndValueEntityView()
      {
         InitializeComponent();
         _unitControl = new UxComboBoxUnit<TPathAndValueDTO>(gridControl);
      }

      public override void InitializeBinding()
      {
         _screenBinder = new ScreenBinder<TPathAndValueDTO>();
         _gridViewBinder = new GridViewBinder<TPathAndValueDTO>(gridView);
         _gridViewBinder.AutoBind(x => x.Name).AsReadOnly();
         _gridViewBinder.Bind(x => x.Value).
            WithEditorConfiguration(configureRepository).
            WithFormat(RepositoryFormatter).
            WithOnValueUpdating(setValue);
         gridView.HiddenEditor += (o, e) => hideEditor();

         _unitControl.ParameterUnitSet += (o, e) => OnEvent(() => setUnit(o,e));
         gridView.OptionsCustomization.AllowGroup = false;
         gridView.OptionsView.ShowGroupPanel = false;

         _screenBinder.Bind(x => x.DistributionType).To(lblDistributionType);
      }

      private void setValue(TPathAndValueDTO dto, PropertyValueSetEventArgs<double?> arg2)
      {
         _presenter.SetParameterValue(dto, arg2.NewValue);
      }

      private void setUnit(TPathAndValueDTO distributionSubParameterDTO, Unit unit)
      {
         _presenter.SetParameterUnit(distributionSubParameterDTO, unit);
      }

      private void hideEditor()
      {
         _unitControl.Hide();
      }

      protected IFormatter<double?> RepositoryFormatter(TPathAndValueDTO distributionSubParameterDTO)
      {
         return FormatterFor(distributionSubParameterDTO);
      }

      protected abstract IFormatter<double?> FormatterFor(TPathAndValueDTO dto);

      private void configureRepository(BaseEdit activeEditor, TPathAndValueDTO pathAndValueDTO)
      {
         _unitControl.UpdateUnitsFor(activeEditor, pathAndValueDTO);
      }

      public void BindTo(TPathAndValueDTO dto)
      {
         _gridViewBinder.BindToSource(dto.SubParameters);
         _screenBinder.BindToSource(dto);
      }

      private void disposeBinders()
      {
         _gridViewBinder.Dispose();
      }

      public void AttachPresenter(TPresenter presenter)
      {
         _presenter = presenter;
      }
   }
}
