using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Model.Diagram;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Assets;
using OSPSuite.Utility.Extensions;

namespace MoBi.Core.Services
{
   public class RemoveMoleculeFromReactionBuildingBlockCommand : BuildingBlockChangeCommandBase<MoBiReactionBuildingBlock>
   {
      private readonly string _moleculeNodeId;
      private readonly string _moleculeNodeName;

      public RemoveMoleculeFromReactionBuildingBlockCommand(MoBiReactionBuildingBlock reactionBuildingBlock,
         string moleculeNodeId) : base(reactionBuildingBlock)
      {
         _moleculeNodeId = moleculeNodeId;
         ObjectType = ObjectTypes.Molecule;
         CommandType = AppConstants.Commands.DeleteCommand;
         _moleculeNodeName = reactionBuildingBlock.DiagramModel.GetNode(_moleculeNodeId).Name;
         Description = AppConstants.Commands.RemoveFromDescription(ObjectType, _moleculeNodeName, reactionBuildingBlock.Name);
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return BuildingBlockChangeBaseCommandExtensions.AsInverseFor(new AddMoleculeToReactionBuildingBlockCommand(_buildingBlock, _moleculeNodeName), this);
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         var reactionDiagramManager = _buildingBlock.DiagramManager.DowncastTo<IMoBiReactionDiagramManager>();
         var diagramUpdater = context.Resolve<IDiagramUpdater>();
         diagramUpdater.RemoveMoleculeNodeFromDiagram(_moleculeNodeId, reactionDiagramManager, _buildingBlock.DiagramModel);
      }
   }
}