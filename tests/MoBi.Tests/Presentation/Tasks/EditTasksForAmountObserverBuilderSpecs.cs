using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Repository;
using MoBi.Core.Services;
using MoBi.HelpersForTests;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks
{
   public class concern_for_EditTasksForAmountObserverBuilder : ContextSpecification<EditTasksForAmountObserverBuilder>
   {
      protected IInteractionTaskContext _interactionTaskContext;
      private IMoBiContext _context;
      private MoBiProject _project;
      protected ObserverBuildingBlock _buildingBlock;
      protected AmountObserverBuilder _amountObserver;
      protected AmountObserverBuilder _amountObserverBuilderWithForbiddenName;
      protected IInteractionTask _interactionTask;

      protected IMoBiApplicationController _applicationController;
      private BuildingBlockRepository _buildingBlockRepository;
      private MoBiProjectRetriever _moBiProjectRetriever;

      protected override void Context()
      {
         _interactionTaskContext = A.Fake<IInteractionTaskContext>();
         _context = A.Fake<IMoBiContext>();
         _project = DomainHelperForSpecs.NewProject();
         _amountObserver = A.Fake<AmountObserverBuilder>();
         _amountObserverBuilderWithForbiddenName = new AmountObserverBuilder { Name = "forbidden name" };
         _buildingBlock = new ObserverBuildingBlock { _amountObserverBuilderWithForbiddenName, _amountObserver };
         _interactionTask = A.Fake<IInteractionTask>();
         _applicationController = A.Fake<IMoBiApplicationController>();
         _moBiProjectRetriever = new MoBiProjectRetriever(_context);
         _buildingBlockRepository = new BuildingBlockRepository(_moBiProjectRetriever);

         A.CallTo(() => _context.CurrentProject).Returns(_project);
         A.CallTo(() => _interactionTaskContext.Context).Returns(_context);
         A.CallTo(() => _interactionTaskContext.InteractionTask).Returns(_interactionTask);
         A.CallTo(() => _interactionTaskContext.ApplicationController).Returns(_applicationController);
         A.CallTo(() => _interactionTaskContext.BuildingBlockRepository).Returns(_buildingBlockRepository);

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
      public void must_use_the_context_start_the_dedicated_new_name_presenter()
      {
         A.CallTo(() => _interactionTaskContext.NamingTask.RenameFor(_amountObserverBuilder, A<IReadOnlyList<string>>._)).MustHaveHappened();
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
         A.CallTo(() => _interactionTaskContext.NamingTask.RenameFor(_amountObserver, A<IReadOnlyList<string>>.That.Contains(_amountObserverBuilderWithForbiddenName.Name))).MustHaveHappened();
      }
   }
}