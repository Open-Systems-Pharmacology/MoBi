using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Core.Helper;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class RemoveBuildingBlockFromModuleCommand<T> : MoBiReversibleCommand where T : class, IBuildingBlock
   {
      protected Module _existingModule;
      private readonly string _existingModuleId;
      public bool Silent { get; set; }

      protected T _buildingBlock;
      private byte[] _serializationStream;

      public RemoveBuildingBlockFromModuleCommand(T buildingBlock, Module existingModule)
      {
         ObjectType = new ObjectTypeResolver().TypeFor<T>();
         CommandType = AppConstants.Commands.DeleteCommand;
         _existingModule = existingModule;
         _existingModuleId = existingModule.Id;
         _buildingBlock = buildingBlock;

         Silent = false;
         Description = AppConstants.Commands.RemoveFromDescription(ObjectType, buildingBlock.Name, _existingModule.Name);
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         DoExecute(context);
         RaiseEvents(context);
      }

      protected virtual void RaiseEvents(IMoBiContext context)
      {
         context.PublishEvent(new RemovedEvent(_buildingBlock, _existingModule));
      }

      protected virtual void DoExecute(IMoBiContext context)
      {
         removeBuildingBlockFromModule();
         context.Unregister(_buildingBlock);
         _serializationStream = context.Serialize(_buildingBlock);
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         _buildingBlock = context.Deserialize<T>(_serializationStream);
         _existingModule = context.Get<Module>(_existingModuleId);
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddBuildingBlockToModuleCommand<IBuildingBlock>(_buildingBlock, _existingModule).AsInverseFor(this);
      }

      protected override void ClearReferences()
      {
         _buildingBlock = null;
         _existingModule = null;
      }

      private void removeBuildingBlockFromModule()
      {
         _existingModule.Remove(_buildingBlock);
      }
   }
}