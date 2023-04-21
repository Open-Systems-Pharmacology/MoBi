using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;

namespace MoBi.Presentation.Mapper
{
   public class concern_for_ModuleConfigurationToModuleConfigurationDTOMapper : ContextSpecification<ModuleConfigurationToModuleConfigurationDTOMapper>
   {
      protected IMoBiProjectRetriever _projectRetriever;

      protected override void Context()
      {
         _projectRetriever = A.Fake<IMoBiProjectRetriever>();
         sut = new ModuleConfigurationToModuleConfigurationDTOMapper(_projectRetriever);
      }
   }

   public class When_mapping_project_module_configurations_to_dto : concern_for_ModuleConfigurationToModuleConfigurationDTOMapper
   {
      private ModuleConfigurationDTO _result;
      private ModuleConfiguration _existingConfiguration;
      private MoBiProject _project;
      private Module _projectModule;

      protected override void Context()
      {
         base.Context();
         _projectModule = new Module().WithName("common name");
         _project = new MoBiProject();
         _project.AddModule(_projectModule);
         A.CallTo(() => _projectRetriever.Current).Returns(_project);
         _existingConfiguration = new ModuleConfiguration(_projectModule);
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_existingConfiguration);
      }

      [Observation]
      public void should_return_a_dto_with_module_configuration_from_the_project()
      {
         _result.ModuleConfiguration.ShouldBeEqualTo(_existingConfiguration);
         _result.ModuleConfiguration.Module.ShouldBeEqualTo(_projectModule);
      }
   }

   public class When_mapping_simulation_module_configurations_to_dto : concern_for_ModuleConfigurationToModuleConfigurationDTOMapper
   {
      private ModuleConfigurationDTO _result;
      private ModuleConfiguration _existingConfiguration;
      private Module _existingModule;
      private MoBiProject _project;
      private Module _projectModule;

      protected override void Context()
      {
         base.Context();
         _existingModule = new Module().WithName("common name");
         _projectModule = new Module().WithName("common name");
         _project = new MoBiProject();
         _project.AddModule(_projectModule);
         A.CallTo(() => _projectRetriever.Current).Returns(_project);
         _existingConfiguration = new ModuleConfiguration(_existingModule);
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_existingConfiguration);
      }

      [Observation]
      public void should_return_a_dto_with_module_configuration_from_the_project()
      {
         _result.ModuleConfiguration.ShouldNotBeEqualTo(_existingConfiguration);
         _result.ModuleConfiguration.Module.ShouldBeEqualTo(_projectModule);
      }
   }
}