using MoBi.Core.Domain.Model;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;

namespace MoBi.Core.Commands
{
   public class RemoveContainerFromSpatialStructureCommand : RemoveObjectBaseCommand<IContainer, IContainer>
   {
      private MoBiSpatialStructure _spatialStructure;
      private readonly string _spatialStructureId;

      public RemoveContainerFromSpatialStructureCommand(IContainer parent, IContainer childToRemove, MoBiSpatialStructure spatialStructure)
         : base(parent, childToRemove, spatialStructure)
      {
         _spatialStructure = spatialStructure;
         _spatialStructureId = _spatialStructure.Id;
      }

      protected override void RemoveFrom(IContainer childToRemove, IContainer parent, IMoBiContext context)
      {
         _spatialStructure.DiagramManager.RemoveObjectBase(childToRemove);
         parent.RemoveChild(childToRemove);
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _spatialStructure = null;
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _spatialStructure = context.Get<MoBiSpatialStructure>(_spatialStructureId);
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddContainerToSpatialStructureCommand(_parent, _itemToRemove, _spatialStructure).AsInverseFor(this);
      }
   }
}