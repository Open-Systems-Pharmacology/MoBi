using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using MoBi.Core.Services;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Service
{
   public abstract class concern_for_MoleculeResolver : ContextSpecification<MoleculeResolver>
   {
      private IMoleculeBuildingBlock _moleculeBuildingBlock;
      private ISpatialStructure _spatialStructure;
      private MoleculeStartValue _moleculeStartValue;

      protected ContainerMode _containerMode = ContainerMode.Physical;
      protected string _firstPathEntry = "The";
      protected string _moleculeName = "name";
      protected MoleculeBuilder _builder;
      protected ObjectPath _containerPath;
      protected IMoleculeBuilder _result;

      protected override void Context()
      {
         _moleculeBuildingBlock = new MoleculeBuildingBlock { Id = "Id" };
         _builder = new MoleculeBuilder { Name = "name" };
         _moleculeBuildingBlock.Add(_builder);
         _spatialStructure = new SpatialStructure();
         var firstContainer = new Container { Name = _firstPathEntry };

         firstContainer.Add(new Container { Name = "Path", Mode = _containerMode });
         _spatialStructure.Add(firstContainer);
         _moleculeStartValue = new MoleculeStartValue { Name = _moleculeName, ContainerPath = new ObjectPath("The", "Path") };

         sut = new MoleculeResolver();
      }

      protected override void Because()
      {
         _result = sut.Resolve(_moleculeStartValue.ContainerPath, _moleculeStartValue.MoleculeName, _spatialStructure, _moleculeBuildingBlock);
      }
   }

   public class When_resolving_molecules_that_dont_have_an_existing_name_from_the_building_block : concern_for_MoleculeResolver
   {
      protected override void Context()
      {
         _moleculeName = "AnotherName";
         base.Context();
      }

      [Observation]
      public void should_not_resolve()
      {
         _result.ShouldBeNull();
      }
   }

   public class When_resolving_molecules_with_path_that_doesnt_exist : concern_for_MoleculeResolver
   {
      protected override void Context()
      {
         _firstPathEntry = "This";
         base.Context();
      }

      [Observation]
      public void should_not_resolve()
      {
         _result.ShouldBeNull();
      }
   }

   public class When_resolving_molecules_with_path_that_leads_to_logical_container : concern_for_MoleculeResolver
   {
      protected override void Context()
      {
         _containerMode = ContainerMode.Logical;
         base.Context();
      }

      [Observation]
      public void should_not_resolve()
      {
         _result.ShouldBeNull();
      }
   }

   public class When_resolving_molecules_with_paths_that_lead_to_physical_containers : concern_for_MoleculeResolver
   {
      [Observation]
      public void should_resolve_a_molecule_only_if_the_molecule_exists_and_the_spatial_structure_exists()
      {
         _result.ShouldBeEqualTo(_builder);
      }
   }
}
