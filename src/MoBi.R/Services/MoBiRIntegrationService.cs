using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using ISerializationTask = MoBi.Presentation.Tasks.ISerializationTask;

namespace MoBi.R.Services
{
   public interface IMoBiRIntegrationService
   {
      IndividualBuildingBlock CreateIndividual(string name);
      void SetParameterValue(ParameterValuesBuildingBlock pvBB, string parameterName, double newValue);
   }

   public class MoBiRIntegrationService : IMoBiRIntegrationService
   {
      public IndividualBuildingBlock CreateIndividual(string name)
      {
         return new IndividualBuildingBlock { Name = name };
      }

      public void SetParameterValue(ParameterValuesBuildingBlock pvBB, string parameterName, double newValue)
      {
         var parameter = pvBB.FirstOrDefault(p => p.Name == parameterName);
         if (parameter != null)
            parameter.Value = newValue;
      }
   }
}