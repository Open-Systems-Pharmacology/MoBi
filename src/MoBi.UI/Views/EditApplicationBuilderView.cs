using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.RepositoryItems;
using OSPSuite.Utility.Extensions;
using DevExpress.XtraBars;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Base;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.UI.Views;
using OSPSuite.Presentation;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Services;

namespace MoBi.UI.Views
{
   public partial class EditApplicationBuilderView : BaseUserControl, IEditApplicationBuilderView, IViewWithPopup
   {
      private IEditApplicationBuilderPresenter _presenter;
      private GridViewBinder<ApplicationMoleculeBuilderDTO> _gridMoleculesBinder;
      private ScreenBinder<ApplicationBuilderDTO> _screenBinder;

      public EditApplicationBuilderView(IImageListRetriever imageListRetriever)
      {
         InitializeComponent();
         barManager.Images = imageListRetriever.AllImages16x16;
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _screenBinder = new ScreenBinder<ApplicationBuilderDTO>();
         _screenBinder.Bind(dto => dto.Name).To(btName).OnValueUpdating += OnValueUpdating;
         _screenBinder.Bind(dto => dto.Description).To(htmlEditor).OnValueUpdating += OnValueUpdating;
         _screenBinder.Bind(dto => dto.MoleculeName).To(cbApplicatedMoleculeName).WithValues(dto => getMoleculeNames())
            .OnValueUpdating += OnValueUpdating;

         cbApplicatedMoleculeName.Properties.TextEditStyle = TextEditStyles.Standard;

         _gridMoleculesBinder = new GridViewBinder<ApplicationMoleculeBuilderDTO>(grdMoleculeBuilder);

         var column = _gridMoleculesBinder.Bind(dto => dto.RelativeContainerPath)
            .WithCaption(AppConstants.Captions.RelativeContainerPath)
            .WithRepository(selectPathRepository)
            .WithOnValueUpdating((o, e) => OnEvent(() => _presenter.SetRelativeContainerPath(o, e.NewValue)));

         column.XtraColumn.ToolTip = ToolTips.Applications.ApplicationMoleculeBuilderPath;

         var colFormula = _gridMoleculesBinder.Bind(dto => dto.Formula)
            .WithRepository(createFormulaComboboxRepositoryItem)
            .WithOnValueUpdating(onApplicationMoleculeBuilderFormulaSet)
            .WithToolTip(ToolTips.Applications.ApplicationMoleculeBuilderFormula);

         var buttonRepository = createAddRemoveButtonRepository();
         _gridMoleculesBinder.AddUnboundColumn()
            .WithCaption(OSPSuite.UI.UIConstants.EMPTY_COLUMN)
            .WithShowButton(ShowButtonModeEnum.ShowAlways)
            .WithRepository(dto => buttonRepository)
            .WithFixedWidth(OSPSuite.UI.UIConstants.Size.EMBEDDED_BUTTON_WIDTH * 2);
         buttonRepository.ButtonClick += (o, e) => OnEvent(() => onButtonClicked(e, _gridMoleculesBinder.FocusedElement));
         gridControlMolecules.MouseClick += (o, e) => OnEvent(gridControlMoleculesMouseClick, o, e);

         RegisterValidationFor(_screenBinder, NotifyViewChanged);

         btName.ButtonClick += (o, e) => OnEvent(_presenter.RenameSubject);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         btName.ToolTip = ToolTips.Applications.ApplicationName;
         cbApplicatedMoleculeName.ToolTip = ToolTips.Applications.ApplicatedMolecule;
         htmlEditor.ToolTip = ToolTips.Description;
         layoutGroupContainer.Text = AppConstants.Captions.InContainerWith;
         layoutGroupApplicationBuilder.Text = AppConstants.Captions.ApplicationMoleculeBuilder;
         layoutItemMolecule.Text = AppConstants.Captions.AdministeredMolecule.FormatForLabel();
      }

      public void Activate()
      {
         ActiveControl = btName;
      }

