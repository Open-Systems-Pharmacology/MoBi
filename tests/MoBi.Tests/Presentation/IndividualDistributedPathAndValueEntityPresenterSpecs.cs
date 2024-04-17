using FakeItEasy;
using MoBi.Presentation;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation
{
   public class concern_for_IndividualDistributedPathAndValueEntityPresenter : ContextSpecification<IndividualDistributedPathAndValueEntityPresenter>
   {
      protected IIndividualDistributedPathAndValueEntityView _view;
      private IInteractionTasksForIndividualBuildingBlock _interactionTasks;

      protected override void Context()
      {
         _view = A.Fake<IIndividualDistributedPathAndValueEntityView>();
         _interactionTasks = A.Fake<IInteractionTasksForIndividualBuildingBlock>();
         sut = new IndividualDistributedPathAndValueEntityPresenter(_view, _interactionTasks);
         sut.InitializeWith(A.Fake<ICommandCollector>());
      }
   }
}

public class When_editing_a_distributed_path_and_value_entity : concern_for_IndividualDistributedPathAndValueEntityPresenter
{
   private IndividualParameterDTO _dto;
   private IndividualBuildingBlock _buildingBlock;

   protected override void Context()
   {
      base.Context();
      _buildingBlock = new IndividualBuildingBlock();
      _dto = new IndividualParameterDTO(new IndividualParameter());
      sut.Edit(_dto, _buildingBlock);
   }

   [Observation]
   public void Should_bind_to_the_dto()
   {
      A.CallTo(() => _view.BindTo(_dto)).MustHaveHappened();
   }
}
