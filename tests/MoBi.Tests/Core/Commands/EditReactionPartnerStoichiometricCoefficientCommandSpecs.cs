using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_EditReactionPartnerStoichiometricCoefficientCommand : ContextSpecification<EditReactionPartnerStoichiometricCoefficientCommand>
   {
      protected double _newCoefficient;
      protected ReactionBuilder _reaction;
      protected ReactionPartnerBuilder _reactionPartner;
      private MoBiReactionBuildingBlock _buildingBlock;
      protected double _oldCoefficient;
      protected IMoBiContext _context;

      protected override void Context()
      {
         _newCoefficient = 2;
         _oldCoefficient = 5;
         _reaction = new ReactionBuilder().WithId("R");
         _reactionPartner = new ReactionPartnerBuilder("A", _oldCoefficient);
         _buildingBlock = new MoBiReactionBuildingBlock();
         _context = A.Fake<IMoBiContext>();
         A.CallTo(() => _context.Get<ReactionBuilder>(_reaction.Id)).Returns(_reaction);
         AddPartnerToReaction();
         sut = new EditReactionPartnerStoichiometricCoefficientCommand(_newCoefficient, _reaction, _reactionPartner, _buildingBlock);
      }

      protected abstract void AddPartnerToReaction();
   }

   public class When_executing_the_set_stochiometric_coeffcient_for_an_educt_partner_command : concern_for_EditReactionPartnerStoichiometricCoefficientCommand
   {
      protected override void AddPartnerToReaction()
      {
         _reaction.AddEduct(_reactionPartner);
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void should_update_the_coefficient()
      {
         _reactionPartner.StoichiometricCoefficient.ShouldBeEqualTo(_newCoefficient);
      }
   }

   public class When_executing_the_set_stochiometric_coeffcient_for_an_product_partner_command : concern_for_EditReactionPartnerStoichiometricCoefficientCommand
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
      public void should_update_the_coefficient()
      {
         _reactionPartner.StoichiometricCoefficient.ShouldBeEqualTo(_newCoefficient);
      }
   }

   public class When_reverting_the_set_stochiometric_coefficient_command : concern_for_EditReactionPartnerStoichiometricCoefficientCommand
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
         _reactionPartner.StoichiometricCoefficient.ShouldBeEqualTo(_oldCoefficient);
      }
   }
}