using System.Collections.Generic;
using System.Linq;
using MoBi.R.Services;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.R.Domain;
using ParameterValue = OSPSuite.Core.Domain.Populations.ParameterValue;

namespace MoBi.R.Tests.Services
{
   internal abstract class concern_for_MoBiRIntegrationServiceSpecs : ContextForIntegration<IMoBiRIntegrationService>
   {
      public override void GlobalContext()
      {
         base.GlobalContext();
         sut = Api.GetIntegrationTask();
      }
   }

   internal class when_creating_an_individual : concern_for_MoBiRIntegrationServiceSpecs
   {
      private IndividualBuildingBlock _individual;

      protected override void Because()
      {
         _individual = sut.CreateIndividual("TestIndividual");
      }

      [Observation]
      public void should_create_individual_with_given_name()
      {
         _individual.Name.ShouldBeEqualTo("TestIndividual");
      }
   }

   internal class when_creating_an_individual_and_setting_values : concern_for_MoBiRIntegrationServiceSpecs
   {
      private IndividualBuildingBlock _individual;

      protected override void Because()
      {
         _individual = sut.CreateIndividual("TestIndividual");
      }

      [Observation]
      public void should_create_individual_with_given_name_and_characteristics()
      {
         //Pull this 2 types (distributed parameters ) to core
         //Create values here with this type
         //not only map but allow the creation of them with the properties
         var lstParameters = new List<ParameterValue>();
         //why not send directly the parameterValue? 
         //do they need to provide the percentile? or is it fixed and default?
         lstParameters.Add(new ParameterValue("AGE", 45, 0.5));
         lstParameters.Add(new ParameterValue("Weight", 70, 0.5));
         lstParameters.Add(new ParameterValue("Height", 175, 0.5));

         _individual = sut.AddParametersToIndividual(_individual, lstParameters);
         _individual.Any(x => x.Name.Equals("AGE")).ShouldBeTrue();
      }
   }
}