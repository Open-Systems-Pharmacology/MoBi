using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Serialization.SimModel.Services;
using OSPSuite.Core.Services;

namespace MoBi.Presentation.Tasks
{
   public interface ISimModelManagerFactory
   {
      ISimModelManager Create();
   }

   public class SimModelManagerFactory : ISimModelManagerFactory
   {
      private readonly ISimModelExporter _simModelExporter;
      private readonly ISimModelSimulationFactory _simModelSimulationFactory;
      private readonly IMoBiContext _context;
      private readonly IDataNamingService _dataNamingService;
      private readonly IDisplayUnitRetriever _displayUnitRetriever;

      public SimModelManagerFactory(ISimModelExporter simModelExporter, ISimModelSimulationFactory simModelSimulationFactory, IMoBiContext context, IDataNamingService dataNamingService, IDisplayUnitRetriever displayUnitRetriever)
      {
         _simModelExporter = simModelExporter;
         _simModelSimulationFactory = simModelSimulationFactory;
         _context = context;
         _dataNamingService = dataNamingService;
         _displayUnitRetriever = displayUnitRetriever;
      }

      public ISimModelManager Create()
      {
         return new SimModelManager(_simModelExporter, _simModelSimulationFactory, createDataFactory());
      }

      private DataFactory createDataFactory()
      {
         return new DataFactory(_context.DimensionFactory, _dataNamingService, _context.ObjectPathFactory, _displayUnitRetriever);
      }
   }
}