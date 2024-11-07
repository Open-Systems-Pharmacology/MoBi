using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface ICreateNeighborhoodBuilderPresenter : ICreatePresenter<NeighborhoodBuilder>, IPresenterWithBuildingBlock
   {
      void UpdateName(string name);
   }

   public class CreateNeighborhoodBuilderPresenter : AbstractCommandCollectorPresenter<ICreateNeighborhoodBuilderView, ICreateNeighborhoodBuilderPresenter>, ICreateNeighborhoodBuilderPresenter
   {
      private readonly IEditTaskFor<NeighborhoodBuilder> _editTask;
      private readonly INeighborhoodBuilderToNeighborhoodBuilderDTOMapper _neighborhoodBuilderMapper;
      private readonly ISelectNeighborPathPresenter _firstNeighborPresenter;
      private readonly ISelectNeighborPathPresenter _secondNeighborPresenter;
      private NeighborhoodBuilderDTO _neighborhoodBuilderDTO;
      private NeighborhoodBuilder _neighborhoodBuilder;
      public IBuildingBlock BuildingBlock { get; set; }

      public CreateNeighborhoodBuilderPresenter(ICreateNeighborhoodBuilderView view,
         IEditTaskFor<NeighborhoodBuilder> editTask,
         INeighborhoodBuilderToNeighborhoodBuilderDTOMapper neighborhoodBuilderMapper,
         ISelectNeighborPathPresenter firstNeighborPresenter,
         ISelectNeighborPathPresenter secondNeighborPresenter) : base(view)
      {
         _editTask = editTask;
         _neighborhoodBuilderMapper = neighborhoodBuilderMapper;
         _firstNeighborPresenter = firstNeighborPresenter;
         _secondNeighborPresenter = secondNeighborPresenter;
         AddSubPresenters(_firstNeighborPresenter, _secondNeighborPresenter);
         _view.AddFirstNeighborView(_firstNeighborPresenter.BaseView);
         _view.AddSecondNeighborView(_secondNeighborPresenter.BaseView);

         _firstNeighborPresenter.StatusChanged += (o, e) => updateFirstNeighbor();
         _secondNeighborPresenter.StatusChanged += (o, e) => updateSecondNeighbor();
      }

      private void updateSecondNeighbor()
      {
         _neighborhoodBuilder.SecondNeighborPath = _secondNeighborPresenter.NeighborPath;
         _firstNeighborPresenter.RefreshView();
      }

      private void updateFirstNeighbor()
      {
         _neighborhoodBuilder.FirstNeighborPath = _firstNeighborPresenter.NeighborPath;
         _secondNeighborPresenter.RefreshView();
      }

      public void Edit(NeighborhoodBuilder neighborhoodBuilder, IReadOnlyList<IObjectBase> existingObjectsInParent)
      {
         _neighborhoodBuilderDTO = _neighborhoodBuilderMapper.MapFrom(neighborhoodBuilder, existingObjectsInParent.OfType<NeighborhoodBuilder>().ToList());
         _neighborhoodBuilder = neighborhoodBuilder;
         _neighborhoodBuilderDTO.AddUsedNames(_editTask.GetForbiddenNamesWithoutSelf(neighborhoodBuilder, existingObjectsInParent));
         _firstNeighborPresenter.Init(AppConstants.Captions.FirstNeighbor, _neighborhoodBuilderDTO.FirstNeighborDTO);
         _secondNeighborPresenter.Init(AppConstants.Captions.SecondNeighbor, _neighborhoodBuilderDTO.SecondNeighborDTO);

         _view.BindTo(_neighborhoodBuilderDTO);
      }

      public void UpdateName(string name)
      {
         _neighborhoodBuilder.Name = name;
      }
   }
}