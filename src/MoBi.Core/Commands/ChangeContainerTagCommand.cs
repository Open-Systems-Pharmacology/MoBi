using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Assets;

namespace MoBi.Core.Commands
{
   public class ChangeContainerTagCommand : BuildingBlockChangeCommandBase<IBuildingBlock>
   {
      private IContainer _container;
      public string ContainerId { get; set; }
      public string OldTag { get; set; }
      public string NewTag { get; set; }

      public ChangeContainerTagCommand(string newTag, string oldTag, IContainer container, IBuildingBlock buildingBlock) : base(buildingBlock)
      {
         NewTag = newTag;
         OldTag = oldTag;
         _container = container;
         ContainerId = container.Id;
         CommandType = AppConstants.Commands.EditCommand;
         ObjectType = ObjectTypes.Container;
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new ChangeContainerTagCommand(OldTag, NewTag, _container, _buildingBlock)
         {
            Visible = Visible
         }.AsInverseFor(this);
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _container = null;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         _container.RemoveTag(OldTag);
         _container.AddTag(NewTag);
         Description = AppConstants.Commands.EditTagDescription(ObjectTypes.Container, OldTag, NewTag, _container.Name);
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _container = context.Get<IContainer>(ContainerId);
      }
   }
}