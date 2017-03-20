using System.Collections.Generic;
using OSPSuite.BDDHelper;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks
{
   public class concern_for_EditTasksForAmountObserverBuilder : ContextSpecification<EditTasksForAmountObserverBuilder>
   {
      private IInteractionTaskContext _interactionTaskContext;
      private IMoBiContext _context;
      private IMoBiProject _project;
      protected ObserverBuildingBlock _buildingBlock;
      protected IAmountObserverBuilder _amountObserver;
      protected AmountObserverBuilder _amountObserverBuilderWithForbiddenName;
      protected IInteractionTask _interactionTask;

      protected override void Context()
      {
         _interactionTaskContext = A.Fake<IInteractionTaskContext>();
         _context = A.Fake<IMoBiContext>();
         _project = A.Fake<IMoBiProject>();
         _amountObserver = A.Fake<IAmountObserverBuilder>();
         _amountObserverBuilderWithForbiddenName = new AmountObserverBuilder {Name = "forbidden name"};
         _buildingBlock = new ObserverBuildingBlock {_amountObserverBuilderWithForbiddenName, _amountObserver};
         _interactionTask = A.Fake<IInteractionTask>();
         
         A.CallTo(() => _context.CurrentProject).Returns(_project);
         A.CallTo(() => _interactionTaskContext.Context).Returns(_context);
         A.CallTo(() => _project.ObserverBlockCollection).Returns(new[] {_buildingBlock});
         A.CallTo(() => _interactionTaskContext.InteractionTask).Returns(_interactionTask);

         sut = new EditTasksForAmountObserverBuilder(_interactionTaskContext);
      }
   }

   public class When_renaming_an_amount_observer_without_parent : concern_for_EditTasksForAmountObserverBuilder
   {
      protected override void Because()
      {
         sut.Rename(_amountObserver, null, _buildingBlock);
      }

      [Observation]
      public void the_list_of_forbidden_names_should_be_the_list_of_chidren_from_the_active_building_block()
      {
         A.CallTo(() => _interactionTask.Rename(_amountObserver, A<IEnumerable<string>>.That.Contains(_amountObserverBuilderWithForbiddenName.Name), _buildingBlock)).MustHaveHappened();
      }
   }
}
