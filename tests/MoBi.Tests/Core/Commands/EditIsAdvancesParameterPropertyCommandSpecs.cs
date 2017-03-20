using FakeItEasy;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_EditIsAdvancesParameterPropertyCommandSpecs : ContextSpecification<EditIsAdvancesParameterPropertyCommand>
   {
      protected IParameter _testParameter;
      protected bool _newValue;

      protected override void Context()
      {
         _testParameter = A.Fake<IParameter>().WithName("Test").WithId("Test");
         _newValue = true;
         _testParameter.Visible = _newValue;
         sut = new EditIsAdvancesParameterPropertyCommand(_testParameter,_newValue,A.Fake<IBuildingBlock>());
      }
   }

   class When_executing_the_EditIsAdvancesParameterPropertyCommand : concern_for_EditIsAdvancesParameterPropertyCommandSpecs
   {
      protected override void Because()
      {
         sut.Execute(A.Fake<IMoBiContext>());
      }
      [Observation]
      public void should_set_the_visible_property_of_parameter_to_inverse_newValue()
      {
         _testParameter.Visible.ShouldBeEqualTo(!_newValue);
      }
   }
}	