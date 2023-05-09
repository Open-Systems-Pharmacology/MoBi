using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_ChangeInitialConditionNameCommand : ContextSpecification<ChangeInitialConditionNameCommand>
   {
      protected IMoBiContext _context;
      protected InitialCondition _initialCondition;
      protected InitialConditionsBuildingBlock _buildingBlock;

      protected override void Context()
      {
         _context = A.Fake<IMoBiContext>();

         _initialCondition = new InitialCondition { Path = new ObjectPath("Path1", "Path2", "Name")};
         _buildingBlock = new InitialConditionsBuildingBlock { _initialCondition };
         sut = new ChangeInitialConditionNameCommand(
            _buildingBlock,
            _initialCondition.Path,
            "Name2");

         A.CallTo(() => _context.Get<InitialConditionsBuildingBlock>(_buildingBlock.Id)).Returns(_buildingBlock);
      }
   }

   public class When_changing_name_of_start_value : concern_for_ChangeInitialConditionNameCommand
   {
      protected override void Context()
      {
         base.Context();
         _initialCondition.Formula = new ExplicitFormula("");
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void formula_added_to_building_block_cache()
      {
         _buildingBlock.FormulaCache.ShouldContain(_initialCondition.Formula);
      }
   }

   public class When_changing_name_of_molecule_start_value : concern_for_ChangeInitialConditionNameCommand
   {
      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void name_of_start_value_has_been_changed()
      {
         _initialCondition.MoleculeName.ShouldBeEqualTo("Name2");
      }

      [Observation]
      public void name_of_start_value_becomes_key_for_building_block()
      {
         _buildingBlock[_initialCondition.Path].ShouldNotBeNull();
      }
   }

   public class When_reverting_command_for_rename_of_molecule_start_value : concern_for_ChangeInitialConditionNameCommand
   {
      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void name_change_should_be_reverted()
      {
         _initialCondition.MoleculeName.ShouldBeEqualTo("Name");
      }
   }
}