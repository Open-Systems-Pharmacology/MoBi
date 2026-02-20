using System;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Extensions;
using MoBi.Core.Services;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Extensions;

namespace MoBi.R.Services;

public interface IInitialConditionsTask : IPathAndValuesTask<InitialConditionsBuildingBlock, InitialCondition>
{
   void DeleteInitialConditions(InitialConditionsBuildingBlock buildingBlock, string[] pathsToDelete);

   void ExtendInitialConditions(InitialConditionsBuildingBlock buildingBlock, MoBiSpatialStructure spatialStructure, MoleculeBuildingBlock moleculeBuildingBlock, string[] moleculeNames);

   void SetInitialConditions(InitialConditionsBuildingBlock buildingBlock, string[] quantityPaths, string[] dimensionNames, double[] quantityValues, double[] scaleDivisors, bool[] isPresent, bool[] negativeAllowed);

   string[] AllMoleculeNamesFrom(InitialConditionsBuildingBlock buildingBlock, string[] paths);

   double[] AllScaleDivisorsFrom(InitialConditionsBuildingBlock buildingBlock, string[] paths);

   bool[] AllIsPresentFrom(InitialConditionsBuildingBlock buildingBlock, string[] paths);

   bool[] AllNegativeValuesAllowedFrom(InitialConditionsBuildingBlock buildingBlock, string[] paths);
}

public class InitialConditionsTask : PathAndValuesTask<InitialConditionsBuildingBlock, InitialCondition>, IInitialConditionsTask
{
   private readonly IInitialConditionsBuildingBlockExtendManager _extendManager;

   public InitialConditionsTask(IMoBiContext context, IObjectTypeResolver objectTypeResolver, IInitialConditionsBuildingBlockExtendManager extendManager) : base(context, objectTypeResolver, extendManager)
   {
      _extendManager = extendManager;
   }

   public void DeleteInitialConditions(InitialConditionsBuildingBlock buildingBlock, string[] pathsToDelete) => Delete(buildingBlock, pathsToDelete);

   public void ExtendInitialConditions(InitialConditionsBuildingBlock buildingBlock, MoBiSpatialStructure spatialStructure, MoleculeBuildingBlock moleculeBuildingBlock, string[] moleculeNames) =>
      Extend(buildingBlock, spatialStructure, moleculeBuildingBlock, moleculeNames);

   public void SetInitialConditions(InitialConditionsBuildingBlock buildingBlock, string[] quantityPaths, string[] dimensionNames, double[] quantityValues, double[] scaleDivisors, bool[] isPresent, bool[] negativeAllowed)
   {
      if (!ArrayLengthsAreConsistent(quantityPaths, dimensionNames, quantityValues, scaleDivisors, isPresent, negativeAllowed))
         throw new ArgumentException(AppConstants.Exceptions.AllArraysMustHaveTheSameLength);

      var macroCommand = MacroCommandForUpdateAndInsert();

      quantityPaths.Each((quantityPath, i) => macroCommand.Add(_extendManager.MergeWithUpdate(buildingBlock,
         new InitialConditionPropertiesForMerge
         {
            ObjectPath = quantityPath.ToObjectPath(),
            DimensionName = dimensionNames[i],
            ValueInBaseUnit = quantityValues[i],
            ScaleDivisor = scaleDivisors[i],
            IsPresent = isPresent[i],
            NegativeAllowed = negativeAllowed[i]
         })));

      _context.AddToHistory(macroCommand.RunCommand(_context));
   }

   public string[] AllMoleculeNamesFrom(InitialConditionsBuildingBlock buildingBlock, string[] paths) => AllFrom(buildingBlock, paths, x => x.MoleculeName);

   public double[] AllScaleDivisorsFrom(InitialConditionsBuildingBlock buildingBlock, string[] paths) => AllFrom(buildingBlock, paths, x => x.ScaleDivisor);

   public bool[] AllIsPresentFrom(InitialConditionsBuildingBlock buildingBlock, string[] paths) => AllFrom(buildingBlock, paths, x => x.IsPresent);

   public bool[] AllNegativeValuesAllowedFrom(InitialConditionsBuildingBlock buildingBlock, string[] paths) => AllFrom(buildingBlock, paths, x => x.NegativeValuesAllowed);

   protected override string RemoveCommandDescription() => AppConstants.Commands.RemoveMultipleInitialConditions;

   protected override IMoBiCommand RemoveCommandFor(InitialConditionsBuildingBlock buildingBlock, ObjectPath path) =>
      new RemoveInitialConditionFromBuildingBlockCommand(buildingBlock, path);
}