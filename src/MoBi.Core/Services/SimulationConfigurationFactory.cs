using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Extensions;

namespace MoBi.Core.Services
{
   public interface ISimulationConfigurationFactory
   {
      /// <summary>
      ///    Creates a new SimulationConfiguration with default SimulationSettings and calculation methods from the project
      /// </summary>
      SimulationConfiguration Create();

      /// <summary>
      ///    Creates a new SimulationConfiguration from the current project modules and building blocks based on the given
      ///    configuration.
      /// </summary>
      SimulationConfiguration CreateFromProjectTemplatesBasedOn(SimulationConfiguration configuration);
   }

   public class SimulationConfigurationFactory : ISimulationConfigurationFactory
   {
      private readonly ICoreCalculationMethodRepository _calculationMethodRepository;
      private readonly ICloneManagerForBuildingBlock _cloneManager;
      private readonly IMoBiProjectRetriever _projectRetriever;
      private readonly ITemplateResolverTask _templateResolverTask;

      public SimulationConfigurationFactory(ICoreCalculationMethodRepository calculationMethodRepository,
         ICloneManagerForBuildingBlock cloneManager,
         IMoBiProjectRetriever projectRetriever,
         ITemplateResolverTask templateResolverTask)
      {
         _calculationMethodRepository = calculationMethodRepository;
         _cloneManager = cloneManager;
         _projectRetriever = projectRetriever;
         _templateResolverTask = templateResolverTask;
      }

      public SimulationConfiguration Create()
      {
         var simulationConfiguration = new SimulationConfiguration
         {
            SimulationSettings = _cloneManager.Clone(_projectRetriever.Current.SimulationSettings)
         };

         _calculationMethodRepository.All().Each(simulationConfiguration.AddCalculationMethod);

         return simulationConfiguration;
      }

      public SimulationConfiguration CreateFromProjectTemplatesBasedOn(SimulationConfiguration configuration)
      {
         var simulationConfiguration = Create();
         simulationConfiguration.CopyPropertiesFrom(configuration);
         simulationConfiguration.SimulationSettings = _cloneManager.Clone(configuration.SimulationSettings);

         configuration.ModuleConfigurations.Each(moduleConfiguration => { simulationConfiguration.AddModuleConfiguration(templateModuleConfigurationFor(moduleConfiguration)); });

         simulationConfiguration.Individual = templateBuildingBlockFor(configuration.Individual);

         configuration.ExpressionProfiles.Each(x => { simulationConfiguration.AddExpressionProfile(templateBuildingBlockFor(x)); });

         return simulationConfiguration;
      }

      private ModuleConfiguration templateModuleConfigurationFor(ModuleConfiguration moduleConfiguration)
      {
         return new ModuleConfiguration(_templateResolverTask.TemplateModuleFor(moduleConfiguration.Module),
            templateBuildingBlockFor(moduleConfiguration.SelectedInitialConditions),
            templateBuildingBlockFor(moduleConfiguration.SelectedParameterValues));
      }

      private TBuildingBlock templateBuildingBlockFor<TBuildingBlock>(TBuildingBlock buildingBlock) where TBuildingBlock : class, IBuildingBlock
      {
         return _templateResolverTask.TemplateBuildingBlockFor(buildingBlock) as TBuildingBlock;
      }
   }
}