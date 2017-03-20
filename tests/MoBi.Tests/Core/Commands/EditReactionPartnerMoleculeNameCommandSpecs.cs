using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Model.Diagram;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_EditReactionPartnerMoleculeNameCommand : ContextSpecification<EditReactionPartnerMoleculeNameCommand>
   {
      protected ReactionBuilder _reaction;
      protected ReactionPartnerBuilder _reactionPartner;
      protected IMoBiReactionBuildingBlock _buildingBlock;
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
         _buildingBlock = A.Fake<IMoBiReactionBuildingBlock>().WithId("BB");
         _context = A.Fake<IMoBiContext>();
         A.CallTo(() => _context.Get<IReactionBuilder>(_reaction.Id)).Returns(_reaction);
         A.CallTo(() => _context.Get<IMoBiReactionBuildingBlock>(_buildingBlock.Id)).Returns(_buildingBlock);
         AddPartnerToReaction();
         _diagramManager = A.Fake<IMoBiReactionDiagramManager>();
         A.CallTo(() => _buildingBlock.DiagramManager).Returns(_diagramManager);
         sut = new EditReactionPartnerMoleculeNameCommand(_newMoleculeName, _reaction, _reactionPartner, _buildingBlock);
      }

      protected abstract void AddPartnerToReaction();
   }

   public class When_executing_the_set_reaction_partner_molecule_name_command_for_an_educt_and_an_initialized_diagram : concern_for_EditReactionPartnerMoleculeNameCommand
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _diagramManager.IsInitialized).Returns(true);
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
      public void should_update_the_diagram()
      {
         A.CallTo(() => _diagramManager.RenameMolecule(_reaction, _oldMoleculeName, _newMoleculeName)).MustHaveHappened();
      }
   }

   public class When_executing_the_set_reaction_partner_molecule_name_command_for_a_product_and_the_diagram_is_not_initialized : concern_for_EditReactionPartnerMoleculeNameCommand
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _diagramManager.IsInitialized).Returns(false);
      }

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

      [Observation]
      public void should_not_update_the_diagram()
      {
         A.CallTo(() => _diagramManager.RenameMolecule(_reaction, _oldMoleculeName, _newMoleculeName)).MustNotHaveHappened();
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