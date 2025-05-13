using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Base;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using MoBi.UI.Extensions;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.Presentation.DTO;
using OSPSuite.UI.Binders;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.RepositoryItems;
using OSPSuite.UI.Services;
using OSPSuite.UI.Views;
using OSPSuite.Utility.Extensions;
using IToolTipCreator = MoBi.UI.Services.IToolTipCreator;
using ToolTips = MoBi.Assets.ToolTips;

namespace MoBi.UI.Views
{
   public partial class EditParameterListView : BaseUserControl, IEditParameterListView, IViewWithPopup
   {
      private readonly PathElementsBinder<ParameterDTO> _pathBinder;
      private readonly GridViewBinder<ParameterDTO> _gridViewBinder;
      private readonly UxComboBoxUnit<ParameterDTO> _unitControl;
      private IEditParameterListPresenter _presenter;
      private RepositoryItemButtonEdit _isFixedParameterEditRepository;
      private readonly RepositoryItemTextEdit _standardParameterEditRepository = new RepositoryItemTextEdit();
      private readonly UxRepositoryItemCheckEdit _favoriteRepository;
      private readonly IToolTipCreator _toolTipCreator;
      private readonly ValueOriginBinder<ParameterDTO> _valueOriginBinder;
      private IGridViewBoundColumn<ParameterDTO, string> _colModule;
      private IGridViewBoundColumn<ParameterDTO, string> _colBuildingBlock;
      private IGridViewColumn<ParameterDTO> _colNavigate;
      private readonly RepositoryItemButtonEdit _navigateButtonRepository = new UxRepositoryItemButtonEdit(ButtonPredefines.Search);
      private readonly IImageListRetriever _imageListRetriever;
      public BarManager PopupBarManager { get; }

      public EditParameterListView(PathElementsBinder<ParameterDTO> pathBinder,
         IImageListRetriever imageListRetriever,
         IToolTipCreator toolTipCreator,
         ValueOriginBinder<ParameterDTO> valueOriginBinder)
      {
         InitializeComponent();
         _imageListRetriever = imageListRetriever;
         _valueOriginBinder = valueOriginBinder;
         _gridViewBinder = new GridViewBinder<ParameterDTO>(_gridView);
         _unitControl = new UxComboBoxUnit<ParameterDTO>(_gridControl);
         _pathBinder = pathBinder;
         _toolTipCreator = toolTipCreator;
         _gridView.HiddenEditor += (o, e) => hideEditor();
         _gridView.ShowRowIndicator = true;
         _gridView.OptionsSelection.EnableAppearanceFocusedRow = true;
         _gridView.OptionsView.ShowGroupPanel = false;
         _gridControl.MouseDoubleClick += onDoubleClick;
         _gridView.MouseDown += (o, e) => OnEvent(onGridViewMouseDown, e);
         _favoriteRepository = new UxRepositoryItemCheckEdit(_gridView);
         PopupBarManager = new BarManager { Form = this, Images = imageListRetriever.AllImages16x16 };

         var toolTipController = new ToolTipController { AllowHtmlText = true };
         toolTipController.Initialize();
         toolTipController.GetActiveObjectInfo += onToolTipControllerGetActiveObjectInfo;
         _gridControl.ToolTipController = toolTipController;
      }

      private RepositoryItem buildingBlockRepository(ParameterDTO parameterDTO)
      {
         var buildingBlock = parameterDTO.BuildingBlock;
         var repository = new UxRepositoryItemImageComboBox(_gridView, _imageListRetriever);
         if (buildingBlock != null)
            repository.AddItem(buildingBlock.Name, buildingBlock.Icon);

         return repository;
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         _navigateButtonRepository.Buttons[0].ToolTip = ToolTips.ParameterList.GoToSource;
      }

