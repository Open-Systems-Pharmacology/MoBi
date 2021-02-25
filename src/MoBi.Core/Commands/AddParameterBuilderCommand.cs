using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class AddParameterBuilderCommand : AddObjectBaseCommand<IParameter,IContainer>
   {
      public AddParameterBuilderCommand(IContainer parent, IParameter itemToAdd, IBuildingBlock buildingBlock)
         : base(parent, itemToAdd, buildingBlock)
      {
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveParameterBuilderCommand(_parent,_itemToAdd,_buildingBlock).AsInverseFor(this);
      }

      protected override void AddTo(IParameter child, IContainer parent, IMoBiContext context)
      {
         _parent.Add(child);
      }
   }

   public class RemoveParameterBuilderCommand : RemoveObjectBaseCommand<IParameter, IContainer>
   {
      public RemoveParameterBuilderCommand(IContainer parent, IParameter itemToRemove, IBuildingBlock buildingBlock)
         : base(parent, itemToRemove, buildingBlock)
      {
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddParameterBuilderCommand(_parent,_itemToRemove,_buildingBlock).AsInverseFor(this);
      }

      protected override void RemoveFrom(IParameter childToRemove, IContainer parent, IMoBiContext context)
      {
         parent.RemoveChild(childToRemove);
      }
   }
}