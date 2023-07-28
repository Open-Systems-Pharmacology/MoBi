using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Core.Service
{
   public class concern_for_SimulationConfigurationFactory : ContextSpecification<SimulationConfigurationFactory>
   {
      protected ICoreCalculationMethodRepository _calculationMethodRepository;
      protected ICloneManagerForBuildingBlock _cloneManager;
      private IMoBiProjectRetriever _projectRetriever;
      protected SimulationSettings _clonedSimulationSettings;
      private MoBiProject _currentProject;
      protected CoreCalculationMethod _method3;
      protected CoreCalculationMethod _method2;
      protected CoreCalculationMethod _method1;

      protected override void Context()
      {
         _calculationMethodRepository = new CoreCalculationMethodRepository();
         _cloneManager = A.Fake<ICloneManagerForBuildingBlock>();
         _projectRetriever = A.Fake<IMoBiProjectRetriever>();
         
         _clonedSimulationSettings = new SimulationSettings();
         _currentProject = new MoBiProject
         {
            SimulationSettings = new SimulationSettings()
         };
         
         A.CallTo(() => _projectRetriever.CurrentProject).Returns(_currentProject);
         A.CallTo(() => _projectRetriever.Current).Returns(_currentProject);
         A.CallTo(() => _cloneManager.Clone(_currentProject.SimulationSettings)).Returns(_clonedSimulationSettings);

         _method1 = new CoreCalculationMethod();
         _calculationMethodRepository.AddCalculationMethod(_method1);
         _method2 = new CoreCalculationMethod();
         _calculationMethodRepository.AddCalculationMethod(_method2);
         _method3 = new CoreCalculationMethod();
         _calculationMethodRepository.AddCalculationMethod(_method3);

         sut = new SimulationConfigurationFactory(_calculationMethodRepository, _cloneManager, _projectRetriever);
      }
   }

   public class When_creating_a_new_simulation_configuration : concern_for_SimulationConfigurationFactory
   {
      private SimulationConfiguration _simulationConfiguration;

      protected override void Because()
      {
         _simulationConfiguration = sut.Create();
      }

      [Observation]
      public void the_simulation_configuration_should_contain_the_original_calculation_methods()
      {
         _simulationConfiguration.AllCalculationMethods.ShouldOnlyContain(_method1, _method2, _method3);
      }

      [Observation]
      public void the_simulation_configuration_should_have_a_clone_of_project_default_simulation_settings()
      {
         _simulationConfiguration.SimulationSettings.ShouldBeEqualTo(_clonedSimulationSettings);
      }
   }

}
