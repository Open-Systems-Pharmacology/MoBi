using System.Linq;
using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Exceptions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Core.Services
{
   public interface IOutputSchemaTask
   {
      ICommand AddOuputIntervalTo(ISimulationSettings simulationSettings, IMoBiSimulation simulation);
      ICommand RemoveOuputInterval(OutputInterval outputInterval, ISimulationSettings simulationSettings, IMoBiSimulation simulation);
   }

   public class OutputSchemaTask : IOutputSchemaTask
   {
      private readonly IOutputIntervalFactory _outputIntervalFactory;
      private readonly IContainerTask _containerTask;
      private readonly IMoBiContext _context;

      public OutputSchemaTask(IOutputIntervalFactory outputIntervalFactory, IContainerTask containerTask, IMoBiContext context)
      {
         _outputIntervalFactory = outputIntervalFactory;
         _containerTask = containerTask;
         _context = context;
      }

      public ICommand AddOuputIntervalTo(ISimulationSettings simulationSettings, IMoBiSimulation simulation)
      {
         var schema = simulationSettings.OutputSchema;
         var interval = _outputIntervalFactory.CreateDefault();
         interval.Name = _containerTask.CreateUniqueName(schema, interval.Name);
         return getAddCommand(schema, interval, simulationSettings, simulation).Run(_context);

      }

      public ICommand RemoveOuputInterval(OutputInterval outputInterval, ISimulationSettings simulationSettings, IMoBiSimulation simulation)
      {
         var schema = simulationSettings.OutputSchema;
         if (schema.Intervals.Count() > 1)
            return getRemoveCommand(schema, outputInterval,simulationSettings, simulation).Run(_context);
         else
            throw new MoBiException(AppConstants.Exceptions.CanNotRemoveLastItem);
      }

      private IMoBiCommand getAddCommand(OutputSchema schema, OutputInterval interval, ISimulationSettings simulationSettings, IMoBiSimulation simulation)
      {
         if (simulation == null)
            return new AddOutputIntervalCommand(schema, interval, simulationSettings);

         return new AddOutputIntervalInSimulationCommand(schema, interval, simulation);
      }

      private IMoBiCommand getRemoveCommand(OutputSchema schema, OutputInterval interval, ISimulationSettings simulationSettings, IMoBiSimulation simulation)
      {
         if (simulation == null)
            return new RemoveOutputIntervalCommand(schema, interval, simulationSettings);

         return new RemoveOutputIntervalFromSimulationCommand(schema, interval, simulation);
      }
   }
}