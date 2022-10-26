using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Model.Diagram;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Core
{
   public abstract class concern_for_MoBiSimulation : ContextSpecification<MoBiSimulation>
   {
      protected override void Context()
      {
         sut = new MoBiSimulation();
      }
   }

   public class When_setting_the_results_for_the_simulation : concern_for_MoBiSimulation
   {
      protected override void Context()
      {
         base.Context();
         sut.Creation.Version = "a version";
         sut.ResultsDataRepository = A.Fake<DataRepository>();
         sut.Creation.Version = "another version";
      }

      protected override void Because()
      {
         sut.ResultsDataRepository = A.Fake<DataRepository>();
      }

      [Observation]
      public void the_results_should_be_considered_up_to_date()
      {
         sut.HasUpToDateResults.ShouldBeTrue();
      }
   }

   public class When_updating_properties_from_source_simulation : concern_for_MoBiSimulation
   {
      private ICloneManager _cloneManager;
      private MoBiSimulation _moBiSimulation;
      private ISimulationDiagramManager _simulationDiagramManager;
      protected override void Context()
      {
         base.Context();
         _cloneManager = A.Fake<ICloneManager>();
         _simulationDiagramManager = A.Fake<ISimulationDiagramManager>();
         _moBiSimulation = new MoBiSimulation {DiagramManager = _simulationDiagramManager};
      }

      protected override void Because()
      {
         sut.UpdatePropertiesFrom(_moBiSimulation, _cloneManager);
      }

      [Observation]
      public void the_source_diagram_manager_creates_new_diagram_manager_for_target()
      {
         A.CallTo(() => _simulationDiagramManager.Create()).MustHaveHappened();
      }
   }

   public class When_removing_analyses_from_the_simulation : concern_for_MoBiSimulation
   {
      private ISimulationAnalysis _simulationAnalysis;

      protected override void Context()
      {
         base.Context();
         _simulationAnalysis = A.Fake<ISimulationAnalysis>();
         sut.AddAnalysis(_simulationAnalysis);
      }

      protected override void Because()
      {
         sut.RemoveAnalysis(_simulationAnalysis);
      }

      [Observation]
      public void the_simulation_must_indicate_it_has_changed()
      {
         sut.HasChanged.ShouldBeTrue();
      }
   }

   public class When_adding_new_analyses_to_the_simulation : concern_for_MoBiSimulation
   {
      protected override void Because()
      {
         sut.AddAnalysis(A.Fake<ISimulationAnalysis>());
      }

      [Observation]
      public void the_simulation_must_indicate_it_has_changed()
      {
         sut.HasChanged.ShouldBeTrue();
      }
   }
}
