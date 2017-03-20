using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using MoBi.Core.Domain.Model;

namespace MoBi.IntegrationTests
{
   public abstract class concern_for_BuildConfigurationLoad : ContextWithLoadedProject
   {
   }

   public class When_loading_a_pkml_file_created_in_pksim : concern_for_BuildConfigurationLoad
   {
      [Observation]
      public void should_be_able_to_load_the_build_configuraion()
      {
         var buildConfiguration = LoadPKML<IMoBiBuildConfiguration>("warnings");
         buildConfiguration.ShouldNotBeNull();
      }
   }
}