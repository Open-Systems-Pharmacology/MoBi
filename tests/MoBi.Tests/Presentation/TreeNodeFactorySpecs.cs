using System.Collections.Generic;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Presentation.Nodes;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Helpers;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Repositories;
using OSPSuite.Presentation.Services;
using ITreeNodeFactory = MoBi.Presentation.Nodes.ITreeNodeFactory;

namespace MoBi.Presentation
{
   public abstract class concern_for_TreeNodeFactory : ContextSpecification<ITreeNodeFactory>
   {
      private IToolTipPartCreator _toolTipPartCreator;
      private IObservedDataRepository _observedDataRepository;

      protected override void Context()
      {
         _toolTipPartCreator = A.Fake<IToolTipPartCreator>();
         _observedDataRepository = A.Fake<IObservedDataRepository>();
         sut = new Nodes.TreeNodeFactory(_observedDataRepository, _toolTipPartCreator);
      }
   }

   public class When_creating_tree_for_simulation_without_individual : concern_for_TreeNodeFactory
   {
      private MoBiSimulation _simulation;
      private ClassifiableSimulation _classifiableSimulation;
      private SpatialStructure _spatialStructure;
      private EventGroupBuildingBlock _eventGroups;
      private MoleculeBuildingBlock _molecules;
      private ReactionBuildingBlock _reactions;
      private ObserverBuildingBlock _observers;
      private PassiveTransportBuildingBlock _passiveTransports;
      private ITreeNode _nodes;

      protected override void Context()
      {
         base.Context();
         _simulation = new MoBiSimulation
         {
            Configuration = new SimulationConfiguration()
         };
         
         _spatialStructure = new SpatialStructure().WithId("1");
         _eventGroups = new EventGroupBuildingBlock().WithId("2");
         _molecules = new MoleculeBuildingBlock().WithId("3");
         _reactions = new ReactionBuildingBlock().WithId("4");
         _observers = new ObserverBuildingBlock().WithId("5");
         _passiveTransports = new PassiveTransportBuildingBlock().WithId("7");
         
         _simulation.Configuration.AddModuleConfiguration(new ModuleConfiguration(new Module
         {
            _spatialStructure,
            _eventGroups,
            _molecules,
            _reactions,
            _observers,
            _passiveTransports,

         }));
         _classifiableSimulation = new ClassifiableSimulation
         {
            Subject = _simulation
         };
      }

      protected override void Because()
      {
         _nodes = sut.CreateFor(_classifiableSimulation);
      }

      [Observation]
      public void should_create_tree_with_nodes_for_each_building_block()
      {
         _nodes.AllNodes.Any(node => Equals(_spatialStructure, node.TagAsObject)).ShouldBeTrue();
         _nodes.AllNodes.Any(node => Equals(_eventGroups, node.TagAsObject)).ShouldBeTrue();
         _nodes.AllNodes.Any(node => Equals(_molecules, node.TagAsObject)).ShouldBeTrue();
         _nodes.AllNodes.Any(node => Equals(_reactions, node.TagAsObject)).ShouldBeTrue();
         _nodes.AllNodes.Any(node => Equals(_observers, node.TagAsObject)).ShouldBeTrue();
         _nodes.AllNodes.Any(node => Equals(_passiveTransports, node.TagAsObject)).ShouldBeTrue();
      }
   }

   public class When_creating_the_node_for_a_molecule_building_block : concern_for_TreeNodeFactory
   {
      private MoleculeBuildingBlock _moleculeBuildingBlock;
      private ITreeNode _node;

      protected override void Context()
      {
         base.Context();
         _moleculeBuildingBlock = new MoleculeBuildingBlock { Id = "1" };
         _moleculeBuildingBlock.Add(A.Fake<MoleculeBuilder>().WithId("2"));
         _moleculeBuildingBlock.Add(A.Fake<MoleculeBuilder>().WithId("3"));
      }

      protected override void Because()
      {
         _node = sut.CreateFor(_moleculeBuildingBlock);
      }

      [Observation]
      public void should_add_a_sub_node_for_each_molecules_defined_in_the_building_block()
      {
         _node.Children.Count().ShouldBeEqualTo(2);
         _node.Children.First(x => x.Id == "2").ShouldNotBeNull();
         _node.Children.First(x => x.Id == "3").ShouldNotBeNull();
      }
   }
}