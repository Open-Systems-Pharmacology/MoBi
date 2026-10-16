using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.HelpersForTests;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using IBuildingBlockRepository = MoBi.Core.Domain.Repository.IBuildingBlockRepository;

namespace MoBi.Core.Service
{
   public abstract class concern_for_InitialConditionPathResolver : ContextSpecification<InitialConditionPathResolver>
   {
      protected IBuildingBlockRepository _buildingBlockRepository;

      protected override void Context()
      {
         _buildingBlockRepository = A.Fake<IBuildingBlockRepository>();
         sut = new InitialConditionPathResolver(_buildingBlockRepository, new ObjectPathFactoryForSpecs());
      }

      protected MoleculeBuilder Resolve(params string[] path) => sut.Resolve(new ObjectPath(path), null);
   }

   public class When_resolving_initial_condition_paths_in_the_building_blocks_of_the_project : concern_for_InitialConditionPathResolver
   {
      private MoleculeBuilder _drug;

      protected override void Context()
      {
         base.Context();
         _drug = new MoleculeBuilder().WithName("Drug");

         var organism = new Container().WithName("Organism").WithMode(ContainerMode.Logical);
         organism.Add(new Container().WithName("Plasma").WithMode(ContainerMode.Physical));
         var spatialStructure = new MoBiSpatialStructure();
         spatialStructure.AddTopContainer(organism);

         var tumor = new Container { ParentPath = new ObjectPath("Organism") }.WithName("Tumor").WithMode(ContainerMode.Logical);
         tumor.Add(new Container().WithName("Interstitial").WithMode(ContainerMode.Physical));
         var extensionSpatialStructure = new MoBiSpatialStructure();
         extensionSpatialStructure.AddTopContainer(tumor);

         A.CallTo(() => _buildingBlockRepository.SpatialStructureCollection).Returns(new[] { spatialStructure, extensionSpatialStructure });
         A.CallTo(() => _buildingBlockRepository.MoleculeBlockCollection).Returns(new[] { new MoleculeBuildingBlock { _drug } });
      }

      [Observation]
      public void should_resolve_a_molecule_in_a_physical_container()
      {
         Resolve("Organism", "Plasma", "Drug").ShouldBeEqualTo(_drug);
      }

      [Observation]
      public void should_resolve_a_molecule_in_a_physical_container_placed_under_a_parent_path()
      {
         Resolve("Organism", "Tumor", "Interstitial", "Drug").ShouldBeEqualTo(_drug);
      }

      [Observation]
      public void should_not_resolve_a_molecule_in_a_logical_container()
      {
         Resolve("Organism", "Drug").ShouldBeNull();
      }

      [Observation]
      public void should_not_resolve_a_molecule_in_an_unknown_container()
      {
         Resolve("Organism", "Kidney", "Drug").ShouldBeNull();
      }

      [Observation]
      public void should_not_resolve_an_unknown_molecule()
      {
         Resolve("Organism", "Plasma", "Unknown").ShouldBeNull();
      }

      [Observation]
      public void should_not_resolve_a_path_without_a_container()
      {
         Resolve("Drug").ShouldBeNull();
      }
   }

   public class When_resolving_an_initial_condition_path_for_a_molecule_defined_in_several_modules : concern_for_InitialConditionPathResolver
   {
      private MoleculeBuilder _drugInFirstModule;
      private MoleculeBuilder _drugInSecondModule;
      private Module _secondModule;

      protected override void Context()
      {
         base.Context();
         _drugInFirstModule = new MoleculeBuilder().WithName("Drug");
         _drugInSecondModule = new MoleculeBuilder().WithName("Drug");

         var organism = new Container().WithName("Organism").WithMode(ContainerMode.Logical);
         organism.Add(new Container().WithName("Plasma").WithMode(ContainerMode.Physical));
         var spatialStructure = new MoBiSpatialStructure();
         spatialStructure.AddTopContainer(organism);

         var firstMolecules = new MoleculeBuildingBlock { _drugInFirstModule };
         var secondMolecules = new MoleculeBuildingBlock { _drugInSecondModule };
         _ = new Module { firstMolecules };
         _secondModule = new Module { secondMolecules };

         A.CallTo(() => _buildingBlockRepository.SpatialStructureCollection).Returns(new[] { spatialStructure });
         A.CallTo(() => _buildingBlockRepository.MoleculeBlockCollection).Returns(new[] { firstMolecules, secondMolecules });
      }

      [Observation]
      public void should_resolve_the_molecule_of_the_given_module_first()
      {
         sut.Resolve(new ObjectPath("Organism", "Plasma", "Drug"), _secondModule).ShouldBeEqualTo(_drugInSecondModule);
      }

      [Observation]
      public void should_resolve_the_first_molecule_found_when_the_given_module_does_not_define_it()
      {
         sut.Resolve(new ObjectPath("Organism", "Plasma", "Drug"), new Module()).ShouldBeEqualTo(_drugInFirstModule);
      }
   }
}