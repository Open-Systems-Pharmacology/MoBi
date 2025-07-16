using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.Utility.Container;
using MoBi.CLI.Core;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.CLI.Core.Services;

namespace MoBi.IntegrationTests
{
   public abstract class ContextForCLIIntegration<T> : ContextSpecification<T>
   {
      protected override void Context()
      {
         base.Context();
         ApplicationStartup.Initialize();
         ApplicationStartup.Start();
         sut = IoC.Resolve<T>();
      }
   }

   public abstract class concern_for_BatchRunnerSpecs<T> : ContextForCLIIntegration<IBatchRunner<T>>
   {
      [TestCase]
      public void should_be_able_to_instantiate_the_instance_of_the_batch_runner()
      {
         sut.ShouldNotBeNull();
      }
   }
}
