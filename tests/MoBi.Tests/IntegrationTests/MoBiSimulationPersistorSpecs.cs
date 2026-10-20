using System.Linq;
using System.Xml.Linq;
using MoBi.Core.Domain.Model;
using MoBi.Core.Serialization.Exchange;
using MoBi.Core.Serialization.Xml;
using MoBi.Core.Services;
using MoBi.Presentation.Tasks;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Serialization.Exchange;
using OSPSuite.Utility;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;

namespace MoBi.IntegrationTests
{
   public abstract class concern_for_MoBiSimulationPersistor : ContextWithLoadedProject
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
         IoC.Resolve<IMoBiSimulationPersistor>().Save(_simulation, _pkmlFile);

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

   public class When_saving_a_simulation_with_tracked_changes_to_pkml : concern_for_MoBiSimulationPersistor
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
      public void should_save_a_file_that_can_still_be_loaded_as_a_core_simulation()
      {
         var simulationTransfer = IoC.Resolve<ISimulationPersistor>().Load(_pkmlFile);
         simulationTransfer.Simulation.Model.ShouldNotBeNull();
      }
   }

   public class When_saving_a_simulation_without_tracked_changes_to_pkml : concern_for_MoBiSimulationPersistor
   {
      [Observation]
      public void should_not_add_tracked_changes_to_the_file()
      {
         var simulationElement = XElement.Load(_pkmlFile).Element(Constants.Serialization.SIMULATION);
         simulationElement.Element(SerializationConstants.OriginalQuantityValues).ShouldBeNull();
         simulationElement.Attribute(SerializationConstants.HasUntraceableChanges).ShouldBeNull();
      }

      [Observation]
      public void should_load_a_simulation_without_tracked_changes_in_mobi()
      {
         _reloadedSimulation.OriginalQuantityValues.ShouldBeEmpty();
         _reloadedSimulation.HasUntraceableChanges.ShouldBeFalse();
      }
   }
}
