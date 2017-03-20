using System.Collections.Generic;
using OSPSuite.BDDHelper;
using FakeItEasy;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.UICommand;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;

namespace MoBi.Presentation
{
   public abstract class concern_for_AddProductUICommand : ContextSpecification<AddProductUICommand>
   {
      protected IInteractionTasksForReactionBuilder _interactionTasksForReactionBuilder;
      protected IActiveSubjectRetriever _activeSubjectRetriever;
      protected IMoBiContext _moBiContext;
      protected IMoBiReactionBuildingBlock _moBiReactionBuildingBlock;
      private IReactionBuilder _reactionBuilder;

      protected override void Context()
      {
         _interactionTasksForReactionBuilder = A.Fake<IInteractionTasksForReactionBuilder>();
         _activeSubjectRetriever = A.Fake<IActiveSubjectRetriever>();
         _moBiContext = A.Fake<IMoBiContext>();
         _reactionBuilder = new ReactionBuilder();

         sut = new AddProductUICommand(_moBiContext, _activeSubjectRetriever, _interactionTasksForReactionBuilder);
         sut.For(_reactionBuilder);

         _moBiReactionBuildingBlock = A.Fake<IMoBiReactionBuildingBlock>();
         A.CallTo(() => _activeSubjectRetriever.Active<IMoBiReactionBuildingBlock>()).Returns(_moBiReactionBuildingBlock);
      }
   }

   public class When_adding_an_product_to_a_reaction : concern_for_AddProductUICommand
   {
      protected override void Because()
      {
         sut.Execute();
      }

      [Observation]
      public void the_interaction_task_retrieves_the_name_of_possible_product_partners()
      {
         A.CallTo(() => _interactionTasksForReactionBuilder.SelectMoleculeNames(_moBiReactionBuildingBlock, A<IEnumerable<string>>._, A<string>._, AppConstants.Captions.Products)).MustHaveHappened();
      }
   }
}