using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using MoBi.Core.Services;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Service
{
   public abstract class concern_for_ParameterResolver : ContextSpecification<ParameterResolver>
   {
      protected IMoleculeBuildingBlock _moleculeBuildingBlock;
      protected ISpatialStructure _spatialStructure;
      protected ObjectPath _containerPath;

      protected override void Context()
      {
         sut = new ParameterResolver();
         _moleculeBuildingBlock = new MoleculeBuildingBlock();
         _spatialStructure = new SpatialStructure {GlobalMoleculeDependentProperties = new Container()};
         _containerPath = new ObjectPath();
      }
   }

   public class When_resolving_molecule_properties_in_spatial_structure : concern_for_ParameterResolver
   {
      private IParameter _result;

      protected override void Context()
      {
         base.Context();
         _spatialStructure.GlobalMoleculeDependentProperties.Add(new Parameter{Name="GMDPParameter"});
      }

      protected override void Because()
      {
         _result = sut.Resolve(ObjectPath.Empty, "GMDPParameter", _spatialStructure, _moleculeBuildingBlock);
      }

      [Observation]
      public void resolves_molecule_parameter_in_spatial_structure_global_parameters()
      {
         _result.Name.ShouldBeEqualTo("GMDPParameter");
      }
   }

   public class When_resolving_molecule_parameter_in_spatial_structure_and_container_cannot_be_resolved : concern_for_ParameterResolver
   {
      private IParameter _result;

      protected override void Context()
      {
         base.Context();

         var subContainer = new Container { Name = Constants.MOLECULE_PROPERTIES };
         subContainer.Add(new Parameter { Name = "moleculeParameter" });

         var container = new Container { Name = "topContainer" };
         container.Add(subContainer);

         _spatialStructure.AddTopContainer(container);
      }

      protected override void Because()
      {
         _result = sut.Resolve(new ObjectPath("topContainer", "subcontainer", "C1"), "moleculeParameter", _spatialStructure, _moleculeBuildingBlock);
      }

      [Observation]
      public void does_not_resolve_the_molecule_parameter()
      {
         _result.ShouldBeNull();
      }
   }

   public class When_resolving_molecule_parameter_in_spatial_structure_and_container_can_be_resolved : concern_for_ParameterResolver
   {
      private IParameter _result;

      protected override void Context()
      {
         base.Context();

         var subContainer = new Container {Name = Constants.MOLECULE_PROPERTIES};
         subContainer.Add(new Parameter{Name="moleculeParameter"});

         var container = new Container {Name = "topContainer"};
         container.Add(subContainer);

         _spatialStructure.AddTopContainer(container);
      }

      protected override void Because()
      {
         _result = sut.Resolve(new ObjectPath("topContainer", "C1"), "moleculeParameter", _spatialStructure, _moleculeBuildingBlock);
      }

      [Observation]
      public void resolves_molecule_parameter()
      {
         _result.Name.ShouldBeEqualTo("moleculeParameter");
      }
   }

   public class When_resolving_container_parameter_in_spatial_structure : concern_for_ParameterResolver
   {
      private IParameter _result;

      protected override void Context()
      {
         base.Context();
         var container = new Container {Mode = ContainerMode.Physical, Name = "containerName"};

         var subContainer = new Container {Mode = ContainerMode.Physical, Name = "subContainerName"};

         subContainer.Add(new Parameter { Name = "containerParameter" });
         container.Add(subContainer);
         _spatialStructure.AddTopContainer(container);
      }

      protected override void Because()
      {
         _result = sut.Resolve(new ObjectPath("containerName", "subContainerName"), "containerParameter", _spatialStructure, _moleculeBuildingBlock);
      }

      [Observation]
      public void resolves_container_parameter()
      {
         _result.Name.ShouldBeEqualTo("containerParameter");
      }
   }

   public class When_resolving_molecule_builder_global_parameters : concern_for_ParameterResolver
   {
      private IParameter _result;

      protected override void Context()
      {
         base.Context();
         var builder = new MoleculeBuilder { Name = "C1" };
         builder.AddParameter(new Parameter { BuildMode = ParameterBuildMode.Global, Name = "localParameter" });

         _moleculeBuildingBlock.Add(builder);
      }

      protected override void Because()
      {
         _result = sut.Resolve(new ObjectPath("C1"), "localParameter", _spatialStructure, _moleculeBuildingBlock);
      }

      [Observation]
      public void resolves_local_molecule_parameter()
      {
         _result.Name.ShouldBeEqualTo("localParameter");
      }
   }

   public class When_resolving_parameter_in_an_empty_container: concern_for_ParameterResolver
   {
      private IParameter _result;

      protected override void Because()
      {
         _result = sut.Resolve(new ObjectPath(), "", _spatialStructure, _moleculeBuildingBlock);
      }

      [Observation]
      public void should_not_crash()
      {
         _result.ShouldBeNull();
      }
   }
   public class When_resolving_molecule_builder_local_parameters_and_the_container_exists_in_the_spatial_structure : concern_for_ParameterResolver
   {
      private IParameter _result;

      protected override void Context()
      {
         base.Context();
         var container = new Container { Mode = ContainerMode.Physical, Name = "containerName" };

         var subContainer = new Container { Mode = ContainerMode.Physical, Name = "subContainerName" };

         subContainer.Add(new Parameter { Name = "containerParameter" });
         container.Add(subContainer);
         _spatialStructure.AddTopContainer(container);

         var builder = new MoleculeBuilder { Name = "C1" };
         builder.AddParameter(new Parameter { BuildMode = ParameterBuildMode.Local, Name = "localParameter" });

         _moleculeBuildingBlock.Add(builder);
      }

      protected override void Because()
      {
         _result = sut.Resolve(new ObjectPath("containerName", "subContainerName", "C1"), "localParameter", _spatialStructure, _moleculeBuildingBlock);
      }

      [Observation]
      public void the_parameter_should_resolve_the_original_molecule_parameter()
      {
         _result.Name.ShouldBeEqualTo("localParameter");
      }
   }

   public class When_resolving_molecule_builder_local_parameters_When_the_container_path_is_invalid : concern_for_ParameterResolver
   {
      private IParameter _result;

      protected override void Context()
      {
         base.Context();
         var builder = new MoleculeBuilder {Name="C1"};
         builder.AddParameter(new Parameter {BuildMode = ParameterBuildMode.Local, Name = "localParameter"});
         
         _moleculeBuildingBlock.Add(builder);
      }

      protected override void Because()
      {
         _result = sut.Resolve(new ObjectPath("a_container", "C1"), "localParameter", _spatialStructure, _moleculeBuildingBlock);
      }

      [Observation]
      public void cannot_resolve_the_parameter()
      {
         _result.ShouldBeNull();
      }
   }
}
