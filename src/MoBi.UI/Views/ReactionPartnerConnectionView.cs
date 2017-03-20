using MoBi.Assets;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.UI;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Views;
using OSPSuite.UI.Services;
using OSPSuite.UI.Views;

namespace MoBi.UI.Views
{
   public class ReactionPartnerView : ReactionBuilderView<ReactionPartnerBuilderDTO>, IReactionPartnerView, IViewWithPopup
   {
      public ReactionPartnerView(IImageListRetriever imageListRetriever) : base(imageListRetriever)
      {
      }

      public override void InitializeBinding()
      {
         _gridBinder.Bind(item => item.MoleculeName)
            .AsReadOnly()
            .WithCaption(AppConstants.Captions.MoleculeNames)
            .OnValueSet += onMoleculeNameSet;

         _gridBinder.Bind(item => item.StoichiometricCoefficient)
            .WithCaption(AppConstants.Captions.StoichiometricCoefficient)
            .OnValueSet += onCoefficentSet;
         base.InitializeBinding();
      }

      private void onMoleculeNameSet(ReactionPartnerBuilderDTO reactionPartner, PropertyValueSetEventArgs<string> e)
      {
         OnEvent(() => _presenter.SetPartnerMoleculeName(e.NewValue, reactionPartner));
      }

      private void onCoefficentSet(ReactionPartnerBuilderDTO reactionPartner, PropertyValueSetEventArgs<double> e)
      {
         OnEvent(() => _presenter.SetStochiometricCoefficient(e.NewValue, reactionPartner));
      }
   }
}