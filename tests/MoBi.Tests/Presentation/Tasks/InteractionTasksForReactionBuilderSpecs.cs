using System.Collections.Generic;
using System.Linq;
using OSPSuite.BDDHelper;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.UI.Diagram.DiagramManagers;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.Diagram.Elements;
using OSPSuite.UI.Diagram.Elements;

namespace MoBi.Presentation.Tasks
{
   public abstract class concern_for_InteractionTasksForReactionBuilder : ContextSpecification<InteractionTasksForReactionBuilder>
   {
      protected IReactionDimensionRetriever _reactionDimensionRetriever;
      protected IEditTaskFor<ReactionBuilder> _editTaskFor;
      protected IInteractionTaskContext _interactionTaskContext;
      protected IMoBiApplicationController _moBiApplicationController;
      protected IMultipleStringSelectionPresenter _multipleStringSelectionPresenter;

      protected override void Context()
      {
         _reactionDimensionRetriever = A.Fake<IReactionDimensionRetriever>();
         _editTaskFor = A.Fake<IEditTaskFor<ReactionBuilder>>();
         _interactionTaskContext = A.Fake<IInteractionTaskContext>();
         sut = new InteractionTasksForReactionBuilder(_interactionTaskContext, _editTaskFor, _reactionDimensionRetriever);

         _moBiApplicationController = A.Fake<IMoBiApplicationController>();
         _multipleStringSelectionPresenter = A.Fake<IMultipleStringSelectionPresenter>();
         A.CallTo(() => _interactionTaskContext.ApplicationController).Returns(_moBiApplicationController);
         A.CallTo(() => _moBiApplicationController.Start<IMultipleStringSelectionPresenter>()).Returns(_multipleStringSelectionPresenter);
      }
   }

   public class When_retrieving_valid_names_for_molecules : concern_for_InteractionTasksForReactionBuilder
   {
      private MoBiReactionBuildingBlock _reactionBuildingBlock;

      protected override void Context()
      {
         base.Context();
         _reactionBuildingBlock = new MoBiReactionBuildingBlock
         {
            DiagramManager = new MoBiReactionDiagramManager(),
            DiagramModel = new DiagramModel()
         };

         var builderToAdd = new ReactionBuilder();
         builderToAdd.AddEduct(new ReactionPartnerBuilder("unallowedMolecule", 1.0));
         _reactionBuildingBlock.Add(builderToAdd);

         builderToAdd = new ReactionBuilder();
         builderToAdd.AddEduct(new ReactionPartnerBuilder("allowedMolecule", 1.0));

         _reactionBuildingBlock.Add(builderToAdd);

         _reactionBuildingBlock.DiagramManager.InitializeWith(_reactionBuildingBlock, new DiagramOptions());
      }

      protected override void Because()
      {
         sut.SelectMoleculeNames(_reactionBuildingBlock, new List<string> {"unallowedMolecule"}, "reactionName", "Products");
      }

      [Observation]
      public void must_exclude_the_unallowed_names()
      {
         A.CallTo(() => _multipleStringSelectionPresenter.Show(A<string>._, A<string>._, A<IEnumerable<string>>.That.Matches(x => x.Contains("allowedMolecule") && !x.Contains("unallowedMolecule")), A<string>._, A<bool>._)).MustHaveHappened();
      }
   }
}