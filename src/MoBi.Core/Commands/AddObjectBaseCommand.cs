using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Events;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public abstract class AddObjectBaseCommand<TChild, TParent> : AddItemCommand<TChild, TParent, IBuildingBlock>, ISilentCommand
      where TChild : class, IObjectBase
      where TParent : class, IObjectBase
   {
      private string childId { get; }
      public bool Silent { get; set; }

      protected AddObjectBaseCommand(TParent parent, TChild itemToAdd, IBuildingBlock buildingBlock)
         : base(parent, itemToAdd, buildingBlock)
      {
         Silent = false;
         childId = itemToAdd.Id;
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _itemToAdd = null;
         _parent = null;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         Description = AppConstants.Commands.AddToDescription(ObjectType, _itemToAdd.Name, _parent.Name, context.TypeFor(_buildingBlock), _buildingBlock.Name);
         register(_itemToAdd, context);
         AddTo(_itemToAdd, _parent, context);

         if (!Silent)
            context.PublishEvent(new AddedEvent<TChild>(_itemToAdd, _parent));
      }

      private void register(TChild itemToAdd, IMoBiContext context) => context.Resolve<IRegisterTask>().RegisterAllIn(itemToAdd);

      protected abstract void AddTo(TChild child, TParent parent, IMoBiContext context);

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _itemToAdd = context.Get<TChild>(childId);
      }
   }
}