using System.Linq;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.R.Services
{
   public interface IParameterValuesTask
   {
      void SetParameterValue(ParameterValuesBuildingBlock pvBB, string parameterName, double newValue);
   }

   public class ParameterValuesTask : IParameterValuesTask
   {
      public void SetParameterValue(ParameterValuesBuildingBlock pvBB, string parameterName, double newValue)
      {
         var parameter = pvBB.FirstOrDefault(p => p.Name == parameterName);
         if (parameter != null)
            parameter.Value = newValue;
      }
   }
}