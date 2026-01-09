using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.R.Domain;

namespace MoBi.R.Services
{
   public interface IIndividualTask
   {
      IndividualBuildingBlock CreateIndividual(string name);
      void AddDistributedParameter(IndividualBuildingBlock individual, ParameterValueWithUnit parameter);
      void AddDerivedParameter(IndividualBuildingBlock individual, ParameterValueWithUnit parameter);
      IndividualBuildingBlock CreateIndividualFrom(string name, ParameterValueWithUnit[] distributedParameters, ParameterValueWithUnit[] derivedParameters);
   }

   public class IndividualTask : IIndividualTask
   {
      public IndividualBuildingBlock CreateIndividual(string name)
      {
         return new IndividualBuildingBlock { Name = name };
      }

      public void AddDistributedParameter(IndividualBuildingBlock individual, ParameterValueWithUnit parameter)
      {
         if (individual == null || parameter == null) return;
         applyParameter(individual, parameter);
      }

      public void AddDerivedParameter(IndividualBuildingBlock individual, ParameterValueWithUnit parameter)
      {
         if (individual == null || parameter == null) return;
         applyParameter(individual, parameter);
      }

      public IndividualBuildingBlock CreateIndividualFrom(string name, ParameterValueWithUnit[] distributedParameters, ParameterValueWithUnit[] derivedParameters)
      {
         var individual = new IndividualBuildingBlock { Name = name };

         foreach (var pv in distributedParameters ?? System.Array.Empty<ParameterValueWithUnit>())
            applyParameter(individual, pv);

         foreach (var pv in derivedParameters ?? System.Array.Empty<ParameterValueWithUnit>())
            applyParameter(individual, pv);

         return individual;
      }

      private static void applyParameter(IndividualBuildingBlock individual, ParameterValueWithUnit pv)
      {
         var param = new IndividualParameter
         {
            Path = new ObjectPath(pv.ParameterPath),
            Value = pv.Value
         };
         individual.Add(param);
      }
   }
}
