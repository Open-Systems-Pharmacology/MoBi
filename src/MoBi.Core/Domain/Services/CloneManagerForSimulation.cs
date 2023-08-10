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
      SimulationConfiguration CloneSimulationConfiguration(SimulationConfiguration simulationConfiguration);
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
         var model = _cloneManagerForModel.CloneModel(simulationToClone.Model);

         var simulation = _simulationFactory.CreateFrom(CloneSimulationConfiguration(simulationToClone.Configuration), model);
         simulation.UpdatePropertiesFrom(simulationToClone, _cloneManagerForModel);
         return simulation;
      }

      public T CloneBuildingBlock<T>(T toClone) where T : class, IBuildingBlock
      {
         var formulaCache = new FormulaCache();
         var copy = _cloneManagerForBuildingBlock.Clone(toClone, formulaCache);
         formulaCache.Each(copy.AddFormula);
         return copy;
      }

      public SimulationConfiguration CloneSimulationConfiguration(SimulationConfiguration simulationConfiguration)
      {
         return _cloneManagerForBuildingBlock.Clone(simulationConfiguration);
      }
   }
}