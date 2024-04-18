using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Events;
using MoBi.Core.Services;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Tasks
{
   public abstract class concern_for_InteractionTasksForExpressionProfileBuildingBlock : ContextSpecification<InteractionTasksForExpressionProfileBuildingBlock>
   {
      protected IMoBiFormulaTask _formulaTask;
      protected IInteractionTaskContext _interactionTaskContext;
      protected IEditTasksForExpressionProfileBuildingBlock _editTask;
      protected ExpressionProfileBuildingBlock _buildingBlock;
      protected ExpressionParameter _expressionParameter;
      protected IFormula _formula;
      protected IPKSimStarter _pkSimStarter;
      private IContainerTask _containerTask;
      private IParameterFactory _parameterFactory;

      protected override void Context()
      {
         _editTask = A.Fake<IEditTasksForExpressionProfileBuildingBlock>();
         _formulaTask = A.Fake<IMoBiFormulaTask>();
         _interactionTaskContext = A.Fake<IInteractionTaskContext>();
         _pkSimStarter = A.Fake<IPKSimStarter>();
         _containerTask = new ContainerTask(A.Fake<IObjectBaseFactory>(), A.Fake<IEntityPathResolver>(), A.Fake<IObjectPathFactory>());
         _parameterFactory = A.Fake<IParameterFactory>();

         sut = new InteractionTasksForExpressionProfileBuildingBlock(_interactionTaskContext, _editTask, _formulaTask, _pkSimStarter, _containerTask, _parameterFactory);

         _formula = new ExplicitFormula("y=mx+b");
         _expressionParameter = GetExpressionParameter();
         _buildingBlock = new ExpressionProfileBuildingBlock
         {
            _expressionParameter
         };
      }

      protected virtual ExpressionParameter GetExpressionParameter()
      {
         return new ExpressionParameter
         {
            Formula = null,
            Value = 1.0,
            Path = new ObjectPath("Organism|Liver|Volume")
         };
      }

      public class When_adding_an_expression_profile_to_project : concern_for_InteractionTasksForExpressionProfileBuildingBlock
      {
         private MoBiProject _project;
         private ExpressionProfileBuildingBlock _expressionProfileToAdd;
         private IMoBiCommand _command;
         private AddedEvent<ExpressionProfileBuildingBlock> _addEvent;
         private IInteractionTask _interactionTask;
         private IMoBiContext _context;

         protected override void Context()
         {
            base.Context();
            _interactionTask = A.Fake<IInteractionTask>();
            _context = A.Fake<IMoBiContext>();
            A.CallTo(() => _interactionTaskContext.InteractionTask).Returns(_interactionTask);
            A.CallTo(() => _interactionTaskContext.Context).Returns(_context);
            _project = new MoBiProject();
            _expressionProfileToAdd = new ExpressionProfileBuildingBlock().WithName("Molecule|Species|Category");
            A.CallTo(() => _interactionTask.CorrectName(_expressionProfileToAdd, A<IEnumerable<string>>._)).Returns(true);
            A.CallTo(() => _editTask.NewNameFromSuggestions(_expressionProfileToAdd.MoleculeName, _expressionProfileToAdd.Species, A<string>._, _expressionProfileToAdd.Type, A<IReadOnlyList<string>>._, A<bool>._)).Returns("Molecule|Species|Category 1");
            A.CallTo(() => _context.PublishEvent(A<AddedEvent<ExpressionProfileBuildingBlock>>._))
               .Invokes(x => _addEvent = x.GetArgument<AddedEvent<ExpressionProfileBuildingBlock>>(0));

            _project.AddExpressionProfileBuildingBlock(new ExpressionProfileBuildingBlock().WithName("Molecule|Species|Category"));
         }

         protected override void Because()
         {
            _command = sut.AddTo(_expressionProfileToAdd, _project, null);
         }

         [Observation]
         public void should_add_the_molecule_to_the_building_block_using_a_silent_add_command()
         {
            var macroCommand = _command.DowncastTo<IMoBiMacroCommand>();
            var addCommand = macroCommand.All().First() as ISilentCommand;
            addCommand.Silent.ShouldBeTrue();
         }

         [Observation]
         public void the_added_expression_should_be_renamed()
         {
            _addEvent.AddedObject.Name.ShouldBeEqualTo("Molecule|Species|Category 1");
         }

         [Observation]
         public void should_notify_the_added_event_command()
         {
            _addEvent.AddedObject.ShouldBeEqualTo(_expressionProfileToAdd);
         }
      }

      public class When_updating_expression_parameters_from_pksim : concern_for_InteractionTasksForExpressionProfileBuildingBlock
      {
         private List<ExpressionParameterValueUpdate> _expressionProfileUpdate;
         private MoBiMacroCommand _result;

         protected override void Context()
         {
            base.Context();
            _expressionProfileUpdate = new List<ExpressionParameterValueUpdate>();
            _expressionProfileUpdate.AddRange(_buildingBlock.Select<ExpressionParameter, ExpressionParameterValueUpdate>(x => new ExpressionParameterValueUpdate(x.Path)));
            _expressionProfileUpdate.Each(x => { x.UpdatedValue = 99.0; });
            A.CallTo(() => _pkSimStarter.UpdateExpressionProfileFromDatabase(_buildingBlock)).Returns(_expressionProfileUpdate);
         }

         protected override void Because()
         {
            _result = sut.UpdateExpressionProfileFromDatabase(_buildingBlock) as MoBiMacroCommand;
         }

         [Observation]
         public void the_value_should_be_updated()
         {
            _buildingBlock.First<ExpressionParameter>().Value.Value.ShouldBeEqualTo(99.0);
         }

         [Observation]
         public void the_command_should_update_parameters()
         {
            _result.Count.ShouldBeEqualTo(1);
            _result.All().First().ShouldBeAnInstanceOf<PathAndValueEntityValueOrUnitChangedCommand<ExpressionParameter, ExpressionProfileBuildingBlock>>();
         }

         [Observation]
         public void the_pk_sim_starter_is_used_to_update()
         {
            A.CallTo(() => _pkSimStarter.UpdateExpressionProfileFromDatabase(_buildingBlock)).MustHaveHappened();
         }
      }

      public class When_cloning_an_expression_profile : concern_for_InteractionTasksForExpressionProfileBuildingBlock
      {
         protected override void Because()
         {
            sut.Clone(_buildingBlock);
         }

         [Observation]
         public void the_new_name_should_be_collected_using_the_expression_profile_namer()
         {
            // Ignore the Category because it will contain a suggested name change due to cloning
            A.CallTo(() => _editTask.NewNameFromSuggestions(_buildingBlock.MoleculeName, _buildingBlock.Species, A<string>._, _buildingBlock.Type, A<IReadOnlyList<string>>._, A<bool>._)).MustHaveHappened();
         }
      }

      public class When_setting_a_formula_in_a_building_block : concern_for_InteractionTasksForExpressionProfileBuildingBlock
      {
         protected override void Because()
         {
            sut.SetFormula(_buildingBlock, _expressionParameter, _formula);
         }

         [Observation]
         public void the_formula_should_be_set_in_the_expression_parameter()
         {
            _expressionParameter.Formula.ShouldBeEqualTo(_formula);
         }

         [Observation]
         public void the_value_should_be_NotANumber()
         {
            _expressionParameter.Value.ShouldBeEqualTo(double.NaN);
         }


      }

      public class When_setting_a_value_in_a_building_block : concern_for_InteractionTasksForExpressionProfileBuildingBlock
      {

         protected override void Because()
         {
            sut.SetValue(_buildingBlock, 1.0, _expressionParameter);
         }

         protected override ExpressionParameter GetExpressionParameter()
         {
            return new ExpressionParameter { Formula = new ExplicitFormula("y=b"), Value = null };
         }

         [Observation]
         public void the_formula_should_be_cleared_in_the_expression_parameter()
         {
            _expressionParameter.Formula.ShouldBeEqualTo(null);
         }

         [Observation]
         public void the_value_should_be_set_in_the_expression_parameter()
         {
            _expressionParameter.Value.ShouldBeEqualTo(1.0);
         }
      }

      public class When_setting_a_unit_in_a_building_block : concern_for_InteractionTasksForExpressionProfileBuildingBlock
      {
         private Unit _unit1;
         private Dimension _dimension;
         private Unit _unit2;

         protected override ExpressionParameter GetExpressionParameter()
         {
            _dimension = new Dimension(new BaseDimensionRepresentation(), "dimensionName", "baseUnit");
            _unit1 = new Unit("displayUnit", 1, 0);
            _unit2 = new Unit("anotherUnit", 10, 0);
            _dimension.AddUnit(_unit1);
            _dimension.AddUnit(_unit2);

            return new ExpressionParameter { Formula = null, Value = 1.0 };
         }

         protected override void Because()
         {
            sut.SetUnit(_buildingBlock, _expressionParameter, _unit2);
         }

         [Observation]
         public void the_unit_should_be_changed()
         {
            _expressionParameter.DisplayUnit.ShouldBeEqualTo(_unit2);
         }
      }
   }
}
