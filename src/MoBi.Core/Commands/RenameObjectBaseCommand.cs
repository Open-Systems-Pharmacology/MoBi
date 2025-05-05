using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Helper;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Events;

namespace MoBi.Core.Commands
{
   public class RenameObjectBaseCommand : BuildingBlockChangeCommandBase<IBuildingBlock>
   {
      private IObjectBase _objectBase;
      protected readonly string _newName;
      public string OldName { get; private set; }
      public string ObjectId { get; }

      public RenameObjectBaseCommand(IObjectBase objectBase, string newName, IBuildingBlock buildingBlock) : base(buildingBlock)
      {
         _objectBase = objectBase;
         _newName = newName;
         ObjectId = objectBase.Id;
         ObjectType = new ObjectTypeResolver().TypeFor(objectBase);
         CommandType = AppConstants.Commands.RenameCommand;
         Description = AppConstants.Commands.RenameDescription(objectBase, newName);
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RenameObjectBaseCommand(_objectBase, OldName, _buildingBlock).AsInverseFor(this);
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _objectBase = null;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         RenameObjectBase(context);

         context.PublishEvent(new RenamedEvent(_objectBase));
      }

      protected virtual void RenameObjectBase(IMoBiContext context)
      {
         var renameInSimulationTask = context.Resolve<IRenameInSimulationTask>();
         OldName = _objectBase.Name;

         switch (_objectBase)
         {
            case IBuildingBlock buildingBlock:
               _objectBase.Name = _newName;
               renameInSimulationTask.RenameInSimulationUsingTemplateBuildingBlock(OldName, buildingBlock);
               break;
            case Module module:
               _objectBase.Name = _newName;
               renameInSimulationTask.RenameInSimulationUsingTemplateModule(OldName, module);
               break;
            case IEntity entity:
               var simulationEntitySourceUpdater = context.Resolve<ISimulationEntitySourceUpdater>();
               var entityPathResolver = context.Resolve<IEntityPathResolver>();
               var originalEntityPath = entityPathResolver.ObjectPathFor(entity);
               _objectBase.Name = _newName;
               var newEntityPath = entityPathResolver.ObjectPathFor(entity);
               simulationEntitySourceUpdater.UpdateEntitySourcesForEntityRename(newEntityPath, originalEntityPath, _buildingBlock);
               break;
         }
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _objectBase = context.Get<IObjectBase>(ObjectId);
      }
   }
}