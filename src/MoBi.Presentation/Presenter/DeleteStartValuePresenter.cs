using System.Collections.Generic;
using MoBi.Assets;
using MoBi.Presentation.Views;

namespace MoBi.Presentation.Presenter
{
   public interface IDeleteStartValuePresenter : IApplyToSelectionPresenter
   {
   }

   public class DeleteStartValuePresenter : ApplyToSelectionPresenter, IDeleteStartValuePresenter
   {
      public DeleteStartValuePresenter(IApplyToSelectionView view)
         : base(view, SelectOption.DeleteSelected, AppConstants.Captions.DeleteValues)
      {
      }

      public override IEnumerable<SelectOption> AvailableSelectOptions
      {
         get { return new[] {SelectOption.DeleteSelected, SelectOption.DeleteSourceNotDefined}; }
      }
   }
}