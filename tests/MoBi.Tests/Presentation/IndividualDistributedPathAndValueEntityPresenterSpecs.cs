using FakeItEasy;
using MoBi.Presentation;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Presentation
{
   public class concern_for_IndividualDistributedPathAndValueEntityPresenter : ContextSpecification<IndividualDistributedPathAndValueEntityPresenter>
   {
      protected IInteractionTasksForIndividualBuildingBlock _interactionTasks;
      protected IIndividualDistributedPathAndValueEntityView _view;

      protected override void Context()
      {
         _view = A.Fake<IIndividualDistributedPathAndValueEntityView>();
         _interactionTasks = A.Fake<IInteractionTasksForIndividualBuildingBlock>();
         sut = new IndividualDistributedPathAndValueEntityPresenter(_view, _interactionTasks);
         sut.InitializeWith(A.Fake<ICommandCollector>());
      }
   }

   public class When_changing_a_distributed_sub_parameter_unit : concern_for_IndividualDistributedPathAndValueEntityPresenter
   {
      private IndividualParameterDTO _dto;
      private IndividualBuildingBlock _buildingBlock;
      private IndividualParameterDTO _subParameter;
      private Unit _newUnit;

      protected override void Context()
      {
         base.Context();
         _buildingBlock = new IndividualBuildingBlock();
         _subParameter = new IndividualParameterDTO(new IndividualParameter().WithName(Constants.Distribution.MEAN));
         _dto = new IndividualParameterDTO(new IndividualParameter());
         _dto.AddSubParameter(_subParameter);
         _newUnit = new Unit("new unit", 1.0, 0.0);
         sut.Edit(_dto, _buildingBlock);
      }

      protected override void Because()
      {
         sut.SetParameterUnit(_subParameter, _newUnit);
      }

      [Observation]
      public void should_use_the_interaction_task_to_raise_the_commands()
      {
         A.CallTo(() => _interactionTasks.SetUnit(_buildingBlock, _subParameter.PathWithValueObject, _newUnit)).MustHaveHappened();
      }
   }

   public class When_changing_a_distributed_sub_parameter_value : concern_for_IndividualDistributedPathAndValueEntityPresenter
   {
      private IndividualParameterDTO _dto;
      private IndividualBuildingBlock _buildingBlock;
      private IndividualParameterDTO _subParameter;

      protected override void Context()
      {
         base.Context();
         _buildingBlock = new IndividualBuildingBlock();
         _subParameter = new IndividualParameterDTO(new IndividualParameter().WithName(Constants.Distribution.MEAN));
         _dto = new IndividualParameterDTO(new IndividualParameter());
         _dto.AddSubParameter(_subParameter);
         sut.Edit(_dto, _buildingBlock);
      }

      protected override void Because()
      {
         sut.SetParameterValue(_subParameter, 1.0);
      }

      [Observation]
      public void should_use_the_interaction_task_to_raise_the_commands()
      {
         A.CallTo(() => _interactionTasks.SetValue(_buildingBlock, 1.0, _subParameter.PathWithValueObject)).MustHaveHappened();
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
