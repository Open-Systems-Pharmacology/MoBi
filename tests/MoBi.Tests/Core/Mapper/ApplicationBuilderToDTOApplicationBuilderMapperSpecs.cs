using System.Linq;
using OSPSuite.Utility.Container;
using FakeItEasy;

using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using MoBi.IntegrationTests;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;


namespace MoBi.Core.Mapper
{
 
   public abstract class concern_for_ApplicationBuilderToDTOApplicationBuilderMapperIntegrationTests : ContextForIntegration<IApplicationBuilderToDTOApplicationBuilderMapper>
   {
      protected override void Context()
      {
         sut = IoC.Resolve<IApplicationBuilderToDTOApplicationBuilderMapper>();
      }
   }

   class When_mapping_an_application_builder_to_the_dto : concern_for_ApplicationBuilderToDTOApplicationBuilderMapperIntegrationTests
   {
      private ApplicationBuilderDTO _resultEventGroupBuilderDTO;
      private IApplicationBuilder _applicationBuilder;

      protected override void Context()
      {
         base.Context();
         _applicationBuilder = new ApplicationBuilder().WithName("Application").WithId("App");
         _applicationBuilder.MoleculeName = "Drug";
         _applicationBuilder.Add(new TransportBuilder().WithName("Trans"));
         _applicationBuilder.AddMolecule(new ApplicationMoleculeBuilder().WithName("Drug"));
         _applicationBuilder.Add(new Container().WithName("Protocol Schema Item"));
         _applicationBuilder.Add(new EventGroupBuilder().WithName("EG"));
         _applicationBuilder.Add(new Parameter().WithName("P1"));
         _applicationBuilder.Add(new ApplicationBuilder().WithName("App2"));
      }


      protected override void Because()
      {
        _resultEventGroupBuilderDTO = sut.MapFrom(_applicationBuilder);
      }

      [Observation]
      public void should_return_right_initialised_dto()
      {
         _resultEventGroupBuilderDTO.ShouldNotBeNull();
         _resultEventGroupBuilderDTO.Name.ShouldBeEqualTo(_applicationBuilder.Name);
      }

      [Observation]
      public void should_set_up_the_right_child_dtos()
      {
         _resultEventGroupBuilderDTO.Transports.Count().ShouldBeEqualTo(1);
         _resultEventGroupBuilderDTO.ChildContainer.Count().ShouldBeEqualTo(1);
         _resultEventGroupBuilderDTO.EventGroups.Count().ShouldBeEqualTo(1);
         _resultEventGroupBuilderDTO.Molecules.Count().ShouldBeEqualTo(1);
         _resultEventGroupBuilderDTO.Applications.Count().ShouldBeEqualTo(1);
         _resultEventGroupBuilderDTO.Parameters.Count().ShouldBeEqualTo(1);
      }
   }
}	