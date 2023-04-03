using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Helper;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class SwapBuildingBlockCommand<T> : MoBiCommand where T : class, IBuildingBlock
   {
      protected T _oldBuildingBlock;
      protected T _newBuildingBlock;

      public SwapBuildingBlockCommand(T oldBuildingBlock, T newBuildingBlock)
      {
         _oldBuildingBlock = oldBuildingBlock;
         _newBuildingBlock = newBuildingBlock;
         CommandType = AppConstants.Commands.UpdateCommand;
         ObjectType = new ObjectTypeResolver().TypeFor<T>();
         Description = AppConstants.Commands.SwapBuildingCommandDescription(ObjectType, _oldBuildingBlock.Name);
      }

      protected override void ClearReferences()
      {
         _oldBuildingBlock = null;
         _newBuildingBlock = null;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         //Remove old building block
         //var removeCommand = new RemoveBuildingBlockCommand<T>(_oldBuildingBlock);
         //removeCommand.Execute(context);

         //Update id before adding the new building block
         _newBuildingBlock.Id = _oldBuildingBlock.Id;

         //Add new building block
         //var addCommand = new AddBuildingBlockCommand<T>(_newBuildingBlock);
         //addCommand.Execute(context);
      }
   }
}