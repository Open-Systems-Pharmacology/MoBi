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
      public MoleculeNegativeValuesAllowedSelectionPresenter(IApplyToSelectionButtonView view)
         : base(view, SelectOption.SelectedNegativeValuesAllowed, AppConstants.Captions.MarkNegativeValuesAllowed)
      {
      }

      public override IEnumerable<SelectOption> AvailableSelectOptions
      {
         get { return new[] { SelectOption.AllNegativeValuesAllowed, SelectOption.AllNegativeValuesNotAllowed, SelectOption.SelectedNegativeValuesAllowed, SelectOption.SelectedNegativeValuesNotAllowed }; }
      }
   }
}