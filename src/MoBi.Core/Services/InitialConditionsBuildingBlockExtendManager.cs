using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Utility.Extensions;

namespace MoBi.Core.Services;

public interface IInitialConditionsBuildingBlockExtendManager : IExtendPathAndValuesManager<InitialCondition>
{
   IMoBiCommand MergeWithUpdate(InitialConditionsBuildingBlock buildingBlock, InitialConditionPropertiesForMerge properties);
}

public class InitialConditionsBuildingBlockExtendManager : ExtendPathAndValuesManager<InitialCondition>, IInitialConditionsBuildingBlockExtendManager
{
   private readonly IInitialConditionsCreator _initialConditionsCreator;
   private readonly IDimensionFactory _dimensionFactory;

   public InitialConditionsBuildingBlockExtendManager(
      IInitialConditionsCreator initialConditionsCreator,
      IMoBiFormulaTask moBiFormulaTask,
      IObjectTypeResolver objectTypeResolver,
      IMoBiContext moBiContext,
      IDimensionFactory dimensionFactory) : base(moBiFormulaTask, objectTypeResolver, moBiContext)
   {
      _initialConditionsCreator = initialConditionsCreator;
      _dimensionFactory = dimensionFactory;
   }

   private void updateDefaultIsPresentToFalseForSpecificExtendedValues(IReadOnlyList<InitialCondition> allInitialConditions, IReadOnlyList<InitialCondition> templateValues)
   {
      var templateInitialConditions = templateValues.ToCache();
      var entitiesThatShouldPotentiallyNotBePresent = allInitialConditions.ToCache().KeyValues.Where(x => AppConstants.Organs.DefaultIsPresentShouldBeFalse.Any(organ => x.Key.Contains(organ)));
      var newInitialConditionsToUpdate = entitiesThatShouldPotentiallyNotBePresent.Where(x => !templateInitialConditions.Contains(x.Key));
      newInitialConditionsToUpdate.Each(x => x.Value.IsPresent = false);
   }

   protected override IReadOnlyList<InitialCondition> CreatePathAndValueEntitiesBasedOnUsedTemplates(SpatialStructure spatialStructure, IReadOnlyList<MoleculeBuilder> molecules, ILookupBuildingBlock<InitialCondition> buildingBlock)
   {
      var newEntities = _initialConditionsCreator.CreateFrom(spatialStructure, molecules).ToList();
      updateDefaultIsPresentToFalseForSpecificExtendedValues(newEntities, buildingBlock.ToList());
      return newEntities;
   }

   public override IMoBiCommand GenerateAddCommand(ILookupBuildingBlock<InitialCondition> targetBuildingBlock, InitialCondition initialCondition) => new AddInitialConditionToBuildingBlockCommand(targetBuildingBlock, initialCondition);

   protected override IMoBiCommand GenerateRemoveCommand(ILookupBuildingBlock<InitialCondition> targetBuildingBlock, InitialCondition initialCondition) => new RemoveInitialConditionFromBuildingBlockCommand(targetBuildingBlock, initialCondition.Path);

   public IMoBiCommand MergeWithUpdate(InitialConditionsBuildingBlock buildingBlock, InitialConditionPropertiesForMerge properties)
   {
      var pathAndValueEntity = buildingBlock[properties.ObjectPath];
      if (pathAndValueEntity != null)
         return new UpdateInitialConditionInBuildingBlockCommand(buildingBlock, properties.ObjectPath, properties.ValueInBaseUnit, properties.IsPresent, properties.ScaleDivisor, properties.NegativeAllowed);

      var moleculeName = properties.ObjectPath.Last();
      var containerPath = properties.ObjectPath.Clone<ObjectPath>();
      containerPath.RemoveAt(containerPath.Count - 1);
      var dimension = _dimensionFactory.Dimension(properties.DimensionName);

      var initialCondition = _initialConditionsCreator.CreateInitialCondition(
         containerPath,
         moleculeName,
         dimension,
         displayUnit: null,
         valueOrigin: null,
         properties.IsPresent,
         properties.ValueInBaseUnit,
         properties.ScaleDivisor,
         properties.NegativeAllowed
      );

      return GenerateAddCommand(buildingBlock, initialCondition);
   }
}