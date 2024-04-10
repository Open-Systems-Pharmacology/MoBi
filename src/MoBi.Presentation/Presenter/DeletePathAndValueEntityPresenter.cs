using System.Collections.Generic;
using MoBi.Assets;
using MoBi.Presentation.Views;

namespace MoBi.Presentation.Presenter
{
   public interface IDeletePathAndValueEntityPresenter : IApplyToSelectionPresenter
   {
   }

   public class DeletePathAndValueEntityPresenter : ApplyToSelectionPresenter, IDeletePathAndValueEntityPresenter
   {
      public DeletePathAndValueEntityPresenter(IApplyToSelectionView view)
         : base(view, SelectOption.DeleteSelected, AppConstants.Captions.DeleteValues)
      {
      }

      public override IEnumerable<SelectOption> AvailableSelectOptions
      {
         get { return new[] { SelectOption.DeleteSelected }; }
      }
   }
}