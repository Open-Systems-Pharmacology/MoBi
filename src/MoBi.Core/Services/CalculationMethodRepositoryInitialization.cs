using MoBi.Assets;
using MoBi.Core.Serialization.Xml.Services;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Extensions;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace MoBi.Core.Services
{
   public static class CalculationMethodRepositoryInitialization
   {
      public static void Initialize(IContainer container)
      {
         var calculationMethodsRepositoryPersistor = container.Resolve<ICalculationMethodsRepositoryPersistor>();
         calculationMethodsRepositoryPersistor.Load();

         // Add Empty CMs to use in non PK-Sim models
         var repository = container.Resolve<ICoreCalculationMethodRepository>();
         var objectBaseFactory = container.Resolve<IObjectBaseFactory>();

         repository
            .GetAllCategoriesDefault()
            .Each(cm => repository.AddCalculationMethod(createDefaultCalculationMethodForCategory(cm.Category, objectBaseFactory)));
      }

      private static CoreCalculationMethod createDefaultCalculationMethodForCategory(string category, IObjectBaseFactory objectBaseFactory)
      {
         var cm = objectBaseFactory.Create<CoreCalculationMethod>()
            .WithName(AppConstants.DefaultNames.EmptyCalculationMethod)
            .WithDescription(AppConstants.DefaultNames.EmptyCalculationMethodDescription);

         cm.Category = category;
         return cm;
      }
   }
}