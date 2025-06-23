using System;
using System.IO;
using System.Threading;
using Castle.Facilities.TypedFactory;
using FakeItEasy;
using MoBi.Assets;
using MoBi.Core;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Model.Diagram;
using MoBi.Core.Domain.Services;
using MoBi.Core.Serialization.Xml.Services;
using MoBi.Core.Services;
using MoBi.Engine;
using MoBi.HelpersForTests;
using MoBi.Presentation;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Serialization;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Views.BaseDiagram;
using MoBi.UI.Services;
using OSPSuite.BDDHelper;
using OSPSuite.Core;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Journal;
using OSPSuite.Core.Serialization.Diagram;
using OSPSuite.Core.Services;
using OSPSuite.Infrastructure;
using OSPSuite.Infrastructure.Container.Castle;
using OSPSuite.Presentation.Diagram.Elements;
using OSPSuite.Presentation.Services;
using OSPSuite.Presentation.Views.ContextMenus;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.FileLocker;
using CoreRegister = OSPSuite.Core.CoreRegister;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace MoBi.IntegrationTests
{
   [IntegrationTests]
   public abstract class ContextForIntegration<T> : ContextSpecification<T>
   {
      public override void GlobalContext()
      {
         base.GlobalContext();

         var container = new CastleWindsorContainer();
         IoC.InitializeWith(container);
         IoC.RegisterImplementationOf(container);
         container.RegisterImplementationOf(container.DowncastTo<IContainer>());
         container.WindsorContainer.AddFacility<TypedFactoryFacility>();

         //need to register these services for which the default implementation is in the UI
         using (container.OptimizeDependencyResolution())
         {
            container.RegisterImplementationOf(new SynchronizationContext());
            container.Register<IExceptionManager, ExceptionManagerForSpecs>(LifeStyle.Singleton);
            container.RegisterImplementationOf(A.Fake<IUserSettings>());
            container.RegisterImplementationOf(A.Fake<IDialogCreator>());
            container.RegisterImplementationOf(A.Fake<IProgressUpdater>());
            container.RegisterImplementationOf(A.Fake<IMoBiHistoryManager>());
            container.RegisterImplementationOf(A.Fake<IXmlContentSelector>());
            container.RegisterImplementationOf(A.Fake<IDiagramModel>());
            container.RegisterImplementationOf(A.Fake<IDiagramTask>());
            container.RegisterImplementationOf(A.Fake<IMRUProvider>());
            container.RegisterImplementationOf(A.Fake<IContextMenuView>());
            container.RegisterImplementationOf(A.Fake<IFileLocker>());
            container.RegisterImplementationOf(A.Fake<IDisplayUnitRetriever>());
            container.RegisterImplementationOf(A.Fake<IJournalDiagramManagerFactory>());
            container.RegisterImplementationOf(A.Fake<ISimulationDiagramManager>());
            container.RegisterImplementationOf(A.Fake<ISpatialStructureDiagramManager>());
            container.RegisterImplementationOf(A.Fake<IMoBiReactionDiagramManager>());
            container.RegisterImplementationOf(A.Fake<ISimulationDiagramView>());
            container.RegisterImplementationOf(A.Fake<IContainerBaseLayouter>());
            container.RegisterImplementationOf(A.Fake<ILayerLayouter>());
            container.RegisterImplementationOf(A.Fake<IEntityValidationTask>());
            container.RegisterImplementationOf(A.Fake<IDiagramLayoutTask>());
            container.RegisterImplementationOf(A.Fake<IPKSimStarter>());

            container.Register<IDiagramModelToXmlMapper, DiagramModelToXmlMapperForSpecs>();
            container.Register<IMoBiConfiguration, MoBiConfiguration>(LifeStyle.Singleton);
            container.Register<IEventPublisher, EventPublisher>(LifeStyle.Singleton);
            IHeavyWorkManager heavyWorkManager = new HeavyWorkManagerForSpecs();
            container.RegisterImplementationOf(heavyWorkManager);
            container.RegisterImplementationOf(A.Fake<IProgressManager>());
            var config = container.Resolve<IMoBiConfiguration>();
            container.RegisterImplementationOf((IApplicationConfiguration) config);

            var register = new SerializerRegister();
            container.AddRegister(x =>
            {
               x.FromType<CoreRegister>();
               x.FromType<Core.CoreRegister>();
               x.FromInstance(new PresentationRegister(false));
               x.FromType<InfrastructureRegister>();
               x.FromType<EngineRegister>();
               x.FromInstance(register);
            });
            register.PerformMappingForSerializerIn(container);

            container.RegisterImplementationOf(A.Fake<IMoBiMainViewPresenter>());

            setupDimensions(container);
            setupCalculationMethods(container);

            var context = container.Resolve<IMoBiContext>();
            container.RegisterImplementationOf<IWorkspace>(context);
         }
         //Required for usage with nunit 3
         Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
      }

      private static void setupCalculationMethods(IContainer container)
      {
         var configuration = IoC.Resolve<IMoBiConfiguration>();
         configuration.CalculationMethodRepositoryFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppConstants.SpecialFileNames.CALCULATION_METHOD_REPOSITORY_FILE_NAME);
         ApplicationStartup.InitCalculationMethodRepository(container);
      }

      private static void setupDimensions(IContainer container)
      {
         ApplicationStartup.InitDimensions(container);
      }

      protected override void Context()
      {
         sut = IoC.Resolve<T>();
      }

      protected void Unregister(IWithId objectWithId)
      {
         if (objectWithId == null)
            return;

         var unregisterTask = IoC.Resolve<IUnregisterTask>();
         unregisterTask.UnregisterAllIn(objectWithId);
      }
   }
}