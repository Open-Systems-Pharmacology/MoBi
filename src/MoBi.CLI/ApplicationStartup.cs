using Castle.Facilities.TypedFactory;
using MoBi.CLI.Core;
using OSPSuite.Infrastructure.Container.Castle;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Format;
using CoreRegister = MoBi.Core.CoreRegister;

namespace MoBi.CLI;

public static class ApplicationStartup
{
   public static void Initialize()
   {
      var container = new CastleWindsorContainer();
      IoC.InitializeWith(container);

      container.WindsorContainer.AddFacility<EventRegisterFacility>();

      //required to used abstract factory pattern with container
      container.WindsorContainer.AddFacility<TypedFactoryFacility>();

      //Register container into container to avoid any reference to dependency in code
      container.RegisterImplementationOf(container.DowncastTo<IContainer>());
   }

   public static void Start()
   {
      var container = IoC.Container;

      using (container.OptimizeDependencyResolution())
      {
         container.RegisterImplementationOf(NumericFormatterOptions.Instance);
         container.AddRegister(x => x.FromType<CoreRegister>());
         container.AddRegister(x => x.FromType<CLIRegister>());
      }
   }
}