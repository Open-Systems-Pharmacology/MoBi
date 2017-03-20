using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public abstract class AddObjectBaseCommand<TChild, TParent> : AddItemCommand<TChild, TParent, IBuildingBlock>, ISilentCommand
      where TChild : class, IObjectBase
      where TParent : class, IObjectBase
   {
      public string ChildId { get; set; }
      public bool Silent { get; set; }

      protected AddObjectBaseCommand(TParent parent, TChild itemToAdd, IBuildingBlock buildingBlock)
         : base(parent, itemToAdd, buildingBlock)
      {
         Silent = false;
         ChildId = itemToAdd.Id;
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _itemToAdd = default(TChild);
         _parent = default(TParent);
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         Description = AppConstants.Commands.AddToDescription(ObjectType, _itemToAdd.Name, _parent.Name, context.TypeFor(_buildingBlock), _buildingBlock.Name);
         context.Register(_itemToAdd);
         AddTo(_itemToAdd, _parent, context);

         if (!Silent)
            context.PublishEvent(new AddedEvent<TChild>(_itemToAdd, _parent));
      }

      protected abstract void AddTo(TChild child, TParent parent, IMoBiContext context);

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _itemToAdd = context.Get<TChild>(ChildId);
      }
   }
}