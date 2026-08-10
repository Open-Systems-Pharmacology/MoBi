using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Repository;
using MoBi.Presentation.Tasks;
using NUnit.Framework;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;

namespace MoBi.IntegrationTests
{
   // Prepares a current-version project file: an existing (older-version) project is loaded and saved once, so
   // the saved file is at the current version. Tests then load that file (its simulations come back as deferred
   // shells) and exercise saving. Uses [Test] methods so each scenario runs deterministically.
   internal abstract class ContextForSimulationSave : ContextWithLoadedProject
   {
      protected IMoBiContext _context;
      protected ISimulationContentRepository _contentRepository;
      protected ISerializationTask _serializationTask;
      protected string _currentVersionFile;
      private readonly List<string> _tempFiles = new List<string>();

      public override void GlobalContext()
      {
         base.GlobalContext();
         _context = IoC.Resolve<IMoBiContext>();
         _contentRepository = IoC.Resolve<ISimulationContentRepository>();
         _serializationTask = IoC.Resolve<ISerializationTask>();

         LoadProject("PK_Manual_Diclofenac");
         _currentVersionFile = newTempFile();
         _context.CurrentProject.FilePath = _currentVersionFile;
         _serializationTask.SaveProject();
         _serializationTask.CloseProject();
      }

      protected string newTempFile()
      {
         var file = Path.Combine(Path.GetTempPath(), $"mobi_save_{Guid.NewGuid()}.mbp3");
         _tempFiles.Add(file);
         return file;
      }

      // Loads the prepared current-version file; its simulations are deferred shells.
      protected MoBiProject loadCurrentVersionProject()
      {
         _serializationTask.LoadProject(_currentVersionFile);
         return _context.CurrentProject;
      }

      protected MoBiProject saveToNewFileAndReload()
      {
         var saveFile = newTempFile();
         _context.CurrentProject.FilePath = saveFile;
         _serializationTask.SaveProject();
         _serializationTask.CloseProject();
         _serializationTask.LoadProject(saveFile);
         return _context.CurrentProject;
      }

      public override void GlobalCleanup()
      {
         base.GlobalCleanup();
         _tempFiles.Each(tryDelete);
      }

      private static void tryDelete(string file)
      {
         try
         {
            if (File.Exists(file))
               File.Delete(file);
         }
         catch
         {
            // best effort temp cleanup
         }
      }
   }

   internal class When_saving_a_project_whose_simulations_were_never_materialized : ContextForSimulationSave
   {
      [Test]
      public void should_not_materialize_any_model_on_save_and_should_round_trip_the_models()
      {
         var project = loadCurrentVersionProject();
         project.Simulations.Count.ShouldBeGreaterThan(0);
         project.Simulations.Each(s => s.IsModelLoaded.ShouldBeFalse());

         var saveFile = newTempFile();
         _context.CurrentProject.FilePath = saveFile;
         _serializationTask.SaveProject();

         // saving must not have built any model
         project.Simulations.Each(s => s.IsModelLoaded.ShouldBeFalse());

         _serializationTask.CloseProject();
         _serializationTask.LoadProject(saveFile);
         var reloaded = _context.CurrentProject.Simulations.First();

         // still deferred after reopening, and the model still builds correctly (bytes preserved)
         reloaded.IsModelLoaded.ShouldBeFalse();
         reloaded.Model.Root.ShouldNotBeNull();
      }
   }

   internal class When_saving_a_changed_simulation_whose_model_was_never_materialized : ContextForSimulationSave
   {
      private const string _piWorkingDirectory = "changed-without-materializing-the-model";

      [Test]
      public void should_reuse_the_retained_model_bytes_instead_of_building_the_model()
      {
         var project = loadCurrentVersionProject();
         var simulation = project.Simulations.First().DowncastTo<MoBiSimulation>();
         var simulationId = simulation.Id;

         // a change that does NOT touch the model
         simulation.ParameterIdentificationWorkingDirectory = _piWorkingDirectory;
         simulation.HasChanged = true;

         var reloaded = saveToNewFileAndReload().Simulations.First(x => x.Id == simulationId).DowncastTo<MoBiSimulation>();

         // saving the changed simulation must not have built its model
         simulation.IsModelLoaded.ShouldBeFalse();
         // the non-model change is persisted
         reloaded.ParameterIdentificationWorkingDirectory.ShouldBeEqualTo(_piWorkingDirectory);
         // and the (unchanged) model is preserved and still builds after reopening
         reloaded.Model.Root.ShouldNotBeNull();
      }
   }
}
