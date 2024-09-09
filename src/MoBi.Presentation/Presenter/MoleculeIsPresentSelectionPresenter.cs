using System.Collections.Generic;
using MoBi.Assets;
using MoBi.Presentation.Views;
using OSPSuite.Assets;

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