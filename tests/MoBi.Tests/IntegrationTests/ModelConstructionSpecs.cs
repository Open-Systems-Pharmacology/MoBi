using System.Linq;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Helpers;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;
using IContainer = OSPSuite.Core.Domain.IContainer;

namespace MoBi.IntegrationTests
{
   public abstract class concern_for_ModelConstruction : ContextForSimulationIntegration<IModelConstructor>
   {
      protected override void Context()
      {
         
      }
   }

   public class When_creating_a_simulation_using_reactions_with_the_flag_create_rate_parameter_checked:  concern_for_ModelConstruction
   {
      private IMoBiBuildConfiguration _buildConfiguration;
      private IMoleculeBuilder _moleculeA;
      private IReactionBuilder _reactionR1;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _buildConfiguration = DomainFactoryForSpecs.CreateDefaultConfiguration();
         var molecules = _buildConfiguration.Molecules;
         _moleculeA = new MoleculeBuilder().WithName("A").WithDimension(DomainFactoryForSpecs.AmountDimension);
         _moleculeA.DefaultStartFormula = new ConstantFormula(10);
         molecules.Add(_moleculeA);

         var reactions = _buildConfiguration.Reactions;
         _reactionR1 = new ReactionBuilder().WithName("R1");
         _reactionR1.CreateProcessRateParameter = true;
         _reactionR1.Formula = new ConstantFormula(5);
         _reactionR1.AddEduct(new ReactionPartnerBuilder(_moleculeA.Name, 2));
         reactions.Add(_reactionR1);

         _simulation = DomainFactoryForSpecs.CreateSimulationFor(_buildConfiguration);
      }

      [Observation]
      public void should_create_a_parameter_name_process_rate_in_each_local_reaction()
      {
         var allReactions = _simulation.Model.Root.GetAllChildren<IReaction>(x => x.IsNamed(_reactionR1.Name));
         allReactions.Count.ShouldBeGreaterThan(0);
         allReactions.Each(x=>x.Parameter(Constants.Parameters.PROCESS_RATE).ShouldNotBeNull());
      }
   }

   public class When_running_a_simulation_with_a_parameter_whose_plot_parameter_flag_was_set_to_true : concern_for_ModelConstruction
   {
      private IMoBiBuildConfiguration _buildConfiguration;
      private IContainer _organism;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _buildConfiguration = DomainFactoryForSpecs.CreateDefaultConfiguration();
         var spatialStructure = _buildConfiguration.SpatialStructure;
         _organism = spatialStructure.TopContainers.ElementAt(0);
         var volumeParameter = _organism.EntityAt<IParameter>(Constants.Parameters.VOLUME);
         volumeParameter.Persistable = true;
         _simulation = DomainFactoryForSpecs.CreateSimulationFor(_buildConfiguration);

         var simulationRunner = IoC.Resolve<ISimulationRunner>();
         simulationRunner.RunSimulation(_simulation);
      }

      [Observation]
      public void should_add_the_parameter_value_to_the_resulting_data_repository()
      {
         _simulation.ResultsDataRepository.ShouldNotBeNull();
         var volumeParmaeterDataColumn = _simulation.ResultsDataRepository.First(x => x.QuantityInfo.PathAsString == new[] {_simulation.Name, _organism.Name, Constants.Parameters.VOLUME}.ToPathString());
         volumeParmaeterDataColumn.ShouldNotBeNull();
      }
   }
}	