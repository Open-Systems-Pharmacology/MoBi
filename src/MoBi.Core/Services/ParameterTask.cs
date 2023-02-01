using MoBi.Core.Domain.Model;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Extensions;

namespace MoBi.Core.Services
{
   public interface IParameterTask : ISetParameterTask
   {
   }

   public class ParameterTask : IParameterTask
   {
      private readonly IQuantityTask _quantityTask;

      public ParameterTask(IQuantityTask quantityTask)
      {
         _quantityTask = quantityTask;
      }

      public ICommand SetParameterValue(IParameter parameter, double value, IModelCoreSimulation simulation)
      {
         return _quantityTask.SetQuantityBaseValue(parameter, value, simulation.DowncastTo<IMoBiSimulation>());
      }

      public ICommand UpdateParameterValueOrigin(IParameter parameter, ValueOrigin valueOrigin, IModelCoreSimulation simulation)
      {
         return _quantityTask.UpdateQuantityValueOriginInSimulation(parameter, valueOrigin, simulation.DowncastTo<IMoBiSimulation>());
      }
   }
}