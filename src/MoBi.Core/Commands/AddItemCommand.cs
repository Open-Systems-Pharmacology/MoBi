using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Helper;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public abstract class AddItemCommand<TChild, TParent, TBuildingBlock> : BuildingBlockChangeCommandBase<TBuildingBlock>
      where TParent : class, IObjectBase where TBuildingBlock : class, IBuildingBlock
   {
      protected TChild _itemToAdd;
      protected TParent _parent;

      protected readonly string _parentId;

      protected AddItemCommand(TParent parent, TChild itemToAdd, TBuildingBlock buidingBlock): base(buidingBlock)
      {
         CommandType = AppConstants.Commands.AddCommand;
         ObjectType = new ObjectTypeResolver().TypeFor<TChild>();
         _parent = parent;
         _parentId = _parent.Id;
         _itemToAdd = itemToAdd;
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _parent = null;
         _itemToAdd = default(TChild);
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _parent = context.Get<TParent>(_parentId);
      }
   }
}