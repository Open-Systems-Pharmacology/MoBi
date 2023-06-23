using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Core.Helper;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public abstract class AddBuildingBlockToProjectCommand<T> : MoBiReversibleCommand, ISilentCommand where T : class, IBuildingBlock
   {
      protected T _buildingBlock;
      public string BuildingBlockId { get; private set; }
      public bool Silent { get; set; }

      protected AddBuildingBlockToProjectCommand(T buildingBlock)
      {
         ObjectType = new ObjectTypeResolver().TypeFor<T>();
         CommandType = AppConstants.Commands.AddCommand;
         _buildingBlock = buildingBlock;
         BuildingBlockId = buildingBlock.Id;
         Description = AppConstants.Commands.AddToProjectDescription(ObjectType, buildingBlock.Name);
         Silent = false;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         var project = context.CurrentProject;
         context.Register(_buildingBlock);
         AddToProject(project);

         if (!Silent)
            context.PublishEvent(new AddedEvent<T>(_buildingBlock, project));
      }

      protected abstract void AddToProject(MoBiProject project);

      public override void RestoreExecutionData(IMoBiContext context)
      {
         _buildingBlock = context.Get<T>(BuildingBlockId);
      }

      protected override void ClearReferences()
      {
         _buildingBlock = null;
      }
   }
}