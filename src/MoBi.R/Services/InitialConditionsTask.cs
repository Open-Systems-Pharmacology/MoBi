using System;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Extensions;
using MoBi.Core.Serialization.Xml.Services;
using MoBi.Core.Services;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Serialization;
using OSPSuite.Utility.Extensions;
using ISerializationTask = MoBi.Core.Serialization.Services.ICoreSerializationTask;

namespace MoBi.R.Services;

public interface IInitialConditionsTask : IPathAndValuesTask<InitialConditionsBuildingBlock, InitialCondition>
{
   void DeleteInitialConditions(InitialConditionsBuildingBlock buildingBlock, params string[] pathsToDelete);
   string[] ExtendInitialConditions(InitialConditionsBuildingBlock buildingBlock, SpatialStructure spatialStructure, MoleculeBuildingBlock moleculeBuildingBlock, params string[] moleculeNames);
   void SetInitialConditions(InitialConditionsBuildingBlock buildingBlock, string[] quantityPaths, string[] dimensionNames, double[] quantityValues, double[] scaleDivisors, bool[] isPresent, bool[] negativeAllowed);

   void SetInitialConditions(InitialConditionsBuildingBlock buildingBlock, string quantityPath, string dimensionName, double quantityValue, double scaleDivisor, bool isPresent, bool negativeAllowed);

   void ExportToPKML(InitialConditionsBuildingBlock buildingBlock, string filePath);

   string[] AllMoleculeNamesFrom(ILookupBuildingBlock<InitialCondition> buildingBlock, params string[] paths);

   double[] AllScaleDivisorsFrom(ILookupBuildingBlock<InitialCondition> buildingBlock, params string[] paths);

   bool[] AllIsPresentFrom(ILookupBuildingBlock<InitialCondition> buildingBlock, params string[] paths);

   bool[] AllNegativeValuesAllowedFrom(ILookupBuildingBlock<InitialCondition> buildingBlock, params string[] paths);
}

public class InitialConditionsTask : ExtendablePathAndValuesTask<InitialConditionsBuildingBlock, InitialCondition>, IInitialConditionsTask
{
   private readonly IInitialConditionsBuildingBlockExtendManager _extendManager;
   private readonly IXmlSerializationService _xmlSerializationService;

   public InitialConditionsTask(IMoBiContext context, IObjectTypeResolver objectTypeResolver, IInitialConditionsBuildingBlockExtendManager extendManager, IXmlSerializationService xmlSerializationService, ISerializationTask serializationTask) : base(context, objectTypeResolver, extendManager, serializationTask)
   {
      _extendManager = extendManager;
      _xmlSerializationService = xmlSerializationService;
   }

   public void DeleteInitialConditions(InitialConditionsBuildingBlock buildingBlock, params string[] pathsToDelete) => Delete(buildingBlock, pathsToDelete);

   public string[] ExtendInitialConditions(InitialConditionsBuildingBlock buildingBlock, SpatialStructure spatialStructure, MoleculeBuildingBlock moleculeBuildingBlock, params string[] moleculeNames) =>
      Extend(buildingBlock, spatialStructure, moleculeBuildingBlock, moleculeNames);

   public void SetInitialConditions(InitialConditionsBuildingBlock buildingBlock, string[] quantityPaths, string[] dimensionNames, double[] quantityValues, double[] scaleDivisors, bool[] isPresent, bool[] negativeAllowed)
   {
      if (!quantityPaths.HasConsistentLengthWith(dimensionNames, quantityValues, scaleDivisors, isPresent, negativeAllowed))
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

   public void SetInitialConditions(InitialConditionsBuildingBlock buildingBlock, string quantityPath, string dimensionName, double quantityValue, double scaleDivisor, bool isPresent, bool negativeAllowed) =>
      SetInitialConditions(buildingBlock, [quantityPath], [dimensionName], [quantityValue], [scaleDivisor], [isPresent], [negativeAllowed]);

   public string[] AllMoleculeNamesFrom(ILookupBuildingBlock<InitialCondition> buildingBlock, params string[] paths) => allMoleculeNamesFrom(buildingBlock, paths);

   public double[] AllScaleDivisorsFrom(ILookupBuildingBlock<InitialCondition> buildingBlock, params string[] paths) => allScaleDivisorsFrom(buildingBlock, paths);

   public bool[] AllIsPresentFrom(ILookupBuildingBlock<InitialCondition> buildingBlock, params string[] paths) => allIsPresentFrom(buildingBlock, paths);

   public bool[] AllNegativeValuesAllowedFrom(ILookupBuildingBlock<InitialCondition> buildingBlock, params string[] paths) => allNegativeValuesAllowedFrom(buildingBlock, paths);

   private static string[] allMoleculeNamesFrom(ILookupBuildingBlock<InitialCondition> buildingBlock, string[] paths) => AllFrom(buildingBlock, paths, x => x.MoleculeName);

   private static double[] allScaleDivisorsFrom(ILookupBuildingBlock<InitialCondition> buildingBlock, string[] paths) => AllFrom(buildingBlock, paths, x => x.ScaleDivisor);

   private static bool[] allIsPresentFrom(ILookupBuildingBlock<InitialCondition> buildingBlock, string[] paths) => AllFrom(buildingBlock, paths, x => x.IsPresent);

   private static bool[] allNegativeValuesAllowedFrom(ILookupBuildingBlock<InitialCondition> buildingBlock, string[] paths) => AllFrom(buildingBlock, paths, x => x.NegativeValuesAllowed);

   protected override string RemoveCommandDescription() => AppConstants.Commands.RemoveMultipleInitialConditions;

   public void ExportToPKML(InitialConditionsBuildingBlock buildingBlock, string filePath) =>
      _xmlSerializationService.SerializeModelPart(buildingBlock).PermissiveSave(filePath);

   protected override IMoBiCommand RemoveCommandFor(InitialConditionsBuildingBlock buildingBlock, ObjectPath path) =>
      new RemoveInitialConditionFromBuildingBlockCommand(buildingBlock, path);
}