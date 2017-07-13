using MoBi.Helpers;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Validation;
using MoBi.Presentation.DTO;

namespace MoBi.Presentation
{
   public abstract class concern_for_DTOValuePoint : ContextSpecification<DTOValuePoint>
   {
      private TableFormulaBuilderDTO _tableFormulaBuilderDTO;

      protected override void Context()
      {
         _tableFormulaBuilderDTO = new TableFormulaBuilderDTO();
         var timeDimension = DimensionFactoryForSpecs.Factory.TryGetDimension(DimensionFactoryForSpecs.DimensionNames.Time);

         sut = new DTOValuePoint(_tableFormulaBuilderDTO)
         {
            X = new ValuePointParameterDTO { Value = 0, Dimension = timeDimension, DisplayUnit = timeDimension.DefaultUnit }
         };
         DTOValuePoint anotherDTOValuePoint = new DTOValuePoint(_tableFormulaBuilderDTO)
         {
            X = new ValuePointParameterDTO { Value = 1, Dimension = timeDimension, DisplayUnit = timeDimension.DefaultUnit }
         };
         _tableFormulaBuilderDTO.ValuePoints = new[] { sut, anotherDTOValuePoint };
      }
   }

   public class When_changing_the_value_of_one_of_the_X_to_the_original_value : concern_for_DTOValuePoint
   {
      protected override void Because()
      {
         sut.XValue = 0;
      }

      [Observation]
      public void the_dto_should_fail_validation()
      {
         // That tests that the sut value itself is not considered a repeat when setting the value
         sut.IsValid().ShouldBeTrue();
      }
   }

   public class When_changing_the_value_of_one_of_the_X : concern_for_DTOValuePoint
   {
      protected override void Because()
      {
         sut.XValue = 1;
      }

      [Observation]
      public void the_dto_should_fail_validation()
      {
         sut.IsValid().ShouldBeFalse();
      }
   }
}
