using DevExpress.XtraLayout;
using DevExpress.XtraLayout.Utils;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Assets;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.Presentation.Extensions;
using OSPSuite.UI.Views;
using static MoBi.Assets.AppConstants.Captions;

namespace MoBi.UI.Views
{
   public abstract partial class BaseModuleContentView<TDTO, TPresenter> : BaseModalView, IBaseModuleContentView<TPresenter>
      where TDTO : ModuleContentDTO where TPresenter : IBaseModuleContentPresenter
   {
      protected readonly ScreenBinder<TDTO> _screenBinder = new ScreenBinder<TDTO>();
      protected TPresenter _presenter;

      protected BaseModuleContentView()
      {
         InitializeComponent();
      }

      public override bool HasError => _screenBinder.HasError;

      public override void InitializeResources()
      {
         base.InitializeResources();
         initialConditionsNameItem.Visibility = LayoutVisibility.Never;
         parameterValuesNameItem.Visibility = LayoutVisibility.Never;
         ApplicationIcon = ApplicationIcons.Module;
         moduleNameItem.Text = $"{Module} {Captions.Name}".FormatForLabel();

         cbSpatialStructure.Text = SpatialStructure;
         cbEventGroup.Text = Event;
         cbReactions.Text = Reactions;
         cbMolecules.Text = Molecules;
         cbObservers.Text = Observer;
         cbPassiveTransports.Text = PassiveTransports;
         cbInitialConditions.Text = InitialConditions;
         cbParameterValues.Text = ParameterValues;
         createBuildingBlocksGroup.Text = CreateBuildingBlocks;
         mergeBehaviorGroup.Text = MergeBehavior;
         mergeBehaviorItem.TextVisible = false;
         initialConditionsNameItem.Text = AppConstants.Captions.Name.FormatForLabel();
         parameterValuesNameItem.Text = AppConstants.Captions.Name.FormatForLabel();

         ShowOrHideNamingItem(initialConditionsNameItem, show: cbInitialConditions.Checked);
         ShowOrHideNamingItem(parameterValuesNameItem, show: cbParameterValues.Checked);

         lblDescription.AllowHtmlString = true;
      }

      public void SetBehaviorDescription(string description)
      {
         lblDescription.Text = description.FormatForDescription();
      }

      protected void ShowStartValueNameControls()
      {
         initialConditionsNameItem.Visibility = LayoutVisibility.Always;
         parameterValuesNameItem.Visibility = LayoutVisibility.Always;
      }

      protected override void SetActiveControl()
      {
         base.SetActiveControl();
         ActiveControl = tbModuleName;
      }

      public virtual void AttachPresenter(TPresenter presenter) => _presenter = presenter;

      public void DisableDefaultMergeBehavior() => mergeBehaviorItem.Enabled = false;

      public void HideDefaultMergeBehaviorGroup() => mergeBehaviorGroup.Visibility = LayoutVisibility.Never; 

      public override void InitializeBinding()
      {
         _screenBinder.Bind(dto => dto.Name).To(tbModuleName);
         _screenBinder.Bind(dto => dto.WithSpatialStructure).To(cbSpatialStructure);
         _screenBinder.Bind(dto => dto.WithEventGroup).To(cbEventGroup);
         _screenBinder.Bind(dto => dto.WithMolecule).To(cbMolecules);
         _screenBinder.Bind(dto => dto.WithObserver).To(cbObservers);
         _screenBinder.Bind(dto => dto.WithPassiveTransport).To(cbPassiveTransports);
         _screenBinder.Bind(dto => dto.WithReaction).To(cbReactions);
         _screenBinder.Bind(dto => dto.WithParameterValues).To(cbParameterValues).OnValueUpdated += (o, newValue) => OnEvent(() => ShowOrHideNamingItem(parameterValuesNameItem, show: newValue));
         _screenBinder.Bind(dto => dto.WithInitialConditions).To(cbInitialConditions).OnValueUpdated += (o, newValue) => OnEvent(() => ShowOrHideNamingItem(initialConditionsNameItem, show: newValue));
         _screenBinder.Bind(dto => dto.MergeBehavior).To(cbDefaultMergeBehavior).WithValues(_presenter.AllMergeBehaviors).Changed += () => OnEvent(_presenter.MergeBehaviorChanged);

         RegisterValidationFor(_screenBinder);
      }

      protected virtual void ShowOrHideNamingItem(LayoutControlItem namingLayoutControlItem, bool show) => namingLayoutControlItem.Visibility = LayoutVisibility.Never;

      public virtual void BindTo(TDTO moduleContentDTO)
      {
         _screenBinder.BindToSource(moduleContentDTO);
         _presenter.MergeBehaviorChanged();
         disableExistingBuildingBlocks(moduleContentDTO);
      }

      private void disableExistingBuildingBlocks(TDTO dto)
      {
         spatialStructureItem.Enabled = dto.CanSelectSpatialStructure;
         eventGroupItem.Enabled = dto.CanSelectEventGroup;
         reactionsItem.Enabled = dto.CanSelectReaction;
         moleculesItem.Enabled = dto.CanSelectMolecule;
         observersItem.Enabled = dto.CanSelectObserver;
         passiveTransportsItem.Enabled = dto.CanSelectPassiveTransport;
         initialConditionsItem.Enabled = dto.CanSelectInitialConditions;
         parameterValuesItem.Enabled = dto.CanSelectParameterValues;
      }

      protected void DisableRename() => tbModuleName.Enabled = false;

      protected virtual void DisposeBinders() => _screenBinder.Dispose();
   }
}