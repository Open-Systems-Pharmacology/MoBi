using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Model.Diagram;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Core.Service
{
   public abstract class concern_for_AffectedBuildingBlockRetriever : ContextSpecification<IAffectedBuildingBlockRetriever>
   {
      protected IMoBiSimulation _simulation;
      protected IModel _model;
      protected IMoleculeBuildingBlock _moleculeBuidingBlock;
      protected IMoBiReactionBuildingBlock _reactionBuildingBlock;
      protected IPassiveTransportBuildingBlock _passiveTransportBuildingBlock;
      protected IEventGroupBuildingBlock _eventGroupBuildingBlock;
      protected IMoleculeStartValuesBuildingBlock _msvBuildingBlock;
      protected IParameterStartValuesBuildingBlock _psvBuildingBlock;
      protected IMoBiSpatialStructure _spatialStructure;
      protected IObserverBuildingBlock _observerBuildingBlock;
      protected IMoBiBuildConfiguration _buildConfiguration;
      protected IObjectPathFactory _objectPathFactory;

      protected EntityPathResolver _entityPathResolver;

      protected override void Context()
      {
         _buildConfiguration = new MoBiBuildConfiguration();
         _moleculeBuidingBlock = new MoleculeBuildingBlock().WithName("M");
         _buildConfiguration.Molecules = _moleculeBuidingBlock;

         _reactionBuildingBlock = new MoBiReactionBuildingBlock().WithName("R");
         _reactionBuildingBlock.Add(new ReactionBuilder().WithName("R1"));
         _buildConfiguration.Reactions = _reactionBuildingBlock;

         _passiveTransportBuildingBlock = new PassiveTransportBuildingBlock().WithName("PT");
         _passiveTransportBuildingBlock.Add(new TransportBuilder().WithName("PT2"));
         _passiveTransportBuildingBlock.Add(new TransportBuilder().WithName("PT1"));
         _buildConfiguration.PassiveTransports = _passiveTransportBuildingBlock;

         _eventGroupBuildingBlock = new EventGroupBuildingBlock().WithName("EG");
         _buildConfiguration.EventGroups = _eventGroupBuildingBlock;

         _msvBuildingBlock = new MoleculeStartValuesBuildingBlock().WithName("MSV");
         _buildConfiguration.MoleculeStartValues = _msvBuildingBlock;

         _psvBuildingBlock = new ParameterStartValuesBuildingBlock().WithName("PSV");
         _buildConfiguration.ParameterStartValues = _psvBuildingBlock;

         _spatialStructure = new MoBiSpatialStructure().WithName("SPST");
         _buildConfiguration.SpatialStructure = _spatialStructure;

         _observerBuildingBlock = new ObserverBuildingBlock().WithName("O");
         _buildConfiguration.Observers = _observerBuildingBlock;

         _objectPathFactory = new ObjectPathFactory(new AliasCreator());
         _entityPathResolver = new EntityPathResolver(_objectPathFactory);
         sut = new AffectedBuildingBlockRetriever(_entityPathResolver);

         //common setup
         _simulation = new MoBiSimulation {BuildConfiguration = _buildConfiguration};
      }
   }

   public class When_asking_for_the_affected_building_block_for_a_parameter_that_is_not_defined_anywhere : concern_for_AffectedBuildingBlockRetriever
   {
      private IBuildingBlockInfo _result;

      protected override void Because()
      {
         _result = sut.RetrieveFor(new Parameter(), _simulation);
      }

      [Observation]
      public void should_return_spatial_structure()
      {
         _result.ShouldBeEqualTo(_buildConfiguration.SpatialStructureInfo);
      }
   }

   public class When_asking_for_the_affected_building_block_for_a_molecule_properties_parameter_not_defined_as_parameter_start_value_entry : concern_for_AffectedBuildingBlockRetriever
   {
      protected IParameter _parameter;
      protected IBuildingBlockInfo _result;

      protected override void Context()
      {
         base.Context();
         _parameter = new Parameter()
            .WithName("P")
            .WithParentContainer(new Container().WithName("Drug"));

         _moleculeBuidingBlock.Add(new MoleculeBuilder().WithName("Drug"));
      }

      protected override void Because()
      {
         _result = sut.RetrieveFor(_parameter, _simulation);
      }

      [Observation]
      public void should_return_parameter_start_value_building_block()
      {
         _result.ShouldBeEqualTo(_buildConfiguration.ParameterStartValuesInfo);
      }
   }

   public abstract class When_asking_for_the_affected_building_block_for_a_parameter_defined_in_a_molecule : concern_for_AffectedBuildingBlockRetriever
   {
      protected ParameterBuildMode _parameterBuildMode;
      protected IParameter _parameter;
      protected IBuildingBlockInfo _result;

      protected override void Context()
      {
         base.Context();
         _parameter = new Parameter()
            .WithName("P")
            .WithMode(_parameterBuildMode)
            .WithParentContainer(new Container().WithName("Drug"));

         var moleculeBuilder = new MoleculeBuilder().WithName("Drug");
         var parameterBuilder = new Parameter()
            .WithName(_parameter.Name)
            .WithMode(_parameterBuildMode);

         moleculeBuilder.AddParameter(parameterBuilder);
         _moleculeBuidingBlock.Add(moleculeBuilder);
      }

      protected override void Because()
      {
         _result = sut.RetrieveFor(_parameter, _simulation);
      }
   }

   public class When_asking_for_the_affected_building_block_for_a_global_molecule_parameter_not_defined_as_parameter_start_value_entry : When_asking_for_the_affected_building_block_for_a_parameter_defined_in_a_molecule
   {
      protected override void Context()
      {
         _parameterBuildMode = ParameterBuildMode.Global;
         base.Context();
      }

      [Observation]
      public void should_return_molecule_building_block()
      {
         _result.ShouldBeEqualTo(_buildConfiguration.MoleculesInfo);
      }
   }

   public class When_asking_for_the_affected_building_block_for_a_global_molecule_parameter_defined_as_parameter_start_value_entry : When_asking_for_the_affected_building_block_for_a_parameter_defined_in_a_molecule
   {
      protected override void Context()
      {
         _parameterBuildMode = ParameterBuildMode.Global;
         base.Context();
         _psvBuildingBlock.Add(new ParameterStartValue {Path = _entityPathResolver.ObjectPathFor(_parameter)});
      }

      [Observation]
      public void should_return_parameter_start_value_building_block()
      {
         _result.ShouldBeEqualTo(_buildConfiguration.ParameterStartValuesInfo);
      }
   }

   public class When_asking_for_the_affected_building_block_for_a_local_molecule_parameter_defined_as_parameter_start_value_entry : When_asking_for_the_affected_building_block_for_a_parameter_defined_in_a_molecule
   {
      protected override void Context()
      {
         _parameterBuildMode = ParameterBuildMode.Local;
         base.Context();
         _psvBuildingBlock.Add(new ParameterStartValue {Path = _entityPathResolver.ObjectPathFor(_parameter)});
      }

      [Observation]
      public void should_return_parameter_start_value_building_block()
      {
         _result.ShouldBeEqualTo(_buildConfiguration.ParameterStartValuesInfo);
      }
   }

   public class When_asking_for_the_affected_building_block_for_a_local_molecule_parameter_not_defined_as_parameter_start_value_entry : When_asking_for_the_affected_building_block_for_a_parameter_defined_in_a_molecule
   {
      protected override void Context()
      {
         _parameterBuildMode = ParameterBuildMode.Local;
         base.Context();
      }

      [Observation]
      public void should_return_parameter_start_value_building_block()
      {
         _result.ShouldBeEqualTo(_buildConfiguration.ParameterStartValuesInfo);
      }
   }

   public class When_asked_for_an_affected_building_block_for_an_observer : concern_for_AffectedBuildingBlockRetriever
   {
      private IBuildingBlockInfo _result;

      protected override void Because()
      {
         _result = sut.RetrieveFor(A.Fake<IObserver>(), _simulation);
      }

      [Observation]
      public void should_return_the_observer_building_block()
      {
         _result.ShouldBeEqualTo(_buildConfiguration.ObserversInfo);
      }
   }

   public class When_asked_for_an_affected_building_block_for_an_event_group_parameter : concern_for_AffectedBuildingBlockRetriever
   {
      private IBuildingBlockInfo _result;
      private IQuantity _egParameter;

      protected override void Context()
      {
         base.Context();
         _egParameter = new Parameter()
            .WithParentContainer(new Container().WithName("Protocol")
               .WithParentContainer(new EventGroup().WithName("EG")));
      }

      protected override void Because()
      {
         _result = sut.RetrieveFor(_egParameter, _simulation);
      }

      [Observation]
      public void should_return_event_group_building_block()
      {
         _result.ShouldBeEqualTo(_buildConfiguration.ParameterStartValuesInfo);
      }
   }

   public class When_asked_for_an_affected_building_block_for_a_passive_transport_parameter_not_defined_in_a_parameter_start_value : concern_for_AffectedBuildingBlockRetriever
   {
      private IBuildingBlockInfo _result;
      private IQuantity _passiveTransportParameter;

      protected override void Context()
      {
         base.Context();
         _passiveTransportParameter = new Parameter().WithParentContainer(new Transport().WithName("PT2"));
      }

      protected override void Because()
      {
         _result = sut.RetrieveFor(_passiveTransportParameter, _simulation);
      }

      [Observation]
      public void should_return_parameter_start_value_building_block_building_block()
      {
         _result.ShouldBeEqualTo(_buildConfiguration.ParameterStartValuesInfo);
      }
   }

   public abstract class When_asking_for_an_affected_building_block_for_a_parameter_defined_in_a_reaction : concern_for_AffectedBuildingBlockRetriever
   {
      protected IBuildingBlockInfo _result;
      protected IParameter _parameter;
      protected ParameterBuildMode _parameterBuildMode;

      protected override void Context()
      {
         base.Context();
         var reaction = new Reaction();
         _parameter = new Parameter()
            .WithMode(_parameterBuildMode)
            .WithParentContainer(reaction);
      }

      protected override void Because()
      {
         _result = sut.RetrieveFor(_parameter, _simulation);
      }
   }

   public class When_asked_for_affected_building_block_for_a_global_reaction_parameter_not_defined_in_parameter_start_value : When_asking_for_an_affected_building_block_for_a_parameter_defined_in_a_reaction
   {
      protected override void Context()
      {
         _parameterBuildMode = ParameterBuildMode.Global;
         base.Context();
      }

      [Observation]
      public void should_return_reaction_buiding_block()
      {
         _result.ShouldBeEqualTo(_buildConfiguration.ReactionsInfo);
      }
   }

   public class When_asked_for_affected_building_block_for_a_global_reaction_parameter_defined_in_parameter_start_value : When_asking_for_an_affected_building_block_for_a_parameter_defined_in_a_reaction
   {
      protected override void Context()
      {
         _parameterBuildMode = ParameterBuildMode.Global;
         base.Context();
         _psvBuildingBlock.Add(new ParameterStartValue {Path = _entityPathResolver.ObjectPathFor(_parameter)});
      }

      [Observation]
      public void should_return_reaction_buiding_block()
      {
         _result.ShouldBeEqualTo(_buildConfiguration.ParameterStartValuesInfo);
      }
   }

   public class When_asked_for_affected_building_block_for_a_propery_reaction_parameter_not_defined_in_parameter_start_value : When_asking_for_an_affected_building_block_for_a_parameter_defined_in_a_reaction
   {
      protected override void Context()
      {
         _parameterBuildMode = ParameterBuildMode.Property;
         base.Context();
      }

      [Observation]
      public void should_return_reaction_buiding_block()
      {
         _result.ShouldBeEqualTo(_buildConfiguration.ReactionsInfo);
      }
   }

   public class When_asked_for_affected_building_block_for_a_local_reaction_parameter_not_defined_in_parameter_start_value : When_asking_for_an_affected_building_block_for_a_parameter_defined_in_a_reaction
   {
      protected override void Context()
      {
         _parameterBuildMode = ParameterBuildMode.Local;
         base.Context();
      }

      [Observation]
      public void should_return_parameter_start_value_building_block()
      {
         _result.ShouldBeEqualTo(_buildConfiguration.ParameterStartValuesInfo);
      }
   }

   internal class When_asked_for_affected_building_block_for_molecule_amount : concern_for_AffectedBuildingBlockRetriever
   {
      private IBuildingBlockInfo _result;

      protected override void Because()
      {
         _result = sut.RetrieveFor(A.Fake<IMoleculeAmount>(), _simulation);
      }

      [Observation]
      public void should_return_the_molecule_building_block()
      {
         _result.ShouldBeEqualTo(_buildConfiguration.MoleculeStartValuesInfo);
      }
   }

   public class When_retrieving_the_affected_building_block_for_an_object_belonging_in_a_simulation_settings : concern_for_AffectedBuildingBlockRetriever
   {
      [Observation]
      public void should_return_the_simulation_settings_building_block()
      {
         sut.RetrieveFor(new CurveChartTemplate(), _simulation).ShouldBeEqualTo(_buildConfiguration.SimulationSettingsInfo);
         sut.RetrieveFor(new OutputSchema(), _simulation).ShouldBeEqualTo(_buildConfiguration.SimulationSettingsInfo);
         sut.RetrieveFor(new OutputInterval(), _simulation).ShouldBeEqualTo(_buildConfiguration.SimulationSettingsInfo);
         sut.RetrieveFor(new OutputSelections(), _simulation).ShouldBeEqualTo(_buildConfiguration.SimulationSettingsInfo);
      }
   }
}