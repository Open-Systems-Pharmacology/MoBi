using System.Collections.Generic;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Repository;
using MoBi.Core.Repositories;
using MoBi.Core.Services;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using MoBi.Helpers;
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
      private readonly string _moleculeName="Drug";
      private readonly string _parameterName="Para";

      protected override void Context()
      {
         base.Context();
         _project = DomainHelperForSpecs.NewProject();
         var molecule = new MoleculeBuilder().WithName(_moleculeName);
         var molecules= new MoleculeBuildingBlock(){molecule};
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
      private MoleculeBuilder _molecule;
      private readonly string _moleculeName = "Drug";
      private readonly string _parameterName = "Para";
      private readonly string _reactionName = "Reaction";
      private readonly string _msvName = "MSV";
      private readonly string _moleculeParameterName = "MW";

      protected override void Context()
      {
         base.Context();
         _project = DomainHelperForSpecs.NewProject();
         _molecule = new MoleculeBuilder().WithName(_moleculeName);
         var moleculeParameter = new Parameter().WithName(_moleculeParameterName);
         _molecule.Add(moleculeParameter);
         var molecules = new MoleculeBuildingBlock() { _molecule };
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
         _forbiddenNames = sut.For(_molecule);
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

   class When_retrieving_forbidden_names_for_a_molecule_with_same_name_in_another_module : concern_for_ForbiddenNamesRetrieverSpecs
   {
      private MoBiProject _project;
      private IEnumerable<string> _forbiddenNames;
      private MoleculeBuilder _moleculeInModule2;
      private readonly string _moleculeNameInModule1 = "Drug";
      private readonly string _reactionNameInModule1 = "Reaction1";
      private readonly string _parameterNameInModule1 = "Para1";
      private readonly string _reactionNameInModule2 = "Reaction2";
      private readonly string _parameterNameInModule2 = "Para2";

      protected override void Context()
      {
         base.Context();
         _project = DomainHelperForSpecs.NewProject();

         // Module 1 with molecule "Drug"
         var molecule1 = new MoleculeBuilder().WithName(_moleculeNameInModule1);
         var molecules1 = new MoleculeBuildingBlock() { molecule1 };
         var reaction1 = new ReactionBuilder().WithName(_reactionNameInModule1);
         var reactions1 = new MoBiReactionBuildingBlock() { reaction1 };
         var root1 = new Container().WithName("Root1");
         root1.Add(new Parameter().WithName(_parameterNameInModule1));
         var spatialStructure1 = new MoBiSpatialStructure().WithTopContainer(root1);

         var module1 = new Module()
         {
            molecules1,
            reactions1,
            spatialStructure1
         };

         // Module 2 with a different molecule that we want to rename to "Drug"
         _moleculeInModule2 = new MoleculeBuilder().WithName("OtherMolecule");
         var molecules2 = new MoleculeBuildingBlock() { _moleculeInModule2 };
         var reaction2 = new ReactionBuilder().WithName(_reactionNameInModule2);
         var reactions2 = new MoBiReactionBuildingBlock() { reaction2 };
         var root2 = new Container().WithName("Root2");
         root2.Add(new Parameter().WithName(_parameterNameInModule2));
         var spatialStructure2 = new MoBiSpatialStructure().WithTopContainer(root2);

         var module2 = new Module()
         {
            molecules2,
            reactions2,
            spatialStructure2
         };

         _project.AddModule(module1);
         _project.AddModule(module2);

         A.CallTo(() => _moBiProjectRetriever.Current).Returns(_project);
      }

      protected override void Because()
      {
         _forbiddenNames = sut.For(_moleculeInModule2);
      }

      [Observation]
      public void should_not_contain_molecule_names_from_other_module()
      {
         _forbiddenNames.ShouldNotContain(_moleculeNameInModule1);
      }

      [Observation]
      public void should_not_contain_reaction_names_from_other_module()
      {
         _forbiddenNames.ShouldNotContain(_reactionNameInModule1);
      }

      [Observation]
      public void should_not_contain_parameter_names_from_other_module()
      {
         _forbiddenNames.ShouldNotContain(_parameterNameInModule1);
      }

      [Observation]
      public void should_contain_reaction_names_from_same_module()
      {
         _forbiddenNames.ShouldContain(_reactionNameInModule2);
      }

      [Observation]
      public void should_contain_parameter_names_from_same_module()
      {
         _forbiddenNames.ShouldContain(_parameterNameInModule2);
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
      private readonly string _topContainerName="Organism";
      private readonly string _eventGroupName="Events";
      private readonly string _simulationName="Test";

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