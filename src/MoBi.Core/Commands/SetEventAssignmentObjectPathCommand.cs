﻿using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Extensions;
using OSPSuite.Assets;

namespace MoBi.Core.Commands
{
   public class SetEventAssignmentObjectPathCommand : BuildingBlockChangeCommandBase<IBuildingBlock>
   {
      private EventAssignmentBuilder _assignment;
      private readonly FormulaUsablePath _oldObjectPath;
      private readonly FormulaUsablePath _newObjectPath;
      private readonly string _assignmentId;

      public SetEventAssignmentObjectPathCommand(EventAssignmentBuilder assignment, FormulaUsablePath newObjectPath, IBuildingBlock buildingBlock)
         : base(buildingBlock)
      {
         _assignment = assignment;
         _assignmentId = assignment.Id;
         _newObjectPath = newObjectPath;
         _oldObjectPath = _assignment.ObjectPath as FormulaUsablePath;
         ObjectType = ObjectTypes.EventAssignmentBuilder;
         CommandType = AppConstants.Commands.EditCommand;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         _assignment.ObjectPath = _newObjectPath;
         Description = AppConstants.Commands.UpdateAssignmentObjectPath(_assignment.EntityPath(), (_newObjectPath ?? new FormulaUsablePath()).ToPathString());
         if (_newObjectPath == null) return;
         updateDimension(_assignment);
         updateDimension(_assignment.Formula);
      }

      private void updateDimension(IWithDimension withDimension)
      {
         if (withDimension == null) return;
         withDimension.Dimension = _newObjectPath.Dimension;
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _assignment = null;
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _assignment = context.Get<EventAssignmentBuilder>(_assignmentId);
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new SetEventAssignmentObjectPathCommand(_assignment, _oldObjectPath, _buildingBlock).AsInverseFor(this);
      }
   }
}