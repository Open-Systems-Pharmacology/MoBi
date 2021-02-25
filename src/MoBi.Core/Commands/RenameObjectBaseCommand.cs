using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Events;

namespace MoBi.Core.Commands
{
   public class RenameObjectBaseCommand : BuildingBlockChangeCommandBase<IBuildingBlock>
   {
      private IObjectBase _objectBase;
      private readonly string _newName;
      public string OldName { get; private set; }
      public string ObjectId { get; private set; }

      public RenameObjectBaseCommand(IObjectBase objectBase,string newName, IBuildingBlock buildingBlock):base(buildingBlock)
      {
         _objectBase = objectBase;
         _newName = newName;
         ObjectId = objectBase.Id;
         CommandType = AppConstants.Commands.RenameCommand;
         Description = AppConstants.Commands.RenameDescription(objectBase, newName);
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return CommandExtensions.AsInverseFor(new RenameObjectBaseCommand(_objectBase, OldName,_buildingBlock), this);
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _objectBase = null;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         OldName = _objectBase.Name;
         _objectBase.Name = _newName;
         if (_objectBase.IsAnImplementationOf<IBuildingBlock>())
         {
            var renameBuildingBlockTask = context.Resolve<IRenameBuildingBlockTask>();
            renameBuildingBlockTask.RenameInSimulationUsingTemplateBuildingBlock(_objectBase.DowncastTo<IBuildingBlock>());
         }

         context.PublishEvent(new RenamedEvent(_objectBase));
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _objectBase = context.Get<IObjectBase>(ObjectId);
      }
   }
}