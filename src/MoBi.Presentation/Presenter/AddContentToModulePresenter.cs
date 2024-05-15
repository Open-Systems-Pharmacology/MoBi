using System.Collections.Generic;
using System.Linq;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;
using OSPSuite.Utility;

namespace MoBi.Presentation.Presenter
{
   public interface IAddContentToModulePresenter : IBaseModuleContentPresenter
   {
      void AddMoleculesSelectionChanged(bool moleculesSelected);
   }

   public abstract class BaseModuleContentPresenter<TView, TPresenter> : AbstractDisposablePresenter<TView, TPresenter> where TView : IView<TPresenter> where TPresenter : IDisposablePresenter
   {
      protected BaseModuleContentPresenter(TView view) : base(view)
      {
      }

      public IReadOnlyList<MergeBehavior> AllMergeBehaviors => EnumHelper.AllValuesFor<MergeBehavior>().ToList();
   }

   public abstract class AddContentToModulePresenter<TView, TPresenter> : BaseModuleContentPresenter<TView, TPresenter>, IAddContentToModulePresenter where TView : IAddContentToModuleView<TPresenter> where TPresenter : IAddContentToModulePresenter
   {
      protected AddContentToModulePresenter(TView view) : base(view)
      {
      }

      protected abstract Module Module { get; }
      protected abstract ModuleContentDTO ContentDTO { get; }

      public void AddMoleculesSelectionChanged(bool moleculesSelected)
      {
         if (!moleculesSelected || Module.InitialConditionsCollection.Any())
            return;

         ContentDTO.WithInitialConditions = true;
         _view.ShowInitialConditionsName();
      }
   }
}