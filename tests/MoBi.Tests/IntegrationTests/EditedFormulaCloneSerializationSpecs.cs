using System.Linq;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Serialization.Xml.Services;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;

namespace MoBi.IntegrationTests
{
   public class When_serializing_a_building_block_with_a_cloned_formula_that_was_edited : ContextForIntegration<IXmlSerializationService>
   {
      private ParameterValuesBuildingBlock _deserializedBuildingBlock;

      protected override void Because()
      {
         var buildingBlock = new ParameterValuesBuildingBlock().WithName("Parameter Values").WithId("ParameterValues");
         var formula = new ExplicitFormula("1+2").WithName("Formula").WithId("Formula").WithOriginId("Origin");
         buildingBlock.AddFormula(formula);

         var clone = IoC.Resolve<ICloneManagerForBuildingBlock>().Clone(formula, new FormulaCache()).WithName("Clone");
         buildingBlock.AddFormula(clone);
         buildingBlock.Add(new ParameterValue { Path = new ObjectPath("Organism", "P"), Formula = clone }.WithId("P"));

         new EditFormulaStringCommand("3+4", clone, buildingBlock).Execute(IoC.Resolve<IMoBiContext>());

         _deserializedBuildingBlock = sut.Deserialize<ParameterValuesBuildingBlock>(sut.SerializeAsString(buildingBlock), new MoBiProject());
      }

      [Observation]
      public void should_keep_the_formula_and_its_edited_clone()
      {
         _deserializedBuildingBlock.FormulaCache.Select(x => x.Name).ShouldOnlyContain("Formula", "Clone");
         _deserializedBuildingBlock.FormulaCache.FindByName("Formula").DowncastTo<ExplicitFormula>().FormulaString.ShouldBeEqualTo("1+2");
         _deserializedBuildingBlock.FormulaCache.FindByName("Clone").DowncastTo<ExplicitFormula>().FormulaString.ShouldBeEqualTo("3+4");
      }

      [Observation]
      public void the_parameter_value_should_still_use_the_edited_clone()
      {
         _deserializedBuildingBlock.Single().Formula.Name.ShouldBeEqualTo("Clone");
      }
   }
}
