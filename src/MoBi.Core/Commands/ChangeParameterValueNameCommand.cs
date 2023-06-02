using MoBi.Core.Domain.Model;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class ChangeParameterValueNameCommand : ChangePathAndValueEntityNameCommand<IBuildingBlock<ParameterValue>, ParameterValue>
   {
      public ChangeParameterValueNameCommand(IBuildingBlock<ParameterValue> buildingBlock, ObjectPath path, string newValue) : base(buildingBlock, path, newValue)
      {
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new ChangeParameterValueNameCommand(_buildingBlock, new ObjectPath(_path), _oldValue).AsInverseFor(this);
      }
   }
}