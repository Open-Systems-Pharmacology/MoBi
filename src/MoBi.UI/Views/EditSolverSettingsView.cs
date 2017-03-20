using System;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.UI.RepositoryItems;
using OSPSuite.Assets;
using OSPSuite.Utility.Extensions;
using DevExpress.XtraEditors.Repository;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.UI.Controls;

namespace MoBi.UI.Views
{
   public partial class EditSolverSettingsView : BaseUserControl, IEditSolverSettingsView
   {
      private IEditSolverSettingsPresenter _presenter;
      private ScreenBinder<SolverSettingsDTO> _screenBinder;
      private GridViewBinder<ISolverOptionDTO> _gridBinder;

      public EditSolverSettingsView()
      {
         InitializeComponent();
      }

      public override void InitializeBinding()
      {
         _screenBinder = new ScreenBinder<SolverSettingsDTO>();
         _screenBinder.Bind(dto => dto.Name).To(cbSolver)
            .WithValues(_presenter.GetSolverNames())
            .OnValueSet += onSolverChanged;

         _gridBinder = new GridViewBinder<ISolverOptionDTO>(gridView);
         _gridBinder.Bind(dtoOption => dtoOption.Name).AsReadOnly();
         _gridBinder.Bind(dtoOption => dtoOption.Value)
            .WithEditRepository(getOptionEditrepositoryItem)
            .OnValueSet += onOptionChanged;

         RegisterValidationFor(_screenBinder, NotifyViewChanged);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         layoutGroup.Text = AppConstants.Captions.Options;
         layoutControl.HideItem(layoutItemSolver);
         Caption = AppConstants.Captions.SolverSettings;
      }

      private RepositoryItem getOptionEditrepositoryItem(ISolverOptionDTO solverOption)
      {
         if (solverOption.Type.IsAnImplementationOf<bool>())
            return createBoolEditRepositoryItem();

         if (solverOption.Type.IsAnImplementationOf<int>())
            return createIntegerEditRepositoryItem();

         return new RepositoryItemTextEdit();
      }

      private RepositoryItem createIntegerEditRepositoryItem()
      {
         return new RepositoryItemSpinEdit {IsFloatValue = false};
      }

      private RepositoryItem createBoolEditRepositoryItem()
      {
         var comboBox = new UxRepositoryItemComboBox(gridView);
         comboBox.Items.AddRange(new object[] {Convert.ToString(true), Convert.ToString(false)});
         return comboBox;
      }

      private void onOptionChanged(ISolverOptionDTO solverOption, PropertyValueSetEventArgs<string> e)
      {
         _presenter.SetSolverPropertyValue(solverOption, e.NewValue, e.OldValue);
      }

      private void onSolverChanged(SolverSettingsDTO solverSettings, PropertyValueSetEventArgs<string> e)
      {
         this.DoWithinExceptionHandler(() => _presenter.SolverChanged(e.NewValue));
      }

      public void AttachPresenter(IEditSolverSettingsPresenter presenter)
      {
         _presenter = presenter;
      }

      public void Show(SolverSettingsDTO dto)
      {
         _screenBinder.BindToSource(dto);
         _gridBinder.BindToSource(dto.SolverOptions);
      }

      public bool ShowGroupCaption
      {
         set
         {
            layoutGroup.TextVisible = value;
            layoutGroup.GroupBordersVisible = value;
         }
      }
      public override ApplicationIcon ApplicationIcon
      {
         get { return _presenter.Icon; }
      }
   }
}