using System.Collections.Generic;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.IntegrationTests;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Container;

namespace MoBi.Presentation.Tasks
{
   public class concern_for_SimulationConfigurationTask : ContextForIntegration<SimulationConfigurationTask>
   {
      private ICloneManagerForBuildingBlock _cloneManager;
      protected ISimulationConfigurationFactory _simulationConfigurationFactory;

      protected override void Context()
      {
         _cloneManager = IoC.Resolve<ICloneManagerForBuildingBlock>();
         _simulationConfigurationFactory = IoC.Resolve<ISimulationConfigurationFactory>();
         var context = IoC.Resolve<IMoBiContext>();
         context.NewProject();

         sut = new SimulationConfigurationTask(_simulationConfigurationFactory, _cloneManager);
      }
   }

   public class When_updating_building_simulation_configuration_from_components : concern_for_SimulationConfigurationTask
   {
      private IReadOnlyList<ExpressionProfileBuildingBlock> _selectedExpressions;
      private IndividualBuildingBlock _selectedIndividual;
      private IReadOnlyList<ModuleConfiguration> _moduleConfigurations;
      private SimulationConfiguration _simulationConfiguration;

      protected override void Context()
      {
         base.Context();
         var objectBaseFactory = IoC.Resolve<IObjectBaseFactory>();
         _simulationConfiguration = _simulationConfigurationFactory.Create();
         var expressionProfileBuildingBlock = objectBaseFactory.Create<ExpressionProfileBuildingBlock>().WithName("expression1");
         expressionProfileBuildingBlock.Type = ExpressionTypes.MetabolizingEnzyme;
         _selectedExpressions = new[] { expressionProfileBuildingBlock };
         _moduleConfigurations = new[] { new ModuleConfiguration(objectBaseFactory.Create<Module>().WithName("module1")) };
         _selectedIndividual = objectBaseFactory.Create<IndividualBuildingBlock>().WithName("individual1");
      }

      protected override void Because()
      {
         sut.UpdateFrom(_simulationConfiguration, _moduleConfigurations, _selectedIndividual, _selectedExpressions);
      }

      [Observation] 
      public void the_simulation_configuration_should_reference_the_template_building_blocks()
      {
         _simulationConfiguration.ExpressionProfiles.Count.ShouldBeEqualTo(1);
         _simulationConfiguration.ExpressionProfiles[0].ShouldBeEqualTo(_selectedExpressions[0]);

         _simulationConfiguration.ModuleConfigurations.Count.ShouldBeEqualTo(1);
         _simulationConfiguration.ModuleConfigurations[0].ShouldNotBeEqualTo(_moduleConfigurations[0]);
         _simulationConfiguration.ModuleConfigurations[0].Module.ShouldBeEqualTo(_moduleConfigurations[0].Module);

         _simulationConfiguration.Individual.ShouldBeEqualTo(_selectedIndividual);
         _simulationConfiguration.Individual.Name.ShouldBeEqualTo(_selectedIndividual.Name);
      }
   }
}
