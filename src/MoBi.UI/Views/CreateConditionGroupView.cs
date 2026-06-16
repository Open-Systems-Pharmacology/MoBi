using System.ComponentModel;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Base;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.Presentation.Extensions;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.Views;
using OSPSuite.Utility;

namespace MoBi.UI.Views
{
   public partial class CreateConditionGroupView : BaseModalView, ICreateConditionGroupView
   {
      private readonly ScreenBinder<EditConditionGroupDTO> _screenBinder = new ScreenBinder<EditConditionGroupDTO>();
      private readonly GridViewBinder<EditConditionGroupOperandDTO> _gridViewBinder;
      private readonly UxRemoveButtonRepository _removeButtonRepository = new UxRemoveButtonRepository();
      private readonly RepositoryItemTextEdit _disabledTagRepository = new RepositoryItemTextEdit { Enabled = false, ReadOnly = true };
      private readonly RepositoryItemComboBox _tagComboBoxRepository = new RepositoryItemComboBox { TextEditStyle = TextEditStyles.Standard };
      private readonly RepositoryItemImageComboBox _typeRepository = new RepositoryItemImageComboBox { TextEditStyle = TextEditStyles.DisableTextEditor };
      private ICreateConditionGroupPresenter _presenter;
      private IGridViewColumn _columnTag;

      public CreateConditionGroupView()
      {
         InitializeComponent();
         _gridViewBinder = new GridViewBinder<EditConditionGroupOperandDTO>(gridView);
         gridView.AllowsFiltering = false;
         _typeRepository.EditValueChanged += (o, e) => gridView.PostEditor();

         gridView.ShowingEditor += onShowingEditor;
      }

      public void AttachPresenter(ICreateConditionGroupPresenter presenter)
      {
         _presenter = presenter;
      }

      public void BindTo(EditConditionGroupDTO dto)
      {
         _screenBinder.BindToSource(dto);
         _gridViewBinder.BindToSource(dto.Operands);
         _tagComboBoxRepository.FillComboBoxRepositoryWith(dto.AvailableTags);
      }

      public void InitializeTagTypes()
      {
         foreach (var tagType in _presenter.SelectableTagTypes)
            _typeRepository.Items.Add(new ImageComboBoxItem(_presenter.DisplayNameFor(tagType), tagType, -1));
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         ApplicationIcon = ApplicationIcons.Edit;
         descriptionLabelControl.Text = AppConstants.Captions.CreateConditionGroup;
         operatorLayoutItem.Text = AppConstants.Captions.Operator.FormatForLabel();
         addConditionButton.InitWithImage(ApplicationIcons.Add, AppConstants.Captions.AddCondition, ImageLocation.MiddleLeft);
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();

         _screenBinder.Bind(x => x.Operator)
            .To(operatorComboBoxEdit)
            .WithValues(EnumHelper.AllValuesFor<CriteriaOperator>());

         _gridViewBinder.Bind(dto => dto.TagType)
            .WithCaption(AppConstants.Captions.Type)
            .WithRepository(dto => _typeRepository)
            .WithOnValueUpdated((dto, e) => OnEvent(() => onTypeChanged(dto)));

         _columnTag = _gridViewBinder.Bind(dto => dto.Tag)
            .WithCaption(AppConstants.Captions.Tag)
            .WithRepository(repositoryForTag);

         _gridViewBinder.AddUnboundColumn()
            .WithCaption(OSPSuite.UI.UIConstants.EMPTY_COLUMN)
            .WithShowButton(ShowButtonModeEnum.ShowAlways)
            .WithRepository(dto => _removeButtonRepository)
            .WithFixedWidth(OSPSuite.UI.UIConstants.Size.EMBEDDED_BUTTON_WIDTH);

         _removeButtonRepository.ButtonClick += (o, e) => OnEvent(() => _presenter.RemoveOperand(_gridViewBinder.FocusedElement));
         addConditionButton.Click += (o, e) => OnEvent(() => _presenter.AddOperand());
      }

      private RepositoryItem repositoryForTag(EditConditionGroupOperandDTO operand)
      {
         return operand.IsTagless ? (RepositoryItem)_disabledTagRepository : _tagComboBoxRepository;
      }

      private void onShowingEditor(object sender, CancelEventArgs e)
      {
         //only guard the Tag column; the Type combo and the remove button must always remain interactive.
         if (gridView.FocusedColumn != _columnTag?.XtraColumn)
            return;

         var operand = _gridViewBinder.FocusedElement;
         if (operand == null)
            return;

         e.Cancel = operand.IsTagless;
      }

      private void onTypeChanged(EditConditionGroupOperandDTO operand)
      {
         //tag is not meaningful for tag-less operand types; drop any leftover value so the snapshot is clean.
         if (operand.IsTagless)
            operand.Tag = string.Empty;
         gridView.RefreshData();
      }
   }
}