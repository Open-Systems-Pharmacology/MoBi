using MoBi.Engine.Sbml;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using MoBi.IntegrationTests;
using OSPSuite.Core.Domain;
using Parameter = libsbmlcs.Parameter;

namespace MoBi.Core.SBML
{
   public class ParameterImporterTests : ContextForIntegration<ParameterImporter>
   {
      private IFormulaUsable _res;

      protected override void Because()
      {
         var parameter = new Parameter(3, 1);
         parameter.setName("Param");
         parameter.setId("p1");
         parameter.setValue(5);

         _res = sut.CreateParameter(parameter);
      }

      [Observation]
      public void ParameterNotNullTest()
      {
         _res.ShouldNotBeNull();
      }

      [Observation]
      public void ParameterCreationTest()
      {
         _res.Name.ShouldBeEqualTo("p1");
         _res.Id.ShouldNotBeEqualTo("p1");
         _res.Value.ToString().ShouldBeEqualTo("5");
      }
   }
}