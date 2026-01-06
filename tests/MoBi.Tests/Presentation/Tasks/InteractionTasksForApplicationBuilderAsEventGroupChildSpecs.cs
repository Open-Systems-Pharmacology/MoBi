using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Descriptors;

namespace MoBi.Presentation.Tasks
{
   internal abstract class concern_for_InteractionTasksForApplicationBuilderAsEventGroupChild : ContextSpecification<InteractionTasksForApplicationBuilderAsEventGroupChild>
   {
      private IEditTaskFor<ApplicationBuilder> _editTask;
      protected IInteractionTaskContext _interactionTaskContext;

      protected override void Context()
      {
         // set up fakes
         _editTask = A.Fake<IEditTaskFor<ApplicationBuilder>>();
         _interactionTaskContext = A.Fake<InteractionTaskContext>();

         sut = new InteractionTasksForApplicationBuilderAsEventGroupChild(_interactionTaskContext, _editTask);
      }
   }

   internal class When_adding_a_new_application_builder_with_colliding_name : concern_for_InteractionTasksForApplicationBuilderAsEventGroupChild
   {
      private ApplicationBuilder _addedApplicationBuilder;
      private EventGroupBuilder _parentEventGroupBuilder;
      private EventGroupBuildingBlock _buildingBlock;
      private IContainer _container;
      private ApplicationBuilder _existingApplicationBuilder;
      private TransportBuilder _newTransport;
      private ApplicationMoleculeBuilder _applicationMoleculeBuilder;

      protected override void Context()
      {
         base.Context();

         _addedApplicationBuilder = new ApplicationBuilder().WithName("application_1");

         _applicationMoleculeBuilder = new ApplicationMoleculeBuilder().WithName("application_1");
         _applicationMoleculeBuilder.RelativeContainerPath = new ObjectPath("..", "application_1");

         _addedApplicationBuilder.AddMolecule(_applicationMoleculeBuilder);
         _addedApplicationBuilder.Tags.Add(new Tag("application_1"));

         _existingApplicationBuilder = new ApplicationBuilder().WithName("application_1");
         _existingApplicationBuilder.Tags.Add(new Tag("application_1"));

         _container = new Container();
         _container.Tags.Add(new Tag("application_1"));
         _addedApplicationBuilder.Add(_container);

         _newTransport = new TransportBuilder().WithName("transport_1");
         _newTransport.SourceCriteria.Add(new MatchTagCondition("application_1"));
         _newTransport.TargetCriteria.Add(new MatchTagCondition("application_1"));
         _addedApplicationBuilder.AddTransport(_newTransport);

         _parentEventGroupBuilder = new EventGroupBuilder { _existingApplicationBuilder };
         _buildingBlock = new EventGroupBuildingBlock { _parentEventGroupBuilder };

         A.CallTo(() => _interactionTaskContext.InteractionTask.CorrectName(_addedApplicationBuilder, A<IEnumerable<string>>._)).Invokes(x => x.Arguments.Get<ApplicationBuilder>(0).Name = "application_2").Returns(true);
      }

      protected override void Because()
      {
         sut.AddTo(_addedApplicationBuilder, _parentEventGroupBuilder, _buildingBlock);
      }

      [Observation]
      public void the_builder_is_added_to_the_event_group()
      {
         _parentEventGroupBuilder.Contains(_addedApplicationBuilder).ShouldBeTrue();
      }

      [Observation]
      public void the_builder_has_been_renamed()
      {
         _addedApplicationBuilder.Name.ShouldBeEqualTo("application_2");
      }

      [Observation]
      public void tags_that_match_the_name_should_be_changed_in_the_added_application()
      {
         _addedApplicationBuilder.Tags.Contains("application_2").ShouldBeTrue();
         _addedApplicationBuilder.Tags.Contains("application_1").ShouldBeFalse();
         _container.Tags.Contains("application_2").ShouldBeTrue();
         _container.Tags.Contains("application_1").ShouldBeFalse();
      }

      [Observation]
      public void transport_builders_with_container_criteria_matching_tags_are_renamed()
      {
         _newTransport.SourceCriteria.Single().Tag.ShouldBeEqualTo("application_2");
         _newTransport.TargetCriteria.Single().Tag.ShouldBeEqualTo("application_2");
      }

      [Observation]
      public void application_molecule_builder_name_is_changed()
      {
         _applicationMoleculeBuilder.Name.ShouldBeEqualTo("application_2");
      }

      [Observation]
      public void application_molecule_builder_relative_path_is_updated()
      {
         _applicationMoleculeBuilder.RelativeContainerPath.PathAsString.ShouldBeEqualTo(new ObjectPath("..", "application_2").PathAsString);
      }

      [Observation]
      public void tags_should_not_be_renamed_in_existing_application()
      {
         _existingApplicationBuilder.Tags.Contains("application_2").ShouldBeFalse();
         _existingApplicationBuilder.Tags.Contains("application_1").ShouldBeTrue();
      }
   }
}