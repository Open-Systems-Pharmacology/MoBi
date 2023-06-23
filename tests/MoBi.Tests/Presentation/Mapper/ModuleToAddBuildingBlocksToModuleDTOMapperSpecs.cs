using FakeItEasy;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Presentation.Mapper
{
   public class concern_for_ModuleToAddBuildingBlocksToModuleDTOMapper : ContextSpecification<ModuleToAddBuildingBlocksToModuleDTOMapper>
   {
      private IContainerTask _containerTask;
      private IObjectPathFactory _objectPathFactory;
      private IEntityPathResolver _entityPathResolver;
      private IObjectBaseFactory _objectBaseFactory;

      protected override void Context()
      {
         _objectBaseFactory = A.Fake<IObjectBaseFactory>();
         _entityPathResolver = A.Fake<IEntityPathResolver>();
         _objectPathFactory = A.Fake<IObjectPathFactory>();
         
         _containerTask = new ContainerTask(_objectBaseFactory, _entityPathResolver, _objectPathFactory);
         sut = new ModuleToAddBuildingBlocksToModuleDTOMapper(_containerTask);
      }
   }

   public class When_mapping_a_dto_from_module_with_start_values_that_have_default_names : concern_for_ModuleToAddBuildingBlocksToModuleDTOMapper
   {
      private Module _module;
      private AddBuildingBlocksToModuleDTO _dto;

      protected override void Context()
      {
         base.Context();
         _module = new Module
         {
            new ParameterValuesBuildingBlock().WithName(DefaultNames.ParameterValues),
            new InitialConditionsBuildingBlock().WithName(DefaultNames.InitialConditions)
         };
      }

      protected override void Because()
      {
         _dto = sut.MapFrom(_module);
      }

      [Observation]
      public void the_dto_should_have_deselected_start_values()
      {
         _dto.WithParameterValues.ShouldBeFalse();
         _dto.WithInitialConditions.ShouldBeFalse();
      }

      [Observation]
      public void the_suggested_name_should_not_be_the_default()
      {
         _dto.InitialConditionsName.ShouldNotBeEqualTo(DefaultNames.InitialConditions);
         _dto.ParameterValuesName.ShouldNotBeEqualTo(DefaultNames.ParameterValues);
      }
   }
}