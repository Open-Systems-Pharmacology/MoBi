using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.UI.Extensions;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Base;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Assets;
using OSPSuite.UI.Controls;
using ToolTips = MoBi.Assets.ToolTips;

namespace MoBi.UI.Views
{
   public partial class MoleculeDependentBuilderView : BaseUserControl, IMoleculeDependentBuilderView
   {
      private IMoleculeDependentBuilderPresenter _presenter;
      private readonly ScreenBinder<MoleculeList> _screenBinder;
      private readonly GridViewBinder<StringDTO> _gridViewIncludedBinder;
      private readonly GridViewBinder<StringDTO> _gridViewExcludedBinder;
      public string BuilderType { get; set; }

      public MoleculeDependentBuilderView()
      {
         InitializeComponent();
         gridViewExcludedMolecules.AllowsFiltering = false;
         gridViewIncludedMolecules.AllowsFiltering = false;
         _screenBinder = new ScreenBinder<MoleculeList>();
         _gridViewIncludedBinder = new GridViewBinder<StringDTO>(gridViewIncludedMolecules);
         _gridViewExcludedBinder = new GridViewBinder<StringDTO>(gridViewExcludedMolecules);
      }

      public void AttachPresenter(IMoleculeDependentBuilderPresenter presenter)
      {
         _presenter = presenter;
      }

      public void BindTo(MoleculeList moleculeList)
      {
         chkForAll.ToolTip = ToolTips.Observer.SelectAll(BuilderType);
         _screenBinder.BindToSource(moleculeList);
         _gridViewIncludedBinder.BindToSource(toDTOString(moleculeList.MoleculeNames));
         _gridViewExcludedBinder.BindToSource(toDTOString(moleculeList.MoleculeNamesToExclude));
      }

      public override void InitializeBinding()
      {
         _screenBinder.Bind(dto => dto.ForAll)
            .To(chkForAll)
            .OnValueSet += (o, e) => OnEvent(onForAllSet, e);

         initializeBinder(_gridViewIncludedBinder, _presenter.RemoveFromIncludeList);
         initializeBinder(_gridViewExcludedBinder, _presenter.RemoveFromExcludeList);

         btnAddToIncludeList.Click += (o, e) => OnEvent(_presenter.AddToIncludeList);
         btnAddToExcludeList.Click += (o, e) => OnEvent(_presenter.AddToExcludeList);

         _screenBinder.Bind(x => x.ForAll)
            .ToEnableOf(gridExcludedMolecules)
            .EnabledWhen(forAll => forAll);

         _screenBinder.Bind(x => x.ForAll)
            .ToEnableOf(gridIncludedMolecules)
            .EnabledWhen(forAll => !forAll);

         _screenBinder.Bind(x => x.ForAll)
            .ToEnableOf(btnAddToExcludeList)
            .EnabledWhen(forAll => forAll);

         _screenBinder.Bind(x => x.ForAll)
            .ToEnableOf(btnAddToIncludeList)
            .EnabledWhen(forAll => !forAll);

         RegisterValidationFor(_screenBinder, NotifyViewChanged);
      }

      private void initializeBinder(GridViewBinder<StringDTO> gridViewBinder, Action<string> deleteAction)
      {
         gridViewBinder.Bind(name => name.Print)
            .WithCaption(AppConstants.Captions.Molecule)
            .AsReadOnly();

         var buttonRepository = createAddRemoveButtonRepository();
         gridViewBinder.AddUnboundColumn()
            .WithCaption(OSPSuite.UI.UIConstants.EMPTY_COLUMN)
            .WithShowButton(ShowButtonModeEnum.ShowAlways)
            .WithRepository(dto => buttonRepository)
            .WithFixedWidth(OSPSuite.UI.UIConstants.Size.EMBEDDED_BUTTON_WIDTH);

         buttonRepository.ButtonClick += (o, e) => OnEvent(() => onButtonClicked(e, gridViewBinder.FocusedElement, deleteAction));
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         layoutGroupInclude.Text = AppConstants.Captions.IncludeList;
         layoutGroupExclude.Text = AppConstants.Captions.ExcludeList;
         btnAddToIncludeList.InitWithImage(ApplicationIcons.Add, AppConstants.Captions.AddMolecule, toolTip: ToolTips.Observer.AddMoleculeToIncludeList);
         btnAddToExcludeList.InitWithImage(ApplicationIcons.Add, AppConstants.Captions.AddMolecule, toolTip: ToolTips.Observer.AddMoleculeToExcludeList);

         layoutGroupMoleculeSelection.Text = AppConstants.Captions.CalculatedForFollowingMolecules.FormatForLabel();
         layouytItemAddToIncludeList.AdjustLongButtonSize();
         layouytItemAddToExcludeList.AdjustLongButtonSize();
      }

      private void onForAllSet(PropertyValueSetEventArgs<bool> e)
      {
         _presenter.SetForAll(e.NewValue);
      }

      private IEnumerable<StringDTO> toDTOString(IEnumerable<string> moleculeNames)
      {
         return moleculeNames.Select(moleculeName => new StringDTO {Print = moleculeName}).ToList();
      }

      private void onButtonClicked(ButtonPressedEventArgs e, StringDTO nameToRemove, Action<string> deleteAction)
      {
         var pressedButton = e.Button;
         if (!pressedButton.Kind.Equals(ButtonPredefines.Delete)) return;

         deleteAction(nameToRemove.Print);
      }

      private RepositoryItemButtonEdit createAddRemoveButtonRepository()
      {
         var buttonRepository = new RepositoryItemButtonEdit {TextEditStyle = TextEditStyles.HideTextEditor};
         buttonRepository.Buttons[0].Kind = ButtonPredefines.Delete;
         buttonRepository.Buttons[0].ToolTip = ToolTips.Observer.DeleteMolecule;
         buttonRepository.Buttons.Add(new EditorButton(ButtonPredefines.Delete));
         return buttonRepository;
      }
   }
}