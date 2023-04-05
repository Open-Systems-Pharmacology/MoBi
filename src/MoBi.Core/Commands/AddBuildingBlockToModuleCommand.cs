using MoBi.Assets;
using MoBi.Core.Domain.Extensions;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Core.Helper;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using DevExpress.XtraReports.Native;

namespace MoBi.Core.Commands
{
   public class AddBuildingBlockToModuleCommand<T> : MoBiReversibleCommand, ISilentCommand where T : class, IBuildingBlock 
   {
      protected T _buildingBlock;
      private string _buildingBlockId;

      private Module _existingModule;
      private string _existingModuleId;

      public bool Silent { get; set; }

      public AddBuildingBlockToModuleCommand(T buildingBlock, Module existingModule)
      {
         ObjectType = new ObjectTypeResolver().TypeFor<T>();
         CommandType = AppConstants.Commands.AddCommand;
         _buildingBlock = buildingBlock;
         _buildingBlockId = buildingBlock.Id;
         _existingModule = existingModule;
         _existingModuleId = existingModule.Id;
         
         Description = AppConstants.Commands.AddToDescription(ObjectType, buildingBlock.Name, _existingModule.Name);
         Silent = false;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         context.Register(_buildingBlock);
         _existingModule.AddBuildingBlock(_buildingBlock);

         if (!Silent)
            context.PublishEvent(new AddedEvent<T>(_buildingBlock, _existingModule));

      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         _buildingBlock = context.Get<T>(_buildingBlockId);
         _existingModule = context.Get<Module>(_existingModuleId);
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveBuildingBlockFromModuleCommand<T>(_buildingBlock, _existingModule).AsInverseFor(this);
      }

      protected override void ClearReferences()
      {
         _buildingBlock = null;
         _existingModule = null;
      }

   }
}