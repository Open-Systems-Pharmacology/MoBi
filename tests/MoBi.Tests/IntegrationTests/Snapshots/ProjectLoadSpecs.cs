using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;

namespace MoBi.IntegrationTests.Snapshots
{
   public class When_loading_a_snapshot_ : ContextWithLoadedSnapshot
   {
      public override void GlobalContext()
      {
         base.GlobalContext();
         LoadSnapshot("snapshot");
      }

      [Observation]
      public void the_observed_data_is_classified_in_a_folder()
      {
         _project.AllObservedData.Count.ShouldBeEqualTo(1);
         _project.AllClassifiablesByType<ClassifiableObservedData>().Count(x => x.Id.Equals(_project.AllObservedData.First().Id)).ShouldBeEqualTo(1);
         _project.AllClassificationsByType(ClassificationType.ObservedData).First().Name.ShouldBeEqualTo("observed data folder");
      }

      [Observation]
      public void the_extension_module_is_classified_in_a_folder()
      {
         _project.Modules.Count(x => !x.IsPKSimModule).ShouldBeEqualTo(1);
         _project.AllClassifiablesByType<ClassifiableModule>().Count(x => x.Id.Equals(_project.Modules.First().Id)).ShouldBeEqualTo(1);
         _project.AllClassificationsByType(ClassificationType.Module).First().Name.ShouldBeEqualTo("module folder");
      }

      [Observation]
      public void the_expression_building_block_is_loaded()
      {
         _project.ExpressionProfileCollection.Count.ShouldBeEqualTo(1);
      }


      [Observation]
      public void the_individual_building_block_is_loaded()
      {
         _project.IndividualsCollection.Count.ShouldBeEqualTo(1);
      }
   }
}
