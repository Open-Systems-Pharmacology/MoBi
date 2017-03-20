using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Assets;

namespace MoBi.Core.Commands
{
   public abstract class OutputIntervalCommandBase : BuildingBlockChangeCommandBase<ISimulationSettings>
   {
      protected OutputSchema _schema;
      protected OutputInterval _interval;

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _schema = null;
         _interval = null;
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _schema = _buildingBlock.OutputSchema;
      }

      protected OutputIntervalCommandBase(OutputSchema schema, OutputInterval interval, ISimulationSettings simulationSettings) : base(simulationSettings)
      {
         _schema = schema;
         _interval = interval;
         ObjectType =ObjectTypes.SimulationSettings;
      }
   }

   public class AddOutputIntervalCommand : OutputIntervalCommandBase
   {
      public AddOutputIntervalCommand(OutputSchema schema, OutputInterval interval, ISimulationSettings simulationSettings) : base(schema, interval, simulationSettings)
      {
         CommandType = AppConstants.Commands.AddCommand;
         Description = AppConstants.Commands.AddOutputIntervalTo(simulationSettings.Name);
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         _schema.AddInterval(_interval);
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveOutputIntervalCommand(_schema, _interval, _buildingBlock).AsInverseFor(this);
      }
   }

   public class RemoveOutputIntervalCommand : OutputIntervalCommandBase
   {
      public RemoveOutputIntervalCommand(OutputSchema schema, OutputInterval interval, ISimulationSettings simulationSettings) : base(schema, interval, simulationSettings)
      {
         CommandType = AppConstants.Commands.DeleteCommand;
         Description = AppConstants.Commands.RemoveOutputIntervalFrom(simulationSettings.Name);
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         _schema.RemoveInterval(_interval);
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddOutputIntervalCommand(_schema, _interval, _buildingBlock).AsInverseFor(this);
      }
   }
}