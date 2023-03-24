using System.Collections.Generic;
using MoBi.Core.Domain.Model;
using MoBi.Core.Helper;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter.BasePresenter;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface IEditNeighborhoodBuilderPresenter : IPresenter<IEditNeighborhoodBuilderView>, ICanEditPropertiesPresenter, IPresenterWithFormulaCache, IEditPresenterWithParameters<NeighborhoodBuilder>
   {
      void SetInitialName(string initialName);
   }

   public class EditNeighborhoodBuilderPresenter : AbstractContainerEditPresenterWithParameters<IEditNeighborhoodBuilderView, IEditNeighborhoodBuilderPresenter, NeighborhoodBuilder>, IEditNeighborhoodBuilderPresenter
   {
      private readonly ITagsPresenter _tagsPresenter;
      private readonly INeighborhoodBuilderToNeighborhoodBuilderDTOMapper _neighborhoodBuilderMapper;
      private NeighborhoodBuilder _neighborhoodBuilder;
      private NeighborhoodBuilderDTO _neighborhoodBuilderDTO;

      public EditNeighborhoodBuilderPresenter(
         IEditNeighborhoodBuilderView view,
         IEditParametersInContainerPresenter editParametersInContainerPresenter,
         ITagsPresenter tagsPresenter,
         IMoBiContext context,
         IEditTaskForContainer editTask,
         INeighborhoodBuilderToNeighborhoodBuilderDTOMapper neighborhoodBuilderMapper
      ) : base(view, editParametersInContainerPresenter, context, editTask)
      {
         _tagsPresenter = tagsPresenter;
         _neighborhoodBuilderMapper = neighborhoodBuilderMapper;
         AddSubPresenters(_tagsPresenter);
         _view.AddTagsView(_tagsPresenter.BaseView);
      }

      public override object Subject => _neighborhoodBuilder;

      protected override IContainer SubjectContainer => _neighborhoodBuilder;

      public override void Edit(NeighborhoodBuilder neighborhoodBuilder, IReadOnlyList<IObjectBase> existingObjectsInParent)
      {
         _neighborhoodBuilder = neighborhoodBuilder;
         base.Edit(neighborhoodBuilder, existingObjectsInParent);
         _neighborhoodBuilderDTO = _neighborhoodBuilderMapper.MapFrom(neighborhoodBuilder);
         _neighborhoodBuilderDTO.AddUsedNames(_editTask.GetForbiddenNamesWithoutSelf(neighborhoodBuilder, existingObjectsInParent));
         _tagsPresenter.Edit(neighborhoodBuilder);
         _view.BindTo(_neighborhoodBuilderDTO);
      }

      public void SetInitialName(string initialName)
      {
         SetPropertyValueFromView(_neighborhoodBuilder.PropertyName(x => x.Name), initialName, string.Empty);
         _neighborhoodBuilderDTO.Name = initialName;
      }

      public override IBuildingBlock BuildingBlock
      {
         set
         {
            base.BuildingBlock = value;
            _tagsPresenter.BuildingBlock = value;
         }
      }
   }
}