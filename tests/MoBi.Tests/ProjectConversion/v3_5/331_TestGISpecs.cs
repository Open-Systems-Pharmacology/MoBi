using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.IntegrationTests;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.ProjectConversion.v3_5
{
   public class When_converting_the_331_TestGI_project : ContextWithLoadedProject
   {
      private IMoBiProject _project;
      private IMoBiSpatialStructure _spatialStructure;
      private IContainer _organism;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _project = LoadProject("331_TestGI");
         _spatialStructure = _project.SpatialStructureCollection.First();
         _organism = _spatialStructure.TopContainers.FindByName(Constants.ORGANISM);
      }


      [Observation]
      public void should_have_created_the_parameter_for_surface_area()
      {
         var aeff = _organism.EntityAt<IParameter>("Lumen", "Duodenum","Effective surface area");
         aeff.ShouldNotBeNull();
         aeff.Formula.IsExplicit().ShouldBeTrue();
         aeff.Formula.DowncastTo<ExplicitFormula>().FormulaString.ShouldBeEqualTo("Ageom * AeffFactor * AeffVariabilityFactor");
      }

      [Observation]
      public void should_have_added_the_variability_parameter()
      {
         var factor = _organism.EntityAt<IDistributedParameter>("Lumen", "Effective surface area variability factor");
         factor.ShouldNotBeNull();
         factor.EntityAt<IParameter>(Constants.Distribution.GEOMETRIC_DEVIATION).Value.ShouldBeEqualTo(1.6);
         factor.EntityAt<IParameter>(Constants.Distribution.PERCENTILE).Value.ShouldBeEqualTo(0.5);
         factor.EntityAt<IParameter>(Constants.Distribution.MEAN).Value.ShouldBeEqualTo(1);
      }
   }
}