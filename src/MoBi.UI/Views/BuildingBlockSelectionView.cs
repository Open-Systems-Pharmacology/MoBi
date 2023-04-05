using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;

namespace MoBi.UI.Views
{
   public class BuildingBlockSelectionView : ObjectBaseSelectionView, IBuildingBlockSelectionView
   {
      private ScreenBinder<BuildingBlockSelectionDTO> _screenBinder;
      private IBuildingBlockSelectionPresenter _presenter;

      public override void InitializeBinding()
      {
         _screenBinder = new ScreenBinder<BuildingBlockSelectionDTO>();
         _screenBinder.Bind(x => x.SelectedObject)
            .To(cbBuildingBlocks)
            .WithValues(x => _presenter.AllAvailableBlocks)
            .AndDisplays(x => _presenter.DisplayNameFor(x))
            .Changed += () => OnEvent(_presenter.SelectedBuildingBlockChanged);

         RegisterValidationFor(_screenBinder, NotifyViewChanged);
         btnNew.Click += (o, e) => OnEvent(() => _presenter.CreateNew());
      }

      public override bool HasError => _screenBinder.HasError;

      public void AttachPresenter(IBuildingBlockSelectionPresenter presenter)
      {
         _presenter = presenter;
      }

      public void BindTo(BuildingBlockSelectionDTO buildingBlockSelectionDTO)
      {
         _screenBinder.BindToSource(buildingBlockSelectionDTO);
         AdjustHeight();
      }

      public void RefreshElementList()
      {
         _screenBinder.RefreshListElements();
      }

      protected override void DisposeBinders()
      {
         _screenBinder.Dispose();
      }
   }
}