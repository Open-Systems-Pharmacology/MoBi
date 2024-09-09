using System.Collections.Generic;
using MoBi.Assets;
using MoBi.Presentation.Views;

namespace MoBi.Presentation.Presenter
{
   public interface IRefreshInitialConditionsPresenter : IApplyToSelectionPresenter
   {
   }

   public class RefreshInitialConditionsPresenter : ApplyToSelectionPresenter, IRefreshInitialConditionsPresenter
   {
      public RefreshInitialConditionsPresenter(IApplyToSelectionButtonView view)
         : base(view, SelectOption.RefreshSelected, AppConstants.Captions.RefreshValues)
      {
      }

      public override IEnumerable<SelectOption> AvailableSelectOptions => new[] { SelectOption.RefreshSelected };
   }
}