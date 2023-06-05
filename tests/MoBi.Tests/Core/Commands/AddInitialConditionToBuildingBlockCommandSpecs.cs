using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_AddInitialConditionToBuildingBlockCommand : ContextSpecification<AddInitialConditionToBuildingBlockCommand>
   {
      protected InitialConditionsBuildingBlock _buildingBlock;
      protected IMoBiContext _context;

      private IDimension _fakeDimension;
      protected InitialCondition _initialCondition;

      protected override void Context()
      {
         _fakeDimension = A.Fake<IDimension>();
         _context = A.Fake<IMoBiContext>();
         _buildingBlock = new InitialConditionsBuildingBlock();

         _initialCondition = new InitialCondition { Path = new ObjectPath("path1"), Dimension = _fakeDimension, Value = -1, DisplayUnit = new Unit("Dimensionless", 1.0, 1) };
         sut = new AddInitialConditionToBuildingBlockCommand(_buildingBlock, _initialCondition);

         A.CallTo(() => _context.Get<ILookupBuildingBlock<InitialCondition>>(A<string>._)).Returns(_buildingBlock);
      }
   }

   public class When_adding_molecule_start_value : concern_for_AddInitialConditionToBuildingBlockCommand
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

   public class after_successful_molecule_start_value_import : concern_for_AddInitialConditionToBuildingBlockCommand
   {
      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void molecule_start_value_added_to_building_block()
      {
         _buildingBlock.ShouldContain(_initialCondition);
      }
   }

   public class inverse_command_removes_molecule_start_value_from_building_block : concern_for_AddInitialConditionToBuildingBlockCommand
   {
      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void molecule_start_value_should_be_removed_from_building_block()
      {
         _buildingBlock.ShouldBeEmpty();
      }
   }
}
