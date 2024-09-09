using MoBi.Assets;
using MoBi.Presentation.Views;

namespace MoBi.Presentation.Presenter
{
   public interface IMoleculeIsPresentSelectionPresenter : IApplyToSelectionPresenter
   {
   }

   public class MoleculeIsPresentSelectionPresenter : ApplyToSelectionPresenter, IMoleculeIsPresentSelectionPresenter
   {
      public MoleculeIsPresentSelectionPresenter(IApplyToSelectionButtonView view)
         : base(view, SelectOption.SelectedPresent, AppConstants.Captions.MarkAsPresent)
      {
      }
   }
}