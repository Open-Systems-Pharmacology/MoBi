using System.Collections.Generic;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface IEditNeighborhoodBuilderPresenter : ICreatePresenter<INeighborhoodBuilder>, IPresenterWithBuildingBlock
   {
   }

   public class EditNeighborhoodBuilderPresenter : AbstractCommandCollectorPresenter<IEditNeighborhoodBuilderView, IEditNeighborhoodBuilderPresenter>, IEditNeighborhoodBuilderPresenter
   {
      private readonly ISelectContainerInTreePresenter _firstNeighborPresenter;
      private readonly ISelectContainerInTreePresenter _secondNeighborPresenter;
      private readonly IContainerToContainerDTOMapper _containerDTOMapper;
      private ObjectBaseDTO _objectBaseDTO;
      public IBuildingBlock BuildingBlock { get; set; }

      public EditNeighborhoodBuilderPresenter(IEditNeighborhoodBuilderView view,
         ISelectContainerInTreePresenter firstNeighborPresenter,
         ISelectContainerInTreePresenter secondNeighborPresenter,
         IContainerToContainerDTOMapper containerDTOMapper) : base(view)
      {
         _firstNeighborPresenter = firstNeighborPresenter;
         _secondNeighborPresenter = secondNeighborPresenter;
         _containerDTOMapper = containerDTOMapper;
         AddSubPresenters(_firstNeighborPresenter, _secondNeighborPresenter);
         _view.AddFirstNeighborView(_firstNeighborPresenter.BaseView);
         _view.AddSecondNeighborView(_secondNeighborPresenter.BaseView);
      }

      public void Edit(INeighborhoodBuilder neighborhoodBuilder, IEnumerable<IObjectBase> existingObjectsInParent)
      {
         //TODO can we have more than one?
         _objectBaseDTO = new ObjectBaseDTO();
         var organism = spatialStructure.TopContainers.Find(x => x.ContainerType == ContainerType.Organism);

         _firstNeighborPresenter.InitTreeStructure(new[] {_containerDTOMapper.MapFrom(organism)});
         _secondNeighborPresenter.InitTreeStructure(new[] {_containerDTOMapper.MapFrom(organism)});

         _view.BindTo(_objectBaseDTO);
      }

      private ISpatialStructure spatialStructure => BuildingBlock as ISpatialStructure;

      public override bool CanClose => base.CanClose && _firstNeighborPresenter.ContainerSelected && _secondNeighborPresenter.ContainerSelected;
   }
}