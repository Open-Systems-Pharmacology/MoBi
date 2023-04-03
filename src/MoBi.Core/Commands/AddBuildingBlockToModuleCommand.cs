using MoBi.Assets;
using MoBi.Core.Domain.Extensions;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Core.Helper;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class AddBuildingBlockToModuleCommand<T> : MoBiReversibleCommand, ISilentCommand where T : class, IBuildingBlock 
   {
      protected T _buildingBlock;
      private string _buildingBlockId;

      private Module _existingModule;
      private string _existingModuleId;

      public bool Silent { get; set; }

      //ALSO ADD TO PROJECT????

      public AddBuildingBlockToModuleCommand(T buildingBlock, Module existingModule)
      {
         ObjectType = new ObjectTypeResolver().TypeFor<T>();
         CommandType = AppConstants.Commands.AddCommand;
         _buildingBlock = buildingBlock;
         _buildingBlockId = buildingBlock.Id;
         _existingModule = existingModule;
         _existingModuleId = existingModule.Id;
         
         //description should also change
         //Description = AppConstants.Commands.AddToProjectDescription(ObjectType, buildingBlock.Name);
         Silent = false;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         context.Register(_buildingBlock);
         _existingModule.AddBuildingBlock(_buildingBlock);

         //this is correct
         if (!Silent)
            context.PublishEvent(new AddedEvent<T>(_buildingBlock, _existingModule));

      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         _buildingBlock = context.Get<T>(_buildingBlockId);
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return null;
         // return new RemoveBuildingBlockFromModuleCommand<T>(_buildingBlock).AsInverseFor(this);
      }

      protected override void ClearReferences()
      {
         _buildingBlock = null;
         _existingModule = null;
      }

   }
}