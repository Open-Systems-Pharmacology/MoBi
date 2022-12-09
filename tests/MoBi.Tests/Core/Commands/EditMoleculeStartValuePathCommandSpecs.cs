using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_EditMoleculeStartValuePathCommand : ContextSpecification<EditMoleculeStartValuePathCommand>
   {
      protected IMoBiContext _context;
      protected IMoleculeStartValuesBuildingBlock _buildingBlock;
      protected MoleculeStartValue _moleculeStartValue;
      protected IObjectPath _path;
      protected override void Context()
      {
         base.Context();
         _context = A.Fake<IMoBiContext>();
         _buildingBlock = new MoleculeStartValuesBuildingBlock();
         _moleculeStartValue = new MoleculeStartValue();
         _path = new ObjectPath("A", "B");
         _moleculeStartValue.ContainerPath = _path;
         _moleculeStartValue.Name = "Name";
         _buildingBlock.Add(_moleculeStartValue);
      }
   }

   public class When_executing_inverse_of_edit_molecule_path : concern_for_EditMoleculeStartValuePathCommand
   {
      protected override void Context()
      {
         base.Context();
         sut = new EditMoleculeStartValuePathCommand(_buildingBlock, _moleculeStartValue, new ObjectPath("X", "Y", "Z"));

         A.CallTo(() => _context.Get<IMoleculeStartValuesBuildingBlock>(_buildingBlock.Id)).Returns(_buildingBlock);
      }

      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void should_restore_original_path()
      {
         _moleculeStartValue.Path.PathAsString.ShouldBeEqualTo("A|B|Name");
      }
   }

   public class When_appending_new_element_to_molecule_container_path : concern_for_EditMoleculeStartValuePathCommand
   {
      protected override void Context()
      {
         base.Context();
         sut = new EditMoleculeStartValuePathCommand(_buildingBlock, _moleculeStartValue, new ObjectPath("X", "Y", "Z"));
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void path_should_contain_one_more_element()
      {
         _moleculeStartValue.Path.Count.ShouldBeEqualTo(4);
      }

      [Observation]
      public void container_path_should_have_new_element_appended()
      {
         _moleculeStartValue.Path.PathAsString.ShouldBeEqualTo("X|Y|Z|Name");
      }
   }
}
