using System.Collections.Generic;
using MoBi.Assets;
using MoBi.Presentation.Views;
using OSPSuite.Assets;

namespace MoBi.Presentation.Presenter
{
   public interface IDeletePathAndValueEntityPresenter : IApplyToSelectionPresenter
   {
   }

   public class DeletePathAndValueEntityPresenter : ApplyToSelectionPresenter, IDeletePathAndValueEntityPresenter
   {
      public DeletePathAndValueEntityPresenter(IApplyToSelectionButtonView view)
         : base(view, SelectOption.DeleteSelected, AppConstants.Captions.DeleteSelected)
      {
      }

      public override IEnumerable<SelectOption> AvailableSelectOptions
      {
         get { return new[] { SelectOption.DeleteSelected }; }
      }
   }
}