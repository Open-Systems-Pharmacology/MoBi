using System.Linq;
using FakeItEasy;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Presentation
{
   public class concern_for_IndividualBuildingBlockPresenter : ContextSpecification<IndividualBuildingBlockPresenter>
   {
      protected IIndividualBuildingBlockView _view;
      protected IIndividualParameterToIndividualParameterDTOMapper _individualParameterToIndividualParameterDTOMapper;
      protected IndividualBuildingBlockToIndividualBuildingBlockDTOMapper _individualBuildingBlockToIndividualBuildingBlockDTOMapper;
      protected IndividualBuildingBlock _buildingBlock;
      protected IndividualParameter _individualParameter1;
      protected IndividualParameter _individualParameter2;
      protected IndividualBuildingBlockDTO _buildingBlockDTO;
      protected IInteractionTasksForIndividualBuildingBlock _interactionTaskForIndividual;
      private ICommandCollector _commandCollector;
      private IFormulaToValueFormulaDTOMapper _formulaToValueFormulaDTOMapper;
      private IDimensionFactory _dimensionFactory;
      private IIndividualDistributedPathAndValueEntityPresenter _distributedParameterPresenter;

      protected override void Context()
      {
         _distributedParameterPresenter = A.Fake<IIndividualDistributedPathAndValueEntityPresenter>();
         _individualParameterToIndividualParameterDTOMapper = new IndividualParameterToIndividualParameterDTOMapper(new FormulaToValueFormulaDTOMapper());
         _dimensionFactory = A.Fake<IDimensionFactory>();
         _individualBuildingBlockToIndividualBuildingBlockDTOMapper = new IndividualBuildingBlockToIndividualBuildingBlockDTOMapper(_individualParameterToIndividualParameterDTOMapper);
         _view = A.Fake<IIndividualBuildingBlockView>();
         _interactionTaskForIndividual = A.Fake<IInteractionTasksForIndividualBuildingBlock>();
         _formulaToValueFormulaDTOMapper = new FormulaToValueFormulaDTOMapper();
         sut = new IndividualBuildingBlockPresenter(_view, _individualBuildingBlockToIndividualBuildingBlockDTOMapper, _interactionTaskForIndividual, _formulaToValueFormulaDTOMapper, _dimensionFactory, _distributedParameterPresenter);
         _commandCollector = A.Fake<ICommandCollector>();
         sut.InitializeWith(_commandCollector);

         _individualParameter1 = new IndividualParameter { Path = new ObjectPath("Path1", "Path2", "Name"), Value = 10 };
         _individualParameter2 = new IndividualParameter { Path = new ObjectPath("Path1", "Path3", "Name"), Value = 1 };
         _buildingBlock = new IndividualBuildingBlock
         {
            _individualParameter1,
            _individualParameter2
         };

         A.CallTo(() => _view.BindTo(A<IndividualBuildingBlockDTO>._)).Invokes(x => _buildingBlockDTO = x.GetArgument<IndividualBuildingBlockDTO>(0));
      }

      public class When_editing_a_distributed_parameter : concern_for_IndividualBuildingBlockPresenter
      {
         private IndividualParameterDTO _distributedParameter;

         protected override void Context()
         {
            base.Context();
            _distributedParameter = new IndividualParameterDTO( new IndividualParameter { DistributionType = DistributionType.Normal });
            sut.Edit(_buildingBlock);
         }

         protected override void Because()
         {
            sut.EditDistributedParameter(_distributedParameter);
         }

         [Observation]
         public void the_distributed_parameter_presenter_is_called()
         {
            A.CallTo(() => _distributedParameterPresenter.Edit(_distributedParameter, _buildingBlock)).MustHaveHappened();
         }
      }

      public class When_searching_for_multiple_distinct_paths : concern_for_IndividualBuildingBlockPresenter
      {
         protected override void Context()
         {
            base.Context();
            sut.Edit(_buildingBlock);
         }

         [Observation]
         public void should_find_distinct_paths_only_where_appropriate()
         {
            sut.HasAtLeastOneValue(0).ShouldBeTrue();
            sut.HasAtLeastOneValue(1).ShouldBeTrue();
            sut.HasAtLeastOneValue(2).ShouldBeFalse();
            sut.HasAtLeastOneValue(3).ShouldBeFalse();
         }
      }

      public class When_editing_the_building_block : concern_for_IndividualBuildingBlockPresenter
      {
         protected override void Because()
         {
            sut.Edit(_buildingBlock);
         }

         [Observation]
         public void the_view_should_bind_to_an_appropriate_dto()
         {
            A.CallTo(() => _view.BindTo(A<IndividualBuildingBlockDTO>._)).MustHaveHappened();
         }
      }
   }

   public class When_setting_the_unit_for_a_individual : concern_for_IndividualBuildingBlockPresenter
   {
      private Unit _unit;

      protected override void Context()
      {
         base.Context();
         sut.Edit(_buildingBlock);
      }

      protected override void Because()
      {
         _unit = new Unit("", 1, 0);
         sut.SetUnit(_buildingBlockDTO.Parameters.First(), _unit);
      }

      [Observation]
      public void the_presenter_uses_the_task_to_set_the_unit()
      {
         A.CallTo(() => _interactionTaskForIndividual.SetUnit(_buildingBlock, _individualParameter1, _unit)).MustHaveHappened();
      }
   }

   public class When_adding_a_new_formula_to_the_individual_building_block : concern_for_IndividualBuildingBlockPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.Edit(_buildingBlock);
      }

      protected override void Because()
      {
         sut.AddNewFormula(_buildingBlockDTO.Parameters.First());
      }

      [Observation]
      public void the_presenter_uses_the_interaction_task_to_add_the_formula()
      {
         A.CallTo(() => _interactionTaskForIndividual.AddNewFormulaAtBuildingBlock(_buildingBlock, _individualParameter1, null)).MustHaveHappened();
      }
   }

   public class When_changing_the_individual_parameter_formula : concern_for_IndividualBuildingBlockPresenter
   {
      private ExplicitFormula _formula;

      protected override void Context()
      {
         base.Context();
         sut.Edit(_buildingBlock);
         _formula = new ExplicitFormula();
      }

      protected override void Because()
      {
         sut.SetFormula(_buildingBlockDTO.Parameters.First(), _formula);
      }

      [Observation]
      public void the_presenter_uses_the_task_to_set_the_formula()
      {
         A.CallTo(() => _interactionTaskForIndividual.SetFormula(_buildingBlock, _individualParameter1, _formula)).MustHaveHappened();
      }
   }

   public class When_changing_the_individual_parameter_value : concern_for_IndividualBuildingBlockPresenter
   {
      private double? _newValue;

      protected override void Context()
      {
         base.Context();
         sut.Edit(_buildingBlock);
         _newValue = 99;
      }

      protected override void Because()
      {
         sut.SetValue(_buildingBlockDTO.Parameters.First(), _newValue);
      }

      [Observation]
      public void the_presenter_uses_the_task_to_set_the_value()
      {
         A.CallTo(() => _interactionTaskForIndividual.SetValue(_buildingBlock, _newValue, _individualParameter1)).MustHaveHappened();
      }
   }
}