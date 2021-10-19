using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class AddParameterToContainerCommand : AddObjectBaseCommand<IParameter, IContainer>
   {
      public AddParameterToContainerCommand(IContainer parent, IParameter itemToAdd, IBuildingBlock buildingBlock)
         : base(parent, itemToAdd, buildingBlock)
      {
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveParameterFromContainerCommand(_parent, _itemToAdd, _buildingBlock).AsInverseFor(this);
      }

      protected override void AddTo(IParameter parameter, IContainer parent, IMoBiContext context)
      {
         if (!parent.CanSetBuildModeForParameters())
            parameter.BuildMode = parent.DefaultParameterBuildMode();

         _parent.Add(parameter);
      }
   }

   public class RemoveParameterFromContainerCommand : RemoveObjectBaseCommand<IParameter, IContainer>
   {
      public RemoveParameterFromContainerCommand(IContainer parent, IParameter itemToRemove, IBuildingBlock buildingBlock)
         : base(parent, itemToRemove, buildingBlock)
      {
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddParameterToContainerCommand(_parent, _itemToRemove, _buildingBlock).AsInverseFor(this);
      }

      protected override void RemoveFrom(IParameter childToRemove, IContainer parent, IMoBiContext context)
      {
         parent.RemoveChild(childToRemove);
      }
   }
}