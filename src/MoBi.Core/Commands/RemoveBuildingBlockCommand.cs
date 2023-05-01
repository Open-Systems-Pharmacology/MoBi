using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Core.Helper;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class RemoveProjectBuildingBlockCommand<T> : MoBiReversibleCommand where T : class, IBuildingBlock
   {
      protected T _buildingBlock;
      private byte[] _serializationStream;

      public RemoveProjectBuildingBlockCommand(T buildingBlock)
      {
         ObjectType = new ObjectTypeResolver().TypeFor<T>();
         CommandType = AppConstants.Commands.DeleteCommand;
         Description = AppConstants.Commands.RemoveFromProjectDescription(ObjectType, buildingBlock.Name);
         _buildingBlock = buildingBlock;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         var project = context.CurrentProject;
         removeFromProject(project);
         context.Unregister(_buildingBlock);
         _serializationStream = context.Serialize(_buildingBlock);
         context.PublishEvent(new RemovedEvent(_buildingBlock,project));
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         _buildingBlock = context.Deserialize<T>(_serializationStream);
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddProjectBuildingBlockCommand<T>(_buildingBlock).AsInverseFor(this);
      }

      protected override void ClearReferences()
      {
         _buildingBlock = null;
      }

      private void removeFromProject(MoBiProject project)
      {
         project.RemoveBuildingBlock(_buildingBlock);
      }
   }
}