using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSPSuite.Core;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Journal;
using OSPSuite.Core.Serialization.Diagram;
using OSPSuite.Core.Services;
using OSPSuite.Infrastructure.Import.Services;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Utility.Format;

namespace MoBi.CLI
{
   public static class ApplicationStartup
   {
      public static void Initialize()
      {
         //var container = InfrastructureRegister.Initialize();
         //container.RegisterImplementationOf(new SynchronizationContext());
         //container.Register<IExceptionManager, CLIExceptionManager>(LifeStyle.Singleton);
      }

      public static void Start()
      {
         var container = IoC.Container;

         using (container.OptimizeDependencyResolution())
         {
            container.RegisterImplementationOf(NumericFormatterOptions.Instance);

            registerCLITypes(container);

            //container.AddRegister(x => x.FromType<CoreRegister>());
            //container.AddRegister(x => x.FromType<InfrastructureRegister>());
            //container.AddRegister(x => x.FromType<CLIRegister>());

            //InfrastructureRegister.LoadSerializers(container);
         }
      }

      private static void registerCLITypes(IContainer container)
      {
         //container.Register<IProgressUpdater, NoneProgressUpdater>();
         //container.Register<IDialogCreator, CLIDialogCreator>();
         //container.Register<IDisplayUnitRetriever, CLIDisplayUnitRetriever>();
         //container.Register<IJournalDiagramManagerFactory, CLIJournalDiagramManagerFactory>();
         //container.Register<IDiagramModel, CLIDiagramModel>();
         //container.Register<IDataImporter, CLIDataImporter>();
         //container.Register<IEntityValidationTask, CLIEntityValidationTask>();
         //container.Register<IOntogenyTask, CLIIndividualOntogenyTask>();
         //container.Register<IDiagramModelToXmlMapper, CLIDiagramModelToXmlMapper>(LifeStyle.Singleton);
         //container.Register<IHistoryManager, HistoryManager<IExecutionContext>>();
         //container.Register<ICoreUserSettings, OSPSuite.Core.ICoreUserSettings, CLIUserSettings>(LifeStyle.Singleton);
         //container.Register<ICoreWorkspace, IWorkspace, CLIWorkspace>(LifeStyle.Singleton);
      }
   }
}
