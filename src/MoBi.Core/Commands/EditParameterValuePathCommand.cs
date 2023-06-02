using System.Linq;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class EditParameterValuePathCommand : EditPathAndValueEntityPathCommand<IBuildingBlock<ParameterValue>, ParameterValue>
   {
      /// <summary>
      ///    Changes a path for a Parameter value
      /// </summary>
      /// <param name="buildingBlock">The building block this start value is part of</param>
      /// <param name="parameterValue">The parameter value being modified</param>
      /// <param name="newContainerPath">The new container path for the start value</param>
      public EditParameterValuePathCommand(IBuildingBlock<ParameterValue> buildingBlock, ParameterValue parameterValue, ObjectPath newContainerPath)
         : base(buildingBlock, parameterValue, newContainerPath)
      {
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new EditParameterValuePathCommand(_buildingBlock, _buildingBlock.Single(x => Equals(x.Path, _newPath)), _originalContainerPath).AsInverseFor(this);
      }
   }
}