using System.Collections.Generic;
using MoBi.Assets;
using MoBi.Presentation.Views;

namespace MoBi.Presentation.Presenter
{
   public interface IMoleculeNegativeValuesAllowedSelectionPresenter : IApplyToSelectionPresenter
   {
   }

   public class MoleculeNegativeValuesAllowedSelectionPresenter : ApplyToSelectionPresenter, IMoleculeNegativeValuesAllowedSelectionPresenter
   {
      public MoleculeNegativeValuesAllowedSelectionPresenter(IApplyToSelectionView view)
         : base(view, SelectOption.AllNegativeValuesAllowed, AppConstants.Captions.NegativeValues)
      {
      }

      public override IEnumerable<SelectOption> AvailableSelectOptions
      {
         get { return new[] { SelectOption.AllNegativeValuesAllowed, SelectOption.AllNegativeValuesNotAllowed, SelectOption.SelectedNegativeValuesAllowed, SelectOption.SelectedNegativeValuesNotAllowed }; }
      }
   }
}