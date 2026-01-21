using MoBi.Assets;
using MoBi.Core.Serialization.Xml.Services;
using MoBi.Presentation.Serialization;
using OSPSuite.CLI.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Infrastructure;
using OSPSuite.R;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;
using CoreRegister = MoBi.Core.CoreRegister;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace MoBi.R.Bootstrap
{
   internal class ApplicationStartup
   {
      public static void Initialize(ApiConfig apiConfig)
      {
         OSPSuite.R.Api.InitializeOnce(apiConfig, registerAction);
         new SerializerRegister().PerformMappingForMoBiSerializerRepository(OSPSuite.R.Api.Container);
         initCalculationMethodRepository(OSPSuite.R.Api.Container);
      }

      private static void initCalculationMethodRepository(IContainer container)
      {
         var calculationMethodsRepositoryPersistor = container.Resolve<ICalculationMethodsRepositoryPersistor>();
         calculationMethodsRepositoryPersistor.Load();
         //Add Empty CMs to use in non PK-Sim Models
         var rep = container.Resolve<ICoreCalculationMethodRepository>();
         var objectBaseFactory = container.Resolve<IObjectBaseFactory>();
         rep.GetAllCategoriesDefault().Each(cm => rep.AddCalculationMethod(createDefaultCalculationMethodForCategory(cm.Category, objectBaseFactory)));
      }

      private static CoreCalculationMethod createDefaultCalculationMethodForCategory(string category, IObjectBaseFactory objectBaseFactory)
      {
         var cm = objectBaseFactory.Create<CoreCalculationMethod>()
            .WithName(AppConstants.DefaultNames.EmptyCalculationMethod)
            .WithDescription(AppConstants.DefaultNames.EmptyCalculationMethodDescription);

         cm.Category = category;
         return cm;
      }

      private static void registerAction(IContainer container)
      {
         container.AddRegister(x => x.FromType<CoreRegister>());
         container.AddRegister(x => x.FromType<InfrastructureRegister>());
         container.AddRegister(x => x.FromType<RRegister>());
         container.AddRegister(x => x.FromType<CLIRegister>());
      }
   }
}