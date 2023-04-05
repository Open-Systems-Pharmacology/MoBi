using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Extensions;

namespace MoBi.Core.Domain.Services
{
   public interface ICloneManagerForSimulation
   {
      IMoBiSimulation CloneSimulation(IMoBiSimulation simulationToClone);
      T CloneBuildingBlock<T>(T toClone) where T : class, IBuildingBlock;
   }

   internal class CloneManagerForSimulation : ICloneManagerForSimulation
   {
      private readonly ICloneManagerForBuildingBlock _cloneManagerForBuildingBlock;
      private readonly ICloneManagerForModel _cloneManagerForModel;
      private readonly ISimulationFactory _simulationFactory;

      public CloneManagerForSimulation(ICloneManagerForBuildingBlock cloneManagerForBuildingBlock, ICloneManagerForModel cloneManagerForModel, ISimulationFactory simulationFactory)
      {
         _cloneManagerForBuildingBlock = cloneManagerForBuildingBlock;
         _cloneManagerForModel = cloneManagerForModel;
         _simulationFactory = simulationFactory;
      }

      public IMoBiSimulation CloneSimulation(IMoBiSimulation simulationToClone)
      {
         // var buildConfig = new MoBiBuildConfiguration
         // {
         //    MoleculesInfo = CloneBuildingBlockInfo(simulationToClone.MoBiBuildConfiguration.MoleculesInfo),
         //    ReactionsInfo = CloneBuildingBlockInfo(simulationToClone.MoBiBuildConfiguration.ReactionsInfo),
         //    SpatialStructureInfo = CloneBuildingBlockInfo(simulationToClone.MoBiBuildConfiguration.SpatialStructureInfo),
         //    PassiveTransportsInfo = CloneBuildingBlockInfo(simulationToClone.MoBiBuildConfiguration.PassiveTransportsInfo),
         //    ObserversInfo = CloneBuildingBlockInfo(simulationToClone.MoBiBuildConfiguration.ObserversInfo),
         //    EventGroupsInfo = CloneBuildingBlockInfo(simulationToClone.MoBiBuildConfiguration.EventGroupsInfo),
         //    ParameterStartValuesInfo = CloneBuildingBlockInfo(simulationToClone.MoBiBuildConfiguration.ParameterStartValuesInfo),
         //    MoleculeStartValuesInfo = CloneBuildingBlockInfo(simulationToClone.MoBiBuildConfiguration.MoleculeStartValuesInfo),
         //    SimulationSettingsInfo = CloneBuildingBlockInfo(simulationToClone.MoBiBuildConfiguration.SimulationSettingsInfo)
         // };

         var model = _cloneManagerForModel.CloneModel(simulationToClone.Model);

         var simulation = _simulationFactory.CreateFrom(simulationConfigurationCloneFor(simulationToClone.Configuration), model);
         simulation.UpdatePropertiesFrom(simulationToClone, _cloneManagerForModel);
         return simulation;
      }

      // TODO: do we need to create an actual clone? SIMULATION_CONFIGURATION
      private SimulationConfiguration simulationConfigurationCloneFor(SimulationConfiguration configuration)
      {
         return configuration;
      }

      public T CloneBuildingBlock<T>(T toClone) where T : class, IBuildingBlock
      {
         var formulaCache = new FormulaCache();
         var copy = _cloneManagerForBuildingBlock.Clone(toClone, formulaCache);
         formulaCache.Each(copy.AddFormula);
         return copy;
      }
   }
}