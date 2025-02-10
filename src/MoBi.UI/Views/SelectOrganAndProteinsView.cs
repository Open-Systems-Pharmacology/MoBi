using DevExpress.Utils;
using DevExpress.XtraLayout;
using MoBi.Assets;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Assets;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.Views;

namespace MoBi.UI.Views
{
   public partial class SelectOrganAndProteinsView : BaseModalView, ISelectOrganAndProteinsView
   {
      private ISelectOrganAndProteinsPresenter _presenter;

      public SelectOrganAndProteinsView()
      {
         InitializeComponent();
      }

      public override void InitializeResources()
      {
         base.InitializeResources();

         configureLayoutControlItem(layoutControlItemMolecules, AppConstants.Captions.Molecules.FormatForLabel());
         configureLayoutControlItem(layoutControlItemOrgan, Captions.Organ.FormatForLabel());
         descriptionLabel.AsDescription();
         descriptionLabel.Text = AppConstants.Captions.AddExpressionDescription;
      }

      public override void Display()
      {
         SetOkButtonEnable();
         base.Display();
      }

      private static void configureLayoutControlItem(LayoutControlItem controlItem, string label)
      {
         controlItem.Text = label;
         controlItem.TextLocation = Locations.Top;
         controlItem.TextVisible = true;
      }

      public void AttachPresenter(ISelectOrganAndProteinsPresenter presenter)
      {
         _presenter = presenter;
      }

      public void AddMoleculeSelectionView(IView view)
      {
         moleculeSelectionPanel.FillWith(view);
      }

      public void AddOrganSelectionView(IView view)
      {
         organSelectionPanel.FillWith(view);
      }

      public override bool HasError => !_presenter.CanClose || base.HasError;

      public void SelectionChanged()
      {
         SetOkButtonEnable();
      }
   }
}