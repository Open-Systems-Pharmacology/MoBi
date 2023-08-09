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
      ICommand AddOutputIntervalTo(SimulationSettings simulationSettings);
      ICommand RemoveOutputInterval(OutputInterval outputInterval, SimulationSettings simulationSettings);
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

      public ICommand AddOutputIntervalTo(SimulationSettings simulationSettings)
      {
         var schema = simulationSettings.OutputSchema;
         var interval = _outputIntervalFactory.CreateDefault();
         interval.Name = _containerTask.CreateUniqueName(schema, interval.Name);
         return getAddCommand(schema, interval, simulationSettings).Run(_context);

      }

      public ICommand RemoveOutputInterval(OutputInterval outputInterval, SimulationSettings simulationSettings)
      {
         var schema = simulationSettings.OutputSchema;
         if (schema.Intervals.Count() > 1)
            return getRemoveCommand(schema, outputInterval,simulationSettings).Run(_context);
         else
            throw new MoBiException(AppConstants.Exceptions.CanNotRemoveLastItem);
      }

      private IMoBiCommand getAddCommand(OutputSchema schema, OutputInterval interval, SimulationSettings simulationSettings)
      {
         return new AddOutputIntervalCommand(schema, interval, simulationSettings);
      }

      private IMoBiCommand getRemoveCommand(OutputSchema schema, OutputInterval interval, SimulationSettings simulationSettings)
      {
         return new RemoveOutputIntervalCommand(schema, interval, simulationSettings);
      }
   }
}