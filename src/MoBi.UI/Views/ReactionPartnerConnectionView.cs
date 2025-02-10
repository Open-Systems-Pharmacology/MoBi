using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Views;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
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
            .WithCaption(AppConstants.Captions.MoleculeName)
            .OnValueUpdating += onMoleculeNameSet;

         _gridBinder.Bind(item => item.StoichiometricCoefficient)
            .WithCaption(AppConstants.Captions.StoichiometricCoefficient)
            .OnValueUpdating += onCoefficentSet;
         base.InitializeBinding();
      }

      private void onMoleculeNameSet(ReactionPartnerBuilderDTO reactionPartner, PropertyValueSetEventArgs<string> e)
      {
         OnEvent(() => _presenter.SetPartnerMoleculeName(e.NewValue, reactionPartner));
      }

      private void onCoefficentSet(ReactionPartnerBuilderDTO reactionPartner, PropertyValueSetEventArgs<double> e)
      {
         OnEvent(() => _presenter.SetStoichiometricCoefficient(e.NewValue, reactionPartner));
      }
   }
}