using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Serialization.SimModel.Services;
using OSPSuite.Core.Services;

namespace MoBi.Core.Services
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
      private readonly IDisplayUnitRetriever _displayUnitRetriever;
      private readonly IDataRepositoryTask _dataRepositoryTask;

      public SimModelManagerFactory(ISimModelExporter simModelExporter, ISimModelSimulationFactory simModelSimulationFactory, IMoBiContext context,  IDisplayUnitRetriever displayUnitRetriever, IDataRepositoryTask dataRepositoryTask)
      {
         _simModelExporter = simModelExporter;
         _simModelSimulationFactory = simModelSimulationFactory;
         _context = context;
         _displayUnitRetriever = displayUnitRetriever;
         _dataRepositoryTask = dataRepositoryTask;
      }

      public ISimModelManager Create()
      {
         return new SimModelManager(_simModelExporter, _simModelSimulationFactory, createDataFactory());
      }

      private DataFactory createDataFactory()
      {
         return new DataFactory(_context.DimensionFactory, _context.ObjectPathFactory, _displayUnitRetriever, _dataRepositoryTask);
      }
   }
}