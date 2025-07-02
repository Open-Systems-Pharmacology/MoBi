using System.Collections.Generic;
using FakeItEasy;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Repository;
using MoBi.Core.Repositories;
using MoBi.Core.Services;
using MoBi.HelpersForTests;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using IBuildingBlockRepository = MoBi.Core.Domain.Repository.IBuildingBlockRepository;

namespace MoBi.Core.Service
{
   public abstract class concern_for_ForbiddenNamesRetrieverSpecs : ContextSpecification<ForbiddenNamesRetriever>
   {
      protected IBuildingBlockRepository _buildingBlockRepository;
      protected IMoBiProjectRetriever _moBiProjectRetriever;
      private SimulationRepository _simulationRepository;

      protected override void Context()
      {
         _moBiProjectRetriever = A.Fake<IMoBiProjectRetriever>();
         _buildingBlockRepository = new BuildingBlockRepository(_moBiProjectRetriever);
         _simulationRepository = new SimulationRepository(_moBiProjectRetriever);
         sut = new ForbiddenNamesRetriever(_buildingBlockRepository, _simulationRepository);
      }
   }

   class When_retrieving_forbidden_names_for_a_container : concern_for_ForbiddenNamesRetrieverSpecs
   {
      private MoBiProject _project;
      private IEnumerable<string> _forbiddenNames;
      private readonly string _moleculeName = "Drug";
      private readonly string _parameterName = "Para";
      private readonly string _reactionName = "Reaction";
      private readonly string _moleculeParameterName = "MW";

      protected override void Context()
      {
         base.Context();
         _project = DomainHelperForSpecs.NewProject();
         var molecule = new MoleculeBuilder().WithName(_moleculeName);
         var moleculeParameter = new Parameter().WithName(_moleculeParameterName);
         molecule.Add(moleculeParameter);
         var molecules = new MoleculeBuildingBlock() { molecule };

         var parameter = new Parameter().WithName(_parameterName);
         var root = new Container().WithName("Root");
         root.Add(parameter);
         var spatialStructure = new MoBiSpatialStructure().WithTopContainer(root);
         var reactionBuilder = new ReactionBuilder().WithName(_reactionName);
         var reactions = new MoBiReactionBuildingBlock() { reactionBuilder };

         var module = new Module()
         {
            reactions,
            spatialStructure,
            molecules
         };

         _project.AddModule(module);

         A.CallTo(() => _moBiProjectRetriever.Current).Returns(_project);
      }

      protected override void Because()
      {
         _forbiddenNames = sut.For(new Container());
      }

      [Observation]
      public void should_retrieve_reaction_name()
      {
         _forbiddenNames.ShouldContain(_moleculeName);
      }
   }

   class When_retrieving_forbidden_names_for_a_distributed_parameter : concern_for_ForbiddenNamesRetrieverSpecs
   {
      private MoBiProject _project;
      private IEnumerable<string> _forbiddenNames;
      private readonly string _moleculeName = "Drug";
      private readonly string _parameterName = "Para";
      private readonly string _reactionName = "Reaction";
      private readonly string _moleculeParameterName = "MW";

      protected override void Context()
      {
         base.Context();
         _project = DomainHelperForSpecs.NewProject();
         var molecule = new MoleculeBuilder().WithName(_moleculeName);
         var moleculeParameter = new Parameter().WithName(_moleculeParameterName);
         molecule.Add(moleculeParameter);
         var molecules = new MoleculeBuildingBlock() { molecule };
         var parameter = new Parameter().WithName(_parameterName);
         var root = new Container().WithName("Root");
         root.Add(parameter);
         var spatialStructure = new MoBiSpatialStructure().WithTopContainer(root);
         var reactionBuilder = new ReactionBuilder().WithName(_reactionName);
         var reactions = new MoBiReactionBuildingBlock() { reactionBuilder };

         var module = new Module()
         {
            reactions,
            spatialStructure,
            molecules
         };

         _project.AddModule(module);

         A.CallTo(() => _moBiProjectRetriever.Current).Returns(_project);
      }

      protected override void Because()
      {
         _forbiddenNames = sut.For(new DistributedParameter());
      }

      [Observation]
      public void should_look_for_reaction_name()
      {
         _forbiddenNames.ShouldContain(_reactionName);
      }

      [Observation]
      public void should_look_for_molecule_name()
      {
         _forbiddenNames.ShouldContain(_moleculeName);
      }
   }

   class When_retrieving_forbidden_names_for_an_reaction : concern_for_ForbiddenNamesRetrieverSpecs
   {
      private MoBiProject _project;
      private IEnumerable<string> _forbiddenNames;
      private readonly string _moleculeName = "Drug";
      private readonly string _parameterName = "Para";

      protected override void Context()
      {
         base.Context();
         _project = DomainHelperForSpecs.NewProject();
         var molecule = new MoleculeBuilder().WithName(_moleculeName);
         var molecules = new MoleculeBuildingBlock() { molecule };
         var parameter = new Parameter().WithName(_parameterName);
         var root = new Container().WithName("Root");
         root.Add(parameter);
         var spatialStructure = new MoBiSpatialStructure().WithTopContainer(root);
         A.CallTo(() => _moBiProjectRetriever.Current).Returns(_project);

         var module = new Module()
         {
            spatialStructure,
            molecules
         };

         _project.AddModule(module);
      }

      protected override void Because()
      {
         _forbiddenNames = sut.For(new ReactionBuilder());
      }

