using System.Collections.Generic;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Repositories;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Journal;
using OSPSuite.Core.Serialization.Exchange;

namespace MoBi.Presentation.Tasks
{
   public abstract class concern_for_PKSimExportTask : ContextSpecification<IPKSimExportTask>
   {
      protected IPKSimStarter _pkSimStarter;
      protected ISimulationPersistor _simulationPersister;
      protected IFavoriteRepository _favoriteRepository;
      private IJournalRetriever _journalRetriever;

      protected override void Context()
      {
         _pkSimStarter = A.Fake<IPKSimStarter>();
         _simulationPersister = A.Fake<ISimulationPersistor>();
         _favoriteRepository = A.Fake<IFavoriteRepository>();
         _journalRetriever = A.Fake<IJournalRetriever>();
         A.CallTo(() => _journalRetriever.JournalFullPath).Returns("JournalFullPath");

         sut = new PKSimExportTask(_pkSimStarter, _simulationPersister, _favoriteRepository, _journalRetriever);
      }
   }

   internal class When_sending_a_simulation_to_PKSim : concern_for_PKSimExportTask
   {
      private SimulationTransfer _simulationTransfer;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _simulationPersister.Save(A<SimulationTransfer>._, A<string>._))
            .Invokes(x => _simulationTransfer = x.GetArgument<SimulationTransfer>(0));

         A.CallTo(() => _favoriteRepository.Favorites).Returns(new Favorites{"A", "B"});
      }

      protected override void Because()
      {
         sut.SendSimulationToPKSim(A.Fake<IMoBiSimulation>());
      }

      [Observation]
      public void should_check_that_PKSim_is_installed()
      {
         A.CallTo(() => _pkSimStarter.CheckPKSimInstallation()).MustHaveHappened();
      }

      [Observation]
      public void should_export_simualtion_transfer_to_a_pkml_file()
      {
         A.CallTo(() => _simulationPersister
            .Save(A<SimulationTransfer>._, A<string>.That.Matches(s => s.EndsWith(Constants.Filter.PKML_EXTENSION))))
            .MustHaveHappened();
      }

      [Observation]
      public void should_start_pksim()
      {
         A.CallTo(() => _pkSimStarter.StartWithSimulationFile(A<string>._)).MustHaveHappened();
      }

      [Observation]
      public void should_add_the_favorites_defined_in_the_project()
      {
         _simulationTransfer.Favorites.ShouldOnlyContain("A", "B");
      }

      [Observation]
      public void should_add_the_journal_path_defined_in_the_project()
      {
         _simulationTransfer.JournalPath.ShouldBeEqualTo("JournalFullPath");
      }
   }
}