using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using NUnit.Framework;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using IRegisterAllVisitor = MoBi.Core.Domain.Services.IRegisterAllVisitor;

namespace MoBi.Core
{
   public abstract class concern_for_BuildConfigurationFactory : ContextSpecification<IBuildConfigurationFactory>
   {
      private IRegisterAllVisitor _registerAllVisitor;
      private ICloneManagerForBuildingBlock _cloneManager;
      private ICoreCalculationMethodRepository _calculationMethodRepository;

      protected override void Context()
      {
         _cloneManager = A.Fake<ICloneManagerForBuildingBlock>();
         _registerAllVisitor = A.Fake<IRegisterAllVisitor>();
         _calculationMethodRepository = A.Fake<ICoreCalculationMethodRepository>();
         sut = new BuildConfigurationFactory(_registerAllVisitor, _cloneManager, _calculationMethodRepository);
      }
   }

   public class When_creating_a_build_configuration_from_an_existing_build_configuration_using_the_existing_building_block : concern_for_BuildConfigurationFactory
   {
      private IMoBiBuildConfiguration _newConfiguration;
      private IMoBiBuildConfiguration _buildConfiguration;

      protected override void Because()
      {
         _buildConfiguration = A.Fake<IMoBiBuildConfiguration>();
         A.CallTo(() => _buildConfiguration.ObserversInfo.BuildingBlockChanged).Returns(true);
         _newConfiguration = sut.CreateFromReferencesUsedIn(_buildConfiguration);
      }

      [Observation]
      public void should_simply_use_the_existing_references()
      {
         _newConfiguration.ObserversInfo.TemplateBuildingBlock.ShouldBeEqualTo(_buildConfiguration.ObserversInfo.TemplateBuildingBlock);
         _newConfiguration.ObserversInfo.BuildingBlock.ShouldBeEqualTo(_buildConfiguration.ObserversInfo.BuildingBlock);
      }
   }

   public class Whenen_creating_a_build_configuration_from_an_existing_build_configuration_that_should_override_the_used_building_block_with_a_template_one : concern_for_BuildConfigurationFactory
   {
      private IMoBiBuildConfiguration _newConfiguration;
      private IMoBiBuildConfiguration _buildConfiguration;
      private IObserverBuildingBlock _templateBuildingBlock;

      protected override void Context()
      {
         base.Context();
         _buildConfiguration = A.Fake<IMoBiBuildConfiguration>();
         _templateBuildingBlock = new ObserverBuildingBlock().WithId("TOTO").WithName("TOTO");
         _buildConfiguration.ObserversInfo = new ObserverBuildingBlockInfo
         {
            TemplateBuildingBlock = _templateBuildingBlock,
            SimulationChanges = 1
         };
      }

      protected override void Because()
      {
         _newConfiguration = sut.CreateFromReferencesUsedIn(_buildConfiguration, _templateBuildingBlock);
      }

      [Observation]
      public void should_simply_use_the_existing_references()
      {
         Assert.AreSame(_newConfiguration.ObserversInfo.TemplateBuildingBlock, _templateBuildingBlock);
         Assert.AreSame(_newConfiguration.ObserversInfo.BuildingBlock, _templateBuildingBlock);
      }
   }

   public class Whenen_creating_a_build_configuration_from_an_existing_build_configuration_that_should_not_override_the_used_building_block_with_a_template_one : concern_for_BuildConfigurationFactory
   {
      private IMoBiBuildConfiguration _newConfiguration;
      private IMoBiBuildConfiguration _buildConfiguration;
      private IObserverBuildingBlock _templateBuildingBlock;
      private IObserverBuildingBlock _usedBuildingBlock;

      protected override void Context()
      {
         base.Context();
         _buildConfiguration = A.Fake<IMoBiBuildConfiguration>();
         _templateBuildingBlock = new ObserverBuildingBlock().WithId("TOTO").WithName("TOTO");
         _usedBuildingBlock = new ObserverBuildingBlock().WithId("TOTO IN SIM").WithName("TOTO IN SIM");
         _buildConfiguration.ObserversInfo = new ObserverBuildingBlockInfo
         {
            TemplateBuildingBlock = _templateBuildingBlock,
            BuildingBlock = _usedBuildingBlock,
            SimulationChanges = 1
         };
      }

      protected override void Because()
      {
         _newConfiguration = sut.CreateFromReferencesUsedIn(_buildConfiguration);
      }

      [Observation]
      public void should_simply_use_the_existing_references()
      {
         Assert.AreSame(_newConfiguration.ObserversInfo.TemplateBuildingBlock, _templateBuildingBlock);
         Assert.AreSame(_newConfiguration.ObserversInfo.BuildingBlock, _usedBuildingBlock);
      }
   }

   public class When_creating_a_build_configuration_from_an_existing_build_configuration_overriding_the_existing_building_block_with_templates : concern_for_BuildConfigurationFactory
   {
      private IMoBiBuildConfiguration _newConfiguration;
      private IMoBiBuildConfiguration _buildConfiguration;

      protected override void Because()
      {
         _buildConfiguration = A.Fake<IMoBiBuildConfiguration>();
         _newConfiguration = sut.CreateFromReferencesUsedIn(_buildConfiguration);
      }

      [Observation]
      public void should_simply_use_the_existing_references()
      {
         Assert.AreSame(_newConfiguration.ObserversInfo.TemplateBuildingBlock, _buildConfiguration.ObserversInfo.TemplateBuildingBlock);
         Assert.AreSame(_newConfiguration.ObserversInfo.BuildingBlock, _buildConfiguration.ObserversInfo.TemplateBuildingBlock);
      }
   }
}