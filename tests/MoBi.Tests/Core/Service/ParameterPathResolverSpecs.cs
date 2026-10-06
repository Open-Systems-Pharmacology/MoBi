using System.Collections.Generic;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using IBuildingBlockRepository = MoBi.Core.Domain.Repository.IBuildingBlockRepository;

namespace MoBi.Core.Service
{
   public abstract class concern_for_ParameterPathResolver : ContextSpecification<ParameterPathResolver>
   {
      protected IBuildingBlockRepository _buildingBlockRepository;

      protected override void Context()
      {
         _buildingBlockRepository = A.Fake<IBuildingBlockRepository>();
         sut = new ParameterPathResolver(_buildingBlockRepository);
      }

      protected IWithDisplayUnit Resolve(params string[] path) => sut.Resolve(new ObjectPath(path));
   }

   public class When_resolving_parameter_paths_in_the_building_blocks_of_the_project : concern_for_ParameterPathResolver
   {
      private Parameter _volume;
      private Parameter _localMoleculeProperty;
      private Parameter _globalMoleculeProperty;
      private Parameter _localMoleculeParameter;
      private Parameter _globalMoleculeParameter;
      private Parameter _reactionParameter;
      private Parameter _eventParameter;
      private IndividualParameter _individualParameter;
      private ExpressionParameter _expressionParameter;

      protected override void Context()
      {
         base.Context();
         _volume = new Parameter { Name = "Volume" };
         _localMoleculeProperty = new Parameter { Name = "LocalMoleculeProperty" };
         _globalMoleculeProperty = new Parameter { Name = "GlobalMoleculeProperty" };
         _localMoleculeParameter = new Parameter { Name = "LocalMoleculeParameter", BuildMode = ParameterBuildMode.Local };
         _globalMoleculeParameter = new Parameter { Name = "GlobalMoleculeParameter", BuildMode = ParameterBuildMode.Global };
         _reactionParameter = new Parameter { Name = "k", BuildMode = ParameterBuildMode.Global };
         _eventParameter = new Parameter { Name = "Dose" };
         _individualParameter = new IndividualParameter { Path = new ObjectPath("Organism", "Weight") };
         _expressionParameter = new ExpressionParameter { Path = new ObjectPath("Organism", "Liver", "Intracellular", "CYP3A4", "Relative expression") };

         var moleculeProperties = new Container { Name = Constants.MOLECULE_PROPERTIES };
         moleculeProperties.Add(_localMoleculeProperty);
         var intracellular = new Container { Name = "Intracellular" };
         intracellular.Add(moleculeProperties);
         var liver = new Container { Name = "Liver" };
         liver.Add(_volume);
         liver.Add(intracellular);
         var organism = new Container { Name = "Organism" };
         organism.Add(liver);

         var spatialStructure = new MoBiSpatialStructure { GlobalMoleculeDependentProperties = new Container { Name = Constants.MOLECULE_PROPERTIES } };
         spatialStructure.AddTopContainer(organism);
         spatialStructure.GlobalMoleculeDependentProperties.Add(_globalMoleculeProperty);

         var molecule = new MoleculeBuilder { Name = "Drug" };
         molecule.Add(_localMoleculeParameter);
         molecule.Add(_globalMoleculeParameter);

         var reaction = new ReactionBuilder { Name = "R1" };
         reaction.Add(_reactionParameter);

         var eventGroup = new EventGroupBuilder { Name = "Application" };
         eventGroup.Add(_eventParameter);

         var individual = new IndividualBuildingBlock();
         individual.Add(_individualParameter);

         var expressionProfile = new ExpressionProfileBuildingBlock();
         expressionProfile.Add(_expressionParameter);

         A.CallTo(() => _buildingBlockRepository.SpatialStructureCollection).Returns(new List<MoBiSpatialStructure> { spatialStructure });
         A.CallTo(() => _buildingBlockRepository.MoleculeBlockCollection).Returns(new List<MoleculeBuildingBlock> { new MoleculeBuildingBlock { molecule } });
         A.CallTo(() => _buildingBlockRepository.ReactionBlockCollection).Returns(new List<MoBiReactionBuildingBlock> { new MoBiReactionBuildingBlock { reaction } });
         A.CallTo(() => _buildingBlockRepository.EventBlockCollection).Returns(new List<EventGroupBuildingBlock> { new EventGroupBuildingBlock { eventGroup } });
         A.CallTo(() => _buildingBlockRepository.IndividualsCollection).Returns(new List<IndividualBuildingBlock> { individual });
         A.CallTo(() => _buildingBlockRepository.ExpressionProfileCollection).Returns(new List<ExpressionProfileBuildingBlock> { expressionProfile });
      }

      [Observation]
      public void should_resolve_parameters_in_spatial_structures()
      {
         Resolve("Organism", "Liver", "Volume").ShouldBeEqualTo(_volume);
      }

      [Observation]
      public void should_resolve_molecule_properties_defined_in_the_spatial_structure()
      {
         Resolve("Organism", "Liver", "Intracellular", "Drug", "LocalMoleculeProperty").ShouldBeEqualTo(_localMoleculeProperty);
         Resolve("Drug", "GlobalMoleculeProperty").ShouldBeEqualTo(_globalMoleculeProperty);
      }

      [Observation]
      public void should_resolve_parameters_defined_in_molecules()
      {
         Resolve("Organism", "Liver", "Intracellular", "Drug", "LocalMoleculeParameter").ShouldBeEqualTo(_localMoleculeParameter);
         Resolve("Drug", "GlobalMoleculeParameter").ShouldBeEqualTo(_globalMoleculeParameter);
      }

      [Observation]
      public void should_resolve_global_reaction_parameters()
      {
         Resolve("R1", "k").ShouldBeEqualTo(_reactionParameter);
      }

      [Observation]
      public void should_resolve_event_parameters_under_the_events_container()
      {
         Resolve(Constants.EVENTS, "Application", "Dose").ShouldBeEqualTo(_eventParameter);
      }

      [Observation]
      public void should_resolve_individual_and_expression_parameters()
      {
         Resolve("Organism", "Weight").ShouldBeEqualTo(_individualParameter);
         Resolve("Organism", "Liver", "Intracellular", "CYP3A4", "Relative expression").ShouldBeEqualTo(_expressionParameter);
      }

      [Observation]
      public void should_return_null_when_the_path_cannot_be_resolved()
      {
         Resolve("Organism", "Kidney", "Volume").ShouldBeNull();
         Resolve("Organism", "Kidney", "Drug", "LocalMoleculeParameter").ShouldBeNull();
         Resolve("Volume").ShouldBeNull();
      }
   }
}
