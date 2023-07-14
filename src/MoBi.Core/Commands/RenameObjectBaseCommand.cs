using System;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Helper;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Events;
using OSPSuite.Utility.Extensions;

namespace MoBi.Core.Commands
{
   public class RenameObjectBaseCommand : BuildingBlockChangeCommandBase<IBuildingBlock>
   {
      private IObjectBase _objectBase;
      protected readonly string _newName;
      public string OldName { get; private set; }
      public string ObjectId { get; }

      public RenameObjectBaseCommand(IObjectBase objectBase, string newName, IBuildingBlock buildingBlock) : base(buildingBlock)
      {
         _objectBase = objectBase;
         _newName = newName;
         ObjectId = objectBase.Id;
         ObjectType = new ObjectTypeResolver().TypeFor(objectBase);
         CommandType = AppConstants.Commands.RenameCommand;
         Description = AppConstants.Commands.RenameDescription(objectBase, newName);
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RenameObjectBaseCommand(_objectBase, OldName, _buildingBlock).AsInverseFor(this);
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _objectBase = null;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         RenameObjectBase(context);

         context.PublishEvent(new RenamedEvent(_objectBase));
      }

      protected virtual void RenameObjectBase(IMoBiContext context)
      {
         OldName = _objectBase.Name;
         _objectBase.Name = _newName;

         if (_objectBase.IsAnImplementationOf<IBuildingBlock>())
         {
            doWithRenameInSimulationTask(task => task.RenameInSimulationUsingTemplateBuildingBlock(OldName, _objectBase.DowncastTo<IBuildingBlock>()), context);
         }
         else if (_objectBase.IsAnImplementationOf<Module>())
         {
            doWithRenameInSimulationTask(task => task.RenameInSimulationUsingTemplateModule(OldName, _objectBase.DowncastTo<Module>()), context);
         }
      }

      private void doWithRenameInSimulationTask(Action<IRenameInSimulationTask> action, IMoBiContext context)
      {
         var renameInSimulationTask = context.Resolve<IRenameInSimulationTask>();
         action(renameInSimulationTask);
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _objectBase = context.Get<IObjectBase>(ObjectId);
      }
   }
}