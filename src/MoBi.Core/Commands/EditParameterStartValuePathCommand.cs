using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class EditParameterStartValuePathCommand : EditStartValuePathCommand<ParameterValuesBuildingBlock, ParameterValue>
   {
      /// <summary>
      /// Changes a path for a Parameter start value
      /// </summary>
      /// <param name="buildingBlock">The building block this start value is part of</param>
      /// <param name="startValue">The start value being modified</param>
      /// <param name="newContainerPath">The new container path for the start value</param>
      public EditParameterStartValuePathCommand(ParameterValuesBuildingBlock buildingBlock, ParameterValue startValue, ObjectPath newContainerPath)
         : base(buildingBlock, startValue, newContainerPath)
      {

      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new EditParameterStartValuePathCommand(_buildingBlock, _buildingBlock[_newStartValuePath], _originalContainerPath).AsInverseFor(this);
      }
   }
}