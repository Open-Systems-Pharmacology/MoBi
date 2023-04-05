using MoBi.Assets;
using MoBi.Core.Domain.Extensions;
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
      public string ExistingModuleId { get; private set; }
      public string ModuleWithAddedBuildingBlocksId { get; private set; }
      public bool Silent { get; set; }

      protected T _buildingBlock;
      private byte[] _serializationStream;

      public RemoveBuildingBlockFromModuleCommand(T buildingBlock, Module existingModule)
      {
         ObjectType = new ObjectTypeResolver().TypeFor<T>();
         CommandType = AppConstants.Commands.DeleteCommand;
         _existingModule = existingModule;
         _buildingBlock = buildingBlock;

         Silent = false;
         Description = AppConstants.Commands.RemoveFromDescription(ObjectType, buildingBlock.Name, _existingModule.Name);
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         removeBuildingBlockFromModule();
         context.Unregister(_buildingBlock);
         _serializationStream = context.Serialize(_buildingBlock);
         context.PublishEvent(new RemovedEvent(_buildingBlock, _existingModule));
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         _buildingBlock = context.Deserialize<T>(_serializationStream);
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
         _existingModule.RemoveBuildingBlock(_buildingBlock);
      }
   }
}