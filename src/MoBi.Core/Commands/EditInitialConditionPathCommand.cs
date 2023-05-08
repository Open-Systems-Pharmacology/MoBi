using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class EditInitialConditionPathCommand : EditStartValuePathCommand<InitialConditionsBuildingBlock, InitialCondition>
   {
      /// <summary>
      /// Changes a path for an initial condition
      /// </summary>
      /// <param name="buildingBlock">The building block this start value is a part of</param>
      /// <param name="initialCondition">The initial condition being modified</param>
      /// <param name="newContainerPath">The new container path for this start value</param>
      public EditInitialConditionPathCommand(InitialConditionsBuildingBlock buildingBlock, InitialCondition initialCondition, ObjectPath newContainerPath)
         : base(buildingBlock, initialCondition, newContainerPath)
      {

      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new EditInitialConditionPathCommand(_buildingBlock, _buildingBlock[_newStartValuePath], _originalContainerPath).AsInverseFor(this);
      }
   }
}