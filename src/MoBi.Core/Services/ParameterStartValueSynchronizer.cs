using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Core.Services
{
   public interface IParameterStartValueSynchronizer
   {
      void SynchronizeValue(IModelCoreSimulation simulation, IParameter parameter);
   }

   public class ParameterStartValueSynchronizer : IParameterStartValueSynchronizer
   {
      private readonly IEntityPathResolver _entityPathResolver;

      public ParameterStartValueSynchronizer(IEntityPathResolver entityPathResolver)
      {
         _entityPathResolver = entityPathResolver;
      }

      public void SynchronizeValue(IModelCoreSimulation simulation, IParameter parameter)
      {
         if (parameter == null) return;
         var parameterStartValues = simulation.Configuration.ParameterStartValues;
         var objectPath = _entityPathResolver.ObjectPathFor(parameter);
         var parameterStartValue = parameterStartValues[objectPath];

         if (parameterStartValue == null)
            return;

         parameterStartValue.Value = parameter.Value;
         parameterStartValue.Dimension = parameter.Dimension;
         parameterStartValue.DisplayUnit = parameter.DisplayUnit;
      }
   }
}