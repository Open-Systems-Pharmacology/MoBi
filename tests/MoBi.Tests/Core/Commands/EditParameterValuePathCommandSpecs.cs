using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_EditParameterValuePathCommand : ContextSpecification<EditParameterValuePathCommand>
   {
      protected IMoBiContext _context;
      protected ParameterValuesBuildingBlock _buildingBlock;
      protected ParameterValue _parameterValue;
      protected ObjectPath _path;
      protected override void Context()
      {
         base.Context();
         _context = A.Fake<IMoBiContext>();
         _buildingBlock = new ParameterValuesBuildingBlock();
         _parameterValue = new ParameterValue();
         _path = new ObjectPath("A", "B");
         _parameterValue.ContainerPath = _path;
         _parameterValue.Name = "Name";
         _buildingBlock.Add(_parameterValue);
      }
   }

   public class When_executing_inverse_of_edit_parameter_path : concern_for_EditParameterValuePathCommand
   {
      protected override void Context()
      {
         base.Context();
         sut = new EditParameterValuePathCommand(_buildingBlock, _parameterValue, new ObjectPath("X", "Y", "Z"));

         A.CallTo(() => _context.Get<ParameterValuesBuildingBlock>(_buildingBlock.Id)).Returns(_buildingBlock);
      }

      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void should_restore_original_path()
      {
         _parameterValue.Path.PathAsString.ShouldBeEqualTo("A|B|Name");
      }
   }

   public class When_appending_new_element_to_container_path : concern_for_EditParameterValuePathCommand
   {
      protected override void Context()
      {
         base.Context();
         sut = new EditParameterValuePathCommand(_buildingBlock, _parameterValue, new ObjectPath("X", "Y", "Z"));
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void path_should_contain_one_more_element()
      {
         _parameterValue.Path.Count.ShouldBeEqualTo(4);
      }

      [Observation]
      public void container_path_should_have_new_element_appended()
      {
         _parameterValue.Path.PathAsString.ShouldBeEqualTo("X|Y|Z|Name");
      }
   }
}
