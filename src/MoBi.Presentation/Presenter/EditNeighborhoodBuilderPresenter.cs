﻿using System.Collections.Generic;
using MoBi.Assets;
using MoBi.Presentation.DTO;
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
      private readonly ISelectNeighborPathPresenter _firstNeighborPresenter;
      private readonly ISelectNeighborPathPresenter _secondNeighborPresenter;
      private ObjectBaseDTO _objectBaseDTO;
      public IBuildingBlock BuildingBlock { get; set; }

      public EditNeighborhoodBuilderPresenter(IEditNeighborhoodBuilderView view,
         ISelectNeighborPathPresenter firstNeighborPresenter,
         ISelectNeighborPathPresenter secondNeighborPresenter) : base(view)
      {
         _firstNeighborPresenter = firstNeighborPresenter;
         _secondNeighborPresenter = secondNeighborPresenter;
         AddSubPresenters(_firstNeighborPresenter, _secondNeighborPresenter);
         _view.AddFirstNeighborView(_firstNeighborPresenter.BaseView);
         _view.AddSecondNeighborView(_secondNeighborPresenter.BaseView);
      }

      public void Edit(INeighborhoodBuilder neighborhoodBuilder, IEnumerable<IObjectBase> existingObjectsInParent)
      {
         _objectBaseDTO = new ObjectBaseDTO();
         _firstNeighborPresenter.Init(spatialStructure, AppConstants.Captions.FirstNeighbor);
         _secondNeighborPresenter.Init(spatialStructure, AppConstants.Captions.SecondNeighbor);

         _view.BindTo(_objectBaseDTO);
      }

      private ISpatialStructure spatialStructure => BuildingBlock as ISpatialStructure;
   }
}