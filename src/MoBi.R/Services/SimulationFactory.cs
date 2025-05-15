using System;
using System.Linq;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.R.Domain;
using OSPSuite.Core.Domain;
using OSPSuite.R.Domain;
using OSPSuite.Utility.Extensions;

namespace MoBi.R.Services
{
    public interface ISimulationFactory
    {
        Simulation CreateSimulation(SimulationConfiguration configuration, MoBiProject moBiProject);
    }

    public class SimulationFactory : ISimulationFactory
    {
        private readonly IMoBiContext _context;
        private readonly ISimulationConfigurationFactory _configurationFactory;
        private readonly Core.Domain.Services.ISimulationFactory _simulationFactory;

        public SimulationFactory(ISimulationConfigurationFactory configurationFactory, Core.Domain.Services.ISimulationFactory simulationFactory, IMoBiContext context)
        {
            _configurationFactory = configurationFactory;
            _simulationFactory = simulationFactory;
            _context = context;
        }

        public Simulation CreateSimulation(SimulationConfiguration configuration, MoBiProject moBiProject)
        {
            if (string.IsNullOrWhiteSpace(configuration.SimulationName))
                throw new InvalidOperationException("Simulation name is required");

            var simulationConfiguration = _configurationFactory.Create();

            configuration.ModuleConfigurations.Each(x =>
            {
                var module = _context.CurrentProject.ModuleByName(x.ModuleName);
                simulationConfiguration.AddModuleConfiguration(new OSPSuite.Core.Domain.ModuleConfiguration(module, module.InitialConditionsCollection.FindByName(x.SelectedInitialConditionsName),
                module.ParameterValuesCollection.FindByName(x.SelectedParameterValueName)));
            });

            configuration.ExpressionProfileNames.Each(x =>
            {
                var expressionProfile = _context.CurrentProject.ExpressionProfileCollection.Single(y => y.Name == x);
                simulationConfiguration.AddExpressionProfile(expressionProfile);
            });

            if (!string.IsNullOrWhiteSpace(configuration.IndividualName))
            {
                var individual = moBiProject.IndividualsCollection.FirstOrDefault(x => x.Name == configuration.IndividualName);
                if (individual != null)
                    simulationConfiguration.Individual = individual;
            }

            simulationConfiguration.ShouldValidate = true;

            var simulation = _simulationFactory.CreateSimulationAndValidate(simulationConfiguration, configuration.SimulationName);
            return new Simulation(simulation);
        }
    }
}