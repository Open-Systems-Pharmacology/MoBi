using OSPSuite.Utility;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Mappers
{
   public interface IBuildConfigurationToMoBiBuildconfigurationMapper : IMapper<IBuildConfiguration, IMoBiBuildConfiguration>
   {
   }

   internal class BuildConfigurationToMoBiBuildconfigurationMapper : IBuildConfigurationToMoBiBuildconfigurationMapper
   {
      public IMoBiBuildConfiguration MapFrom(IBuildConfiguration buildConfiguration)
      {
         return new MoBiBuildConfiguration
         {
            Molecules = buildConfiguration.Molecules,
            SpatialStructure = buildConfiguration.SpatialStructure,
            Reactions = buildConfiguration.Reactions,
            PassiveTransports = buildConfiguration.PassiveTransports,
            EventGroups = buildConfiguration.EventGroups,
            Observers = buildConfiguration.Observers,
            MoleculeStartValues = buildConfiguration.MoleculeStartValues,
            ParameterStartValues = buildConfiguration.ParameterStartValues,
            SimulationSettings = buildConfiguration.SimulationSettings,
         };
      }
   }
}