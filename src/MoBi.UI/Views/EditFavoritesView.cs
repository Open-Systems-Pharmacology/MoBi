using System.Collections.Generic;
using System.Windows.Forms;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.UI;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.RepositoryItems;
using OSPSuite.Utility.Extensions;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Base;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Formatters;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.DTO;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.UI.Binders;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Services;
using OSPSuite.UI.Views;
using IToolTipCreator = MoBi.UI.Services.IToolTipCreator;
using ToolTips = MoBi.Assets.ToolTips;

namespace MoBi.UI.Views
{
   public partial class EditFavoritesView : BaseUserControl, IEditFavoritesView, IViewWithPopup
   {
      private readonly PathElementsBinder<FavoriteParameterDTO> _pathBinder;
      private readonly GridViewBinder<FavoriteParameterDTO> _gridViewBinder;
      private readonly UxComboBoxUnit<FavoriteParameterDTO> _unitControl;
      private IEditFavoritesPresenter _presenter;
      private RepositoryItemButtonEdit _isFixedParameterEditRepository;
      private readonly RepositoryItemTextEdit _stantdardParameterEditRepository = new RepositoryItemTextEdit();
      private readonly UxRepositoryItemCheckEdit _favoriteRepository;
      private readonly IToolTipCreator _toolTipCreator;
      private ValueOriginBinder<FavoriteParameterDTO> _valueOriginBinder;

      public EditFavoritesView(PathElementsBinder<FavoriteParameterDTO> pathBinder, 
         IImageListRetriever imageListRetriever, 
         IToolTipCreator toolTipCreator,
         ValueOriginBinder<FavoriteParameterDTO> valueOriginBinder )
      {
         InitializeComponent();
         _valueOriginBinder = valueOriginBinder;
         _gridViewBinder = new GridViewBinder<FavoriteParameterDTO>(_gridView);
         _unitControl = new UxComboBoxUnit<FavoriteParameterDTO>(_gridControl);
         _pathBinder = pathBinder;
         _toolTipCreator = toolTipCreator;
         _gridView.HiddenEditor += (o, e) => hideEditor();
         _gridView.ShowRowIndicator = true;
         _gridView.OptionsSelection.EnableAppearanceFocusedRow = true;
         _gridView.OptionsView.ShowGroupPanel = false;
         _gridControl.MouseDoubleClick += onDoubleClick;
         _gridView.MouseDown += (o, e) => OnEvent(onGridViewMouseDown, e);
         _favoriteRepository = new UxRepositoryItemCheckEdit(_gridView);
         PopupBarManager = new BarManager {Form = this, Images = imageListRetriever.AllImages16x16};

         var toolTipController = new ToolTipController {AllowHtmlText = true};
         toolTipController.GetActiveObjectInfo += onToolTipControllerGetActiveObjectInfo;
         _gridControl.ToolTipController = toolTipController;
      }

      private void onToolTipControllerGetActiveObjectInfo(object sender, ToolTipControllerGetActiveObjectInfoEventArgs e)
      {
         var parameterDTO = _gridViewBinder.ElementAt(e);
         if (parameterDTO == null) return;

         var superToolTip = getToolTipFor(parameterDTO);

         //An object that uniquely identifies a row cell
         e.Info = new ToolTipControlInfo(parameterDTO, string.Empty) {SuperTip = superToolTip, ToolTipType = ToolTipType.SuperTip};
      }

      private SuperToolTip getToolTipFor(FavoriteParameterDTO parameterDTO)
      {
         return _toolTipCreator.ToolTipFor(parameterDTO);
      }

      private void onGridViewMouseDown(MouseEventArgs e)
      {
         if (e.Button != MouseButtons.Right) return;

         var rowHandle = _gridView.RowHandleAt(e);
         _presenter.ShowContextMenu(_gridViewBinder.ElementAt(rowHandle), e.Location);
      }

      private void onDoubleClick(object sender, MouseEventArgs e)
      {
         OnEvent(() => _presenter.GoTo(_gridViewBinder.FocusedElement));
      }

      public void AttachPresenter(IEditFavoritesPresenter presenter)
      {
         _presenter = presenter;
      }

      public void Show(IEnumerable<FavoriteParameterDTO> favorites)
      {
         _gridViewBinder.BindToSource(favorites);
         _gridView.RefreshData();
      }

