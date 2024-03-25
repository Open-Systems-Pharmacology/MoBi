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
      public RefreshInitialConditionsPresenter(IApplyToSelectionView view)
         : base(view, SelectOption.RefreshAll, AppConstants.Captions.RefreshValues)
      {
      }

      public override IEnumerable<SelectOption> AvailableSelectOptions => new[] { SelectOption.RefreshAll, SelectOption.RefreshSelected };
   }
}