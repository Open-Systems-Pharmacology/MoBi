using MoBi.Core.Domain.Model;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;

namespace MoBi.Core.Commands
{
   public class AddTopContainerCommand : AddObjectBaseCommand<IContainer, MoBiSpatialStructure>
   {
      public AddTopContainerCommand(MoBiSpatialStructure spatialStructure, IContainer containerToAdd) : base(spatialStructure, containerToAdd, spatialStructure)
      {
      }

      protected override void AddTo(IContainer containerToAdd, MoBiSpatialStructure spatialStructure, IMoBiContext context)
      {
         spatialStructure.AddTopContainer(containerToAdd);
         spatialStructure.DiagramManager.AddObjectBase(containerToAdd);
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveTopContainerCommand(_parent, _itemToAdd).AsInverseFor(this);
      }
   }

   public class RemoveTopContainerCommand : RemoveObjectBaseCommand<IContainer, MoBiSpatialStructure>
   {
      public RemoveTopContainerCommand(MoBiSpatialStructure spatialStructure, IContainer containerToRemove) : base(spatialStructure, containerToRemove, spatialStructure)
      {
      }

      protected override void RemoveFrom(IContainer containerToRemove, MoBiSpatialStructure spatialStructure, IMoBiContext context)
      {
         spatialStructure.RemoveTopContainer(containerToRemove);
         spatialStructure.DiagramManager.RemoveObjectBase(containerToRemove);
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddTopContainerCommand(_parent, _itemToRemove).AsInverseFor(this);
      }
   }
}