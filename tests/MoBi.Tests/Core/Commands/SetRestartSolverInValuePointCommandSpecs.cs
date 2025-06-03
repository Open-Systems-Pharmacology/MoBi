using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.HelpersForTests;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_SetRestartSolverInValuePointCommand : ContextSpecification<SetRestartSolverInValuePointCommand>
   {
      private IBuildingBlock _buildingBlock;
      private TableFormula _tableFormula;
      protected ValuePoint _valuePoint;
      private bool _newRestartSolverValue;
      protected IMoBiContext _context;

      protected override void Context()
      {
         _buildingBlock = A.Fake<IBuildingBlock>();
         _tableFormula = new TableFormula() { Dimension = DomainHelperForSpecs.AmountDimension, XDimension = DomainHelperForSpecs.TimeDimension };
         _valuePoint = new ValuePoint(1.0, 1.0) { RestartSolver = false };
         _newRestartSolverValue = true;
         _tableFormula.AddPoint(_valuePoint);

         _context = A.Fake<IMoBiContext>();
         A.CallTo(() => _context.Get<TableFormula>(_tableFormula.Id)).Returns(_tableFormula);
         sut = new SetRestartSolverInValuePointCommand(_tableFormula, _valuePoint, _newRestartSolverValue, _buildingBlock);
      }
   }

   public class When_reverting_the_restart_solver_property_on_a_value_point : concern_for_SetRestartSolverInValuePointCommand
   {
      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void the_restart_solver_property_should_be_restored()
      {
         _valuePoint.RestartSolver.ShouldBeFalse();
      }
   }

   public class When_changing_the_restart_solver_property_on_a_value_point : concern_for_SetRestartSolverInValuePointCommand
   {
      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void the_restart_solver_property_should_be_changed()
      {
         _valuePoint.RestartSolver.ShouldBeTrue();
      }
   }
}