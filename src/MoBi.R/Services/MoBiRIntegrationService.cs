using OSPSuite.Core.Domain.Builder;
using OSPSuite.R.Domain;
using OSPSuite.Utility.Collections;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;
using ParameterValue = OSPSuite.Core.Domain.Populations.ParameterValue;

namespace MoBi.R.Services
{
   public interface IMoBiRIntegrationService
   {
      IndividualBuildingBlock CreateIndividual(string name);
      void SetParameterValuesParameter(ParameterValuesBuildingBlock buildingBlock, string parameterName, double newValue);
      void SetInitialConditionParameter(InitialConditionsBuildingBlock buildingBlock, string parameterName, double newValue);
      IndividualBuildingBlock AddParametersToIndividual(IndividualBuildingBlock individual, List<ParameterValue> parameters);

   }

   // This service is created as a temporary one to group some which do not fit in the other services yet.
   public class MoBiRIntegrationService : IMoBiRIntegrationService
   {
      public IndividualBuildingBlock CreateIndividual(string name) =>
         new IndividualBuildingBlock { Name = name };

      public IndividualBuildingBlock AddParametersToIndividual(IndividualBuildingBlock individual, List<ParameterValue> parameters)
      {
         var lstParameters = new List<ParameterValueWithUnit>();
         //why not send directly the parameterValue? 
         //do they need to provide the percentile? or is it fixed and default?
         lstParameters.AddRange(parameters.Select(p => new ParameterValueWithUnit(p)));
         

         var distributedParameters = new Cache<string, ParameterValueWithUnit>(x => x.ParameterPath);
         distributedParameters.AddRange(lstParameters);
         distributedParameters
            .Select(p => new IndividualParameter
            {
               Path = new ObjectPath(p.ParameterPath),
               Value = p.Value,
               DisplayUnit = new Unit(p.Unit, 1, 0)
            })
            .ToList()
            .ForEach(p => individual.Add(p));
         return individual;
      }



      public void SetParameterValuesParameter(ParameterValuesBuildingBlock buildingBlock, string parameterName, double newValue)
      {
         var parameter = buildingBlock.FirstOrDefault(p => p.Name == parameterName);
         if (parameter != null)
            parameter.Value = newValue;
      }

      public void SetInitialConditionParameter(InitialConditionsBuildingBlock buildingBlock, string parameterName, double newValue)
      {
         var parameter = buildingBlock.FirstOrDefault(p => p.Name == parameterName);
         if (parameter != null)
            parameter.Value = newValue;
      }
   }
}