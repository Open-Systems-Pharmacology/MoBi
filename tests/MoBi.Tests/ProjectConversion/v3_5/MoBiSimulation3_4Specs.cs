using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.IntegrationTests;
using OSPSuite.Core.Serialization.Exchange;

namespace MoBi.ProjectConversion.v3_5
{
   public class When_converting_the_MoBiSimulation3_4 : ContextWithLoadedProject
   {
      private IMoBiSimulation _simulation;

      protected override void Context()
      {
         base.Context();
         _simulation = LoadPKML<IMoBiSimulation>("MoBiSimulation3_4");
      }

      [Observation]
      public void should_have_loaded_the_ParameterIdentificationWorkingDirectory_as_defined_in_the_project()
      {
         _simulation.ParameterIdentificationWorkingDirectory.ShouldNotBeNull();
      }
   }
}