using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Extensions;
using MoBi.Core.Serialization.Xml.Services;
using MoBi.Core.Services;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Serialization;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using static MoBi.Assets.AppConstants;

namespace MoBi.R.Services;

public interface IParameterValuesTask : IPathAndValuesTask<ParameterValuesBuildingBlock, ParameterValue>
{
   void SetParameterValues(ParameterValuesBuildingBlock buildingBlock, string[] quantityPaths, double[] quantityValues, string[] dimensionNames);
   
   void SetParameterValues(ParameterValuesBuildingBlock buildingBlock, string quantityPath, double quantityValue, string dimensionName);

   void DeleteParameterValues(ParameterValuesBuildingBlock buildingBlock, params string[] pathsToDelete);
   
   void DeleteParameterValues(ParameterValuesBuildingBlock buildingBlock, string pathToDelete);

   string[] AddLocalMoleculeParameters(ParameterValuesBuildingBlock buildingBlock, SpatialStructure spatialStructure, MoleculeBuildingBlock moleculeBuildingBlock, params string[] moleculeNames);
   
   string[] AddProteinExpressionParameters(ParameterValuesBuildingBlock buildingBlock,
      ExpressionProfileBuildingBlock referenceExpression,
      MoleculeBuildingBlock moleculeBuildingBlock, 
      string moleculeName, 
      SpatialStructure spatialStructure, 
      params string[] organPaths);

   void ExportToPKML(ParameterValuesBuildingBlock buildingBlock, string filePath);
}

public class ParameterValuesTask : ExtendablePathAndValuesTask<ParameterValuesBuildingBlock, ParameterValue>, IParameterValuesTask
{
   private readonly IParameterValueBuildingBlockExtendManager _extendManager;
   private readonly IXmlSerializationService _xmlSerializationService;
   private readonly IParameterValuesCreator _parameterValuesCreator;
   private readonly IContainerTask _containerTask;

   public ParameterValuesTask(IMoBiContext context, IObjectTypeResolver objectTypeResolver, IParameterValueBuildingBlockExtendManager extendManager, IXmlSerializationService xmlSerializationService, IParameterValuesCreator parameterValuesCreator, IContainerTask containerTask) : base(context, objectTypeResolver, extendManager)
   {
      _extendManager = extendManager;
      _xmlSerializationService = xmlSerializationService;
      _parameterValuesCreator = parameterValuesCreator;
      _containerTask = containerTask;
   }

   public void SetParameterValues(ParameterValuesBuildingBlock buildingBlock, string[] quantityPaths, double[] quantityValues, string[] dimensionNames)
   {
      if (!quantityPaths.HasConsistentLengthWith(dimensionNames, quantityValues))
         throw new ArgumentException(Exceptions.AllArraysMustHaveTheSameLength);

      var macroCommand = MacroCommandForUpdateAndInsert();

      quantityPaths.Each((quantityPath, i) => macroCommand.Add(_extendManager.MergeWithUpdate(buildingBlock, quantityPath.ToObjectPath(), quantityValues[i], dimensionNames[i])));

      _context.AddToHistory(macroCommand.RunCommand(_context));
   }

   public void SetParameterValues(ParameterValuesBuildingBlock buildingBlock, string quantityPath, double quantityValue, string dimensionName) => SetParameterValues(buildingBlock, [quantityPath], [quantityValue], [dimensionName]);

   public void DeleteParameterValues(ParameterValuesBuildingBlock buildingBlock, string[] pathsToDelete) => Delete(buildingBlock, pathsToDelete);

   public void DeleteParameterValues(ParameterValuesBuildingBlock buildingBlock, string pathToDelete) => DeleteParameterValues(buildingBlock, [pathToDelete]);

   public string[] AddLocalMoleculeParameters(ParameterValuesBuildingBlock buildingBlock, SpatialStructure spatialStructure, MoleculeBuildingBlock moleculeBuildingBlock, params string[] moleculeNames) =>
      Extend(buildingBlock, spatialStructure, moleculeBuildingBlock, moleculeNames);

   public string[] AddProteinExpressionParameters(ParameterValuesBuildingBlock buildingBlock, ExpressionProfileBuildingBlock referenceExpression, MoleculeBuildingBlock moleculeBuildingBlock, string moleculeName, SpatialStructure spatialStructure, params string[] organPaths)
   {
      if (!string.Equals(referenceExpression.MoleculeName, moleculeName))
         throw new ArgumentException(Exceptions.ExpressionProfileMoleculeNameMismatch(referenceExpression.MoleculeName, moleculeName));

      var molecule = moleculeBuildingBlock.FirstOrDefault(x => string.Equals(x.Name, moleculeName))
                     ?? throw new ArgumentException(Exceptions.MoleculeNotFoundInBuildingBlock(moleculeName));

      var organs = resolveOrgans(spatialStructure, organPaths);

      var existingPaths = new HashSet<string>(buildingBlock.Select(x => x.Path.PathAsString));

      var newParameterValues = organs
         .SelectMany(x => _parameterValuesCreator.CreateExpressionFrom(x, molecule, referenceExpression))
         .Where(x => !existingPaths.Contains(x.Path.PathAsString))
         .ToList();

      _context.AddToHistory(_extendManager.Extend(newParameterValues, buildingBlock));

      return newParameterValues.Select(x => x.Path.PathAsString).ToArray();
   }

   private IReadOnlyList<IContainer> resolveOrgans(SpatialStructure spatialStructure, string[] organPaths)
   {
      // We only need the top containers if creating an expression for the whole organism
      // it will be recursively populated in all organs
      if (organPaths == null || !organPaths.Any())
         return spatialStructure.TopContainers.ToList();

      var topContainerNames = organPaths
         .Select(x => x.ToPathArray().First())
         .Distinct()
         .ToHashSet();

      var containerCache = new Cache<string, IContainer>(onMissingKey:_ => null);

      foreach (var topContainer in spatialStructure.TopContainers.Where(x => topContainerNames.Contains(x.Name)))
      {
         var physicalContainers = _containerTask.CacheAllChildrenSatisfying<IContainer>(topContainer, c => c.Mode.Is(ContainerMode.Physical));
         foreach (var kvp in physicalContainers.KeyValues)
            containerCache[kvp.Key] = kvp.Value;
      }

      return organPaths.Distinct().Select(path => containerCache[path] ?? throw new ArgumentException(Exceptions.OrganNotFoundInSpatialStructure(path))
      ).ToList();
   }

   public void ExportToPKML(ParameterValuesBuildingBlock buildingBlock, string filePath) =>
      _xmlSerializationService.SerializeModelPart(buildingBlock).PermissiveSave(filePath);

   protected override string RemoveCommandDescription() => Commands.RemoveManyParameterValues;

   protected override IMoBiCommand RemoveCommandFor(ParameterValuesBuildingBlock buildingBlock, ObjectPath path) => 
      new RemoveParameterValueFromBuildingBlockCommand(buildingBlock, path);
}