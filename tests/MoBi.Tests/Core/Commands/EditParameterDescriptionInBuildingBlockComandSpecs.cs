using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_EditParameterDescriptionInBuildingBlockCommand : ContextSpecification<EditParameterDescriptionInBuildingBlockComand>
   {
      private IBuildingBlock _buildingBlock;
      protected IParameter _parameter;
      protected string _newDescription;
      protected IMoBiContext _context;
      protected string _oldDescription;

      protected override void Context()
      {
         _oldDescription = "old Description";
         _parameter = new Parameter { Description = _oldDescription};
         _newDescription = "new Description";
         _buildingBlock = A.Fake<IBuildingBlock>();
         _context = A.Fake<IMoBiContext>();
         A.CallTo(() => _context.Get<IParameter>(_parameter.Id)).Returns(_parameter);
         sut = new EditParameterDescriptionInBuildingBlockComand(_newDescription, _parameter.Description, _parameter, _buildingBlock);
      }
   }

   public class When_reverting_the_update_of_a_parameter_description : concern_for_EditParameterDescriptionInBuildingBlockCommand
   {
      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void the_description_should_be_reverted()
      {
         _parameter.Description.ShouldBeEqualTo(_oldDescription);
      }
   }

   public class When_updating_the_description_of_a_parameter : concern_for_EditParameterDescriptionInBuildingBlockCommand
   {
      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void the_description_should_be_changed()
      {
         _parameter.Description.ShouldBeEqualTo(_newDescription);
      }
   }
}
