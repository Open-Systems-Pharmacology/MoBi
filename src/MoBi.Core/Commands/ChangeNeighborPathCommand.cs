using Antlr.Runtime.Misc;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using OSPSuite.Assets;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Extensions;

namespace MoBi.Core.Commands
{
   public abstract class ChangeNeighborPathCommand : BuildingBlockChangeCommandBase<IBuildingBlock>
   {
      protected readonly string _newPath;
      protected NeighborhoodBuilder _neighborhoodBuilder;
      protected readonly string _oldPath;
      private readonly string _neighborhoodBuilderId;

      protected ChangeNeighborPathCommand(
         string newPath,
         NeighborhoodBuilder neighborhoodBuilder,
         IBuildingBlock buildingBlock, Func<NeighborhoodBuilder, ObjectPath> getPathFunc,
         string type) : base(buildingBlock)
      {
         _newPath = newPath;
         _neighborhoodBuilder = neighborhoodBuilder;
         _neighborhoodBuilderId = neighborhoodBuilder.Id;
         _oldPath = getPathFunc(neighborhoodBuilder)?.ToPathString() ?? string.Empty;
         Description = AppConstants.Commands.ChangeNeighborPathDescription(_neighborhoodBuilder.Name, _newPath, _oldPath, type);
         ObjectType = ObjectTypes.Neighborhood;
         CommandType = AppConstants.Commands.EditCommand;
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _neighborhoodBuilder = null;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         SetNeighborPath(createNewObjectPath());
         context.PublishEvent(new NeighborhoodChangedEvent(_neighborhoodBuilder));
      }

      protected abstract void SetNeighborPath(ObjectPath neighborPath);

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _neighborhoodBuilder = context.Get<NeighborhoodBuilder>(_neighborhoodBuilderId);
      }

      private ObjectPath createNewObjectPath() => string.IsNullOrEmpty(_newPath) ? null : new ObjectPath(_newPath.ToPathArray());
   }

   public class ChangeFirstNeighborPathCommand : ChangeNeighborPathCommand
   {
      public ChangeFirstNeighborPathCommand(string newPath, NeighborhoodBuilder neighborhoodBuilder, IBuildingBlock buildingBlock) :
         base(newPath, neighborhoodBuilder, buildingBlock, x => x.FirstNeighborPath, AppConstants.Captions.FirstNeighbor)
      {
      }

      protected override void SetNeighborPath(ObjectPath neighborPath)
      {
         _neighborhoodBuilder.FirstNeighborPath = neighborPath;
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new ChangeFirstNeighborPathCommand(_oldPath, _neighborhoodBuilder, _buildingBlock).AsInverseFor(this);
      }
   }

   public class ChangeSecondNeighborPathCommand : ChangeNeighborPathCommand
   {
      public ChangeSecondNeighborPathCommand(string newPath, NeighborhoodBuilder neighborhoodBuilder, IBuildingBlock buildingBlock) :
         base(newPath, neighborhoodBuilder, buildingBlock, x => x.SecondNeighborPath, AppConstants.Captions.SecondNeighbor)
      {
      }

      protected override void SetNeighborPath(ObjectPath neighborPath)
      {
         _neighborhoodBuilder.SecondNeighborPath = neighborPath;
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new ChangeSecondNeighborPathCommand(_oldPath, _neighborhoodBuilder, _buildingBlock).AsInverseFor(this);
      }
   }
}