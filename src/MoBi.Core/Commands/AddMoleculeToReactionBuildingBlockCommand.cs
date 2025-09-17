using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Model.Diagram;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Assets;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Services;

namespace MoBi.Core.Commands
{
    public class AddMoleculeToReactionBuildingBlockCommand : BuildingBlockChangeCommandBase<MoBiReactionBuildingBlock>
    {
        private readonly string _moleculeName;
        private string _moleculeNodeId;

        public AddMoleculeToReactionBuildingBlockCommand(MoBiReactionBuildingBlock reactionBuildingBlock, string moleculeName) : base(reactionBuildingBlock)
        {
            _moleculeName = moleculeName;
            ObjectType = ObjectTypes.Molecule;
            CommandType = AppConstants.Commands.AddCommand;
            Description = AppConstants.Commands.AddToDescription(ObjectType, _moleculeName, reactionBuildingBlock.Name);
        }

        protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
        {
            return new RemoveMoleculeFromReactionBuildingBlockCommand(_buildingBlock, _moleculeNodeId).AsInverseFor(this);
        }

        protected override void ExecuteWith(IMoBiContext context)
        {
            base.ExecuteWith(context);
            var reactionDiagramManager = _buildingBlock.DiagramManager.DowncastTo<IMoBiReactionDiagramManager>();
            var moleculeNode = reactionDiagramManager.AddMoleculeNode(_moleculeName);
            _moleculeNodeId = moleculeNode.Id;
        }
    }
}