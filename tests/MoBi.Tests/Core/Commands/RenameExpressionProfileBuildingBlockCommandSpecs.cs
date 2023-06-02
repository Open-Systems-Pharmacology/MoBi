using System.Linq;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class concern_for_RenameExpressionProfileBuildingBlockCommand : ContextSpecification<RenameExpressionProfileBuildingBlockCommand>
   {
      protected ExpressionProfileBuildingBlock _buildingBlock;
      protected IMoBiContext _context;
      private readonly IObjectPathFactory _objectPathFactory = new ObjectPathFactory(A.Fake<IAliasCreator>());

      protected override void Context()
      {
         _context = A.Fake<IMoBiContext>();
         A.CallTo(() => _context.ObjectPathFactory).Returns(_objectPathFactory);
         
         _buildingBlock = new ExpressionProfileBuildingBlock().WithName("OldMolecule|OldSpecies|OldCategory");
         _buildingBlock.Add(new ExpressionParameter { Path = new ObjectPath("A", "B", "C", "OldMolecule", "Name") });
         _buildingBlock.Add(new ExpressionParameter { Path = new ObjectPath("A", "B", "C", "OldMolecule", "Name2") });
         _buildingBlock.Id = "theID";
         
         A.CallTo(() => _context.Get<ExpressionProfileBuildingBlock>(_buildingBlock.Id)).Returns(_buildingBlock);
         A.CallTo(() => _context.Get<IObjectBase>(_buildingBlock.Id)).Returns(_buildingBlock);

         sut = new RenameExpressionProfileBuildingBlockCommand(_buildingBlock, "NewMolecule|NewSpecies|NewCategory", null);
      }
   }

   public class When_reversing_the_rename_command : concern_for_RenameExpressionProfileBuildingBlockCommand
   {
      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void the_building_block_name_should_revert()
      {
         _buildingBlock.Name.ShouldBeEqualTo("OldMolecule|OldSpecies|OldCategory");
      }

      [Observation]
      public void the_parameters_should_have_renamed_molecule()
      {
         _buildingBlock.Any<ExpressionParameter>(x => x.Path.Contains("NewMolecule")).ShouldBeFalse();
         _buildingBlock.All<ExpressionParameter>(x => x.Path.Contains("OldMolecule")).ShouldBeTrue();
      }
   }
   
   public class When_executing_the_rename_command : concern_for_RenameExpressionProfileBuildingBlockCommand
   {
      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void the_building_block_name_should_change()
      {
         _buildingBlock.Name.ShouldBeEqualTo("NewMolecule|NewSpecies|NewCategory");
      }

      [Observation]
      public void the_parameters_should_have_renamed_molecule()
      {
         _buildingBlock.Any<ExpressionParameter>(x => x.Path.Contains("OldMolecule")).ShouldBeFalse();
         _buildingBlock.All<ExpressionParameter>(x => x.Path.Contains("NewMolecule")).ShouldBeTrue();
      }
   }
}
