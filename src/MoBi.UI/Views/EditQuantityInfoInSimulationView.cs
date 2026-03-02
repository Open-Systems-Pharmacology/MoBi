using System.Collections.Generic;
using System.Linq;
using DevExpress.Utils;
using DevExpress.XtraLayout.Utils;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Formatters;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Extensions;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.Presentation.Extensions;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;

namespace MoBi.UI.Views
{
   public partial class EditQuantityInfoInSimulationView : BaseUserControl, IEditQuantityInfoInSimulationView
   {
      private readonly ScreenBinder<QuantityDTO> _screenBinder = new ScreenBinder<QuantityDTO>();
      private IEditQuantityInfoInSimulationPresenter _presenter;
      private GridViewBinder<UsedCalculationMethod> _gridBinder;

      public EditQuantityInfoInSimulationView()
      {
         InitializeComponent();
      }

      public override void InitializeResources()
      {
         nameLayoutControlItem.Text = AppConstants.Captions.Name.FormatForLabel();
         descriptionLayoutControlItem.Text = AppConstants.Captions.Description.FormatForLabel();
         layoutControlItemSource.Text = AppConstants.Captions.Source.FormatForLabel();

         htmlEditor.ReadOnly = true;
         tbName.ReadOnly = true;
         tbSource.ReadOnly = true;

         btnGoToSource.InitWithImage(ApplicationIcons.Search, text: AppConstants.Captions.GoToSource);
         layoutControlItemGoToSource.AdjustControlSize(OSPSuite.UI.UIConstants.Size.BUTTON_WIDTH, layoutControlItemGoToSource.ControlMaxSize.Height);
         layoutControlItemCalculationMethods.Text = AppConstants.Captions.UsedCalculationMethods.FormatForLabel();
         gridViewCalculationMethods.OptionsView.ShowGroupPanel = false;
         layoutControlItemCalculationMethods.TextLocation = Locations.Top;
         layoutControlItemCalculationMethods.TextVisible = true;

         gridViewCalculationMethods.OptionsBehavior.Editable = false;
         showCalculationMethods(shouldShow: false);
         Resize += (_, _) => OnEvent(resizeGrid);
      }

      private void showCalculationMethods(bool shouldShow)
      {
         layoutControlItemCalculationMethods.Visibility = LayoutVisibilityConvertor.FromBoolean(shouldShow);
      }

      public override void InitializeBinding()
      {
         _screenBinder.Bind(item => item.Description).To(htmlEditor);
         _screenBinder.Bind(item => item.Name).To(tbName);
         _screenBinder.Bind(item => item.SourceDisplayName).To(tbSource);
         RegisterValidationFor(_screenBinder, NotifyViewChanged);
         btnGoToSource.Click += (o, e) => OnEvent(() => _presenter.NavigateToQuantitySource());
         _gridBinder = new GridViewBinder<UsedCalculationMethod>(gridViewCalculationMethods);
         _gridBinder.Bind(x => x.Category)
            .AsReadOnly()
            .WithFormat(new UsedCalculationMethodCategoryFormatter());

         _gridBinder.Bind(x => x.CalculationMethod)
            .WithCaption(nameof(UsedCalculationMethod.CalculationMethod).SplitToUpperCase())
            .AsReadOnly();
      }

      public void BindTo(QuantityDTO dto)
      {
         _screenBinder.BindToSource(dto);
         layoutControlItemSource.Visibility = LayoutVisibilityConvertor.FromBoolean(dto.SourceReference != null);
         layoutControlItemGoToSource.Visibility = layoutControlItemSource.Visibility;
      }

      public void BindTo(IReadOnlyCollection<UsedCalculationMethod> dtoUsedCalculationMethods)
      {
         _gridBinder.BindToSource(dtoUsedCalculationMethods);
         showCalculationMethods(shouldShow: dtoUsedCalculationMethods.Any());
         resizeGrid();
      }

      private void resizeGrid()
      {
         var size = layoutControlItemCalculationMethods.Size;
         size.Height = gridViewCalculationMethods.OptimalHeight + layoutControlItemCalculationMethods.TextSize.Height + layoutControlItemCalculationMethods.Padding.Height;
         layoutControlItemCalculationMethods.Size = size;
      }

      public void AttachPresenter(IEditQuantityInfoInSimulationPresenter presenter)
      {
         _presenter = presenter;
      }
   }
}