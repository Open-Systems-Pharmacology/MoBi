using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;

namespace MoBi.UI.Views
{
   public class ModuleSelectionView : ObjectBaseSelectionView, IModuleSelectionView
   {
      private IModuleSelectionPresenter _presenter;
      private ScreenBinder<ModuleSelectionDTO> _screenBinder;

      public override void InitializeBinding()
      {
         _screenBinder = new ScreenBinder<ModuleSelectionDTO>();
         _screenBinder.Bind(x => x.SelectedObject)
            .To(cbBuildingBlocks)
            .WithValues(x => _presenter.AllAvailableModules)
            .AndDisplays(x => _presenter.DisplayNameFor(x))
            .Changed += () => OnEvent(_presenter.SelectedModuleChanged);

         RegisterValidationFor(_screenBinder, NotifyViewChanged);
         btnNew.Click += (o, e) => OnEvent(() => _presenter.CreateNew());
      }

      protected override void DisposeBinders()
      {
         _screenBinder.Dispose();
      }

      public void AttachPresenter(IModuleSelectionPresenter presenter)
      {
         _presenter = presenter;
      }

      public void BindTo(ModuleSelectionDTO moduleSelectionDTO)
      {
         _screenBinder.BindToSource(moduleSelectionDTO);
         AdjustHeight();
      }
   }
}