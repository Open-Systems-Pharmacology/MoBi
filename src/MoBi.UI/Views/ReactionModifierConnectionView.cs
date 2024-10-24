using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Views;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.UI.Services;
using OSPSuite.UI.Views;

namespace MoBi.UI.Views
{
   public class ReactionModifierView : ReactionBuilderView<ReactionModifierBuilderDTO>, IReactionModifierView, IViewWithPopup
   {
      public ReactionModifierView(IImageListRetriever imageListRetriever) : base(imageListRetriever)
      {
      }

      public override void InitializeBinding()
      {
         _gridBinder.Bind(item => item.ModiferName)
            .AsReadOnly()
            .WithCaption(AppConstants.Captions.MoleculeName);
         base.InitializeBinding();
      }
   }
}