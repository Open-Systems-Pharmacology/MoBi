using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;

namespace MoBi.UI.Views
{
   public abstract class AddContentToModuleView<TDTO, TPresenter> : BaseModuleContentView<TDTO>, IAddContentToModuleView<TPresenter> where TDTO : ModuleContentDTO where TPresenter : IAddContentToModulePresenter
   {
      protected TPresenter _presenter;

      public void AttachPresenter(TPresenter presenter)
      {
         _presenter = presenter;
      }

      public void ShowInitialConditionsName()
      {
         ShowOrHideNamingItem(initialConditionsNameItem, true);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         cbMolecules.CheckedChanged += (o, e) => OnEvent(moleculesChanged);
      }

      private void moleculesChanged()
      {
         _presenter.AddMoleculesSelectionChanged(cbMolecules.Checked);
      }
   }
}