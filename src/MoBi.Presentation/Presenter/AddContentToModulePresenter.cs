using System.Linq;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;

namespace MoBi.Presentation.Presenter
{
   public interface IAddContentToModulePresenter : IBaseModuleContentPresenter
   {
      void AddMoleculesSelectionChanged(bool moleculesSelected);
   }

   public abstract class AddContentToModulePresenter<TView, TPresenter> : BaseModuleContentPresenter<TView, TPresenter>, IAddContentToModulePresenter where TView : IAddContentToModuleView<TPresenter> where TPresenter : IAddContentToModulePresenter
   {
      protected AddContentToModulePresenter(TView view) : base(view)
      {
      }

      protected abstract Module Module { get; }
      protected abstract ModuleContentDTO ContentDTO { get; }

      public override MergeBehavior SelectedBehavior => ContentDTO.MergeBehavior;

      public void AddMoleculesSelectionChanged(bool moleculesSelected)
      {
         if (!moleculesSelected || (Module?.InitialConditionsCollection != null && Module.InitialConditionsCollection.Any()))
            return;

         ContentDTO.WithInitialConditions = true;
         _view.ShowInitialConditionsName();
      }
   }
}