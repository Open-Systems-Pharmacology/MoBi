using System.Linq;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Container;

namespace MoBi.IntegrationTests.Snapshots
{
   public class When_loading_a_snapshot : ContextWithLoadedSnapshot
   {
      public override void GlobalContext()
      {
         base.GlobalContext();
         var starter = IoC.Resolve<IPKSimStarter>();

         var module = new Module
         {
            IsPKSimModule = true,
            Name = "Henrist oral Hot stage extrusion as table"
         };

         A.CallTo(() => starter.LoadModuleFromSnapshot(A<string>._)).Returns(module);

         // EntityValidationTask is a fake
         var validationTask = IoC.Resolve<IEntityValidationTask>();
         A.CallTo(() => validationTask.Validate(A<MoBiSimulation>._)).Returns(true);

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
         _project.AllClassifiablesByType<ClassifiableModule>().Count(x => x.Id.Equals(_project.Modules.First(m => !m.IsPKSimModule).Id)).ShouldBeEqualTo(1);
         _project.AllClassificationsByType(ClassificationType.Module).First().Name.ShouldBeEqualTo("module folder");
      }

      [Observation]
      public void the_pksim_module_is_loaded()
      {
         _project.Modules.Count(x => x.IsPKSimModule).ShouldBeEqualTo(1);
      }

      [Observation]
      public void the_parameter_identification_is_classified_in_a_folder()
      {
         _project.AllParameterIdentifications.Count.ShouldBeEqualTo(1);
         _project.AllClassifiablesByType<ClassifiableParameterIdentification>().Count(x => x.Id.Equals(_project.AllParameterIdentifications.First().Id)).ShouldBeEqualTo(1);
         _project.AllClassificationsByType(ClassificationType.ParameterIdentification).First().Name.ShouldBeEqualTo("pi folder");
      }

      [Observation]
      public void the_expression_building_blocks_are_loaded()
      {
         _project.ExpressionProfileCollection.Count.ShouldBeEqualTo(2);
      }

      [Observation]
      public void the_individual_building_blocks_are_loaded()
      {
         _project.IndividualsCollection.Count.ShouldBeEqualTo(1);
      }
   }
}