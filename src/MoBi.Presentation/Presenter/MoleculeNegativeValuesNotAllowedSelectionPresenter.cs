using MoBi.Assets;
using MoBi.Presentation.Views;

namespace MoBi.Presentation.Presenter
{
   public interface IMoleculeNegativeValuesNotAllowedSelectionPresenter : IApplyToSelectionPresenter
   {
   }

   public class MoleculeNegativeValuesNotAllowedSelectionPresenter : ApplyToSelectionPresenter, IMoleculeNegativeValuesNotAllowedSelectionPresenter
   {
      public MoleculeNegativeValuesNotAllowedSelectionPresenter(IApplyToSelectionButtonView view)
         : base(view, SelectOption.SelectedNegativeValuesNotAllowed, AppConstants.Captions.MarkNegativeValuesNotAllowed)
      {
      }
   }
}