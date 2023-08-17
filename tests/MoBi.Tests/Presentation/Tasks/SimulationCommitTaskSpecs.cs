using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Helper;
using MoBi.Core.Services;
using MoBi.Helpers;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Presentation.Tasks
{
   public class concern_for_SimulationCommitTask : ContextSpecification<SimulationCommitTask>
   {
      protected MoBiSimulation _simulationWithChanges;
      protected INameCorrector _nameCorrector;
      protected IInitialConditionsCreator _initialConditionsCreator;
      protected IParameterValuesCreator _parameterValuesCreator;
      protected IEntitiesInSimulationRetriever _entitiesInSimulationRetriever;
      protected ITemplateResolverTask _templateResolverTask;
      protected IMoBiContext _context;
      protected ModuleConfiguration _moduleConfiguration;
      protected Module _module;

      protected override void Context()
      {
         base.Context();
         _simulationWithChanges = new MoBiSimulation();
         _module = new Module();
         _moduleConfiguration = new ModuleConfiguration(_module);
         _simulationWithChanges.Configuration = new SimulationConfiguration();
         _simulationWithChanges.Configuration.AddModuleConfiguration(_moduleConfiguration);

         _context = A.Fake<IMoBiContext>();
         _templateResolverTask = A.Fake<ITemplateResolverTask>();
         _entitiesInSimulationRetriever = A.Fake<IEntitiesInSimulationRetriever>();
         _initialConditionsCreator = A.Fake<IInitialConditionsCreator>();
         _parameterValuesCreator = A.Fake<IParameterValuesCreator>();
         _nameCorrector = A.Fake<INameCorrector>();

         sut = new SimulationCommitTask(_context, _templateResolverTask, _entitiesInSimulationRetriever, _initialConditionsCreator, _parameterValuesCreator, _nameCorrector, new ObjectTypeResolver());
      }
   }

   public class When_committing_from_a_simulation_without_any_changes : concern_for_SimulationCommitTask
   {
      private Module _projectModule;

      protected override void Context()
      {
         base.Context();

         _projectModule = new Module();
         A.CallTo(() => _templateResolverTask.TemplateModuleFor(_module)).Returns(_projectModule);
         var parameterPathCache = new PathCacheForSpecs<Parameter>
         {
            new Parameter { Name = "name" }
         };

         var moleculeAmount = new MoleculeAmount { Name = "name" };
         new Container().WithName("top").Add(moleculeAmount);
         var initialConditionsPathCache = new PathCacheForSpecs<MoleculeAmount>
         {
            moleculeAmount
         };

         A.CallTo(() => _entitiesInSimulationRetriever.EntitiesFrom<Parameter>(_simulationWithChanges)).Returns(parameterPathCache);
         A.CallTo(() => _entitiesInSimulationRetriever.EntitiesFrom<MoleculeAmount>(_simulationWithChanges)).Returns(initialConditionsPathCache);
      }

      protected override void Because()
      {
         sut.CommitSimulationChanges(_simulationWithChanges);
      }

      [Observation]
      public void new_building_blocks_are_created()
      {
         _projectModule.ParameterValuesCollection.Count.ShouldBeEqualTo(0);
         _projectModule.InitialConditionsCollection.Count.ShouldBeEqualTo(0);
      }
   }

   public class When_committing_to_configuration_without_selected_parameter_values : concern_for_SimulationCommitTask
   {
      private Module _projectModule;

      protected override void Context()
      {
         base.Context();
         _simulationWithChanges.AddOriginalQuantityValue(new ParameterValue { Path = new ObjectPath("name"), Value = 1.0 });
         _simulationWithChanges.AddOriginalQuantityValue(new ParameterValue { Path = new ObjectPath("top", "name"), Value = 1.0 });

         _projectModule = new Module();
         A.CallTo(() => _templateResolverTask.TemplateModuleFor(_module)).Returns(_projectModule);
         var parameterPathCache = new PathCacheForSpecs<Parameter>
         {
            new Parameter { Name = "name" }
         };

         var moleculeAmount = new MoleculeAmount { Name = "name" };
         new Container().WithName("top").Add(moleculeAmount);
         var initialConditionsPathCache = new PathCacheForSpecs<MoleculeAmount>
         {
            moleculeAmount
         };

         A.CallTo(() => _entitiesInSimulationRetriever.EntitiesFrom<Parameter>(_simulationWithChanges)).Returns(parameterPathCache);
         A.CallTo(() => _entitiesInSimulationRetriever.EntitiesFrom<MoleculeAmount>(_simulationWithChanges)).Returns(initialConditionsPathCache);
      }

      protected override void Because()
      {
         sut.CommitSimulationChanges(_simulationWithChanges);
      }

      [Observation]
      public void new_building_blocks_are_created()
      {
         _projectModule.ParameterValuesCollection.Count.ShouldBeEqualTo(1);
         _projectModule.InitialConditionsCollection.Count.ShouldBeEqualTo(1);
      }
   }
}