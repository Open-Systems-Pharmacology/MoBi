using System;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Extensions;
using MoBi.Core.Serialization.Xml.Services;
using MoBi.Core.Services;
using MoBi.R.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Serialization;
using OSPSuite.Utility.Extensions;

namespace MoBi.R.Services;

public interface IInitialConditionsTask : IPathAndValuesTask<ILookupBuildingBlock<InitialCondition>, InitialCondition>
{
   // Methods that require a concrete InitialConditionsBuildingBlock that is editable
   void DeleteInitialConditions(InitialConditionsBuildingBlock buildingBlock, params string[] pathsToDelete);
   string[] ExtendInitialConditions(InitialConditionsBuildingBlock buildingBlock, SpatialStructure spatialStructure, MoleculeBuildingBlock moleculeBuildingBlock, params string[] moleculeNames);
   void SetInitialConditions(InitialConditionsBuildingBlock buildingBlock, string[] quantityPaths, string[] dimensionNames, double[] quantityValues, double[] scaleDivisors, bool[] isPresent, bool[] negativeAllowed);
   
   void SetInitialConditions(InitialConditionsBuildingBlock buildingBlock, string quantityPath, string dimensionName, double quantityValue, double scaleDivisor, bool isPresent, bool negativeAllowed);
   
   void ExportToPKML(InitialConditionsBuildingBlock buildingBlock, string filePath);

   // Methods that can be applied to any building block that enumerates InitialConditions. eg. ExpressionProfileBuildingBlock
   string[] AllMoleculeNamesFrom(ILookupBuildingBlock<InitialCondition> buildingBlock, params string[] paths);
   
   double[] AllScaleDivisorsFrom(ILookupBuildingBlock<InitialCondition> buildingBlock, params string[] paths);
   
   bool[] AllIsPresentFrom(ILookupBuildingBlock<InitialCondition> buildingBlock, params string[] paths);
   
   bool[] AllNegativeValuesAllowedFrom(ILookupBuildingBlock<InitialCondition> buildingBlock, params string[] paths);
}

public class InitialConditionsTask : ExtendablePathAndValuesTask<ILookupBuildingBlock<InitialCondition>, InitialCondition>, IInitialConditionsTask
{
   private readonly IInitialConditionsBuildingBlockExtendManager _extendManager;
   private readonly IXmlSerializationService _xmlSerializationService;

   public InitialConditionsTask(IMoBiContext context, IObjectTypeResolver objectTypeResolver, IInitialConditionsBuildingBlockExtendManager extendManager, IXmlSerializationService xmlSerializationService) : base(context, objectTypeResolver, extendManager)
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

   public string[] AllMoleculeNamesFrom(ILookupBuildingBlock<InitialCondition> buildingBlock, params string[] paths) => buildingBlock.AllFrom(paths, x => x.MoleculeName);

   public double[] AllScaleDivisorsFrom(ILookupBuildingBlock<InitialCondition> buildingBlock, params string[] paths) => buildingBlock.AllFrom(paths, x => x.ScaleDivisor);

   public bool[] AllIsPresentFrom(ILookupBuildingBlock<InitialCondition> buildingBlock, params string[] paths) => buildingBlock.AllFrom(paths, x => x.IsPresent);

   public bool[] AllNegativeValuesAllowedFrom(ILookupBuildingBlock<InitialCondition> buildingBlock, params string[] paths) => buildingBlock.AllFrom(paths, x => x.NegativeValuesAllowed);

   protected override string RemoveCommandDescription() => AppConstants.Commands.RemoveMultipleInitialConditions;

   public void ExportToPKML(InitialConditionsBuildingBlock buildingBlock, string filePath) =>
      _xmlSerializationService.SerializeModelPart(buildingBlock).PermissiveSave(filePath);

   protected override IMoBiCommand RemoveCommandFor(ILookupBuildingBlock<InitialCondition> buildingBlock, ObjectPath path) =>
      new RemoveInitialConditionFromBuildingBlockCommand(buildingBlock, path);
}