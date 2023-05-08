using System.Linq;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Mapper
{
   public class concern_for_ModuleConfigurationDTOToModuleConfigurationMapper : ContextSpecification<ModuleConfigurationDTOToModuleConfigurationMapper>
   {
      protected Module _module;

      protected override void Context()
      {
         _module = new Module
         {
            new InitialConditionsBuildingBlock(),
            new ParameterValuesBuildingBlock()
         };

         sut = new ModuleConfigurationDTOToModuleConfigurationMapper();
      }
   }

   public class When_mapping_from_dto_with_selected_start_values : concern_for_ModuleConfigurationDTOToModuleConfigurationMapper
   {
      private ModuleConfigurationDTO _dto;
      private ModuleConfiguration _result;

      protected override void Context()
      {
         base.Context();

         _dto = new ModuleConfigurationDTO(new ModuleConfiguration(_module))
         {
            SelectedInitialConditions = _module.InitialConditionsCollection.First(),
            SelectedParameterValues = _module.ParameterValuesCollection.First()
         };
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_dto);
      }

      [Observation]
      public void should_return_a_module_configuration_with_real_molecule_start_values()
      {
         _result.SelectedInitialConditions.ShouldBeEqualTo(_module.InitialConditionsCollection.First());
      }

      [Observation]
      public void should_return_a_module_configuration_with_real_parameter_start_values()
      {
         _result.SelectedParameterValues.ShouldBeEqualTo(_module.ParameterValuesCollection.First());
      }
   }

   public class When_mapping_from_dto_with_null_selected_start_values : concern_for_ModuleConfigurationDTOToModuleConfigurationMapper
   {
      private ModuleConfigurationDTO _dto;
      private ModuleConfiguration _result;

      protected override void Context()
      {
         base.Context();
         var module = new Module
         {
            new InitialConditionsBuildingBlock(),
            new ParameterValuesBuildingBlock()
         };
         _dto = new ModuleConfigurationDTO(new ModuleConfiguration(module))
         {
            SelectedInitialConditions = NullStartValues.NullInitialConditions,
            SelectedParameterValues = NullStartValues.NullParameterValues
         };
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_dto);
      }

      [Observation]
      public void should_return_a_module_configuration_with_null_molecule_start_values()
      {
         _result.SelectedInitialConditions.ShouldBeNull();
      }

      [Observation]
      public void should_return_a_module_configuration_with_null_parameter_start_values()
      {
         _result.SelectedParameterValues.ShouldBeNull();
      }
   }
}