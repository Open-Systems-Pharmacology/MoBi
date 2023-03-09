using MoBi.Assets;
using MoBi.Core.Domain.Model;
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
      private readonly IObjectPathFactory _objectPathFactory;
      private readonly ISpatialStructureToSpatialStructureDTOMapper _spatialStructureDTOMapper;

      public SelectContainerPresenter(
         ISelectObjectPathView view,
         IMoBiContext context,
         ISelectContainerInTreePresenter selectContainerInTreePresenter,
         IContainerToContainerDTOMapper containerDTOMapper,
         IObjectPathFactory objectPathFactory,
         ISpatialStructureToSpatialStructureDTOMapper spatialStructureDTOMapper) : base(view)
      {
         _context = context;
         _selectContainerInTreePresenter = selectContainerInTreePresenter;
         _objectPathFactory = objectPathFactory;
         _spatialStructureDTOMapper = spatialStructureDTOMapper;
         AddSubPresenters(_selectContainerInTreePresenter);
         _view.Caption = AppConstants.Captions.SelectContainer;
         _view.AddSelectionView(_selectContainerInTreePresenter.View);
      }

      public ObjectPath Select()
      {
         init();
         _view.Display();
         return _view.Canceled ? null : _objectPathFactory.CreateAbsoluteObjectPath(_selectContainerInTreePresenter.SelectedEntity);
      }

      private void init()
      {
         var project = _context.CurrentProject;
         var list = project.SpatialStructureCollection.MapAllUsing(_spatialStructureDTOMapper);
         _selectContainerInTreePresenter.InitTreeStructure(list);
      }
   }
}