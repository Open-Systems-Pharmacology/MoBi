using System.Linq;
using System.Xml.Linq;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Presentation.Tasks;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Serialization.Exchange;
using OSPSuite.Core.Serialization.SimModel.Services;
using OSPSuite.Core.Services;
using OSPSuite.Utility;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;

namespace MoBi.IntegrationTests
{
   public abstract class concern_for_saving_a_simulation_to_pkml : ContextWithLoadedProject
   {
      protected IMoBiSimulation _simulation;
      protected IMoBiSimulation _reloadedSimulation;
      protected IMoBiSimulation _simulationAddedToProject;
      protected string _pkmlFile;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _simulation = LoadPKML<SimulationTransfer>("Sim_V12").Simulation.DowncastTo<IMoBiSimulation>();
         ChangeSimulation();

         _pkmlFile = FileHelper.GenerateTemporaryFileName();
         var dialogCreator = A.Fake<IDialogCreator>();
         A.CallTo(() => dialogCreator.AskForFileToSave(A<string>._, A<string>._, A<string>._, A<string>._, A<string>._)).Returns(_pkmlFile);
         var editTasksForSimulation = new EditTasksForSimulation(A.Fake<IInteractionTaskContext>(), IoC.Resolve<ISimulationPersistor>(), dialogCreator,
            A.Fake<IDataRepositoryExportTask>(), A.Fake<IModelReportCreator>(), A.Fake<ISimModelExporter>(), A.Fake<IDimensionFactory>(), A.Fake<IParameterIdentificationSimulationPathUpdater>());
         editTasksForSimulation.Save(_simulation);

         var simulationTransfer = IoC.Resolve<ISerializationTask>().Load<SimulationTransfer>(_pkmlFile);
         _reloadedSimulation = simulationTransfer.Simulation.DowncastTo<IMoBiSimulation>();

         //Adding the simulation to the project clones it, as when a pkml file is loaded in the application
         IoC.Resolve<ISimulationLoader>().AddSimulationToProject(simulationTransfer);
         _simulationAddedToProject = IoC.Resolve<IMoBiContext>().CurrentProject.Simulations.Single();
      }

      protected virtual void ChangeSimulation()
      {
      }

      public override void GlobalCleanup()
      {
         base.GlobalCleanup();
         FileHelper.DeleteFile(_pkmlFile);
      }
   }

   public class When_saving_a_simulation_with_tracked_changes_to_pkml : concern_for_saving_a_simulation_to_pkml
   {
      private IParameter _changedParameter;

      protected override void ChangeSimulation()
      {
         var originalQuantityValueMapper = IoC.Resolve<IQuantityToOriginalQuantityValueMapper>();

         _changedParameter = _simulation.Model.Root.GetAllChildren<IParameter>(x => x.Formula is ConstantFormula).First();
         _simulation.AddOriginalQuantityValue(originalQuantityValueMapper.MapFrom(_changedParameter));
         _changedParameter.Value = _changedParameter.Value * 2 + 1;

         var changedMoleculeAmount = _simulation.Model.Root.GetAllChildren<MoleculeAmount>().First();
         _simulation.AddOriginalQuantityValue(originalQuantityValueMapper.MapFrom(changedMoleculeAmount));
         changedMoleculeAmount.ScaleDivisor = changedMoleculeAmount.ScaleDivisor * 2;

         _simulation.HasUntraceableChanges = true;
      }

      [Observation]
      public void should_restore_the_tracked_changes_when_the_file_is_loaded_in_mobi()
      {
         _reloadedSimulation.OriginalQuantityValues.Select(x => x.Id).ShouldOnlyContain(_simulation.OriginalQuantityValues.Select(x => x.Id).ToArray());

         foreach (var originalQuantityValue in _simulation.OriginalQuantityValues)
         {
            var reloadedQuantityValue = _reloadedSimulation.OriginalQuantityValueFor(originalQuantityValue);
            reloadedQuantityValue.Value.ShouldBeEqualTo(originalQuantityValue.Value);
            reloadedQuantityValue.Dimension.ShouldBeEqualTo(originalQuantityValue.Dimension);
            reloadedQuantityValue.DisplayUnit.Name.ShouldBeEqualTo(originalQuantityValue.DisplayUnit.Name);
         }
      }

      [Observation]
      public void should_restore_the_untraceable_changes_flag_when_the_file_is_loaded_in_mobi()
      {
         _reloadedSimulation.HasUntraceableChanges.ShouldBeTrue();
      }

      [Observation]
      public void should_keep_the_tracked_changes_when_the_loaded_simulation_is_added_to_the_project()
      {
         _simulationAddedToProject.ShouldNotBeEqualTo(_reloadedSimulation);
         _simulationAddedToProject.OriginalQuantityValues.Select(x => x.Id).ShouldOnlyContain(_simulation.OriginalQuantityValues.Select(x => x.Id).ToArray());
         _simulationAddedToProject.HasUntraceableChanges.ShouldBeTrue();
      }

      [Observation]
      public void should_save_the_changed_values_in_the_model()
      {
         var reloadedParameter = _reloadedSimulation.Model.Root.GetAllChildren<IParameter>(x => x.Id == _changedParameter.Id).Single();
         reloadedParameter.Value.ShouldBeEqualTo(_changedParameter.Value);
      }

      [Observation]
      public void should_save_a_file_that_can_be_loaded_as_a_core_simulation_transfer()
      {
         var simulationTransfer = IoC.Resolve<ISimulationPersistor>().Load(_pkmlFile);
         simulationTransfer.Simulation.Model.ShouldNotBeNull();
         simulationTransfer.OriginalQuantityValues.Count.ShouldBeEqualTo(2);
      }
   }

   public class When_saving_a_simulation_without_tracked_changes_to_pkml : concern_for_saving_a_simulation_to_pkml
   {
      [Observation]
      public void should_not_add_tracked_changes_to_the_file()
      {
         XElement.Load(_pkmlFile).Element(nameof(SimulationTransfer.OriginalQuantityValues)).ShouldBeNull();
      }

      [Observation]
      public void should_load_a_simulation_without_tracked_changes_in_mobi()
      {
         _reloadedSimulation.OriginalQuantityValues.ShouldBeEmpty();
         _reloadedSimulation.HasUntraceableChanges.ShouldBeFalse();
      }
   }
}
