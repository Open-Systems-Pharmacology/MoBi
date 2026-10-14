using System;
using System.Drawing;
using System.IO;
using System.Linq;
using FakeItEasy;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Model.Diagram;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.UI.Diagram.DiagramManagers;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Serialization.Diagram;
using OSPSuite.Infrastructure.Container.Castle;
using OSPSuite.Presentation.Diagram.Elements;
using OSPSuite.Presentation.Diagram.Services;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;

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

   public abstract class concern_for_DiagramTask_with_the_organism_template : concern_for_DiagramTask
   {
      private OSPSuite.Utility.Container.IContainer _originalContainer;

      protected string TemplateFile => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppConstants.SpecialFileNames.SPATIAL_STRUCTURE_TEMPLATE);

      public override void GlobalContext()
      {
         base.GlobalContext();
         _originalContainer = IoC.Container;
         var container = new CastleWindsorContainer();
         IoC.InitializeWith(container);
         container.Register<IDiagramModelToXmlMapper, DiagramModelToXmlMapper>();
         var diagramModelFactory = A.Fake<IDiagramModelFactory>();
         A.CallTo(() => diagramModelFactory.Create()).ReturnsLazily(() => new DiagramModel());
         container.RegisterImplementationOf(diagramModelFactory);
      }

      public override void GlobalCleanup()
      {
         if (_originalContainer != null)
            IoC.InitializeWith(_originalContainer);

         base.GlobalCleanup();
      }
   }

   public class When_loading_the_organism_diagram_template : concern_for_DiagramTask_with_the_organism_template
   {
      private IDiagramModel _template;

      protected override void Because()
      {
         _template = sut.LoadDiagramTemplate(TemplateFile);
      }

      [Observation]
      public void should_create_a_ui_free_diagram_model()
      {
         _template.ShouldBeAnInstanceOf<DiagramModel>();
      }

      [Observation]
      public void should_load_the_container_nodes_of_the_template()
      {
         _template.GetAllChildren<IContainerNode>().Any(x => string.Equals(x.Name, "VenousBlood")).ShouldBeTrue();
      }

      [Observation]
      public void should_load_the_nested_container_nodes_of_the_template()
      {
         _template.FindByName("VenousBlood").DowncastTo<IContainerNode>().GetDirectChildren<IContainerNode>().Any(x => string.Equals(x.Name, "Plasma")).ShouldBeTrue();
      }
   }

   public class When_applying_the_organism_diagram_template_to_a_spatial_structure_diagram : concern_for_DiagramTask_with_the_organism_template
   {
      private DiagramModel _model;
      private ContainerNode _venousBlood;
      private ContainerNode _plasma;
      private bool _refreshed;

      protected override void Context()
      {
         base.Context();
         _model = new DiagramModel();
         _venousBlood = createContainerNode("VenousBlood", _model);
         _plasma = createContainerNode("Plasma", _venousBlood);
      }

      private ContainerNode createContainerNode(string name, IContainerBase parent)
      {
         var node = _model.CreateNode<ContainerNode>(name, PointF.Empty, parent);
         node.Name = name;
         return node;
      }

      protected override void Because()
      {
         sut.ApplyLayoutTemplate(_model, TemplateFile, _model, () => _refreshed = true, recursive: true);
      }

      [Observation]
      public void should_copy_the_size_of_the_template_onto_the_matching_container_node()
      {
         _plasma.Size.Width.ShouldBeEqualTo(144.049438F, 0.01);
         _plasma.Size.Height.ShouldBeEqualTo(28.0913086F, 0.01);
      }

      [Observation]
      public void should_enclose_the_nested_container_node_in_its_parent()
      {
         _venousBlood.Bounds.Contains(_plasma.Bounds).ShouldBeTrue();
         (_plasma.Location.X - _venousBlood.Location.X).ShouldBeEqualTo(ContainerNode.LEFT_MARGIN, 0.01);
         (_plasma.Location.Y - _venousBlood.Location.Y).ShouldBeEqualTo(ContainerNode.TOP_MARGIN, 0.01);
      }

      [Observation]
      public void should_mark_the_diagram_as_layouted_and_refresh_the_diagram_options()
      {
         _model.IsLayouted.ShouldBeTrue();
         _refreshed.ShouldBeTrue();
      }
   }
}
