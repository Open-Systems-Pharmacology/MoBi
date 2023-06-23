using FluentNHibernate.Utils;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Core.Services
{
   public interface IParameterValueSynchronizer
   {
      void SynchronizeValue(IModelCoreSimulation simulation, IParameter parameter);
   }

   public class ParameterValueSynchronizer : IParameterValueSynchronizer
   {
      private readonly IEntityPathResolver _entityPathResolver;

      public ParameterValueSynchronizer(IEntityPathResolver entityPathResolver)
      {
         _entityPathResolver = entityPathResolver;
      }

      public void SynchronizeValue(IModelCoreSimulation simulation, IParameter parameter)
      {
         if (parameter == null) 
            return;
         
         var buildingBlocks = simulation.Configuration.All<ParameterValuesBuildingBlock>();
         var objectPath = _entityPathResolver.ObjectPathFor(parameter);

         buildingBlocks.Each(parameterValues => synchronizeValue(parameter, parameterValues, objectPath));
      }

      private static void synchronizeValue(IParameter parameter, ParameterValuesBuildingBlock parameterValues, ObjectPath objectPath)
      {
         var parameterValue = parameterValues[objectPath];

         if (parameterValue == null)
            return;

         parameterValue.Value = parameter.Value;
         parameterValue.Dimension = parameter.Dimension;
         parameterValue.DisplayUnit = parameter.DisplayUnit;
      }
   }
}