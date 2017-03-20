using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Core.Domain.Services
{
   public interface ICloneManagerForSimulation
   {
      IMoBiSimulation CloneSimulation(IMoBiSimulation simulationToClone);
      T CloneBuidingBlockInfo<T>(T toClone) where T : class, IBuildingBlockInfo,new() ;
      T CloneBuidingBlock<T>(T toClone) where T : class, IBuildingBlock;
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
         var buildConfig = new MoBiBuildConfiguration
         {
            MoleculesInfo = CloneBuidingBlockInfo(simulationToClone.MoBiBuildConfiguration.MoleculesInfo),
            ReactionsInfo = CloneBuidingBlockInfo(simulationToClone.MoBiBuildConfiguration.ReactionsInfo),
            SpatialStructureInfo = CloneBuidingBlockInfo(simulationToClone.MoBiBuildConfiguration.SpatialStructureInfo),
            PassiveTransportsInfo = CloneBuidingBlockInfo(simulationToClone.MoBiBuildConfiguration.PassiveTransportsInfo),
            ObserversInfo = CloneBuidingBlockInfo(simulationToClone.MoBiBuildConfiguration.ObserversInfo),
            EventGroupsInfo = CloneBuidingBlockInfo(simulationToClone.MoBiBuildConfiguration.EventGroupsInfo),
            ParameterStartValuesInfo = CloneBuidingBlockInfo(simulationToClone.MoBiBuildConfiguration.ParameterStartValuesInfo),
            MoleculeStartValuesInfo = CloneBuidingBlockInfo(simulationToClone.MoBiBuildConfiguration.MoleculeStartValuesInfo),
            SimulationSettingsInfo = CloneBuidingBlockInfo(simulationToClone.MoBiBuildConfiguration.SimulationSettingsInfo)
         };

         var model = _cloneManagerForModel.CloneModel(simulationToClone.Model);
         var simulation = _simulationFactory.CreateFrom(buildConfig, model);
         simulation.UpdatePropertiesFrom(simulationToClone,_cloneManagerForModel);
         return simulation;
      }

      public T CloneBuidingBlock<T>(T toClone) where T : class, IBuildingBlock
      {
         var formulaCache = new FormulaCache();
         var copy = _cloneManagerForBuildingBlock.Clone(toClone, formulaCache);
         formulaCache.Each(copy.AddFormula);
         return copy;
      }

      public T CloneBuidingBlockInfo<T>(T toClone) where T : class, IBuildingBlockInfo, new()
      {
         var formulaCache = new FormulaCache();
         var copy = new T {UntypedBuildingBlock = CloneBuidingBlock(toClone.UntypedBuildingBlock)};
         formulaCache.Each(copy.UntypedBuildingBlock.AddFormula);
         copy.TemplateBuildingBlockId = toClone.TemplateBuildingBlockId;
         copy.SimulationChanges = toClone.SimulationChanges;
         return copy;
      }
   }
}