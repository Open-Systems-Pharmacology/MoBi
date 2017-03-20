using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Helper;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public abstract class RemoveItemCommand<TChild, TParent,TBuildingBlock> : BuildingBlockChangeCommandBase<TBuildingBlock>
      where TParent : class, IObjectBase where TBuildingBlock : class, IBuildingBlock
   {
      protected TChild _itemToRemove;
      protected TParent _parent;

      protected string _parentId;

      protected RemoveItemCommand(TParent parent, TChild itemToRemove, TBuildingBlock buildingBlock)
         : base(buildingBlock)
      {
         CommandType = AppConstants.Commands.DeleteCommand;
         ObjectType = new ObjectTypeResolver().TypeFor<TChild>();
         _parent = parent;
         _parentId = _parent.Id;
         _itemToRemove = itemToRemove;
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _parent = context.Get<TParent>(_parentId);
      }
   }
}