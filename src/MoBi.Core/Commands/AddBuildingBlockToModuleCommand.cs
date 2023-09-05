using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Core.Helper;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class AddBuildingBlockToModuleCommand<T> : ModuleContentChangedCommand<T>, ISilentCommand where T : class, IBuildingBlock
   {
      private readonly string _buildingBlockId;
      public bool Silent { get; set; }

      public AddBuildingBlockToModuleCommand(T buildingBlock, Module existingModule) : base(buildingBlock, existingModule)
      {
         ObjectType = new ObjectTypeResolver().TypeFor<T>();
         CommandType = AppConstants.Commands.AddCommand;
         _buildingBlockId = buildingBlock.Id;

         Description = AppConstants.Commands.AddToDescription(ObjectType, buildingBlock.Name, _existingModule.Name);
         Silent = false;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         DoExecute(context);

         RaiseEvents(context);
      }

      protected virtual void RaiseEvents(IMoBiContext context)
      {
         if (!Silent)
            context.PublishEvent(new AddedEvent<T>(_buildingBlock, _existingModule));
         
         PublishSimulationStatusChangedEvents(_existingModule, context);
      }

      protected virtual void DoExecute(IMoBiContext context)
      {
         context.Register(_buildingBlock);
         _existingModule.Add(_buildingBlock);
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _buildingBlock = context.Get<T>(_buildingBlockId);
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveBuildingBlockFromModuleCommand<T>(_buildingBlock, _existingModule).AsInverseFor(this);
      }
   }
}