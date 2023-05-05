using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_RemoveMoleculeStartValueFromBuildingBlockCommand : ContextSpecification<RemoveMoleculeStartValueFromBuildingBlockCommand>
   {
      protected InitialConditionsBuildingBlock _buildingBlock;
      protected IMoBiContext _context;

      private IDimension _fakeDimension;
      protected InitialCondition _msv;

      protected override void Context()
      {
         _fakeDimension = A.Fake<IDimension>();
         _context = A.Fake<IMoBiContext>();
         _buildingBlock = new InitialConditionsBuildingBlock();

         _msv = new InitialCondition { Path= new ObjectPath("path1"), Dimension = _fakeDimension, StartValue = -1, DisplayUnit = new Unit("Dimensionless", 1.0, 1) };
         _buildingBlock.Add(_msv);
         sut = new RemoveMoleculeStartValueFromBuildingBlockCommand(_buildingBlock, _msv.Path);

         A.CallTo(() => _context.Get<IStartValuesBuildingBlock<InitialCondition>>(_buildingBlock.Id)).Returns(_buildingBlock);
      }
   }

   public class When_removing_molecule_start_value_from_path : concern_for_RemoveMoleculeStartValueFromBuildingBlockCommand
   {
      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void molecule_start_value_should_not_be_contained_in_building_block()
      {
         _buildingBlock.ShouldBeEmpty();
      }
   }

   public class When_retrieving_the_inverse_command : concern_for_RemoveMoleculeStartValueFromBuildingBlockCommand
   {
      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void msv_should_be_added_to_building_block()
      {
         _buildingBlock.ShouldContain(_msv);
      }
   }
}