      private IEnumerable<string> getMoleculeNames()
      {
         return _presenter.GetMoleculeNamesWithSelf();
      }

      private void OnValueUpdating<T>(ApplicationBuilderDTO applicationBuilderDTO, PropertyValueSetEventArgs<T> e)
      {
         OnEvent(() => _presenter.SetPropertyValueFromView(e.PropertyName, e.NewValue, e.OldValue));
      }

      private RepositoryItem selectPathRepository(ApplicationMoleculeBuilderDTO applicationBuilderDTO)
      {
         var repository = new RepositoryItemButtonEdit();
         repository.Buttons[0].Kind = ButtonPredefines.Ellipsis;
         repository.ButtonClick += (o, e) => OnEvent(() => _presenter.SelectRelativeContainerPath(_gridMoleculesBinder.FocusedElement));
         return repository;
      }

      private RepositoryItem createFormulaComboboxRepositoryItem(ApplicationMoleculeBuilderDTO applicationBuilderDTO)
      {
         var comboBox = new UxRepositoryItemComboBox(grdMoleculeBuilder);
         comboBox.FillComboBoxRepositoryWith(_presenter.GetFormulas());
         return comboBox;
      }

      private RepositoryItemButtonEdit createAddRemoveButtonRepository()
      {
         var buttonRepository = new RepositoryItemButtonEdit {TextEditStyle = TextEditStyles.HideTextEditor};
         buttonRepository.Buttons[0].Kind = ButtonPredefines.Plus;
         buttonRepository.Buttons.Add(new EditorButton(ButtonPredefines.Delete));
         return buttonRepository;
      }

      private void onApplicationMoleculeBuilderFormulaSet(ApplicationMoleculeBuilderDTO dto, PropertyValueSetEventArgs<FormulaBuilderDTO> e)
      {
         OnEvent(() => _presenter.UpdateFormula(dto, e.NewValue));
      }

      public void AttachPresenter(IEditApplicationBuilderPresenter presenter)
      {
         _presenter = presenter;
      }

      public void BindTo(ApplicationBuilderDTO eventGroupBuilderDTO)
      {
         initNameEdit(eventGroupBuilderDTO);
         _screenBinder.BindToSource(eventGroupBuilderDTO);
         _gridMoleculesBinder.BindToSource(new BindingList<ApplicationMoleculeBuilderDTO>(eventGroupBuilderDTO.Molecules.ToList()));
      }

      private void initNameEdit(ApplicationBuilderDTO applicationBuilder)
      {
         var name = applicationBuilder.Name;
         var nameIsSet = name.IsNullOrEmpty();
         btName.Properties.ReadOnly = !nameIsSet;
         btName.Properties.Buttons[0].Visible = !nameIsSet;
      }

      public void SetParametersView(IView subView)
      {
         tabParameter.FillWith(subView);
      }

      public bool EnableDescriptors
      {
         get { return layoutGroupContainer.Enabled; }
         set { layoutGroupContainer.Enabled = value; }
      }

      public void ShowParameters()
      {
         tabParameter.Show();
      }

      public void AddDescriptorConditionListView(IDescriptorConditionListView view)
      {
         panelDescriptorCriteria.FillWith(view);
      }

      private void onButtonClicked(ButtonPressedEventArgs buttonPressedEventArgs, ApplicationMoleculeBuilderDTO parameterToEdit)
      {
         var pressedButton = buttonPressedEventArgs.Button;
         if (pressedButton.Kind.Equals(ButtonPredefines.Plus))
         {
            _presenter.AddApplicationMolecule();
         }
         else
         {
            _presenter.RemoveApplicationMolecule(parameterToEdit);
         }
      }

      private void gridControlMoleculesMouseClick(object sender, MouseEventArgs e)
      {
         if (!e.Button.Equals(MouseButtons.Right)) return;

         _presenter.CreatePopupMenuFor(_gridMoleculesBinder.FocusedElement).At(PointToClient(Cursor.Position));
      }

      public BarManager PopupBarManager => barManager;

      public override bool HasError => base.HasError || _screenBinder.HasError;
   }
}