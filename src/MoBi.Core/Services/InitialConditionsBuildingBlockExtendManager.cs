using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Services;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Extensions;
using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Domain.Model;

namespace MoBi.Core.Services;

public interface IInitialConditionsBuildingBlockExtendManager : IExtendPathAndValuesManager<InitialCondition>
{
}

public class InitialConditionsBuildingBlockExtendManager : ExtendPathAndValuesManager<InitialCondition>, IInitialConditionsBuildingBlockExtendManager
{
   private readonly IInitialConditionsCreator _initialConditionsCreator;

   public InitialConditionsBuildingBlockExtendManager(IInitialConditionsCreator initialConditionsCreator, IMoBiFormulaTask moBiFormulaTask, IObjectTypeResolver objectTypeResolver, IMoBiContext moBiContext) : base(moBiFormulaTask, objectTypeResolver, moBiContext)
   {
      _initialConditionsCreator = initialConditionsCreator;
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
}