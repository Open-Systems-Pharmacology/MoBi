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
   IMoBiCommand MergeWithUpdate(InitialConditionsBuildingBlock buildingBlock, ObjectPath objectPath, string dimensionName, int valueInBaseUnit, int scaleDivisor, bool isPresent, bool negativeAllowed);
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

   public IMoBiCommand MergeWithUpdate(InitialConditionsBuildingBlock buildingBlock, ObjectPath objectPath, string dimensionName, int valueInBaseUnit, int scaleDivisor, bool isPresent, bool negativeAllowed)
   {
      var pathAndValueEntity = buildingBlock[objectPath];
      if (pathAndValueEntity != null)
         return new UpdateInitialConditionInBuildingBlockCommand(buildingBlock, objectPath, valueInBaseUnit, isPresent, scaleDivisor, negativeAllowed);

      var moleculeName = objectPath.Last();
      var containerPath = objectPath.Clone<ObjectPath>();
      containerPath.RemoveAt(containerPath.Count - 1);
      var dimension = _dimensionFactory.Dimension(dimensionName);

      var initialCondition = _initialConditionsCreator.CreateInitialCondition(
         containerPath,
         moleculeName,
         dimension,
         displayUnit: null,
         valueOrigin: null,
         isPresent: isPresent,
         valueInBaseUnit: valueInBaseUnit,
         scaleDivisor,
         negativeAllowed
      );

      return GenerateAddCommand(buildingBlock, initialCondition);
   }
}