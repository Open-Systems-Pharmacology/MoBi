using System.Collections.Generic;
using MoBi.Assets;
using MoBi.Presentation.Views;

namespace MoBi.Presentation.Presenter
{
   public interface IRefreshStartValueFromOriginalBuildingBlockPresenter : IApplyToSelectionPresenter
   {
   }

   public class RefreshStartValueFromOriginalBuildingBlockPresenter : ApplyToSelectionPresenter, IRefreshStartValueFromOriginalBuildingBlockPresenter
   {
      public RefreshStartValueFromOriginalBuildingBlockPresenter(IApplyToSelectionView view)
         : base(view, SelectOption.RefreshAll, AppConstants.Captions.RefreshValues)
      {
      }

      public override IEnumerable<SelectOption> AvailableSelectOptions
      {
         get { return new[] {SelectOption.RefreshAll, SelectOption.RefreshSelected}; }
      }
   }
}