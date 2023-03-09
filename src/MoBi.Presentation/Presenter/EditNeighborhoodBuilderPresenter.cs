using System.Collections.Generic;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface IEditNeighborhoodBuilderPresenter : ICreatePresenter<INeighborhoodBuilder>, IPresenterWithBuildingBlock
   {
      void UpdateName(string name);
   }

   public class EditNeighborhoodBuilderPresenter : AbstractCommandCollectorPresenter<IEditNeighborhoodBuilderView, IEditNeighborhoodBuilderPresenter>, IEditNeighborhoodBuilderPresenter
   {
      private readonly IEditTaskFor<INeighborhoodBuilder> _editTask;
      private readonly ISelectNeighborPathPresenter _firstNeighborPresenter;
      private readonly ISelectNeighborPathPresenter _secondNeighborPresenter;
      private ObjectBaseDTO _objectBaseDTO;
      private INeighborhoodBuilder _neighborhoodBuilder;
      public IBuildingBlock BuildingBlock { get; set; }

      public EditNeighborhoodBuilderPresenter(IEditNeighborhoodBuilderView view,
         IEditTaskFor<INeighborhoodBuilder> editTask,
         ISelectNeighborPathPresenter firstNeighborPresenter,
         ISelectNeighborPathPresenter secondNeighborPresenter) : base(view)
      {
         _editTask = editTask;
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
//TODO         _neighborhoodBuilder.SecondNeighbor =  _secondNeighborPresenter.NeighborPath;
      }

      private void updateFirstNeighbor()
      {
       //  _neighborhoodBuilder.FirstNeighbor = _firstNeighborPresenter.NeighborPath;
      }

      public void Edit(INeighborhoodBuilder neighborhoodBuilder, IEnumerable<IObjectBase> existingObjectsInParent)
      {
         _objectBaseDTO = new ObjectBaseDTO();
         _neighborhoodBuilder = neighborhoodBuilder;
         _objectBaseDTO.AddUsedNames(_editTask.GetForbiddenNamesWithoutSelf(neighborhoodBuilder, existingObjectsInParent));
         _firstNeighborPresenter.Init(spatialStructure, AppConstants.Captions.FirstNeighbor);
         _secondNeighborPresenter.Init(spatialStructure, AppConstants.Captions.SecondNeighbor);

         _view.BindTo(_objectBaseDTO);
      }

      public void UpdateName(string name)
      {
         _neighborhoodBuilder.Name = name;
      }

      private ISpatialStructure spatialStructure => BuildingBlock as ISpatialStructure;
   }
}