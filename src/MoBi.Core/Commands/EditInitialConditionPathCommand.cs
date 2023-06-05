using System.Linq;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class EditInitialConditionPathCommand : EditPathAndValueEntityPathCommand<ILookupBuildingBlock<InitialCondition>, InitialCondition>
   {
      /// <summary>
      ///    Changes a path for an initial condition
      /// </summary>
      /// <param name="buildingBlock">The building block this start value is a part of</param>
      /// <param name="initialCondition">The initial condition being modified</param>
      /// <param name="newContainerPath">The new container path for this start value</param>
      public EditInitialConditionPathCommand(ILookupBuildingBlock<InitialCondition> buildingBlock, InitialCondition initialCondition, ObjectPath newContainerPath)
         : base(buildingBlock, initialCondition, newContainerPath)
      {
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new EditInitialConditionPathCommand(_buildingBlock, _buildingBlock.ByPath(_newPath), _originalContainerPath).AsInverseFor(this);
      }
   }
}