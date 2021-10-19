using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Model.Diagram;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Assets;

namespace MoBi.Core.Commands
{
   public class EditReactionPartnerMoleculeNameCommand : EditReactionPartnerCommand
   {
      private readonly string _newMoleculeName;
      private string _oldMoleculeName;

      public EditReactionPartnerMoleculeNameCommand(string newMoleculeName, IReactionBuilder reaction, IReactionPartnerBuilder reactionPartner, IMoBiReactionBuildingBlock buildingBlock) :
         base(reaction, reactionPartner, buildingBlock)
      {
         _newMoleculeName = newMoleculeName;
         Description = AppConstants.Commands.EditDescription(ObjectType, _isEduct ? ObjectTypes.Educt : ObjectTypes.Product, _reactionPartner.MoleculeName, newMoleculeName, _reaction.Name);
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         _oldMoleculeName = _reactionPartner.MoleculeName;
         _reactionPartner.MoleculeName = _newMoleculeName;
         context.PublishEvent(new EditReactionPartnerEvent(_reaction, _reactionPartner));

         var reactionDiagramManager = _buildingBlock.DiagramManager.DowncastTo<IMoBiReactionDiagramManager>();
         if (reactionDiagramManager.IsInitialized)
            reactionDiagramManager.RenameMolecule(_reaction, _oldMoleculeName, _newMoleculeName);
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new EditReactionPartnerMoleculeNameCommand(_oldMoleculeName, _reaction, _reactionPartner, _buildingBlock)
         {
            Visible = Visible
         }.AsInverseFor(this);
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _reactionPartner = RetrievePartner(_newMoleculeName);
      }
   }
}