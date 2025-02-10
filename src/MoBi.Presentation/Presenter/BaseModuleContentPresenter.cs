using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility;

namespace MoBi.Presentation.Presenter
{
   public interface IBaseModuleContentPresenter : IDisposablePresenter
   {
      IReadOnlyList<MergeBehavior> AllMergeBehaviors { get; }
      void MergeBehaviorChanged();
   }

   public abstract class BaseModuleContentPresenter<TView, TPresenter> : AbstractDisposablePresenter<TView, TPresenter> where TView : IBaseModuleContentView<TPresenter> where TPresenter : IBaseModuleContentPresenter
   {
      protected BaseModuleContentPresenter(TView view) : base(view)
      {
         
      }

      public IReadOnlyList<MergeBehavior> AllMergeBehaviors => EnumHelper.AllValuesFor<MergeBehavior>().ToList();


      public void MergeBehaviorChanged()
      {
         updateMergeBehaviorDescription();
      }

      private void updateMergeBehaviorDescription()
      {
         _view.SetBehaviorDescription(descriptionForSelectedBehavior(SelectedBehavior));
      }

      private string descriptionForSelectedBehavior(MergeBehavior selectedBehavior)
      {
         switch (selectedBehavior)
         {
            case MergeBehavior.Extend:
               return AppConstants.Captions.ExtendMergeBehaviorDescription;
            case MergeBehavior.Overwrite:
               return AppConstants.Captions.OverwriteMergeBehaviorDescription;
            default:
               throw new ArgumentOutOfRangeException(nameof(selectedBehavior), selectedBehavior, null);
         }
      }

      public abstract MergeBehavior SelectedBehavior { get; }
   }
}