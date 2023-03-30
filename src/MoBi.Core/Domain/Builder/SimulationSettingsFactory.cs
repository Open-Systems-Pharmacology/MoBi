using MoBi.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Domain.Builder
{
   public interface ISimulationSettingsFactory
   {
      SimulationSettings CreateDefault();
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

      public SimulationSettings CreateDefault()
      {
         var simulationSettings = _objectBaseFactory.Create<SimulationSettings>();
         simulationSettings.OutputSchema = _outputSchemaFactory.CreateDefault();
         simulationSettings.Solver = _solverSettingsFactory.CreateCVODE();
         simulationSettings.OutputSelections = new OutputSelections();
         simulationSettings.Name = AppConstants.Captions.DefaultSimulationSettings;
         return simulationSettings;
      }
   }
}