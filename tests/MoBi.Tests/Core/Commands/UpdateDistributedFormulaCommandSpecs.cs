using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_UpdateDistributedFormulaCommand : ContextSpecification<UpdateDistributedFormulaCommand>
   {
      private IBuildingBlock _buildingBlock;
      protected IDistributedParameter _parameter;
      protected DistributionFormula _newFormula;
      private string _formulaType;
      protected IMoBiContext _context;

      protected override void Context()
      {
         _buildingBlock = A.Fake<IBuildingBlock>();
         _parameter = new DistributedParameter();
         _parameter.Formula = new DiscreteDistributionFormula();
         _newFormula = new LogNormalDistributionFormula();
         _formulaType = "XX";
         sut = new UpdateDistributedFormulaCommand(_parameter, _newFormula, _formulaType, _buildingBlock);
         _context = A.Fake<IMoBiContext>();
      }
   }

   public class When_execitong_the_update_distributed_formula_for_a_distributed_parameter : concern_for_UpdateDistributedFormulaCommand
   {
      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void should_update_the_distributed_formula()
      {
         _parameter.Formula.ShouldBeEqualTo(_newFormula);
      }
   }
}