      public void Select(FavoriteParameterDTO parameterDTO)
      {
         var rowHandle = _gridViewBinder.RowHandleFor(parameterDTO);
         _gridView.FocusedRowHandle = rowHandle;
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();

         createResetButtonItem();

         _pathBinder.InitializeBinding(_gridViewBinder);
         _gridViewBinder.Bind(dto => dto.Value)
            .WithFormat(dto => dto.ParameterFormatter())
            .WithRepository(repositoryForValue)
            .WithEditorConfiguration(configureRepository)
            .WithToolTip(ToolTips.ParameterList.SetParameterValue)
            .WithOnValueUpdating(onParameterValueSet)
            .WithShowButton(ShowButtonModeEnum.ShowAlways);

         _unitControl.ParameterUnitSet += setParameterUnit;

         var colDim = _gridViewBinder.AutoBind(dto => dto.Dimension)
            .AsReadOnly()
            .WithShowInColumnChooser(true);
      
         colDim.Visible = false;

         _valueOriginBinder.InitializeBinding(_gridViewBinder, onParameterValueOriginSet);

         _gridViewBinder.Bind(dto => dto.Description)
            .AsReadOnly().WithShowInColumnChooser(true);

         _gridViewBinder.Bind(dto => dto.IsFavorite)
            .WithCaption(Captions.Favorites)
            .WithWidth(OSPSuite.UI.UIConstants.Size.EMBEDDED_CHECK_BOX_WIDTH)
            .WithRepository(x => _favoriteRepository)
            .WithToolTip(OSPSuite.Assets.ToolTips.FavoritesToolTip)
            .WithOnValueUpdating((o, e) => onIsFavoriteSet(o, e.NewValue));

         _isFixedParameterEditRepository.ButtonClick +=
            (o, e) => this.DoWithinExceptionHandler(() => onResetValue(_gridViewBinder.FocusedElement));

         _pathBinder.SetVisibility(PathElement.Simulation, visible: false);
      }

      private void onResetValue(FavoriteParameterDTO favoriteParameterDTO)
      {
         OnEvent(() =>
         {
            _presenter.ResetValueFor(favoriteParameterDTO);
            _gridView.CloseEditor();
         });
      }

      private void onIsFavoriteSet(FavoriteParameterDTO parameterDTO, bool newValue)
      {
         _presenter.SetIsFavorite(parameterDTO, newValue);
      }

 
      private void createResetButtonItem()
      {
         _isFixedParameterEditRepository = new UxRepositoryItemButtonImage(ApplicationIcons.Reset,
            ToolTips.ResetParameterToolTip) {TextEditStyle = TextEditStyles.Standard};
         _isFixedParameterEditRepository.Buttons[0].IsLeft = true;
      }

      private RepositoryItem repositoryForValue(FavoriteParameterDTO parameter)
      {
         if (_presenter.IsFixedValue(parameter))
            return _isFixedParameterEditRepository;

         return _stantdardParameterEditRepository;
      }

      private void configureRepository(BaseEdit activeEditor, FavoriteParameterDTO parameter)
      {
         _unitControl.UpdateUnitsFor(activeEditor, parameter);
      }

      private void onParameterValueSet(FavoriteParameterDTO parameter, PropertyValueSetEventArgs<double> e)
      {
         this.DoWithinExceptionHandler(() => _presenter.OnParameterValueSet(parameter, e.NewValue));
      }

      private void onParameterValueOriginSet(FavoriteParameterDTO parameter, ValueOrigin valueOrigin)
      {
         OnEvent(() => _presenter.OnParameterValueOriginSet(parameter, valueOrigin));
      }

      private void setParameterUnit(FavoriteParameterDTO parameter, Unit unit)
      {
         OnEvent(() =>
         {
            _gridView.CloseEditor();
            _presenter.SetParamterUnit(parameter, unit);
         });
      }

      public void SetCaptions(IDictionary<PathElement, string> captions)
      {
         captions.Each(kv => _pathBinder.SetCaption(kv.Key, kv.Value));
      }

      public void SetVisibility(PathElement pathElement, bool isVisible)
      {
         _pathBinder.SetVisibility(pathElement, isVisible);
      }

      public void Rebind()
      {
         _gridViewBinder.Rebind();
      }

      private void hideEditor()
      {
         _unitControl.Hide();
      }

      public BarManager PopupBarManager { get; private set; }
   }
}