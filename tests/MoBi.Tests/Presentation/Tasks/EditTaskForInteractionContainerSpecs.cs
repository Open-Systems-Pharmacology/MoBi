using System.Collections.Generic;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands.Core;
using FakeItEasy;
using MoBi.Core.Commands;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Presentation.Tasks
{
   public abstract class concern_for_EditTaskForInteractionContainerSpecs : ContextSpecification<EditTaskForInteractionContainer>
   {
      protected IInteractionTaskContext _interactionTaskContext;

      protected override void Context()
      {
         _interactionTaskContext = A.Fake<IInteractionTaskContext>();
         sut = new EditTaskForInteractionContainer(_interactionTaskContext);
      }
   }

   internal class When_creating_an_interaction_container : concern_for_EditTaskForInteractionContainerSpecs
   {
      private InteractionContainer _newInteractionContainer;
      private ICommandCollector _commands;
      private IBuildingBlock _buildingBlock;
      private IModalPresenter _modalPresenter;

      protected override void Context()
      {
         base.Context();
         _newInteractionContainer = new InteractionContainer().WithParentContainer(new MoleculeBuilder());
         _buildingBlock = new MoleculeBuildingBlock();
         _commands = new MoBiMacroCommand();
         _modalPresenter = A.Fake<IModalPresenter>();
         A.CallTo(() => _modalPresenter.SubPresenter).Returns(A.Fake<ICreatePresenter<IContainer>>());
         A.CallTo(() => _interactionTaskContext.ApplicationController.GetCreateViewFor<IContainer>(_newInteractionContainer, _commands)).Returns(_modalPresenter);
      }

      protected override void Because()
      {
         sut.EditEntityModal(_newInteractionContainer, _newInteractionContainer.ParentContainer, _commands, _buildingBlock);
      }

      [Observation]
      public void should_get_a_edit_presenter_for_container()
      {
         A.CallTo(() => _modalPresenter.Show(null)).MustHaveHappened();
      }
   }

   internal class When_asking_for_forbidden_names : concern_for_EditTaskForInteractionContainerSpecs
   {
      private InteractionContainer _interactionContainer;
      private MoleculeBuilder _parent;
      private Parameter _parameter;
      private TransporterMoleculeContainer _transporterMolecule;
      private IEnumerable<string> _forbiidenNames;

      protected override void Context()
      {
         base.Context();
         _parent = new MoleculeBuilder().WithName("Drug");
         _interactionContainer = new InteractionContainer().WithParentContainer(_parent);
         _parameter = new Parameter().WithName("P1").WithParentContainer(_parent);
         _transporterMolecule = new TransporterMoleculeContainer().WithName("PGP").WithParentContainer(_parent);
      }

      protected override void Because()
      {
         _forbiidenNames = sut.GetForbiddenNames(_interactionContainer, _parent);
      }

      [Observation]
      public void Forbidden_names_should_contain_all_children_names()
      {
         _forbiidenNames.ShouldContain(_parameter.Name, _transporterMolecule.Name);
      }
   }
}