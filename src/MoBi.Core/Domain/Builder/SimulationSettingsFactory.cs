using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Domain.Builder
{
   public interface ISimulationSettingsFactory
   {
      ISimulationSettings CreateDefault();
   }

   public class SimulationSettingsFactory : ISimulationSettingsFactory
   {
      private readonly IObjectBaseFactory _objectBaseFactory;
      private readonly IOutputSchemaFactory _outputSchemaFactory;
      private readonly ISolverSettingsFactory _solverSettingsFactory;

      public SimulationSettingsFactory(IObjectBaseFactory objectBaseFactory, IOutputSchemaFactory outputSchemaFactory, ISolverSettingsFactory solverSettingsFactory)
      {
         _objectBaseFactory = objectBaseFactory;
         _outputSchemaFactory = outputSchemaFactory;
         _solverSettingsFactory = solverSettingsFactory;
      }

      public ISimulationSettings CreateDefault()
      {
         var simulationSettings = _objectBaseFactory.Create<ISimulationSettings>();
         simulationSettings.OutputSchema = _outputSchemaFactory.CreateDefault();
         simulationSettings.Solver = _solverSettingsFactory.CreateCVODE();
         simulationSettings.OutputSelections = new OutputSelections();
         return simulationSettings;
      }
   }
}