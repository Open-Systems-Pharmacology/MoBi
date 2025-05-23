using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Core.Helper;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class RemoveBuildingBlockFromModuleCommand<T> : ModuleContentChangedCommand<T> where T : class, IBuildingBlock
   {

      public bool Silent { get; set; }

      private byte[] _serializationStream;

      public RemoveBuildingBlockFromModuleCommand(T buildingBlock, Module existingModule) : base(buildingBlock, existingModule)  
      {
         ObjectType = new ObjectTypeResolver().TypeFor(buildingBlock);
         CommandType = AppConstants.Commands.DeleteCommand;
         Silent = false;
         Description = AppConstants.Commands.RemoveFromDescription(ObjectType, buildingBlock.Name, _existingModule.Name);
      }

      protected override void RaiseEvents(IMoBiContext context)
      {
         context.PublishEvent(new RemovedEvent(_buildingBlock, _existingModule));
         PublishSimulationStatusChangedEvents(_existingModule, context);
      }

      protected override void DoExecute(IMoBiContext context)
      {
         removeBuildingBlockFromModule();
         context.Unregister(_buildingBlock);
         _serializationStream = context.Serialize(_buildingBlock);
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _buildingBlock = context.Deserialize<T>(_serializationStream);
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddBuildingBlockToModuleCommand<T>(_buildingBlock, _existingModule).AsInverseFor(this);
      }

      private void removeBuildingBlockFromModule()
      {
         _existingModule.Remove(_buildingBlock);
      }
   }
}