using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Model.Diagram;
using MoBi.UI.Diagram.DiagramManagers;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Diagram.Elements;
using OSPSuite.UI.Diagram.Elements;
using OSPSuite.Core.Diagram;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_EditReactionPartnerMoleculeNameCommand : ContextSpecification<EditReactionPartnerMoleculeNameCommand>
   {
      protected ReactionBuilder _reaction;
      protected ReactionPartnerBuilder _reactionPartner;
      protected MoBiReactionBuildingBlock _buildingBlock;
      protected IMoBiContext _context;
      protected string _oldMoleculeName;
      protected string _newMoleculeName;
      protected IMoBiReactionDiagramManager _diagramManager;

      protected override void Context()
      {
         _oldMoleculeName = "A";
         _newMoleculeName = "B";
         _reaction = new ReactionBuilder().WithId("R");
         _reactionPartner = new ReactionPartnerBuilder(_oldMoleculeName, 3);
         _diagramManager = new MoBiReactionDiagramManager();
         _buildingBlock = new MoBiReactionBuildingBlock
         {
            Id = "BB",
            DiagramManager = _diagramManager,
            DiagramModel = new DiagramModel()
         };

         _context = A.Fake<IMoBiContext>();
         A.CallTo(() => _context.Get<ReactionBuilder>(_reaction.Id)).Returns(_reaction);
         A.CallTo(() => _context.Get<MoBiReactionBuildingBlock>(_buildingBlock.Id)).Returns(_buildingBlock);
         _buildingBlock.Add(_reaction);
         AddPartnerToReaction();
         sut = new EditReactionPartnerMoleculeNameCommand(_newMoleculeName, _reaction, _reactionPartner, _buildingBlock);
      }

      protected abstract void AddPartnerToReaction();
   }

   public class When_executing_the_set_reaction_partner_molecule_name_command_for_an_educt_and_an_initialized_diagram : concern_for_EditReactionPartnerMoleculeNameCommand
   {
      protected override void Context()
      {
         base.Context();
         _buildingBlock.DiagramManager.InitializeWith(_buildingBlock, new DiagramOptions());
      }

      protected override void AddPartnerToReaction()
      {
         _reaction.AddEduct(_reactionPartner);
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void should_update_the_reaction_name()
      {
         _reactionPartner.MoleculeName.ShouldBeEqualTo(_newMoleculeName);
      }

      [Observation]
      public void should_rename_the_nodes_in_the_diagram()
      {
         _diagramManager.GetMoleculeNodes().Select(x => x.Name).ShouldContain(_newMoleculeName);
         _diagramManager.GetMoleculeNodes().Select(x => x.Name).ShouldNotContain(_oldMoleculeName);
      }
   }

   public class When_executing_the_set_reaction_partner_molecule_name_command_for_a_product_and_the_diagram_is_not_initialized : concern_for_EditReactionPartnerMoleculeNameCommand
   {
      protected override void AddPartnerToReaction()
      {
         _reaction.AddProduct(_reactionPartner);
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void should_update_the_reaction_name()
      {
         _reactionPartner.MoleculeName.ShouldBeEqualTo(_newMoleculeName);
      }
   }

   public class When_reverting_the_set_reaction_partner_molecule_name_command_command : concern_for_EditReactionPartnerMoleculeNameCommand
   {
      protected override void AddPartnerToReaction()
      {
         _reaction.AddEduct(_reactionPartner);
      }

      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void should_reset_the_original_value()
      {
         _reactionPartner.MoleculeName.ShouldBeEqualTo(_oldMoleculeName);
      }
   }
}