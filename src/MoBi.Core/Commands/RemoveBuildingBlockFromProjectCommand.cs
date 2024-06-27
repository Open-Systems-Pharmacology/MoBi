using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Core.Helper;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public abstract class RemoveBuildingBlockFromProjectCommand<T> : MoBiReversibleCommand where T : class, IBuildingBlock
   {
      protected T _buildingBlock;
      private byte[] _serializationStream;

      protected RemoveBuildingBlockFromProjectCommand(T buildingBlock)
      {
         ObjectType = new ObjectTypeResolver().TypeFor(buildingBlock);
         CommandType = AppConstants.Commands.DeleteCommand;
         Description = AppConstants.Commands.RemoveFromProjectDescription(ObjectType, buildingBlock.Name);
         _buildingBlock = buildingBlock;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         var project = context.CurrentProject;
         RemoveFromProject(project);
         context.Unregister(_buildingBlock);
         _serializationStream = context.Serialize(_buildingBlock);
         context.PublishEvent(new RemovedEvent(_buildingBlock, project));
      }

      protected abstract void RemoveFromProject(MoBiProject project);

      public override void RestoreExecutionData(IMoBiContext context)
      {
         _buildingBlock = context.Deserialize<T>(_serializationStream);
      }

      protected override void ClearReferences()
      {
         _buildingBlock = null;
      }
   }
}