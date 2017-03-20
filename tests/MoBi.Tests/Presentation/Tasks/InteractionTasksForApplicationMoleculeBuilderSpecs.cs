using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Presentation.Tasks
{
   public abstract class concern_for_InteractionTasksForApplicationMoleculeBuilder : ContextSpecification<InteractionTasksForChildren<IApplicationBuilder, IApplicationMoleculeBuilder>>
   {
      protected IIdGenerator _idGenerator;
      private IInteractionTaskContext _interactionTaskContext;
      private IEditTaskFor<IApplicationMoleculeBuilder> _editTask;

      protected override void Context()
      {
         _idGenerator = A.Fake<IIdGenerator>();
         _interactionTaskContext = A.Fake<IInteractionTaskContext>();
         _editTask = A.Fake<IEditTaskFor<IApplicationMoleculeBuilder>>();
         sut = new InteractionTasksForApplicationMoleculeBuilder(_interactionTaskContext, _editTask, _idGenerator);
      }
   }

   public class When_creating_a_new_application_molecule_builder : concern_for_InteractionTasksForApplicationMoleculeBuilder
   {
      private IApplicationBuilder _applicationBuilder;
      private IApplicationMoleculeBuilder _applicationMoleculeBuilder;

      protected override void Context()
      {
         base.Context();
         _applicationBuilder = A.Fake<IApplicationBuilder>();
         _applicationMoleculeBuilder = A.Fake<IApplicationMoleculeBuilder>();
         A.CallTo(() => _idGenerator.NewId()).Returns("XXX");
      }

      protected override void Because()
      {
         _applicationMoleculeBuilder = sut.CreateNewEntity(_applicationBuilder);
      }

      [Observation]
      public void should_set_a_dummy_name_that_will_be_hiiden_to_the_user()
      {
         string.IsNullOrEmpty(_applicationMoleculeBuilder.Name).ShouldBeFalse();
      }
   }
}