      private void onToolTipControllerGetActiveObjectInfo(object sender, ToolTipControllerGetActiveObjectInfoEventArgs e)
      {
         var parameterDTO = _gridViewBinder.ElementAt(e);

         if (parameterDTO == null)
            return;

         e.Info = _gridView.CreateToolTipControlInfoFor(parameterDTO, e.ControlMousePosition, _toolTipCreator.ToolTipFor);
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

      public void AttachPresenter(IEditParameterListPresenter presenter)
      {
         _presenter = presenter;
      }

      public void BindTo(IReadOnlyList<ParameterDTO> parameters)
      {
         _gridViewBinder.BindToSource(parameters);
         _gridView.RefreshData();

         _colModule.Visible = _presenter.HasModules();
         _colBuildingBlock.Visible = _presenter.HasBuildingBlocks();
         _colNavigate.Visible = _colBuildingBlock.Visible;
      }

      public void Select(ParameterDTO parameterDTO)
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
            .WithFormat(dto => dto.ParameterFormatter(checkForEditable: false))
            .WithRepository(repositoryForValue)
            .WithEditorConfiguration(configureRepository)
            .WithToolTip(ToolTips.ParameterList.SetParameterValue)
            .WithOnValueUpdating(onParameterValueSet)
            .WithShowButton(ShowButtonModeEnum.ShowAlways);

         _colModule = _gridViewBinder.Bind(dto => dto.ModuleName)
            .WithCaption(AppConstants.Captions.Module)
            .AsReadOnly();

         _colBuildingBlock = _gridViewBinder.Bind(dto => dto.BuildingBlockName)
            .WithRepository(buildingBlockRepository)
            .WithCaption(AppConstants.Captions.BuildingBlock)
            .AsReadOnly();

         _unitControl.ParameterUnitSet += setParameterUnit;

         _gridViewBinder.AutoBind(dto => dto.Dimension)
            .AsHidden()
            .AsReadOnly()
            .WithShowInColumnChooser(true);

         _valueOriginBinder.InitializeBinding(_gridViewBinder, onParameterValueOriginSet);

         _gridViewBinder.Bind(dto => dto.Description)
            .AsHidden()
            .AsReadOnly()
            .WithShowInColumnChooser(true);

         _gridViewBinder.Bind(dto => dto.IsFavorite)
            .WithCaption(Captions.Favorites)
            .WithWidth(OSPSuite.UI.UIConstants.Size.EMBEDDED_CHECK_BOX_WIDTH)
            .WithRepository(x => _favoriteRepository)
            .WithToolTip(OSPSuite.Assets.ToolTips.FavoritesToolTip)
            .WithOnValueUpdating((o, e) => onIsFavoriteSet(o, e.NewValue));

         _colNavigate = _gridViewBinder.AddUnboundColumn()
            .WithCaption(OSPSuite.UI.UIConstants.EMPTY_COLUMN)
            .WithShowButton(ShowButtonModeEnum.ShowAlways)
            .WithRepository(dto => _navigateButtonRepository)
            .WithFixedWidth(OSPSuite.UI.UIConstants.Size.EMBEDDED_BUTTON_WIDTH)
            .AsHidden();

         _isFixedParameterEditRepository.ButtonClick += (o, e) => OnEvent(() => onResetValue(_gridViewBinder.FocusedElement));

         _pathBinder.SetVisibility(PathElementId.Simulation, visible: false);

         _navigateButtonRepository.ButtonClick += (o, e) => OnEvent(_presenter.NavigateToParameter, _gridViewBinder.FocusedElement);
      }

      private void onResetValue(ParameterDTO favoriteParameterDTO)
      {
         OnEvent(() =>
         {
            _presenter.ResetValueFor(favoriteParameterDTO);
            _gridView.CloseEditor();
         });
      }

      private void onIsFavoriteSet(ParameterDTO parameterDTO, bool newValue)
      {
         _presenter.SetIsFavorite(parameterDTO, newValue);
      }

      private void createResetButtonItem()
      {
         _isFixedParameterEditRepository = new UxRepositoryItemButtonImage(ApplicationIcons.Reset,
            ToolTips.ResetParameterToolTip) { TextEditStyle = TextEditStyles.Standard };
         _isFixedParameterEditRepository.Buttons[0].IsLeft = true;
      }

      private RepositoryItem repositoryForValue(ParameterDTO parameter)
      {
         if (_presenter.IsFixedValue(parameter))
            return _isFixedParameterEditRepository;

         return _standardParameterEditRepository;
      }

      private void configureRepository(BaseEdit activeEditor, ParameterDTO parameter)
      {
         _unitControl.UpdateUnitsFor(activeEditor, parameter);
      }

      private void onParameterValueSet(ParameterDTO parameter, PropertyValueSetEventArgs<double> e)
      {
         this.DoWithinExceptionHandler(() => _presenter.OnParameterValueSet(parameter, e.NewValue));
      }

      private void onParameterValueOriginSet(ParameterDTO parameter, ValueOrigin valueOrigin)
      {
         OnEvent(() => _presenter.OnParameterValueOriginSet(parameter, valueOrigin));
      }

      private void setParameterUnit(ParameterDTO parameter, Unit unit)
      {
         OnEvent(() =>
         {
            _gridView.CloseEditor();
            _presenter.SetParameterUnit(parameter, unit);
         });
      }

      public void SetCaptions(IDictionary<PathElementId, string> captions)
      {
         captions.Each(kv => _pathBinder.SetCaption(kv.Key, kv.Value));
      }

      public void SetVisibility(PathElementId pathElement, bool isVisible)
      {
         _pathBinder.SetVisibility(pathElement, isVisible);
         _pathBinder.ColumnAt(pathElement).WithShowInColumnChooser(!isVisible);
      }

      public IReadOnlyList<ParameterDTO> SelectedParameters
      {
         get { return _gridView.GetSelectedRows().Select(rowHandle => _gridViewBinder.ElementAt(rowHandle)).ToList(); }
         set
         {
            if (!value.Any())
               return;

            //Need to clear selection before setting another one programatically. Otherwise they overlap
            _gridView.ClearSelection();

            var firstRowHandle = _gridViewBinder.RowHandleFor(value.First());
            var lastRowHandle = _gridViewBinder.RowHandleFor(value.Last());
            _gridView.SelectRows(firstRowHandle, lastRowHandle);

            //Required to ensure that the background is still selected
            if (firstRowHandle == lastRowHandle)
               _gridView.FocusedRowHandle = firstRowHandle;
         }
      }

      private void hideEditor()
      {
         _unitControl.Hide();
      }
   }
}