using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Repository;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Presenter
{
   public interface ISelectContainerPresenter : ISelectObjectPathPresenter
   {
      ObjectPath Select();
   }

   public class SelectContainerPresenter : AbstractDisposablePresenter<ISelectObjectPathView, ISelectObjectPathPresenter>, ISelectContainerPresenter
   {
      private readonly IMoBiContext _context;
      private readonly ISelectContainerInTreePresenter _selectContainerInTreePresenter;
      private readonly ISpatialStructureToSpatialStructureDTOMapper _spatialStructureDTOMapper;
      private readonly IBuildingBlockRepository _buildingBlockRepository;

      public SelectContainerPresenter(
         ISelectObjectPathView view,
         IMoBiContext context,
         ISelectContainerInTreePresenter selectContainerInTreePresenter,
         ISpatialStructureToSpatialStructureDTOMapper spatialStructureDTOMapper,
         IBuildingBlockRepository buildingBlockRepository) : base(view)
      {
         _context = context;
         _selectContainerInTreePresenter = selectContainerInTreePresenter;
         _spatialStructureDTOMapper = spatialStructureDTOMapper;
         _buildingBlockRepository = buildingBlockRepository;
         AddSubPresenters(_selectContainerInTreePresenter);
         _view.Caption = AppConstants.Captions.SelectContainer;
         _view.AddSelectionView(_selectContainerInTreePresenter.View);
         _selectContainerInTreePresenter.OnSelectedEntityChanged += (o, e) => _view.OkEnabled = e != null;
      }

      public ObjectPath Select()
      {
         init();
         _view.Display();
         return _view.Canceled ? null : _selectContainerInTreePresenter.SelectedEntityPath;
      }

      private void init()
      {
         var list = _buildingBlockRepository.SpatialStructureCollection.MapAllUsing(_spatialStructureDTOMapper);
         _selectContainerInTreePresenter.InitTreeStructure(list);
      }
   }
}