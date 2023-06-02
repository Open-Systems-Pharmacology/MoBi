using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_EditInitialConditionPathCommand : ContextSpecification<EditInitialConditionPathCommand>
   {
      protected IMoBiContext _context;
      protected InitialConditionsBuildingBlock _buildingBlock;
      protected InitialCondition _initialCondition;
      protected ObjectPath _path;
      protected override void Context()
      {
         base.Context();
         _context = A.Fake<IMoBiContext>();
         _buildingBlock = new InitialConditionsBuildingBlock();
         _initialCondition = new InitialCondition();
         _path = new ObjectPath("A", "B");
         _initialCondition.ContainerPath = _path;
         _initialCondition.Name = "Name";
         _buildingBlock.Add(_initialCondition);
      }
   }

   public class When_executing_inverse_of_edit_molecule_path : concern_for_EditInitialConditionPathCommand
   {
      protected override void Context()
      {
         base.Context();
         sut = new EditInitialConditionPathCommand(_buildingBlock, _initialCondition, new ObjectPath("X", "Y", "Z"));

         A.CallTo(() => _context.Get<IBuildingBlock<InitialCondition>>(_buildingBlock.Id)).Returns(_buildingBlock);
      }

      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void should_restore_original_path()
      {
         _initialCondition.Path.PathAsString.ShouldBeEqualTo("A|B|Name");
      }
   }

   public class When_appending_new_element_to_molecule_container_path : concern_for_EditInitialConditionPathCommand
   {
      protected override void Context()
      {
         base.Context();
         sut = new EditInitialConditionPathCommand(_buildingBlock, _initialCondition, new ObjectPath("X", "Y", "Z"));
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void path_should_contain_one_more_element()
      {
         _initialCondition.Path.Count.ShouldBeEqualTo(4);
      }

      [Observation]
      public void container_path_should_have_new_element_appended()
      {
         _initialCondition.Path.PathAsString.ShouldBeEqualTo("X|Y|Z|Name");
      }
   }
}
