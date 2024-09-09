using MoBi.Assets;
using MoBi.Presentation.Views;

namespace MoBi.Presentation.Presenter
{
   public interface IMoleculeNegativeValuesAllowedSelectionPresenter : IApplyToSelectionPresenter
   {
   }

   public class MoleculeNegativeValuesAllowedSelectionPresenter : ApplyToSelectionPresenter, IMoleculeNegativeValuesAllowedSelectionPresenter
   {
      public MoleculeNegativeValuesAllowedSelectionPresenter(IApplyToSelectionButtonView view)
         : base(view, SelectOption.SelectedNegativeValuesAllowed, AppConstants.Captions.MarkNegativeValuesAllowed)
      {
      }
   }
}