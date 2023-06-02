using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class ChangeInitialConditionNameCommand : ChangePathAndValueEntityNameCommand<IBuildingBlock<InitialCondition>, InitialCondition>
   {
      public ChangeInitialConditionNameCommand(IBuildingBlock<InitialCondition> buildingBlock, ObjectPath path, string newValue): base(buildingBlock, path, newValue)
      {
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new ChangeInitialConditionNameCommand(_buildingBlock, new ObjectPath(_path), _oldValue).AsInverseFor(this);
      }
   }
}