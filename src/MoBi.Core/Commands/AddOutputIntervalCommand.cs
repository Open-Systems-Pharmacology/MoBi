using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Assets;
using OSPSuite.Core.Events;

namespace MoBi.Core.Commands
{
   public abstract class OutputIntervalCommandBase : BuildingBlockChangeCommandBase<SimulationSettings>
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

      protected OutputIntervalCommandBase(OutputSchema schema, OutputInterval interval, SimulationSettings simulationSettings) : base(simulationSettings)
      {
         _schema = schema;
         _interval = interval;
         ObjectType =ObjectTypes.SimulationSettings;
      }
   }

   public class AddOutputIntervalCommand : OutputIntervalCommandBase
   {
      private string _addedIntervalId;

      public AddOutputIntervalCommand(OutputSchema schema, OutputInterval interval, SimulationSettings simulationSettings) : base(schema, interval, simulationSettings)
      {
         CommandType = AppConstants.Commands.AddCommand;
         Description = AppConstants.Commands.AddOutputIntervalTo(simulationSettings.Name);
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         context.Register(_interval);
         _schema.AddInterval(_interval);
         _addedIntervalId = _interval.Id;
         context.PublishEvent(new OutputSchemaChangedEvent(_schema));
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _interval = context.Get<OutputInterval>(_addedIntervalId);
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveOutputIntervalCommand(_schema, _interval, _buildingBlock).AsInverseFor(this);
      }
   }

   public class RemoveOutputIntervalCommand : OutputIntervalCommandBase
   {
      private byte[] _serializedInterval;

      public RemoveOutputIntervalCommand(OutputSchema schema, OutputInterval interval, SimulationSettings simulationSettings) : base(schema, interval, simulationSettings)
      {
         CommandType = AppConstants.Commands.DeleteCommand;
         Description = AppConstants.Commands.RemoveOutputIntervalFrom(simulationSettings.Name);
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         _serializedInterval = context.Serialize(_interval);
         _schema.RemoveInterval(_interval);
         context.Unregister(_interval);
         context.PublishEvent(new OutputSchemaChangedEvent(_schema));
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _interval = context.Deserialize<OutputInterval>(_serializedInterval);
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddOutputIntervalCommand(_schema, _interval, _buildingBlock).AsInverseFor(this);
      }
   }
}