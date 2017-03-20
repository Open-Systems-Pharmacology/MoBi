using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_EditParameterNameInBuildingBlockComand : ContextSpecification<EditParameterNameInBuildingBlockCommand>
   {
      private IBuildingBlock _buildingBlock;
      protected IParameter _parameter;
      protected string _newName;
      protected IMoBiContext _context;
      protected string _oldName;

      protected override void Context()
      {
         _oldName = "old Name";
         _parameter = new Parameter { Name = _oldName };
         _newName = "new Name";
         _buildingBlock = A.Fake<IBuildingBlock>();
         _context = A.Fake<IMoBiContext>();
         A.CallTo(() => _context.Get<IParameter>(_parameter.Id)).Returns(_parameter);
         sut = new EditParameterNameInBuildingBlockCommand(_newName, _parameter.Name, _parameter, _buildingBlock);
      }
   }

   public class When_reverting_the_update_of_a_parameter_name : concern_for_EditParameterNameInBuildingBlockComand
   {
      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void the_name_should_be_reverted()
      {
         _parameter.Name.ShouldBeEqualTo(_oldName);
      }
   }

   public class When_updating_the_name_of_a_parameter : concern_for_EditParameterNameInBuildingBlockComand
   {
      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void the_name_should_be_changed()
      {
         _parameter.Name.ShouldBeEqualTo(_newName);
      }
   }
}
