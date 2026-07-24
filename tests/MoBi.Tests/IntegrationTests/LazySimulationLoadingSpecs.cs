using System;
using System.IO;
using System.Linq;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Repository;
using MoBi.Presentation.Tasks;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;

namespace MoBi.IntegrationTests
{
   // Existing test project files predate the current project version, so their simulations always load eagerly
   // (older content is converted, not deferred). To exercise the deferral, load such a project, save it (which
   // re-serializes the simulations at the current version) and reload it: the reloaded simulations are then
   // current-version and are loaded as deferred shells. This also validates the load -> save -> reload round-trip.
   public abstract class ContextWithReloadedCurrentVersionProject : ContextWithLoadedProject
   {
      protected IMoBiContext _context;
      protected ISimulationContentRepository _contentRepository;
      protected MoBiProject _project;
      private string _tempFile;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _context = IoC.Resolve<IMoBiContext>();
         _contentRepository = IoC.Resolve<ISimulationContentRepository>();
         var serializationTask = IoC.Resolve<ISerializationTask>();

         LoadProject("PK_Manual_Diclofenac");

         _tempFile = Path.Combine(Path.GetTempPath(), $"mobi_lazy_{Guid.NewGuid()}.mbp3");
         _context.CurrentProject.FilePath = _tempFile;
         serializationTask.SaveProject();
         serializationTask.CloseProject();

         serializationTask.LoadProject(_tempFile);
         _project = _context.CurrentProject;
      }

      public override void GlobalCleanup()
      {
         base.GlobalCleanup();
         try
         {
            if (_tempFile != null && File.Exists(_tempFile))
               File.Delete(_tempFile);
         }
         catch
         {
            // best effort temp cleanup
         }
      }
   }

   public class When_loading_a_current_version_project_that_contains_simulations : ContextWithReloadedCurrentVersionProject
   {
      [Observation]
      public void should_have_loaded_the_simulations()
      {
         _project.Simulations.Count.ShouldBeGreaterThan(0);
      }

      [Observation]
      public void should_defer_the_construction_of_every_simulation_model()
      {
         _project.Simulations.Each(simulation =>
         {
            //the compressed content is retained so the model can be built on demand
            _contentRepository.Contains(simulation.Id).ShouldBeTrue();
            //and the model itself is not yet constructed
            simulation.IsModelLoaded.ShouldBeFalse();
         });
      }

      [Observation]
      public void should_not_have_registered_the_deferred_simulations_in_the_object_repository()
      {
         //a deferred shell is not registered until its model is materialized
         _project.Simulations.Each(simulation => _context.Get<IObjectBase>(simulation.Id).ShouldBeNull());
      }
   }

   public class When_accessing_the_model_of_a_deferred_simulation : ContextWithReloadedCurrentVersionProject
   {
      private IMoBiSimulation _accessedSimulation;

      protected override void Because()
      {
         _accessedSimulation = _project.Simulations.First();
         var unused = _accessedSimulation.Model;
      }

      [Observation]
      public void should_build_the_model_on_first_access()
      {
         _accessedSimulation.Model.ShouldNotBeNull();
         _accessedSimulation.Model.Root.ShouldNotBeNull();
         _accessedSimulation.IsModelLoaded.ShouldBeTrue();
      }

      [Observation]
      public void should_register_the_materialized_model_so_its_entities_are_resolvable_by_id()
      {
         _context.Get<IObjectBase>(_accessedSimulation.Model.Root.Id).ShouldNotBeNull();
      }

      [Observation]
      public void should_leave_the_models_of_the_other_simulations_deferred()
      {
         _project.Simulations
            .Where(x => !ReferenceEquals(x, _accessedSimulation))
            .Each(x => x.IsModelLoaded.ShouldBeFalse());
      }
   }
}
