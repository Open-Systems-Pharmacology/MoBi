using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Assets;

namespace MoBi.Core.Commands
{
   public class EditReactionPartnerStoichiometricCoefficientCommand : EditReactionPartnerCommand
   {
      private readonly double _newCoefficient;
      private double _oldCoefficient;
      private readonly string _moleculeName;

      public EditReactionPartnerStoichiometricCoefficientCommand(double newCoefficient, ReactionBuilder reaction, ReactionPartnerBuilder reactionPartner, MoBiReactionBuildingBlock buildingBlock) : base(reaction, reactionPartner, buildingBlock)
      {
         _newCoefficient = newCoefficient;
         _oldCoefficient = reactionPartner.StoichiometricCoefficient;
         _moleculeName = reactionPartner.MoleculeName;
         Description = AppConstants.Commands.EditStochiometricCoefficient(newCoefficient, _reactionPartner.StoichiometricCoefficient, _reaction.Name, _moleculeName, _isEduct ? ObjectTypes.Educt : ObjectTypes.Product);
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         _oldCoefficient = _reactionPartner.StoichiometricCoefficient;
         _reactionPartner.StoichiometricCoefficient = _newCoefficient;
         context.PublishEvent(new EditReactionPartnerEvent(_reaction, _reactionPartner));
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _reactionPartner = RetrievePartner(_moleculeName);
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new EditReactionPartnerStoichiometricCoefficientCommand(_oldCoefficient, _reaction, _reactionPartner, _buildingBlock)
         {
            Visible = Visible
         }.AsInverseFor(this);
      }
   }
}