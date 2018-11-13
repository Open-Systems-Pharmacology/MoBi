using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.UI.Extensions;
using OSPSuite.Utility.Extensions;
using DevExpress.Utils;
using DevExpress.XtraBars;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Presentation;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Services;
using OSPSuite.UI.Views;

namespace MoBi.UI.Views
{
   public partial class EditReactionBuilderView : BaseUserControl, IEditReactionBuilderView, IViewWithPopup
   {
      private IEditReactionBuilderPresenter _presenter;
      private ScreenBinder<ReactionBuilderDTO> _screenBinder;

      public EditReactionBuilderView(IImageListRetriever imageListRetriever)
      {
         InitializeComponent();
         barManager.Images = imageListRetriever.AllImages16x16;
      }

      public void AttachPresenter(IEditReactionBuilderPresenter presenter)
      {
         _presenter = presenter;
      }

      public void Activate()
      {
         ActiveControl = btName;
      }

      public void BindTo(ReactionBuilderDTO reactionBuilderDTO)
      {
         _screenBinder.BindToSource(reactionBuilderDTO);
         enableDisableControls(reactionBuilderDTO);

         layoutItemModifiers.Text = createPartnerPanelTitle(AppConstants.Captions.Modifiers, reactionBuilderDTO.Name);
         layoutItemEducts.Text = createPartnerPanelTitle(AppConstants.Captions.Educts, reactionBuilderDTO.Name);
         layoutItemProducts.Text = createPartnerPanelTitle(AppConstants.Captions.Products, reactionBuilderDTO.Name);
      }

      private void enableDisableControls(ReactionBuilderDTO dtoReactionBuilder)
      {
         var isNewBuilder = dtoReactionBuilder.Name.IsNullOrEmpty();
         btName.Properties.ReadOnly = !isNewBuilder;
         btName.Properties.Buttons[0].Visible = !isNewBuilder;
         ShowStoichiometry = !isNewBuilder;
      }

      public void SetFormulaView(IView view)
      {
         pnlKinetic.FillWith(view);
      }

      public void SetParameterView(IView view)
      {
         tabParameters.FillWith(view);
      }

      public void SetContainerCriteriaView(IView view)
      {
         panelContainerCriteria.FillWith(view);
      }

      public bool ShowStoichiometry
      {
         set
         {
            if (!value)
               xtraTab.TabPages.Remove(tabStoichiometry);
         }
      }

      public void ShowParameters()
      {
         tabParameters.Show();
      }

      public bool PlotProcessRateParameterEnabled
      {
         set => chkPlotParameter.Enabled = value;
      }

      public void SetEductView(IView view)
      {
         eductsPanel.FillWith(view);
      }

      public void SetProductView(IView view)
      {
         productsPanel.FillWith(view);
      }

      public void SetModifierView(IView view)
      {
         modifiersPanel.FillWith(view);
      }

      public BarManager PopupBarManager => barManager;

      public override void InitializeBinding()
      {

         _screenBinder = new ScreenBinder<ReactionBuilderDTO>();
         _screenBinder.Bind(item => item.StoichiometricFormula).To(lblStoichiometricFormula);
         _screenBinder.Bind(item => item.Name).To(btName).OnValueUpdating += onValueUpdating;
         _screenBinder.Bind(item => item.Description).To(htmlEditor).OnValueUpdating += onValueUpdating;
         _screenBinder.Bind(item => item.CreateProcessRateParameter).To(chkCreateParmeter).OnValueUpdating += onCreateParameterSet;
         _screenBinder.Bind(item => item.ProcessRateParameterPersistable).To(chkPlotParameter).OnValueUpdating += onPlotParameterSet;

         btName.ButtonClick += (o, e) => OnEvent(_presenter.RenameSubject);

         RegisterValidationFor(_screenBinder, NotifyViewChanged);
      }

      private void onPlotParameterSet(ReactionBuilderDTO dto, PropertyValueSetEventArgs<bool> e)
      {
         OnEvent(() => _presenter.SetPlotProcessRateParameter(e.NewValue));
      }

      private void onCreateParameterSet(ReactionBuilderDTO dto, PropertyValueSetEventArgs<bool> e)
      {
         OnEvent(() => _presenter.SetCreateProcessRateParameter(e.NewValue));
      }

      private void onValueUpdating<T>(ReactionBuilderDTO reactionBuilder, PropertyValueSetEventArgs<T> e)
      {
         OnEvent(() => _presenter.SetPropertyValueFromView(e.PropertyName, e.NewValue, e.OldValue));
      }

      public override bool HasError => base.HasError
                                       || _screenBinder.HasError
                                       || _presenter.HasEductsError
                                       || _presenter.HasProductsError;

      public override void InitializeResources()
      {
         base.InitializeResources();
         tabStoichiometry.Text = AppConstants.Captions.Stoichiometry;
         layoutItemDescription.Text = AppConstants.Captions.Description.FormatForLabel();
         layoutItemKinetic.Text = AppConstants.Captions.Kinetic;
         layoutItemKinetic.TextLocation = Locations.Top;
         layoutItemName.Text = AppConstants.Captions.Name.FormatForLabel();
         htmlEditor.Properties.ShowIcon = false;
         chkCreateParmeter.Text = AppConstants.Captions.CreateProcessRateParameter;
         chkPlotParameter.Text = AppConstants.Captions.PlotProcessRateParameter;
         tabContainerCriteria.Text = AppConstants.Captions.ContainerCriteria;

         layoutGroupContainerCritieria.Text = AppConstants.Captions.InContainerWith;
         layoutItemContainerCriteria.TextVisible = false;
         lblStoichiometricFormula.AsDescription();

         tabModifiers.Text = AppConstants.Captions.Modifiers;
         
      }

      private static string createPartnerPanelTitle(string partnerType, string reactionName)
      {
         return AppConstants.Captions.PartnerForReaction(partnerType, reactionName);
      }
   }
}