using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public abstract class RemoveObjectBaseCommand<TChild, TParent> : RemoveItemCommand<TChild, TParent, IBuildingBlock>
      where TChild : IObjectBase
      where TParent : class, IObjectBase
   {
      protected RemoveObjectBaseCommand(TParent parent, TChild itemToRemove, IBuildingBlock buildingBlock)
         : base(parent, itemToRemove, buildingBlock)
      {
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         Description = AppConstants.Commands.RemoveFromDescription(ObjectType, _itemToRemove.Name, _parent.Name, context.TypeFor(_buildingBlock), _buildingBlock.Name);
         RemoveFrom(_itemToRemove, _parent, context);
         context.Unregister(_itemToRemove);
         context.PublishEvent(new RemovedEvent(_itemToRemove, _parent));
      }

      protected abstract void RemoveFrom(TChild childToRemove, TParent parent, IMoBiContext context);

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _parent = default(TParent);
      }
   }
}