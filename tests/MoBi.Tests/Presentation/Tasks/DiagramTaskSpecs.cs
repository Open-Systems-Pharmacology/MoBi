using System.Collections.Generic;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands.Core;
using FakeItEasy;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Model.Diagram;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.UI.Diagram.DiagramManagers;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Diagram.Elements;
using OSPSuite.UI.Diagram.Elements;

namespace MoBi.Presentation.Tasks
{
   public abstract class concern_for_DiagramTask : ContextSpecification<DiagramTask>
   {
      protected MoBiReactionBuildingBlock _targetBuildingBlock;
      protected MoBiReactionBuildingBlock _sourceBuildingBlock;

      protected override void Context()
      {
         _targetBuildingBlock = createBuildingBlockWithDiagramModel(A.Fake<IDiagramModel>());
         _sourceBuildingBlock = createBuildingBlockWithDiagramModel(A.Fake<IDiagramModel>());
         _sourceBuildingBlock.DiagramManager.InitializeWith(_sourceBuildingBlock, new DiagramOptions());
         _targetBuildingBlock.DiagramManager.InitializeWith(_targetBuildingBlock, new DiagramOptions());
         sut = new DiagramTask();
      }

      protected MoBiReactionBuildingBlock createBuildingBlockWithDiagramModel(IDiagramModel diagramModel)
      {
         var buildingBlock = new MoBiReactionBuildingBlock
         {
            DiagramModel = diagramModel,
            DiagramManager = A.Fake<IMoBiReactionDiagramManager>()
         };

         return buildingBlock;
      }

      protected ReactionBuilder createReaction()
      {
         var reaction = new ReactionBuilder();
         reaction.AddEduct(new ReactionPartnerBuilder("educt", 1.0));
         reaction.AddProduct(new ReactionPartnerBuilder("product", 1.0));
         reaction.AddModifier("modifier");
         return reaction;
      }
   }

   public class When_moving_a_reaction_node_and_the_partners_are_not_in_the_target_diagram : concern_for_DiagramTask
   {
      private ReactionBuilder _sourceReaction;
      private ReactionBuilder _targetReaction;
      private IMoBiMacroCommand _command;

      protected override void Context()
      {
         base.Context();
         createReaction();
         _sourceReaction = createReaction();
         _targetReaction = new ReactionBuilder();
         _sourceBuildingBlock.Add(_sourceReaction);
         _targetBuildingBlock.Add(_targetReaction);

      }

      protected override void Because()
      {
         _command = sut.MoveDiagramNodes(_sourceBuildingBlock, _targetBuildingBlock, _sourceReaction, _sourceReaction.Name) as IMoBiMacroCommand;
      }

      [Observation]
      public void the_macro_command_should_contain_enough_commands_to_move_all_the_partners_too()
      {
         _command.All().Count(x => !x.IsEmpty()).ShouldBeEqualTo(4);
      }
   }

   public class When_moving_reaction_nodes_and_the_partners_are_already_in_the_target_diagram : concern_for_DiagramTask
   {
      private ReactionBuilder _sourceReaction;
      private ReactionBuilder _targetReaction;
      private IMoBiMacroCommand _command;

      protected override void Context()
      {
         base.Context();
         createReaction();
         _sourceReaction = createReaction();
         _targetReaction = createReaction();
         _sourceBuildingBlock.Add(_sourceReaction);

         _targetBuildingBlock = new MoBiReactionBuildingBlock
         {
            DiagramManager = new MoBiReactionDiagramManager(),
            DiagramModel = new DiagramModel()
         };

         _targetBuildingBlock.Add(_sourceReaction);
         _targetBuildingBlock.Add(_targetReaction);
         _targetBuildingBlock.DiagramManager.InitializeWith(_targetBuildingBlock, new DiagramOptions());
         
      }

      protected override void Because()
      {
         _command = sut.MoveDiagramNodes(_sourceBuildingBlock, _targetBuildingBlock, _sourceReaction, _sourceReaction.Name) as IMoBiMacroCommand;
      }

      [Observation]
      public void the_macro_command_should_only_contain_the_command_to_move_the_reaction()
      {
         _command.All().Count(x => !x.IsEmpty()).ShouldBeEqualTo(1);
      }
   }

   public class When_moving_diagram_nodes_and_the_nodes_are_found_in_both_source_and_target_diagram_models : concern_for_DiagramTask
   {
      private IBaseNode _sourceNode;
      private IBaseNode _targetNode;
      private ReactionBuilder _sourceBuilder;
      private IMoBiMacroCommand _command;

      protected override void Context()
      {
         base.Context();


         _targetBuildingBlock.Add(new ReactionBuilder {Name = "reactionName"});
         _sourceBuilder = new ReactionBuilder {Name = "reactionName"};
         _sourceBuildingBlock.Add(_sourceBuilder);

         _sourceNode = A.Fake<IBaseNode>();
         _targetNode = A.Fake<IBaseNode>();

         A.CallTo(() => _sourceBuildingBlock.DiagramModel.FindByName("reactionName")).Returns(_sourceNode);
         A.CallTo(() => _targetBuildingBlock.DiagramModel.FindByName("reactionName")).Returns(_targetNode);
      }

      protected override void Because()
      {
         _command = sut.MoveDiagramNodes(_sourceBuildingBlock, _targetBuildingBlock, _sourceBuilder, _sourceBuilder.Name) as IMoBiMacroCommand;
      }

      [Observation]
      public void the_command_should_be_the_correct_type_to_move_the_target_node()
      {
         _command.All().First().ShouldBeAnInstanceOf<MoveDiagramNodeCommand>();
      }
   }

   public class When_moving_diagram_nodes_and_diagram_model_is_null : concern_for_DiagramTask
   {
      private MoBiCommand moveDiagramNodes(MoBiReactionBuildingBlock source, MoBiReactionBuildingBlock target)
      {
         ReactionBuilder sourceBuilder = new ReactionBuilder().WithName("sourceBuilder");
         return sut.MoveDiagramNodes(source, target, sourceBuilder, sourceBuilder.Name) as MoBiCommand;
      }

      [Observation]
      public void source_diagram_is_null()
      {
         moveDiagramNodes(createBuildingBlockWithDiagramModel(null), createBuildingBlockWithDiagramModel(A.Fake<IDiagramModel>())).IsEmpty().ShouldBeTrue();
      }

      [Observation]
      public void target_diagram_is_null()
      {
         moveDiagramNodes(createBuildingBlockWithDiagramModel(A.Fake<IDiagramModel>()), createBuildingBlockWithDiagramModel(null)).IsEmpty().ShouldBeTrue();
      }
   }

   public class When_moving_diagram_nodes_to_match_a_source_diagram_model_and_the_node_cannot_be_found_in_the_source_diagram_model : concern_for_DiagramTask
   {
      private ReactionBuilder _sourceBuilder;
      private IMoBiMacroCommand _commands;

      protected override void Context()
      {
         base.Context();

         _sourceBuilder = new ReactionBuilder { Name = "builder" };

         A.CallTo(() => _sourceBuildingBlock.DiagramModel.FindByName(_sourceBuilder.Name)).Returns(null);
      }

      protected override void Because()
      {
         _commands = sut.MoveDiagramNodes(_sourceBuildingBlock, _targetBuildingBlock, _sourceBuilder, _sourceBuilder.Name) as IMoBiMacroCommand;
      }

      [Observation]
      public void the_command_should_be_empty()
      {
         _commands.IsEmptyMacro().ShouldBeTrue();
      }
   }
}
