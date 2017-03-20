using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.MenusAndBars;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_ChangeMoleculeStartValueNameCommand : ContextSpecification<ChangeMoleculeStartValueNameCommand>
   {
      protected IMoBiContext _context;
      protected MoleculeStartValue _moleculeStartValue;
      protected MoleculeStartValuesBuildingBlock _buildingBlock;

      protected override void Context()
      {
         _context = A.Fake<IMoBiContext>();

         _moleculeStartValue = new MoleculeStartValue {Path = new ObjectPath("Path1", "Path2", "Name")};
         _buildingBlock = new MoleculeStartValuesBuildingBlock { _moleculeStartValue };
         sut = new ChangeMoleculeStartValueNameCommand(
            _buildingBlock,
            _moleculeStartValue.Path,
            "Name2");

         A.CallTo(() => _context.Get<IMoleculeStartValuesBuildingBlock>(_buildingBlock.Id)).Returns(_buildingBlock);
      }
   }

   public class when_changing_name_of_start_value : concern_for_ChangeMoleculeStartValueNameCommand
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

   public class when_changing_name_of_molecule_start_value : concern_for_ChangeMoleculeStartValueNameCommand
   {
      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void name_of_start_value_has_been_changed()
      {
         _moleculeStartValue.MoleculeName.ShouldBeEqualTo("Name2");
      }

      [Observation]
      public void name_of_start_value_becomes_key_for_building_block()
      {
         _buildingBlock[_moleculeStartValue.Path].ShouldNotBeNull();
      }
   }

   public class when_reverting_command_for_rename_of_molecule_start_value : concern_for_ChangeMoleculeStartValueNameCommand
   {
      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void name_change_should_be_reverted()
      {
         _moleculeStartValue.MoleculeName.ShouldBeEqualTo("Name");
      }
   }
}