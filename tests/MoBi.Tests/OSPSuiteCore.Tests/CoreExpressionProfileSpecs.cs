using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.OSPSuiteCore.Tests
{
   public class concern_for_CoreExpressionProfile : ContextSpecification<CoreExpressionProfile>
   {
      protected override void Context()
      {
         sut = new CoreExpressionProfile();
      }
   }

   public class when_setting_the_properties_of_the_building_block : concern_for_CoreExpressionProfile
   {
      protected override void Because()
      {
         sut.MoleculeName = "Molecule";
         sut.Species = "Species";
         sut.Category = "Phenotype";
      }

      [Observation]
      public void the_name_should_set_the_category_of_the_building_block()
      {
         sut.Name.ShouldBeEqualTo("Molecule|Species|Phenotype");
      }
   }

   public class when_setting_the_name_of_the_building_block : concern_for_CoreExpressionProfile
   {
      protected override void Because()
      {
         sut.Name = "Molecule|Species|Phenotype";
      }

      [Observation] 
      public void the_name_should_set_the_category_of_the_buidling_block()
      {
         sut.Category.ShouldBeEqualTo("Phenotype");
      }
   }
}
