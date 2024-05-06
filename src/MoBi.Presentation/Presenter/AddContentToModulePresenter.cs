using System.Linq;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface IAddContentToModulePresenter : IDisposablePresenter
   {
      void AddMoleculesSelectionChanged(bool moleculesSelected);
   }

   public abstract class AddContentToModulePresenter<TView, TPresenter> : AbstractDisposablePresenter<TView, TPresenter>, IAddContentToModulePresenter where TView : IAddContentToModuleView<TPresenter> where TPresenter : IAddContentToModulePresenter
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