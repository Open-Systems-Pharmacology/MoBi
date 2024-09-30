using FakeItEasy;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Helpers;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Collections;

namespace MoBi.Presentation
{
   public abstract class concern_for_EditAssignmentBuilderPresenter : ContextSpecification<IEditAssignmentBuilderPresenter>
   {
      private IEditEventAssignmentBuilderView _view;
      private IEventAssignmentBuilderToEventAssignmentDTOMapper _mapper;
      private IEditTaskFor<EventAssignmentBuilder> _editTasksForAssignment;
      private IFormulaToFormulaBuilderDTOMapper _formulaMapper;
      private IEditFormulaInContainerPresenter _editFormulaPresenter;
      private IMoBiContext _context;
      private ISelectReferenceAtEventAssignmentPresenter _referencePresenter;
      private IContextSpecificReferencesRetriever _contextReferenceRetriever;
      protected IMoBiApplicationController _applicationController;
      protected EventAssignmentBuilder _eventAssignmentBuilder;
      private IContainer _parentContainer;
      private MoBiMacroCommand _macroCommand;

      protected override void Context()
      {
         _view = A.Fake<IEditEventAssignmentBuilderView>();
         _mapper = A.Fake<IEventAssignmentBuilderToEventAssignmentDTOMapper>();
         _editTasksForAssignment = A.Fake<IEditTaskFor<EventAssignmentBuilder>>();
         _formulaMapper = A.Fake<IFormulaToFormulaBuilderDTOMapper>();
         _editFormulaPresenter = A.Fake<IEditFormulaInContainerPresenter>();
         _context = A.Fake<IMoBiContext>();
         _referencePresenter = A.Fake<ISelectReferenceAtEventAssignmentPresenter>();
         _contextReferenceRetriever = A.Fake<IContextSpecificReferencesRetriever>();
         _applicationController = A.Fake<IMoBiApplicationController>();

         sut = new EditAssignmentBuilderPresenter(_view, _mapper, _editTasksForAssignment, _formulaMapper, _editFormulaPresenter, _context, _referencePresenter, _contextReferenceRetriever, _applicationController);

         _parentContainer = new Container().WithName("ROOT");
         _eventAssignmentBuilder = new EventAssignmentBuilder {ParentContainer = _parentContainer};
         _macroCommand = new MoBiMacroCommand();

         sut.InitializeWith(_macroCommand);
         sut.Edit(_eventAssignmentBuilder);
      }
   }

   public class When_selecting_the_path_of_the_target_object : concern_for_EditAssignmentBuilderPresenter
   {
      private ISelectEventAssignmentTargetPresenter _selectionTargetPresenter;
      private FormulaUsablePath _selectedObjectPath;

      protected override void Context()
      {
         base.Context();
         _selectedObjectPath = new FormulaUsablePath("A", "B", "C") {Dimension = DomainHelperForSpecs.AmountDimension};

         _selectionTargetPresenter = A.Fake<ISelectEventAssignmentTargetPresenter>();
         A.CallTo(() => _applicationController.Start<ISelectEventAssignmentTargetPresenter>()).Returns(_selectionTargetPresenter);

         A.CallTo(() => _selectionTargetPresenter.Select()).Returns(_selectedObjectPath);
      }

      protected override void Because()
      {
         sut.SelectPath();
      }

      [Observation]
      public void should_leverage_the_select_target_presenter_to_retrieve_the_selection_with_the_root_container()
      {
         A.CallTo(() => _selectionTargetPresenter.Init(_eventAssignmentBuilder.RootContainer, A<Cache<IObjectBase, string>>._)).MustHaveHappened();
      }

      [Observation]
      public void should_set_the_object_path_into_the_event_target_assignment()
      {
         _eventAssignmentBuilder.ObjectPath.ShouldBeEqualTo(_selectedObjectPath);
      }
   }

   public class When_setting_the_dimension_of_the_target : concern_for_EditAssignmentBuilderPresenter
   {
      protected override void Context()
      {
         base.Context();
         _eventAssignmentBuilder.ObjectPath = new ObjectPath("A", "B", "C");
         _eventAssignmentBuilder.Dimension = DomainHelperForSpecs.AmountDimension;
      }

      protected override void Because()
      {
         sut.SetDimension(DomainHelperForSpecs.ConcentrationDimension);
      }

      [Observation]
      public void should_ensure_that_the_path_does_not_changed()
      {
         _eventAssignmentBuilder.ObjectPath.ToString().ShouldBeEqualTo(new ObjectPath("A","B", "C"));
      }

      [Observation]
      public void should_update_the_dimension()
      {
         _eventAssignmentBuilder.Dimension.ShouldBeEqualTo(DomainHelperForSpecs.ConcentrationDimension);
      }
   }

   public class When_setting_the_target_path_manually : concern_for_EditAssignmentBuilderPresenter
   {
      protected override void Context()
      {
         base.Context();
         _eventAssignmentBuilder.ObjectPath = new ObjectPath("A", "B", "C");
         _eventAssignmentBuilder.Dimension = DomainHelperForSpecs.AmountDimension;
      }

      protected override void Because()
      {
         sut.SetEventAssignmentPath("C|D|E");
      }

      [Observation]
      public void should_update_the_path()
      {
         _eventAssignmentBuilder.ObjectPath.ToString().ShouldBeEqualTo(new ObjectPath("C", "D", "E"));
      }

      [Observation]
      public void should_ensure_that_the_dimension_does_not_changed()
      {
         _eventAssignmentBuilder.Dimension.ShouldBeEqualTo(DomainHelperForSpecs.AmountDimension);
      }
   }
}