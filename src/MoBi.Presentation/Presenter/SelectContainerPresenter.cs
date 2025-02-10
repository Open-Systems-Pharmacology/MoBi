using System.Linq;
using MoBi.Assets;
using MoBi.Core.Domain.Repository;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Presenter
{
   public interface ISelectContainerPresenter : ISelectObjectPathPresenter
   {
      ObjectPath Select(ObjectPath excludedObjectPath);
   }

   public class SelectContainerPresenter : AbstractDisposablePresenter<ISelectObjectPathView, ISelectObjectPathPresenter>, ISelectContainerPresenter
   {
      private readonly ISelectContainerInTreePresenter _selectContainerInTreePresenter;
      private readonly IModuleToModuleAndSpatialStructureDTOMapper _moduleToModuleAndSpatialStructureDTOMapper;
      private readonly IBuildingBlockRepository _buildingBlockRepository;
      protected readonly IObjectPathFactory _objectPathFactory;

      public SelectContainerPresenter(
         ISelectObjectPathView view,
         ISelectContainerInTreePresenter selectContainerInTreePresenter,
         IModuleToModuleAndSpatialStructureDTOMapper moduleToModuleAndSpatialStructureDTOMapper,
         IBuildingBlockRepository buildingBlockRepository,
         IInteractionTasksForTopContainer tasksForTopContainer,
         IObjectPathFactory objectPathFactory) : base(view)
      {
         _selectContainerInTreePresenter = selectContainerInTreePresenter;
         _moduleToModuleAndSpatialStructureDTOMapper = moduleToModuleAndSpatialStructureDTOMapper;
         _buildingBlockRepository = buildingBlockRepository;
         AddSubPresenters(_selectContainerInTreePresenter);
         _view.Caption = AppConstants.Captions.SelectContainer;
         _view.AddSelectionView(_selectContainerInTreePresenter.View);
         _selectContainerInTreePresenter.OnSelectedEntityChanged += (o, e) => _view.OkEnabled = e != null;
         _objectPathFactory = objectPathFactory;
      }

      public ObjectPath Select(ObjectPath excludedObjectPath)
      {
         init(excludedObjectPath);
         _view.Display();
         return _view.Canceled ? null : _selectContainerInTreePresenter.SelectedEntityPath;
      }

      private void init(ObjectPath excludedObjectPath)
      {
         var list = _buildingBlockRepository.SpatialStructureCollection.Select(x => _moduleToModuleAndSpatialStructureDTOMapper.MapFrom(x.Module)).ToList();
         list.Each(x => removeTopContainersWithPath(x, excludedObjectPath));
         _selectContainerInTreePresenter.InitTreeStructure(list);
      }

      private void removeTopContainersWithPath(ModuleAndSpatialStructureDTO dto, ObjectPath excludedObjectPath) =>
         dto.SpatialStructure.TopContainers = dto.SpatialStructure.TopContainers
            .Where(containerDTO => containerDTO.ObjectBase is IContainer container && !excludedObjectPath.Equals(_objectPathFactory.CreateAbsoluteObjectPath(container)))
            .ToList();
   }
}