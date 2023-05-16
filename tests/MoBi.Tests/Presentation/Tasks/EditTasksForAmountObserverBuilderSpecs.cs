using System.Collections.Generic;
using System.Linq;
using OSPSuite.BDDHelper;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Helpers;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Core.Domain;
using MoBi.Core.Domain.Repository;
using MoBi.Core.Services;

namespace MoBi.Presentation.Tasks
{
   public class concern_for_EditTasksForAmountObserverBuilder : ContextSpecification<EditTasksForAmountObserverBuilder>
   {
      private IInteractionTaskContext _interactionTaskContext;
      private IMoBiContext _context;
      private MoBiProject _project;
      protected ObserverBuildingBlock _buildingBlock;
      protected AmountObserverBuilder _amountObserver;
      protected AmountObserverBuilder _amountObserverBuilderWithForbiddenName;
      protected IInteractionTask _interactionTask;

      protected IMoBiApplicationController _applicationController;
      protected IRenameObjectPresenter _renameObjectPresenter;
      private BuildingBlockRepository _buildingBlockRepository;
      private MoBiProjectRetriever _moBiProjectRetriever;

      protected override void Context()
      {
         _interactionTaskContext = A.Fake<IInteractionTaskContext>();
         _context = A.Fake<IMoBiContext>();
         _project = DomainHelperForSpecs.NewProject();
         _amountObserver = A.Fake<AmountObserverBuilder>();
         _amountObserverBuilderWithForbiddenName = new AmountObserverBuilder {Name = "forbidden name"};
         _buildingBlock = new ObserverBuildingBlock {_amountObserverBuilderWithForbiddenName, _amountObserver};
         _interactionTask = A.Fake<IInteractionTask>();
         _applicationController = A.Fake<IMoBiApplicationController>();
         _renameObjectPresenter = A.Fake<IRenameObjectPresenter>();
         _moBiProjectRetriever = new MoBiProjectRetriever(_context);
         _buildingBlockRepository = new BuildingBlockRepository(_moBiProjectRetriever);

         A.CallTo(() => _context.CurrentProject).Returns(_project);
         A.CallTo(() => _interactionTaskContext.Context).Returns(_context);
         A.CallTo(() => _interactionTaskContext.InteractionTask).Returns(_interactionTask);
         A.CallTo(() => _interactionTaskContext.ApplicationController).Returns(_applicationController);
         A.CallTo(() => _interactionTaskContext.BuildingBlockRepository).Returns(_buildingBlockRepository);
         A.CallTo(() => _applicationController.Start<IRenameObjectPresenter>()).Returns(_renameObjectPresenter);

         _project.AddModule(new Module()
         {
            _buildingBlock
         });
         
         sut = new EditTasksForAmountObserverBuilder(_interactionTaskContext);
      }
   }

   public class When_renaming_a_different_object_base : concern_for_EditTasksForAmountObserverBuilder
   {
      private AmountObserverBuilder _amountObserverBuilder;

      protected override void Context()
      {
         base.Context();
         _amountObserverBuilder = new AmountObserverBuilder();
      }

      protected override void Because()
      {
         sut.Rename(_amountObserverBuilder, Enumerable.Empty<IObjectBase>(), buildingBlock: null);
      }

      [Observation]
      public void must_use_the_application_controller_to_start_the_dedicated_new_name_presenter()
      {
         A.CallTo(() => _applicationController.Start<IRenameObjectPresenter>()).MustHaveHappened();
      }
   }

   public class When_renaming_an_amount_observer_without_parent : concern_for_EditTasksForAmountObserverBuilder
   {
      protected override void Because()
      {
         sut.Rename(_amountObserver, null, _buildingBlock);
      }

      [Observation]
      public void the_list_of_forbidden_names_should_be_the_list_of_children_from_the_active_building_block()
      {
         A.CallTo(() => _renameObjectPresenter.NewNameFrom(_amountObserver, A<IEnumerable<string>>.That.Contains(_amountObserverBuilderWithForbiddenName.Name), A<string>._)).MustHaveHappened();
      }
   }
}
