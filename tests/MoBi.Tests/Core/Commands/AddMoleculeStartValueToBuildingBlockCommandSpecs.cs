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
   public abstract class concern_for_AddMoleculeStartValueToBuildingBlockCommand : ContextSpecification<AddMoleculeStartValueToBuildingBlockCommand>
   {
      protected IMoleculeStartValuesBuildingBlock _buildingBlock;
      protected IMoBiContext _context;

      private IDimension _fakeDimension;
      protected MoleculeStartValue _moleculeStartValue;

      protected override void Context()
      {
         _fakeDimension = A.Fake<IDimension>();
         _context = A.Fake<IMoBiContext>();
         _buildingBlock = new MoleculeStartValuesBuildingBlock();

         _moleculeStartValue = new MoleculeStartValue { Path = new ObjectPath("path1"), Dimension = _fakeDimension, StartValue = -1, DisplayUnit = new Unit("Dimensionless", 1.0, 1) };
         sut = new AddMoleculeStartValueToBuildingBlockCommand(_buildingBlock, _moleculeStartValue);

         A.CallTo(() => _context.Get<IStartValuesBuildingBlock<MoleculeStartValue>>(A<string>._)).Returns(_buildingBlock);
      }
   }

   public class When_addding_molecule_start_value : concern_for_AddMoleculeStartValueToBuildingBlockCommand
   {
      protected override void Context()
      {
         base.Context();
         _moleculeStartValue.Formula = new ExplicitFormula("");
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void formula_added_to_building_block_cache()
      {
         _buildingBlock.FormulaCache.ShouldContain(_moleculeStartValue.Formula);
      }
   }

   public class after_successful_molecule_start_value_import : concern_for_AddMoleculeStartValueToBuildingBlockCommand
   {
      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void molecule_start_value_added_to_building_block()
      {
         _buildingBlock.ShouldContain(_moleculeStartValue);
      }
   }

   public class inverse_command_removes_molecule_start_value_from_building_block : concern_for_AddMoleculeStartValueToBuildingBlockCommand
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
