using FakeItEasy;
using MoBi.Core.Services;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters.ContextMenus;

namespace MoBi.Presentation
{
   public abstract class concern_for_EditParameterListPresenter : ContextSpecification<EditParameterListPresenter>
   {
      private IEditParameterListView _view;
      private IQuantityTask _quantityTask;
      private IInteractionTaskContext _interactionTaskContext;
      private IFormulaToFormulaBuilderDTOMapper _formulaMapper;
      private IInteractionTasksForParameter _parameterTask;
      private IFavoriteTask _favoriteTask;
      private IViewItemContextMenuFactory _viewItemContextMenuFactory;
      private IParameterToParameterDTOMapper _parameterDTOMapper;
      protected ISourceReferenceNavigator _sourceReferenceNavigator;

      protected override void Context()
      {
         _view = A.Fake<IEditParameterListView>();
         _quantityTask = A.Fake<IQuantityTask>();
         _interactionTaskContext = A.Fake<IInteractionTaskContext>();
         _formulaMapper = A.Fake<IFormulaToFormulaBuilderDTOMapper>();
         _parameterTask = A.Fake<IInteractionTasksForParameter>();
         _favoriteTask = A.Fake<IFavoriteTask>();
         _viewItemContextMenuFactory = A.Fake<IViewItemContextMenuFactory>();
         _parameterDTOMapper = A.Fake<IParameterToParameterDTOMapper>();
         _sourceReferenceNavigator = A.Fake<ISourceReferenceNavigator>();

         sut = new EditParameterListPresenter(_view, _quantityTask, _interactionTaskContext, _formulaMapper, _parameterTask, _favoriteTask, _viewItemContextMenuFactory, _parameterDTOMapper, _sourceReferenceNavigator);
      }
   }

   public class When_navigating_to_parameter_source : concern_for_EditParameterListPresenter
   {
      private ParameterDTO _parameterDTO;
      private IParameter _parameter;

      protected override void Context()
      {
         base.Context();
         _parameter = new Parameter();
         _parameterDTO = new ParameterDTO(_parameter) {SourceReference = new SimulationEntitySourceReference(null, null, null, _parameter)};
      }

      protected override void Because()
      {
         sut.NavigateToParameter(_parameterDTO);
      }

      [Observation]
      public void the_navigation_task_is_used_to_navigate_to_the_source()
      {
         A.CallTo(() => _sourceReferenceNavigator.GoTo(_parameterDTO.SourceReference)).MustHaveHappened();
      }
   }
}