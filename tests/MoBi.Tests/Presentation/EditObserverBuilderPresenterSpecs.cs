using OSPSuite.BDDHelper;
using OSPSuite.Core.Commands.Core;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Core.Helper;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.MenusAndBars.ContextMenus;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.Presenters.ContextMenus;

namespace MoBi.Presentation
{
   public class concern_for_EditObserverBuilderPresenter : ContextSpecification<EditAmountObserverBuilderPresenter>
   {
      protected IEditObserverBuilderView _editObserverBuilderView;
      protected AmountObserverBuilder _observerBuilder;
      protected ObserverBuildingBlock _buildingBlock;
      protected IEditFormulaPresenter _editFormulaPresenter;
      private ICommandCollector _commandCollector;
      private IObserverBuilderToDTOObserverBuilderMapper _observerMapper;

      protected override void Context()
      {
         _observerBuilder = A.Fake<AmountObserverBuilder>();
         _editObserverBuilderView = A.Fake<IEditObserverBuilderView>();
         _buildingBlock= new ObserverBuildingBlock();
         _editFormulaPresenter= A.Fake<IEditFormulaPresenter>();
         _commandCollector= A.Fake<ICommandCollector>();
         _observerMapper = A.Fake<IObserverBuilderToDTOObserverBuilderMapper>();

         sut = new EditAmountObserverBuilderPresenter(
            _editObserverBuilderView,
            A.Fake<IMoBiContext>(),
            A.Fake<IEditTaskFor<AmountObserverBuilder>>(),
            _observerMapper,
            A.Fake<IViewItemContextMenuFactory>(),
            _editFormulaPresenter,
            A.Fake<ISelectReferenceAtAmountObserverPresenter>(),
            A.Fake<IMoleculeDependentBuilderPresenter>(),
            A.Fake<IDescriptorConditionListPresenter<ObserverBuilder>>()
            );


         sut.InitializeWith(_commandCollector);
         sut.BuildingBlock = _buildingBlock;
         sut.Edit(_observerBuilder);
      }

   }

   public class When_responding_to_an_event_where_context_does_not_match_ : concern_for_EditObserverBuilderPresenter
   {
      private AddedEvent<IFormula> _eventToHandle;

      protected override void Context()
      {
         base.Context();
         _eventToHandle = new AddedEvent<IFormula>(A.Fake<IFormula>(), A.Fake<IBuildingBlock>());
      }

      protected override void Because()
      {
         sut.Handle(_eventToHandle);
      }

      [Observation]
      public void should_not_rebind_to_the_view()
      {
         // There will be an initial call to bind, but the view should not be rebound in response to the event
         A.CallTo(() => _editObserverBuilderView.BindTo(A<ObserverBuilderDTO>.Ignored)).MustHaveHappenedOnceExactly();
      }
   }

   public class When_the_dimension_of_the_edited_observer_is_changed : concern_for_EditObserverBuilderPresenter
   {
      private IDimension _newDimension;

      protected override void Context()
      {
         base.Context();
         _newDimension= A.Fake<IDimension>();
      }

      protected override void Because()
      {
         sut.UpdateDimension(_newDimension);
      }

      [Observation]
      public void should_refresh_the_formula_view()
      {
         // There will be an initial call when the view is being bound and there should be another one to update the formula
         A.CallTo(() => _editFormulaPresenter.Init(_observerBuilder, _buildingBlock)).MustHaveHappenedTwiceExactly();
      }
   }
   public class When_responding_to_an_event_where_the_context_matches : concern_for_EditObserverBuilderPresenter
   {
      private AddedEvent<IFormula> _eventToHandle;

      protected override void Because()
      {
         _eventToHandle = new AddedEvent<IFormula>(A.Fake<IFormula>(), sut.BuildingBlock);
         sut.Handle(_eventToHandle);
      }

      [Observation]
      public void should_respond_to_event_by_rebinding_view()
      {
         // Called once for initial binding, then once more when responding to the event
         A.CallTo(() => _editObserverBuilderView.BindTo(A<ObserverBuilderDTO>.Ignored)).MustHaveHappenedTwiceExactly();
      }
   }
}
