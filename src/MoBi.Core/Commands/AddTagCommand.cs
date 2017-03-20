using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Descriptors;

namespace MoBi.Core.Commands
{
   public class AddTagCommand : BuildingBlockChangeCommandBase<IBuildingBlock>
   {
      private IEntity _container;
      private readonly string _tag;
      private readonly string _containerId;

      public AddTagCommand(string tag, IEntity entity, IBuildingBlock parentBuildingBlock) : base(parentBuildingBlock)
      {
         _tag = tag;
         _containerId = entity.Id;
         _container = entity;
         ObjectType = "Tag";
         var containerPath = string.Empty;
         if (entity.ParentContainer != null)
            containerPath = entity.ParentContainer.EntityPath();

         CommandType = AppConstants.Commands.AddCommand;
         Description = AppConstants.Commands.AddTagToEntity(tag, entity.Name, containerPath, parentBuildingBlock.Name);
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _container = null;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         _container.AddTag(new Tag(_tag));
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveTagCommand(_tag, _container, _buildingBlock).AsInverseFor(this);
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _container = context.Get<IEntity>(_containerId);
      }
   }

   public class RemoveTagCommand : BuildingBlockChangeCommandBase<IBuildingBlock>
   {
      private readonly string _tag;
      private readonly string _containerId;

      public RemoveTagCommand(string tag, IEntity container, IBuildingBlock parentBuildingBlock)
         : base(parentBuildingBlock)
      {
         _tag = tag;
         _containerId = container.Id;
         ObjectType = "Tag";
         CommandType = AppConstants.Commands.DeleteCommand;
         Description = AppConstants.Commands.RemoveFromDescription(ObjectType, tag, container.Name);
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         var container = context.Get<IEntity>(_containerId);
         container.RemoveTag(_tag);
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         var container = context.Get<IEntity>(_containerId);
         return new AddTagCommand(_tag, container, _buildingBlock).AsInverseFor(this);
      }
   }
}