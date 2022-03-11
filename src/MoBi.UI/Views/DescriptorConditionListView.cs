using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraGrid.Views.Base;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.Presentation.Extensions;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.Services;
using OSPSuite.UI.Views;
using OSPSuite.Utility;

namespace MoBi.UI.Views
{
   public partial class DescriptorConditionListView : BaseUserControl, IDescriptorConditionListView, IViewWithPopup
   {
      private IDescriptorConditionListPresenter _presenter;
      private readonly GridViewBinder<DescriptorConditionDTO> _gridViewBinder;
      public BarManager PopupBarManager { get; }
      private readonly UxRemoveButtonRepository _removeButtonRepository = new UxRemoveButtonRepository();
      private readonly ScreenBinder<DescriptorCriteriaDTO> _screenBinder = new ScreenBinder<DescriptorCriteriaDTO>();

      public DescriptorConditionListView(IImageListRetriever imageListRetriever)
      {
         InitializeComponent();
         _gridViewBinder = new GridViewBinder<DescriptorConditionDTO>(gridView);
         gridView.AllowsFiltering = false;
         PopupBarManager = new BarManager {Form = this, Images = imageListRetriever.AllImages16x16};
      }

      public void AttachPresenter(IDescriptorConditionListPresenter presenter)
      {
         _presenter = presenter;
      }

      public void BindTo(DescriptorCriteriaDTO descriptorCriteriaDTO)
      {
         _gridViewBinder.BindToSource(descriptorCriteriaDTO.Conditions);
         _screenBinder.BindToSource(descriptorCriteriaDTO);
      }

      public string CriteriaDescription
      {
         set => lblCriteriaDescription.Text = value.FormatForDescription();
      }

      public override void InitializeBinding()
      {
         _gridViewBinder.Bind(dto => dto.TagDescription)
            .WithCaption(AppConstants.Captions.IsMatching)
            .AsReadOnly();

         _gridViewBinder.Bind(dto => dto.Tag)
            .OnValueUpdating += (o, e) => OnEvent(() => onCriteriaTagChanged(o, e));

         _gridViewBinder.AddUnboundColumn()
            .WithCaption(OSPSuite.UI.UIConstants.EMPTY_COLUMN)
            .WithShowButton(ShowButtonModeEnum.ShowAlways)
            .WithRepository(x => _removeButtonRepository)
            .WithFixedWidth(OSPSuite.UI.UIConstants.Size.EMBEDDED_BUTTON_WIDTH);

         gridControl.MouseClick += (o, e) => OnEvent(onGridClick, e);
         _removeButtonRepository.ButtonClick += (o, e) => OnEvent(() => _presenter.RemoveCondition(_gridViewBinder.FocusedElement));

         _screenBinder.Bind(x => x.Operator)
            .To(cbOperator)
            .WithValues(EnumHelper.AllValuesFor<DescriptorCriteriaOperator>())
            .OnValueUpdating += (o, e) => OnEvent(() => _presenter.ChangeOperator(e.NewValue));
      }

      private void onCriteriaTagChanged(DescriptorConditionDTO descriptorCondition, PropertyValueSetEventArgs<string> e)
      {
         _presenter.UpdateCriteriaTag(descriptorCondition, e.NewValue);
      }

      private void onGridClick(MouseEventArgs e)
      {
         if (e.Button != MouseButtons.Right)
            return;

         _presenter.CreatePopupMenuFor(_gridViewBinder.FocusedElement).At(e.Location);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         lblCriteriaDescription.AsDescription();
         layoutItemOperator.Text = AppConstants.Captions.Operator.FormatForLabel();
      }
   }
}