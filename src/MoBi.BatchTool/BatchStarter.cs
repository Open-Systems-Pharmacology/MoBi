using MoBi.BatchTool.Services;
using MoBi.Engine;
using MoBi.Presentation;
using MoBi.Presentation.Serialization;
using MoBi.Presentation.Settings;
using MoBi.UI.Services;
using OSPSuite.Core;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Journal;
using OSPSuite.Core.Serialization.Diagram;
using OSPSuite.Infrastructure;
using OSPSuite.Presentation;
using OSPSuite.Utility.Container;
using CoreRegister = MoBi.Core.CoreRegister;

namespace MoBi.BatchTool
{
   public static class BatchStarter
   {
      public static void Start()
      {
         ApplicationStartup.Initialize();
         var container = IoC.Container;
         using (container.OptimizeDependencyResolution())
         {
            var register = new SerializerRegister();
            container.Register<IPresentationUserSettings, IUserSettings, ICoreUserSettings, BatchUserSettings>(LifeStyle.Transient);

            container.AddRegister(x =>
            {
               x.FromType<CoreRegister>();
               x.FromType<OSPSuite.Core.CoreRegister>();
               x.FromType<BatchRegister>();
               x.FromType<EngineRegister>();
               x.FromType<InfrastructureRegister>();
               x.FromInstance(new PresentationRegister(false));
               x.FromInstance(register);
            });
            register.PerformMappingForSerializerIn(container);


            container.Register<IJournalDiagramManagerFactory, BatchJournalDiagramManagerFactory>();
            container.Register<IDiagramModelToXmlMapper, BatchDiagramModelToXmlMapper>(LifeStyle.Singleton);
            container.Register<IDiagramModel, BatchDiagramModel>(LifeStyle.Singleton);
         }
         setupDimensions(container);
         setupCalculationMethods(container);
      }

      private static void setupCalculationMethods(IContainer container)
      {
         ApplicationStartup.InitCalculationMethodRepository(container);
      }

      private static void setupDimensions(IContainer container)
      {
         ApplicationStartup.InitDimensions(container);
      }
   }
}