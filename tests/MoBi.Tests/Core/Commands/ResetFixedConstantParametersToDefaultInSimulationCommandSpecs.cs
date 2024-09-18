using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Extensions;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_ResetFixedConstantParametersToDefaultInSimulationCommand : ContextSpecification<ResetFixedConstantParametersToDefaultInSimulationCommand<MoleculeBuildingBlock>>
   {
      private MoleculeBuildingBlock _buildingBlock;
      private IMoBiSimulation _simulation;
      protected Parameter _formulaParameter;
      protected Parameter _constantParameter;
      protected IMoBiContext _context;
      private IMoBiFormulaTask _formulaTask;
      protected Parameter _unaffectedParameter;

      protected override void Context()
      {
         _simulation = new MoBiSimulation();
         _buildingBlock = new MoleculeBuildingBlock();
         _formulaParameter = new Parameter().WithFormula(new ExplicitFormula()).WithName("formula");
         _formulaParameter.Value = 9;
         _constantParameter = new Parameter().WithFormula(new ConstantFormula(1.0)).WithName("constant");
         _unaffectedParameter = new Parameter().WithFormula(new ConstantFormula(1.0)).WithName("unaffected");
         _unaffectedParameter.Value = 5.0;
         _constantParameter.Value = 9;
         _simulation.Model = new Model {Root = new Container {_formulaParameter, _constantParameter, _unaffectedParameter}};
         _context = A.Fake<IMoBiContext>();

         sut = new ResetFixedConstantParametersToDefaultInSimulationCommand<MoleculeBuildingBlock>(_simulation, _buildingBlock);

         _formulaTask = A.Fake<IMoBiFormulaTask>();

         A.CallTo(() => _formulaTask.CreateNewFormula<ConstantFormula>(A<IDimension>._)).Returns(new ConstantFormula());

         A.CallTo(() => _context.Resolve<IMoBiFormulaTask>()).Returns(_formulaTask);

      }
   }

   public class When_resetting_simulation_parameters_from_building_block : concern_for_ResetFixedConstantParametersToDefaultInSimulationCommand
   {
      protected override void Because()
      {
         sut.RunCommand(_context);
      }

      [Observation]
      public void the_explicit_formula_parameters_should_not_be_affected()
      {
         _formulaParameter.IsFixedValue.ShouldBeTrue();
      }

      [Observation]
      public void parameter_not_from_affected_building_block_is_unaffected()
      {
         _unaffectedParameter.IsFixedValue.ShouldBeTrue();
         _unaffectedParameter.Value.ShouldBeEqualTo(5.0);
      }
   }
}
