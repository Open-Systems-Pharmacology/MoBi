using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_EditParameterStartValuePathCommand : ContextSpecification<EditParameterStartValuePathCommand>
   {
      protected IMoBiContext _context;
      protected IParameterStartValuesBuildingBlock _buildingBlock;
      protected IParameterStartValue _parameterStartValue;
      protected IObjectPath _path;
      protected override void Context()
      {
         base.Context();
         _context = A.Fake<IMoBiContext>();
         _buildingBlock = new ParameterStartValuesBuildingBlock();
         _parameterStartValue = new ParameterStartValue();
         _path = new ObjectPath("A", "B");
         _parameterStartValue.ContainerPath = _path;
         _parameterStartValue.Name = "Name";
         _buildingBlock.Add(_parameterStartValue);
      }
   }

   public class when_executing_inverse_of_edit_parameter_path : concern_for_EditParameterStartValuePathCommand
   {
      protected override void Context()
      {
         base.Context();
         sut = new EditParameterStartValuePathCommand(_buildingBlock, _parameterStartValue, new ObjectPath("X", "Y", "Z"));

         A.CallTo(() => _context.Get<IParameterStartValuesBuildingBlock>(_buildingBlock.Id)).Returns(_buildingBlock);
      }

      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void should_restore_original_path()
      {
         _parameterStartValue.Path.PathAsString.ShouldBeEqualTo("A|B|Name");
      }
   }

   public class when_appending_new_element_to_container_path : concern_for_EditParameterStartValuePathCommand
   {
      protected override void Context()
      {
         base.Context();
         sut = new EditParameterStartValuePathCommand(_buildingBlock, _parameterStartValue, new ObjectPath("X", "Y", "Z"));
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void path_should_contain_one_more_element()
      {
         _parameterStartValue.Path.Count.ShouldBeEqualTo(4);
      }

      [Observation]
      public void container_path_should_have_new_element_appended()
      {
         _parameterStartValue.Path.PathAsString.ShouldBeEqualTo("X|Y|Z|Name");
      }
   }
}
