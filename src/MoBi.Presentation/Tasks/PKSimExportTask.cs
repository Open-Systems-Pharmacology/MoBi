using System.Collections.Generic;
using System.IO;
using System.Linq;
using OSPSuite.Utility;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Repositories;
using OSPSuite.Core.Journal;
using OSPSuite.Core.Serialization.Exchange;

namespace MoBi.Presentation.Tasks
{
   public interface IPKSimExportTask
   {
      void SendSimulationToPKSim(IMoBiSimulation simulation );
      void StartWithContentFile(string contentFile);
   }

   public class PKSimExportTask : IPKSimExportTask
   {
      private readonly IPKSimStarter _pkSimStarter;
      private readonly ISimulationPersistor _simulationPersister;
      private readonly IFavoriteRepository _favoriteRepository;
      private readonly IJournalRetriever _journalRetriever;

      public PKSimExportTask(IPKSimStarter pkSimStarter, ISimulationPersistor simulationPersister, IFavoriteRepository favoriteRepository,  IJournalRetriever journalRetriever)
      {
         _pkSimStarter = pkSimStarter;
         _simulationPersister = simulationPersister;
         _favoriteRepository = favoriteRepository;
         _journalRetriever = journalRetriever;
      }

      public void SendSimulationToPKSim(IMoBiSimulation simulation)
      {
         var simulationTransfer = new SimulationTransfer
         {
            Simulation = simulation,
            PkmlVersion = Constants.PKML_VERSION,
            AllObservedData = Enumerable.Empty<DataRepository>().ToList(),
            Favorites = _favoriteRepository.Favorites,
            JournalPath = _journalRetriever.JournalFullPath
         };

         var fileName = FileHelper.GenerateTemporaryFileName();
         fileName = Path.ChangeExtension(fileName, Constants.Filter.PKML_EXTENSION);
         _simulationPersister.Save(simulationTransfer, fileName);
         _pkSimStarter.StartPopulationSimulationWithSimulationFile(fileName);
      }

      public void StartWithContentFile(string contentFile)
      {
         _pkSimStarter.StartWithWorkingJournalFile(_journalRetriever.JournalFullPath);
      }
   }
}