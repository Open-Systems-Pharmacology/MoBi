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
      protected CoreCalculationMethod _clonedMethod1, _clonedMethod2, _clonedMethod3;

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

         var method1 = new CoreCalculationMethod();
         _calculationMethodRepository.AddCalculationMethod(method1);
         var method2 = new CoreCalculationMethod();
         _calculationMethodRepository.AddCalculationMethod(method2);
         var method3 = new CoreCalculationMethod();
         _calculationMethodRepository.AddCalculationMethod(method3);

         _clonedMethod1 = new CoreCalculationMethod();
         _clonedMethod2 = new CoreCalculationMethod();
         _clonedMethod3 = new CoreCalculationMethod();
         A.CallTo(() => _cloneManager.Clone(method1)).Returns(_clonedMethod1);
         A.CallTo(() => _cloneManager.Clone(method2)).Returns(_clonedMethod2);
         A.CallTo(() => _cloneManager.Clone(method3)).Returns(_clonedMethod3);

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
      public void the_simulation_configuration_should_contain_clones_of_the_calculation_methods()
      {
         _simulationConfiguration.AllCalculationMethods.ShouldOnlyContain(_clonedMethod1, _clonedMethod2, _clonedMethod3);
      }

      [Observation]
      public void the_simulation_configuration_should_have_a_clone_of_project_default_simulation_settings()
      {
         _simulationConfiguration.SimulationSettings.ShouldBeEqualTo(_clonedSimulationSettings);
      }
   }

}
