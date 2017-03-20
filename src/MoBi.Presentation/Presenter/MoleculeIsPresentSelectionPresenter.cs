using System.Collections.Generic;
using MoBi.Assets;
using MoBi.Presentation.Views;

namespace MoBi.Presentation.Presenter
{
   public interface IMoleculeIsPresentSelectionPresenter : IApplyToSelectionPresenter
   {
   }

   public class MoleculeIsPresentSelectionPresenter : ApplyToSelectionPresenter, IMoleculeIsPresentSelectionPresenter
   {
      public MoleculeIsPresentSelectionPresenter(IApplyToSelectionView view)
         : base(view, SelectOption.AllPresent, AppConstants.Captions.IsPresent)
      {
      }

      public override IEnumerable<SelectOption> AvailableSelectOptions
      {
         get { return new[] {SelectOption.AllPresent, SelectOption.AllNotPresent, SelectOption.SelectedPresent, SelectOption.SelectedNotPresent}; }
      }
   }
}