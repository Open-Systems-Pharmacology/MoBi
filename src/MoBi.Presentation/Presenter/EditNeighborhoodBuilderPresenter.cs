using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Extensions;
using MoBi.Core.Helper;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter.BasePresenter;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Presenter
{
   public interface IEditNeighborhoodBuilderPresenter : IPresenter<IEditNeighborhoodBuilderView>, ICanEditPropertiesPresenter, IPresenterWithFormulaCache, IEditPresenterWithParameters<NeighborhoodBuilder>
   {
      void SetName(string initialName);
      void SetFirstNeighborPath(string neighborPath);
      void SetSecondNeighborPath(string neighborPath);
      void SelectFirstNeighbor();
      void SelectSecondNeighbor();
   }

   public class EditNeighborhoodBuilderPresenter : AbstractContainerEditPresenterWithParameters<IEditNeighborhoodBuilderView, IEditNeighborhoodBuilderPresenter, NeighborhoodBuilder>, IEditNeighborhoodBuilderPresenter
   {
      private readonly ITagsPresenter _tagsPresenter;
      private readonly INeighborhoodBuilderToNeighborhoodBuilderDTOMapper _neighborhoodBuilderMapper;
      private readonly IMoBiApplicationController _applicationController;
      private NeighborhoodBuilder _neighborhoodBuilder;
      private NeighborhoodBuilderDTO _neighborhoodBuilderDTO;
      private IReadOnlyList<IObjectBase> _existingObjectsInParent;

      public EditNeighborhoodBuilderPresenter(
         IEditNeighborhoodBuilderView view,
         IEditParametersInContainerPresenter editParametersInContainerPresenter,
         ITagsPresenter tagsPresenter,
         IMoBiContext context,
         IEditTaskForContainer editTask,
         INeighborhoodBuilderToNeighborhoodBuilderDTOMapper neighborhoodBuilderMapper,
         IMoBiApplicationController applicationController 
      ) : base(view, editParametersInContainerPresenter, context, editTask)
      {
         _tagsPresenter = tagsPresenter;
         _neighborhoodBuilderMapper = neighborhoodBuilderMapper;
         _applicationController = applicationController;
         AddSubPresenters(_tagsPresenter);
         _view.AddTagsView(_tagsPresenter.BaseView);
      }

      public override object Subject => _neighborhoodBuilder;

      protected override IContainer SubjectContainer => _neighborhoodBuilder;

      public override void Edit(NeighborhoodBuilder neighborhoodBuilder, IReadOnlyList<IObjectBase> existingObjectsInParent)
      {
         _neighborhoodBuilder = neighborhoodBuilder;
         _existingObjectsInParent = existingObjectsInParent;
         base.Edit(neighborhoodBuilder, existingObjectsInParent);
         existingObjectsInParent = existingObjectsInParent ?? new List<IObjectBase>();
         _neighborhoodBuilderDTO = _neighborhoodBuilderMapper.MapFrom(neighborhoodBuilder, existingObjectsInParent.OfType<NeighborhoodBuilder>().ToList());
         _neighborhoodBuilderDTO.AddUsedNames(_editTask.GetForbiddenNamesWithoutSelf(neighborhoodBuilder, existingObjectsInParent));
         _tagsPresenter.Edit(neighborhoodBuilder);
         _view.BindTo(_neighborhoodBuilderDTO);
      }

      private void rebind()
      {
         Edit(_neighborhoodBuilder, _existingObjectsInParent);
      }

      public void SetName(string initialName)
      {
         SetPropertyValueFromView(_neighborhoodBuilder.PropertyName(x => x.Name), initialName, string.Empty);
         _neighborhoodBuilderDTO.Name = initialName;
      }

      public void SetFirstNeighborPath(string neighborPath)
      {
         AddCommand(new ChangeFirstNeighborPathCommand(neighborPath, _neighborhoodBuilder, spatialStructure).RunCommand(_context));
      }

      public void SetSecondNeighborPath(string neighborPath)
      {
         AddCommand(new ChangeSecondNeighborPathCommand(neighborPath, _neighborhoodBuilder, spatialStructure).RunCommand(_context));
      }

      private SpatialStructure spatialStructure => BuildingBlock.DowncastTo<SpatialStructure>();

      public void SelectFirstNeighbor() => selectNeighbor(AppConstants.Captions.FirstNeighbor, _neighborhoodBuilderDTO.FirstNeighborDTO, SetFirstNeighborPath);

      public void SelectSecondNeighbor() => selectNeighbor(AppConstants.Captions.SecondNeighbor, _neighborhoodBuilderDTO.SecondNeighborDTO, SetSecondNeighborPath);

      public override IBuildingBlock BuildingBlock
      {
         set
         {
            base.BuildingBlock = value;
            _tagsPresenter.BuildingBlock = value;
         }
      }

      private void selectNeighbor(string label, NeighborhoodObjectPathDTO neighborPath, Action<string> setNeighborAction)
      {
         var neighbor = retrieveNeighborPath(label, neighborPath);
         if (neighbor == null)
            return;

         setNeighborAction(neighbor.ToString());
         //we rebind in order to refresh the path in the UI
         rebind();
      }

      private ObjectPath retrieveNeighborPath(string label, NeighborhoodObjectPathDTO currentNeighborPath)
      {
         using (var modalPresenter = _applicationController.Start<IModalPresenter>())
         {
            modalPresenter.Text = AppConstants.Captions.SelectContainer;
            var selectNeighborPresenter = _applicationController.Start<ISelectNeighborPathPresenter>();
            modalPresenter.Encapsulate(selectNeighborPresenter);
            selectNeighborPresenter.Init(label, selectionDTO: currentNeighborPath);

            return modalPresenter.Show() ? selectNeighborPresenter.NeighborPath : null;
         }
      }
   }
}