using System.Collections.Generic;
using MoBi.Assets;
using MoBi.Presentation.Views;
using OSPSuite.Assets;

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