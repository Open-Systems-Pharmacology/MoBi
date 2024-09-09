using System.Collections.Generic;
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

      public override IEnumerable<SelectOption> AvailableSelectOptions
      {
         get { return new[] { SelectOption.AllNegativeValuesAllowed, SelectOption.AllNegativeValuesNotAllowed, SelectOption.SelectedNegativeValuesAllowed, SelectOption.SelectedNegativeValuesNotAllowed }; }
      }
   }
}