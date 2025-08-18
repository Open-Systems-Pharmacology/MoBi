using System.Linq;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.R.Services
{
   public interface IParameterValuesTask
   {
      void SetParameterValue(ParameterValuesBuildingBlock pvBB, string fullPath, double newValue);
   }

   public class ParameterValuesTask : IParameterValuesTask
   {
      public void SetParameterValue(ParameterValuesBuildingBlock pvBB, string fullPath,  double newValue)
      {
         var parameter = pvBB.FirstOrDefault(p => p.Path == fullPath);
         if (parameter != null)
            parameter.Value = newValue;
      }
   }
}