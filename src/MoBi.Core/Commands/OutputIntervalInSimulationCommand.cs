using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Assets;

namespace MoBi.Core.Commands
{
   public abstract class OutputIntervalInSimulationCommand : SimulationChangeCommandBase
   {
      protected OutputSchema _schema;
      protected OutputInterval _interval;

      protected OutputIntervalInSimulationCommand(OutputSchema schema, OutputInterval interval, IMoBiSimulation simulation)
         : base(simulation.OutputSchema, simulation)
      {
         _schema = schema;
         _interval = interval;
         ObjectType = ObjectTypes.SimulationSettings;
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _schema = _simulation.Settings.OutputSchema;
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _schema = null;
         _interval = null;
      }
   }

   public class AddOutputIntervalInSimulationCommand : OutputIntervalInSimulationCommand
   {
      public AddOutputIntervalInSimulationCommand(OutputSchema schema, OutputInterval interval, IMoBiSimulation simulation)
         : base(schema, interval, simulation)
      {
         CommandType = AppConstants.Commands.AddCommand;
         Description = AppConstants.Commands.AddOutputIntervalTo(simulation.Name);
      }

      protected override void DoExecute(IMoBiContext context)
      {
         _schema.AddInterval(_interval);
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveOutputIntervalFromSimulationCommand(_schema, _interval, _simulation).AsInverseFor(this);
      }
   }

   public class RemoveOutputIntervalFromSimulationCommand : OutputIntervalInSimulationCommand
   {
      public RemoveOutputIntervalFromSimulationCommand(OutputSchema schema, OutputInterval interval, IMoBiSimulation simulation)
         : base(schema, interval, simulation)
      {
         CommandType = AppConstants.Commands.DeleteCommand;
         Description = AppConstants.Commands.RemoveOutputIntervalFrom(simulation.Name);
      }

      protected override void DoExecute(IMoBiContext context)
      {
         _schema.RemoveInterval(_interval);
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddOutputIntervalInSimulationCommand(_schema, _interval, _simulation).AsInverseFor(this);
      }
   }
}