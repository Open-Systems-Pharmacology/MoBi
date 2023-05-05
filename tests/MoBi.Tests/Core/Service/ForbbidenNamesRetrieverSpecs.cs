using System.Collections.Generic;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Model.Diagram;
using MoBi.Core.Services;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using MoBi.Helpers;

namespace MoBi.Core.Service
{
   public abstract class concern_for_ForbbidenNamesRetrieverSpecs : ContextSpecification<IForbiddenNamesRetriever>
   {
      protected IMoBiContext _context;

      protected override void Context()
      {
         _context = A.Fake<IMoBiContext>();
         sut = new ForbiddenNamesRetriever(_context);
      }
   }

   class When_retrieving_forbidden_names_for_a_container : concern_for_ForbbidenNamesRetrieverSpecs
   {
      private MoBiProject _project;
      private IEnumerable<string> _forbiddenNames;
      private string _moleculeName = "Drug";
      private string _parameterName = "Para";
      private string _reactionName = "Reaction";
      private string _moleculeParameterName = "MW";
      protected override void Context()
      {
         base.Context();
         _project = DomainHelperForSpecs.NewProject();
         var molecule = new MoleculeBuilder().WithName(_moleculeName);
         var moleculeParameter = new Parameter().WithName(_moleculeParameterName);
         molecule.Add(moleculeParameter);
         var molecules = new MoleculeBuildingBlock() { molecule };
         _project.AddBuildingBlock(molecules);
         var parameter = new Parameter().WithName(_parameterName);
         var root = new Container().WithName("Root");
         root.Add(parameter);
         var spatialStructure = new MoBiSpatialStructure().WithTopContainer(root);
         _project.AddBuildingBlock(spatialStructure);
         var reactionBuilder = new ReactionBuilder().WithName(_reactionName);
         var reactions = new MoBiReactionBuildingBlock() { reactionBuilder };
         _project.AddBuildingBlock(reactions);

         A.CallTo(() => _context.CurrentProject).Returns(_project);
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

   class When_retrieving_forbidden_names_for_a_distributed_parameter : concern_for_ForbbidenNamesRetrieverSpecs
   {
      private MoBiProject _project;
      private IEnumerable<string> _forbiddenNames;
      private string _moleculeName = "Drug";
      private string _parameterName = "Para";
      private string _reactionName = "Reaction";
      private string _moleculeParameterName = "MW";

      protected override void Context()
      {
         base.Context();
         _project = DomainHelperForSpecs.NewProject();
         var molecule = new MoleculeBuilder().WithName(_moleculeName);
         var moleculeParameter = new Parameter().WithName(_moleculeParameterName);
         molecule.Add(moleculeParameter);
         var molecules = new MoleculeBuildingBlock() { molecule };
         _project.AddBuildingBlock(molecules);
         var parameter = new Parameter().WithName(_parameterName);
         var root = new Container().WithName("Root");
         root.Add(parameter);
         var spatialStructure = new MoBiSpatialStructure().WithTopContainer(root);
         _project.AddBuildingBlock(spatialStructure);
         var reactionBuilder = new ReactionBuilder().WithName(_reactionName);
         var reactions = new MoBiReactionBuildingBlock() { reactionBuilder };
         _project.AddBuildingBlock(reactions);

         A.CallTo(() => _context.CurrentProject).Returns(_project);
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

   class When_retrieving_forbidden_names_for_an_reaction : concern_for_ForbbidenNamesRetrieverSpecs
   {
      private MoBiProject _project;
      private IEnumerable<string> _forbiddenNames;
      private string _moleculeName="Drug";
      private string _parameterName="Para";

      protected override void Context()
      {
         base.Context();
         _project = DomainHelperForSpecs.NewProject();
         var molecule = new MoleculeBuilder().WithName(_moleculeName);
         var molecules= new MoleculeBuildingBlock(){molecule};
         _project.AddBuildingBlock(molecules);
         var parameter = new Parameter().WithName(_parameterName);
         var root = new Container().WithName("Root");
         root.Add(parameter);
         var spatialStructure = new MoBiSpatialStructure().WithTopContainer(root);
         _project.AddBuildingBlock(spatialStructure);
         A.CallTo(()=>_context.CurrentProject).Returns(_project);
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

   class When_retrieving_forbidden_names_for_an_molecule : concern_for_ForbbidenNamesRetrieverSpecs
   {
      private MoBiProject _project;
      private IEnumerable<string> _forbiddenNames;
      private string _moleculeName = "Drug";
      private string _parameterName = "Para";
      private string _reactionName="Reaction";
      private string _msvName ="MSV";
      private string _moleculeParameterName ="MW";

      protected override void Context()
      {
         base.Context();
         _project = DomainHelperForSpecs.NewProject();
         var molecule = new MoleculeBuilder().WithName(_moleculeName);
         var moleculeParameter = new Parameter().WithName(_moleculeParameterName);
         molecule.Add(moleculeParameter);
         var molecules = new MoleculeBuildingBlock() { molecule };
         _project.AddBuildingBlock(molecules);
         var parameter = new Parameter().WithName(_parameterName);
         var root = new Container().WithName("Root");
         root.Add(parameter);
         var spatialStructure = new MoBiSpatialStructure().WithTopContainer(root);
         _project.AddBuildingBlock(spatialStructure);
         var reactionBuilder = new ReactionBuilder().WithName(_reactionName);
         var reactions = new MoBiReactionBuildingBlock() {reactionBuilder};
         _project.AddBuildingBlock(reactions);
         var msv = new InitialCondition { Path=new ObjectPath("A",_msvName)};
         var msv2 = new InitialCondition { Path = new ObjectPath("A", _moleculeName) };
         var moleculeStartValues = new InitialConditionsBuildingBlock() {msv,msv2};
         _project.AddBuildingBlock(moleculeStartValues);
         
         A.CallTo(() => _context.CurrentProject).Returns(_project);

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
      public void should_look_for_molecul_parameters()
      {
         _forbiddenNames.ShouldContain(_moleculeParameterName);
      }
   }

   class When_retrieving_forbidden_names_for_an_simulation : concern_for_ForbbidenNamesRetrieverSpecs
   {
      private MoBiProject _project;
      private IEnumerable<string> _forbiddenNames;
      private string _moleculeName = "Drug";
      private string _parameterName = "Para";
      private string _reactionName = "Reaction";
      private string _moleculeParameterName = "MW";
      private string _topContainerName="Organism";
      private string _eventGroupName="Events";
      private string _simulationName="Test";

      protected override void Context()
      {
         base.Context();
         _project = DomainHelperForSpecs.NewProject();
         var molecule = new MoleculeBuilder().WithName(_moleculeName);
         var moleculeParameter = new Parameter().WithName(_moleculeParameterName);
         molecule.Add(moleculeParameter);
         var molecules = new MoleculeBuildingBlock() { molecule };
         _project.AddBuildingBlock(molecules);
         var parameter = new Parameter().WithName(_parameterName);
         var root = new Container().WithName(_topContainerName);
         root.Add(parameter);
         var spatialStructure = new MoBiSpatialStructure().WithTopContainer(root);
         spatialStructure.GlobalMoleculeDependentProperties = new Container().WithName(Constants.MOLECULE_PROPERTIES);
         spatialStructure.NeighborhoodsContainer = new Container().WithName(Constants.NEIGHBORHOODS);
         _project.AddBuildingBlock(spatialStructure);
         
         var reactionBuilder = new ReactionBuilder().WithName(_reactionName);
         var reactions = new MoBiReactionBuildingBlock() { reactionBuilder };
         _project.AddBuildingBlock(reactions);
         _project.AddSimulation(new MoBiSimulation().WithName(_simulationName));
         var eventGroupBuilder = new EventGroupBuilder().WithName(_eventGroupName);
         var eventGroupBuildingBlock = new EventGroupBuildingBlock();
         eventGroupBuildingBlock.Add(eventGroupBuilder);
         _project.AddBuildingBlock(eventGroupBuildingBlock);


         A.CallTo(() => _context.CurrentProject).Returns(_project);

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
      public void should_look_for_root_EventGroupName()
      {
         _forbiddenNames.ShouldContain(_eventGroupName);
      }

      [Observation]
      public void should_loog_for_old_simualtion_name()
      {
         _forbiddenNames.ShouldContain(_simulationName);
      }
   }
}	