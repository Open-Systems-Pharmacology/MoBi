using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.UI.Diagram.DiagramManagers;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.Diagram.Elements;
using OSPSuite.UI.Diagram.Elements;
using OSPSuite.Utility.Extensions;

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

   public class When_loading_a_reaction_that_is_renamed_and_parameter_formulas_reference_the_name : concern_for_InteractionTasksForReactionBuilder
   {
      private MoBiReactionBuildingBlock _buildingBlock;
      private IReadOnlyCollection<ReactionBuilder> _itemsToAdd;
      private Parameter _parameter;
      private ReactionBuilder _reactionBuilder;

      protected override void Context()
      {
         base.Context();
         _buildingBlock = new MoBiReactionBuildingBlock
         {
            DiagramManager = new MoBiReactionDiagramManager()
         };
         _reactionBuilder = new ReactionBuilder
         {
            Formula = new ExplicitFormula("5"),
            Name = "oldName"
         };
         _reactionBuilder.Formula.AddObjectPath(new FormulaUsablePath("oldName"));
         _parameter = new Parameter
         {
            Formula = new ExplicitFormula("5")
         };
         _parameter.Formula.AddObjectPath(new FormulaUsablePath("oldName"));
         _reactionBuilder.AddParameter(_parameter);

         // add a parameter without a formula
         _reactionBuilder.AddParameter(new Parameter().WithName("no_formula"));

         _itemsToAdd = new List<ReactionBuilder>
         {
            _reactionBuilder,
         };

         A.CallTo(() => _interactionTaskContext.InteractionTask.CorrectName(_reactionBuilder, A<IEnumerable<string>>._)).Invokes(x => x.Arguments.Get<ReactionBuilder>(0).Name = "newName").Returns(true);
      }

      protected override void Because()
      {
         sut.AddTo(_itemsToAdd, _buildingBlock);
      }

      [Observation]
      public void must_add_the_reaction_builders_to_the_building_block()
      {
         _itemsToAdd.Each(x => _buildingBlock.ShouldContain(x));
      }

      [Observation]
      public void the_formulas_object_paths_should_be_adjusted()
      {
         _reactionBuilder.Formula.ObjectPaths.ShouldNotContain(new FormulaUsablePath { "oldName" });
         _reactionBuilder.Formula.ObjectPaths.ShouldContain(new FormulaUsablePath { "newName" });

         _parameter.Formula.ObjectPaths.ShouldNotContain(new FormulaUsablePath { "oldName" });
         _parameter.Formula.ObjectPaths.ShouldContain(new FormulaUsablePath { "newName" });
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
         sut.SelectMoleculeNames(_reactionBuildingBlock, new List<string> { "unallowedMolecule" }, "reactionName", "Products");
      }

      [Observation]
      public void must_exclude_the_unallowed_names()
      {
         A.CallTo(() => _multipleStringSelectionPresenter.Show(A<string>._, A<string>._, A<IEnumerable<string>>.That.Matches(x => x.Contains("allowedMolecule") && !x.Contains("unallowedMolecule")), A<string>._, A<bool>._)).MustHaveHappened();
      }
   }
}