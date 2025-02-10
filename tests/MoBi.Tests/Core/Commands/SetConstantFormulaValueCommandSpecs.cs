using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Extensions;
using MoBi.Helpers;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_SetConstantFormulaValueCommand : ContextSpecification<SetConstantFormulaValueCommand>
   {
      protected IMoBiContext _context;
      protected ConstantFormula _formula;
      protected IEntity _owner;

      protected override void Context()
      {
         _context = A.Fake<IMoBiContext>();
         _owner = new Parameter {Id = "id", Name = "Parameter"};
         _formula = new ConstantFormula(3.0);
         sut = new SetConstantFormulaValueCommand(
            constantFormula: _formula, 
            newValue: 4.0, 
            displayUnit: DomainHelperForSpecs.AmountDimension.DefaultUnit, 
            oldUnit: DomainHelperForSpecs.ConcentrationDimension.DefaultUnit, 
            buildingBlock: new ParameterValuesBuildingBlock(), 
            formulaOwner: _owner);
         A.CallTo(() => _context.Get<IEntity>("id")).Returns(_owner);
      }

      
   }

   public class When_reverting_value_for_constant_formula : concern_for_SetConstantFormulaValueCommand
   {
      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void value_of_constant_formula_should_be_reverted()
      {
         _formula.Value.ShouldBeEqualTo(4.0);
      }
   }

   public class When_setting_value_for_constant_formula : concern_for_SetConstantFormulaValueCommand
   {
      protected override void Because()
      {
         sut.RunCommand(_context);
      }

      [Observation]
      public void value_must_be_updated()
      {
         _formula.Value.ShouldBeEqualTo(4.0);
      }

      [Observation]
      public void command_description_should_describe_changes_to_value_and_display_units()
      {
         sut.Description.ShouldBeEqualTo(AppConstants.Commands.SetConstantValueFormula("Parameter", "4 µmol", "3 µmol/l", _owner.Name));
      }
   }
}
