using MoBi.Assets;
using MoBi.Presentation.Views;

namespace MoBi.Presentation.Presenter
{
   public interface IMoleculeIsNotPresentSelectionPresenter : IApplyToSelectionPresenter
   {
   }

   public class MoleculeIsNotPresentSelectionPresenter : ApplyToSelectionPresenter, IMoleculeIsNotPresentSelectionPresenter
   {
      public MoleculeIsNotPresentSelectionPresenter(IApplyToSelectionButtonView view)
         : base(view, SelectOption.SelectedNotPresent, AppConstants.Captions.MarkAsNotPresent)
      {
      }
   }
}