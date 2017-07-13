using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Domain.Services;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Core.Domain.Model
{
   public interface IBuildConfigurationFactory
   {
      /// <summary>
      ///    Creates an empty <see cref="MoBiBuildConfiguration" />
      /// </summary>
      /// <returns></returns>
      IMoBiBuildConfiguration Create();

      /// <summary>
      ///    Returns a new <see cref="MoBiBuildConfiguration" /> referencing exactly the same template building blocks as the
      ///    given   <paramref name="buildConfiguration" />. However the instance building block used in the returned
      ///    <see cref="MoBiBuildConfiguration" /> are defined as clone of the
      ///    corresponding template building blocks. 
      /// </summary>
      /// <param name="buildConfiguration">Build configuration from which the template to use will be taken and cloned</param>
      IMoBiBuildConfiguration CreateFromTemplateClones(IMoBiBuildConfiguration buildConfiguration);

      void AddCalculationMethodsToBuildConfiguration(IBuildConfiguration buildConfiguration);

      /// <summary>
      ///    Returns a new <see cref="MoBiBuildConfiguration" />  referencing exactly the same template building blocks as the
      ///    given  <paramref name="buildConfiguration" /> . if the used building block was identical to the template building
      ///    block,
      ///    the template building block will also be used as building block. Otherwise the former building block will be used
      /// </summary>
      /// <param name="buildConfiguration">Build configuration containing the building block to use</param>
      /// <param name="templateBuildingBlock">
      ///    If defined the <paramref name="templateBuildingBlock" />will be used instead of the one defined in the simulation
      ///    (only for this building block type)
      /// </param>
      IMoBiBuildConfiguration CreateFromReferencesUsedIn(IMoBiBuildConfiguration buildConfiguration, IBuildingBlock templateBuildingBlock = null);
   }

   public class BuildConfigurationFactory : IBuildConfigurationFactory
   {
      private readonly IRegisterTask _registerTask;
      private readonly ICloneManagerForBuildingBlock _cloneManager;
      private readonly ICoreCalculationMethodRepository _calculatonMethodRepository;

      public BuildConfigurationFactory(IRegisterTask registerTask, ICloneManagerForBuildingBlock cloneManager, ICoreCalculationMethodRepository calculatonMethodRepository)
      {
         _registerTask = registerTask;
         _cloneManager = cloneManager;
         _calculatonMethodRepository = calculatonMethodRepository;
      }

      public IMoBiBuildConfiguration Create()
      {
         return new MoBiBuildConfiguration();
      }

      public IMoBiBuildConfiguration CreateFromTemplateClones(IMoBiBuildConfiguration buildConfiguration)
      {
         var mobiBuildConfiguration = Create();
         cloneTo(mobiBuildConfiguration.SpatialStructureInfo, buildConfiguration.SpatialStructureInfo);
         cloneTo(mobiBuildConfiguration.MoleculesInfo, buildConfiguration.MoleculesInfo);
         cloneTo(mobiBuildConfiguration.ReactionsInfo, buildConfiguration.ReactionsInfo);
         cloneTo(mobiBuildConfiguration.PassiveTransportsInfo, buildConfiguration.PassiveTransportsInfo);
         cloneTo(mobiBuildConfiguration.ObserversInfo, buildConfiguration.ObserversInfo);
         cloneTo(mobiBuildConfiguration.EventGroupsInfo, buildConfiguration.EventGroupsInfo);
         cloneTo(mobiBuildConfiguration.SimulationSettingsInfo, buildConfiguration.SimulationSettingsInfo);
         cloneTo(mobiBuildConfiguration.MoleculeStartValuesInfo, buildConfiguration.MoleculeStartValuesInfo);
         cloneTo(mobiBuildConfiguration.ParameterStartValuesInfo, buildConfiguration.ParameterStartValuesInfo);

         AddCalculationMethodsToBuildConfiguration(mobiBuildConfiguration);
         return mobiBuildConfiguration;
      }

      public IMoBiBuildConfiguration CreateFromReferencesUsedIn(IMoBiBuildConfiguration buildConfiguration, IBuildingBlock templateBuildingBlock = null)
      {
         var mobiBuildConfiguration = Create();
         updateFrom(mobiBuildConfiguration.SpatialStructureInfo, buildConfiguration.SpatialStructureInfo, templateBuildingBlock);
         updateFrom(mobiBuildConfiguration.MoleculesInfo, buildConfiguration.MoleculesInfo, templateBuildingBlock);
         updateFrom(mobiBuildConfiguration.ReactionsInfo, buildConfiguration.ReactionsInfo, templateBuildingBlock);
         updateFrom(mobiBuildConfiguration.PassiveTransportsInfo, buildConfiguration.PassiveTransportsInfo, templateBuildingBlock);
         updateFrom(mobiBuildConfiguration.ObserversInfo, buildConfiguration.ObserversInfo, templateBuildingBlock);
         updateFrom(mobiBuildConfiguration.EventGroupsInfo, buildConfiguration.EventGroupsInfo, templateBuildingBlock);
         updateFrom(mobiBuildConfiguration.SimulationSettingsInfo, buildConfiguration.SimulationSettingsInfo, templateBuildingBlock);
         updateFrom(mobiBuildConfiguration.MoleculeStartValuesInfo, buildConfiguration.MoleculeStartValuesInfo, templateBuildingBlock);
         updateFrom(mobiBuildConfiguration.ParameterStartValuesInfo, buildConfiguration.ParameterStartValuesInfo, templateBuildingBlock);

         AddCalculationMethodsToBuildConfiguration(mobiBuildConfiguration);
         return mobiBuildConfiguration;
      }

      private void cloneTo<T>(IBuildingBlockInfo<T> buildingBlockInfo, IBuildingBlockInfo<T> templateBuildingBlockInfo) where T : class, IBuildingBlock
      {
         var buildingBlockToUse = templateBuildingBlockInfo.BuildingBlock;
         //this is a template building block, we need to clone it. Otherwise use as is
         if (templateBuildingBlockInfo.BuildingBlockIsTemplate)
            buildingBlockToUse = clone(templateBuildingBlockInfo.TemplateBuildingBlock);

         update(buildingBlockInfo, templateBuildingBlockInfo, buildingBlockToUse);

         //since we are using the templatate building block, we reset the simulation change counter
         if (templateBuildingBlockInfo.BuildingBlockIsTemplate)
            buildingBlockInfo.SimulationChanges = 0;

         _registerTask.RegisterAllIn(buildingBlockInfo.BuildingBlock);
      }

      private void updateFrom<T>(IBuildingBlockInfo<T> buildingBlockInfo, IBuildingBlockInfo<T> templateBuildingBlockInfo, IBuildingBlock templateBuildingBlock) where T : class, IBuildingBlock
      {
         var shouldUseTemplate = shouldUseTemplateForUpdate(templateBuildingBlockInfo, templateBuildingBlock);

         var buildingBlockToUse = shouldUseTemplate ? templateBuildingBlockInfo.TemplateBuildingBlock : templateBuildingBlockInfo.BuildingBlock;

         update(buildingBlockInfo, templateBuildingBlockInfo, buildingBlockToUse);

         //since we are using the templatate building block, we reset the simulation change counter
         if (shouldUseTemplate)
            buildingBlockInfo.SimulationChanges = 0;
      }

      private bool shouldUseTemplateForUpdate<T>(IBuildingBlockInfo<T> templateBuildingBlockInfo, IBuildingBlock templateBuildingBlock) where T : class, IBuildingBlock
      {
         if (templateBuildingBlockInfo.TemplateBuildingBlock == null) 
            return false;

         //only use template building block if the one previously used was the template itself or if the building block is unchanged in the simulation
         return templateBuildingBlockInfo.TemplateBuildingBlock == templateBuildingBlock || !templateBuildingBlockInfo.BuildingBlockChanged;
      }

      private void update<T>(IBuildingBlockInfo<T> buildingBlockInfo, IBuildingBlockInfo<T> templateBuildingBlockInfo, T usedBuildingBlock) where T : class, IBuildingBlock
      {
         buildingBlockInfo.BuildingBlock = usedBuildingBlock;
         buildingBlockInfo.TemplateBuildingBlock = templateBuildingBlockInfo.TemplateBuildingBlock;
         buildingBlockInfo.SimulationChanges = templateBuildingBlockInfo.SimulationChanges;
      }

      public void AddCalculationMethodsToBuildConfiguration(IBuildConfiguration buildConfiguration)
      {
         _calculatonMethodRepository.All().Each(
            cm => buildConfiguration.AddCalculationMethod(_cloneManager.Clone(cm, new FormulaCache())));

         correctPathsInCalculationMethods(buildConfiguration.AllCalculationMethods(), firstTopContainerName(buildConfiguration));
      }

      private static string firstTopContainerName(IBuildConfiguration buildConfiguration)
      {
         //NOTE: This could be an issue if spatial structure has more than one top container
         return buildConfiguration.SpatialStructure.TopContainers.Select(x => x.Name).FirstOrDefault();
      }

      private void correctPathsInCalculationMethods(IEnumerable<ICoreCalculationMethod> allCalculationMethods, string organismName)
      {
         foreach (var calculationMethod in allCalculationMethods)
         {
            calculationMethod.FormulaCache.Each(f => correctPathsInFormulas(f, organismName));
            calculationMethod.AllOutputFormulas().Each(f => correctPathsInFormulas(f, organismName));
         }
      }

      private void correctPathsInFormulas(IFormula formula, string organismName)
      {
         formula.ObjectPaths.Each(path => path.Replace(AppConstants.PKSimTopContainer, organismName));
      }

      private T clone<T>(T templateBuildingBlock) where T : class, IBuildingBlock
      {
         return _cloneManager.CloneBuildingBlock(templateBuildingBlock);
      }
   }
}