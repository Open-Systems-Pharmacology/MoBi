using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using OSPSuite.Utility.Format;

namespace MoBi.UI.Views
{
   public abstract partial class DistributedPathAndValueEntityView<TPresenter, TPathAndValueEntity, TPathAndValueDTO, TBuildingBlock> : BaseUserControl
      where TPresenter : IDistributedPathAndValueEntityPresenter<TPathAndValueDTO, TBuildingBlock>
      where TPathAndValueDTO : PathAndValueEntityDTO<TPathAndValueEntity, TPathAndValueDTO> where TPathAndValueEntity : PathAndValueEntity
   {
      private GridViewBinder<TPathAndValueDTO> _gridViewBinder;
      private ScreenBinder<TPathAndValueDTO> _screenBinder;
      private TPresenter _presenter;

      protected DistributedPathAndValueEntityView()
      {
         InitializeComponent();
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         convertToSimpleParameterButton.Text = AppConstants.Captions.ConvertToConstantValue;
         convertButtonLayoutItem.AdjustLargeButtonSize();
      }

      public override void InitializeBinding()
      {
         _screenBinder = new ScreenBinder<TPathAndValueDTO>();
         _gridViewBinder = new GridViewBinder<TPathAndValueDTO>(gridView);
         _gridViewBinder.AutoBind(x => x.Name).AsReadOnly();
         _gridViewBinder.Bind(x => x.Value).WithFormat(RepositoryFormatter).AsReadOnly();

         gridView.OptionsCustomization.AllowGroup = false;
         gridView.OptionsView.ShowGroupPanel = false;

         _screenBinder.Bind(x => x.DistributionType).To(lblDistributionType);

         convertToSimpleParameterButton.Click += (o, e) => OnEvent(convertToSimpleParameter);
      }

      private void convertToSimpleParameter() => _presenter.ConvertToConstantFormula();

      protected IFormatter<double?> RepositoryFormatter(TPathAndValueDTO distributionSubParameterDTO) => FormatterFor(distributionSubParameterDTO);

      protected abstract IFormatter<double?> FormatterFor(TPathAndValueDTO dto);

      public void BindTo(TPathAndValueDTO dto)
      {
         _gridViewBinder.BindToSource(dto.SubParameters);
         _screenBinder.BindToSource(dto);
      }

      private void disposeBinders()
      {
         _gridViewBinder.Dispose();
         _screenBinder.Dispose();
      }

      public void AttachPresenter(TPresenter presenter) => _presenter = presenter;
   }
}