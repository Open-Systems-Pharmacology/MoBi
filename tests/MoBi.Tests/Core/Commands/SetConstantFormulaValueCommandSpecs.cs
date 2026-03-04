using FakeItEasy;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Extensions;
using MoBi.Core.Services;
using MoBi.Helpers;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Events;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_SetConstantFormulaValueCommand : ContextSpecification<SetConstantFormulaValueCommand>
   {
      protected IMoBiContext _context;
      protected ConstantFormula _formula;
      protected IEntity _owner;
      protected Module _module;
      private IBuildingBlockVersionUpdater _buildingBlockVersionUpdater;

      protected override void Context()
      {
         _buildingBlockVersionUpdater = new BuildingBlockVersionUpdater(A.Fake<IMoBiProjectRetriever>(), A.Fake<IEventPublisher>(), A.Fake<IDialogCreator>());
         _context = A.Fake<IMoBiContext>();
         _owner = new Parameter { Id = "id", Name = "Parameter" };
         _formula = new ConstantFormula(3.0);
         var buildingBlock = new ParameterValuesBuildingBlock().WithId("bbid");
         _module = new Module { IsPKSimModule = true };
         _module.Add(buildingBlock);
         sut = new SetConstantFormulaValueCommand(
            constantFormula: _formula,
            newValue: 4.0,
            displayUnit: DomainHelperForSpecs.AmountDimension.DefaultUnit,
            oldUnit: DomainHelperForSpecs.ConcentrationDimension.DefaultUnit,
            buildingBlock: buildingBlock,
            formulaOwner: _owner);
         A.CallTo(() => _context.Get<IEntity>("id")).Returns(_owner);
         A.CallTo(() => _context.Resolve<IBuildingBlockVersionUpdater>()).Returns(_buildingBlockVersionUpdater);
         A.CallTo(() => _context.Get<IBuildingBlock>(buildingBlock.Id)).Returns(buildingBlock);
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

      [Observation]
      public void the_module_is_a_pksim_module()
      {
         _module.IsPKSimModule.ShouldBeTrue();
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

      [Observation]
      public void the_module_is_not__pksim_module()
      {
         _module.IsPKSimModule.ShouldBeFalse();
      }
   }
}