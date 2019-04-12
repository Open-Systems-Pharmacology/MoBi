using System.Collections.Generic;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraGrid.Views.Base;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.Presentation.Extensions;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.Services;
using OSPSuite.UI.Views;

namespace MoBi.UI.Views
{
   public partial class DescriptorConditionListView : BaseUserControl, IDescriptorConditionListView, IViewWithPopup
   {
      private IDescriptorConditionListPresenter _presenter;
      private readonly GridViewBinder<IDescriptorConditionDTO> _gridViewBinder;
      public BarManager PopupBarManager { get; private set; }
      private readonly UxRemoveButtonRepository _removeButtonRepository = new UxRemoveButtonRepository();

      public DescriptorConditionListView(IImageListRetriever imageListRetriever)
      {
         InitializeComponent();
         _gridViewBinder = new GridViewBinder<IDescriptorConditionDTO>(gridView);
         gridView.AllowsFiltering = false;
         PopupBarManager = new BarManager {Form = this, Images = imageListRetriever.AllImages16x16};
      }

      public void AttachPresenter(IDescriptorConditionListPresenter presenter)
      {
         _presenter = presenter;
      }

      public void BindTo(IEnumerable<IDescriptorConditionDTO> descriptorConditions)
      {
         _gridViewBinder.BindToSource(descriptorConditions);
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
            .WithFixedWidth(OSPSuite.UI.UIConstants.Size.EMBEDDED_BUTTON_WIDTH * 2);

         gridControl.MouseClick += (o, e) => OnEvent(onGridClick, e);
         _removeButtonRepository.ButtonClick += (o, e) => OnEvent(() => _presenter.RemoveCondition(_gridViewBinder.FocusedElement));
      }

      private void onCriteriaTagChanged(IDescriptorConditionDTO descriptorCondition, PropertyValueSetEventArgs<string> e)
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
      }
   }
}