      [Observation]
      public void should_look_for_Molecule_names()
      {
         _forbiddenNames.ShouldContain(_moleculeName);
      }

      [Observation]
      public void should_look_for_spatial_structure_parameter_name()
      {
         _forbiddenNames.ShouldContain(_parameterName);
      }
   }

   class When_retrieving_forbidden_names_for_an_molecule : concern_for_ForbiddenNamesRetrieverSpecs
   {
      private MoBiProject _project;
      private IEnumerable<string> _forbiddenNames;
      private readonly string _moleculeName = "Drug";
      private readonly string _parameterName = "Para";
      private readonly string _reactionName = "Reaction";
      private readonly string _msvName = "MSV";
      private readonly string _moleculeParameterName = "MW";

      protected override void Context()
      {
         base.Context();
         _project = DomainHelperForSpecs.NewProject();
         var molecule = new MoleculeBuilder().WithName(_moleculeName);
         var moleculeParameter = new Parameter().WithName(_moleculeParameterName);
         molecule.Add(moleculeParameter);
         var molecules = new MoleculeBuildingBlock() { molecule };
         var parameter = new Parameter().WithName(_parameterName);
         var root = new Container().WithName("Root");
         root.Add(parameter);
         var spatialStructure = new MoBiSpatialStructure().WithTopContainer(root);
         var reactionBuilder = new ReactionBuilder().WithName(_reactionName);
         var reactions = new MoBiReactionBuildingBlock() { reactionBuilder };
         var msv = new InitialCondition { Path = new ObjectPath("A", _msvName) };
         var msv2 = new InitialCondition { Path = new ObjectPath("A", _moleculeName) };
         var initialConditions = new InitialConditionsBuildingBlock() { msv, msv2 };

         var module = new Module()
         {
            reactions,
            spatialStructure,
            molecules,
            initialConditions
         };

         _project.AddModule(module);

         A.CallTo(() => _moBiProjectRetriever.Current).Returns(_project);
      }

      protected override void Because()
      {
         MoleculeBuilder testMolecule = new MoleculeBuilder().WithName(_moleculeName);
         _forbiddenNames = sut.For(testMolecule);
      }

      [Observation]
      public void should_look_for_reaction_Names1()
      {
         _forbiddenNames.ShouldContain(_reactionName);
      }

      [Observation]
      public void should_look_for_spatial_structure_parameter_name()
      {
         _forbiddenNames.ShouldContain(_parameterName);
      }

      [Observation]
      public void should_look_for_molecule_start_values_molecule_names_where_molecule_name_is_used()
      {
         _forbiddenNames.ShouldContain(_msvName);
      }

      [Observation]
      public void should_look_for_molecule_parameters()
      {
         _forbiddenNames.ShouldContain(_moleculeParameterName);
      }
   }

   class When_retrieving_forbidden_names_for_an_simulation : concern_for_ForbiddenNamesRetrieverSpecs
   {
      private MoBiProject _project;
      private IEnumerable<string> _forbiddenNames;
      private readonly string _moleculeName = "Drug";
      private readonly string _parameterName = "Para";
      private readonly string _reactionName = "Reaction";
      private readonly string _moleculeParameterName = "MW";
      private readonly string _topContainerName = "Organism";
      private readonly string _eventGroupName = "Events";
      private readonly string _simulationName = "Test";

      protected override void Context()
      {
         base.Context();
         _project = DomainHelperForSpecs.NewProject();
         var molecule = new MoleculeBuilder().WithName(_moleculeName);
         var moleculeParameter = new Parameter().WithName(_moleculeParameterName);
         molecule.Add(moleculeParameter);
         var molecules = new MoleculeBuildingBlock() { molecule };
         var parameter = new Parameter().WithName(_parameterName);
         var root = new Container().WithName(_topContainerName);
         root.Add(parameter);
         var spatialStructure = new MoBiSpatialStructure().WithTopContainer(root);
         spatialStructure.GlobalMoleculeDependentProperties = new Container().WithName(Constants.MOLECULE_PROPERTIES);
         spatialStructure.NeighborhoodsContainer = new Container().WithName(Constants.NEIGHBORHOODS);

         var reactionBuilder = new ReactionBuilder().WithName(_reactionName);
         var reactions = new MoBiReactionBuildingBlock() { reactionBuilder };
         _project.AddSimulation(new MoBiSimulation().WithName(_simulationName));
         var eventGroupBuilder = new EventGroupBuilder().WithName(_eventGroupName);
         var eventGroupBuildingBlock = new EventGroupBuildingBlock { eventGroupBuilder };

         var module = new Module
         {
            reactions,
            spatialStructure,
            molecules,
            eventGroupBuildingBlock
         };

         _project.AddModule(module);

         A.CallTo(() => _moBiProjectRetriever.Current).Returns(_project);
      }

      protected override void Because()
      {
         var testSimulation = new MoBiSimulation().WithName("Sim");
         _forbiddenNames = sut.For(testSimulation);
      }

      [Observation]
      public void should_look_for_reaction_Names()
      {
         _forbiddenNames.ShouldContain(_reactionName);
      }

      [Observation]
      public void should_look_for_spatial_structure_TopContainer_name()
      {
         _forbiddenNames.ShouldContain(_topContainerName);
      }

      [Observation]
      public void should_allow_for_root_EventGroupName()
      {
         _forbiddenNames.ShouldNotContain(_eventGroupName);
      }

      [Observation]
      public void should_look_for_old_simulation_name()
      {
         _forbiddenNames.ShouldContain(_simulationName);
      }
   }
}