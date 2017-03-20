using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class ChangeParameterStartValueNameCommand : ChangeStartValueNameCommand<IParameterStartValuesBuildingBlock, IParameterStartValue>
   {
      public ChangeParameterStartValueNameCommand(IParameterStartValuesBuildingBlock buildingBlock, IObjectPath path, string newValue) : base(buildingBlock, path, newValue)
      {
      }
      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new ChangeParameterStartValueNameCommand(_buildingBlock, new ObjectPath(_path), _oldValue).AsInverseFor(this);
      }
   }
}