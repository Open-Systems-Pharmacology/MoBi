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
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using static MoBi.Assets.AppConstants;
using ISerializationTask = MoBi.Presentation.Tasks.ISerializationTask;

namespace MoBi.R.Services;

public interface IParameterValuesTask : IExtendablePathAndValuesTask<ParameterValuesBuildingBlock, ParameterValue>
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
}

public class ParameterValuesTask : ExtendablePathAndValuesTask<ParameterValuesBuildingBlock, ParameterValue>, IParameterValuesTask
{
   private readonly IParameterValueBuildingBlockExtendManager _extendManager;
   private readonly IParameterValuesCreator _parameterValuesCreator;
   private readonly IContainerTask _containerTask;

   public ParameterValuesTask(IMoBiContext context, IObjectTypeResolver objectTypeResolver, IParameterValueBuildingBlockExtendManager extendManager, IXmlSerializationService xmlSerializationService, IParameterValuesCreator parameterValuesCreator, IContainerTask containerTask, ISerializationTask serializationTask) : base(context, objectTypeResolver, extendManager, serializationTask, xmlSerializationService)
   {
      _extendManager = extendManager;
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

   public string[] AddProteinExpressionParameters(ParameterValuesBuildingBlock buildingBlock, ExpressionProfileBuildingBlock referenceExpression,
      MoleculeBuildingBlock moleculeBuildingBlock, string moleculeName, SpatialStructure spatialStructure, params string[] organPaths)
   {
      if (!string.Equals(referenceExpression.MoleculeName, moleculeName))
         throw new ArgumentException(Exceptions.ExpressionProfileMoleculeNameMismatch(referenceExpression.MoleculeName, moleculeName));

      var molecule = moleculeBuildingBlock.FirstOrDefault(x => string.Equals(x.Name, moleculeName))
                     ?? throw new ArgumentException(Exceptions.MoleculeNotFoundInBuildingBlock(moleculeName));

      var organs = resolveOrgans(spatialStructure, organPaths.Distinct().ToArray());

      var existingPaths = new HashSet<string>(buildingBlock.Select(x => x.Path.PathAsString));

      var newParameterValues = organs
         .SelectMany(x => _parameterValuesCreator.CreateExpressionFrom(x, molecule, referenceExpression))
         .Where(x => !existingPaths.Contains(x.Path.PathAsString))
         .ToList();

      _context.AddToHistory(_extendManager.Extend(newParameterValues, buildingBlock));

      return newParameterValues.Select(x => x.Path.PathAsString).ToArray();
   }

   /// <summary>
   ///    Resolves the organ containers in the <paramref name="spatialStructure" /> that correspond to the provided
   ///    <paramref name="organPaths" />.
   /// </summary>
   /// <param name="spatialStructure">The spatial structure to resolve organ containers from.</param>
   /// <param name="organPaths">
   ///    The absolute paths of the organs to resolve (e.g. <c>"Organism|Liver"</c>). Each path must match the
   ///    absolute path of a container whose <see cref="IContainer.ContainerType" /> is
   ///    <see cref="ContainerType.Organ" /> in the <paramref name="spatialStructure" />. The absolute path of a top
   ///    container is its <see cref="IContainer.ParentPath" /> joined with its <see cref="IContainer.Name" />, so a top
   ///    container can itself represent an organ — e.g. a top container named <c>"Tumor"</c> with
   ///    <c>ParentPath = "Organism|Liver"</c> is resolved by the organ path <c>"Organism|Liver|Tumor"</c>.
   /// </param>
   /// <returns>The resolved organ containers, one per entry in <paramref name="organPaths" />.</returns>
   /// <exception cref="ArgumentException">
   ///    Thrown when an entry in <paramref name="organPaths" /> does not correspond to an organ container in the
   ///    <paramref name="spatialStructure" />.
   /// </exception>
   private IReadOnlyList<IContainer> resolveOrgans(SpatialStructure spatialStructure, string[] organPaths)
   {
      // We only need the top containers if creating an expression for the whole organism
      // it will be recursively populated in all organs
      if (organPaths == null || !organPaths.Any())
         return spatialStructure.TopContainers.ToList();

      var containerCache = new Cache<string, IContainer>(onMissingKey: _ => null);

      // Build the container cache for every organ container (ContainerType.Organ) that lives at or beneath a top container referenced by one
      // of the organ paths. Top containers may carry a ParentPath, so their absolute path is the ParentPath joined
      // with their Name — a top container itself can therefore be an organ (e.g. top container "Tumor" with
      // ParentPath "Organism|Liver" is reached via organ path "Organism|Liver|Tumor"). The PathCache returned by
      // CacheAllChildrenSatisfying already uses absolute paths (ParentPath-aware), but it only contains descendants,
      // so the top container itself is added explicitly when it is an organ.
      foreach (var topContainer in spatialStructure.TopContainers)
      {
         var topContainerPath = absolutePathFor(topContainer);

         if (!anyOrganPathUnder(organPaths, topContainerPath))
            continue;

         // cache self if necessary
         if (topContainer.ContainerType == ContainerType.Organ)
            containerCache[topContainerPath] = topContainer;

         var organContainers = _containerTask.CacheAllChildrenSatisfying<IContainer>(topContainer, c => c.ContainerType == ContainerType.Organ);
         foreach (var kvp in organContainers.KeyValues)
            containerCache[kvp.Key] = kvp.Value;
      }

      // return the container from the cache that matches each organPath
      return organPaths.Select(path => containerCache[path] ?? throw new ArgumentException(Exceptions.OrganNotFoundInSpatialStructure(path))).ToList();
   }

   private static string absolutePathFor(IContainer topContainer) =>
      topContainer.ParentPath != null
         ? new ObjectPath(topContainer.ParentPath.Append(topContainer.Name))
         : topContainer.Name;

   /// <summary>
   ///    Returns <c>true</c> if any of the <paramref name="organPaths" /> equals <paramref name="topContainerPath" />
   ///    or is a descendant of it (i.e. the organ path starts with <paramref name="topContainerPath" /> followed by
   ///    the path delimiter).
   /// </summary>
   private static bool anyOrganPathUnder(string[] organPaths, string topContainerPath) =>
      organPaths.Any(p => p == topContainerPath || p.StartsWith(topContainerPath + ObjectPath.PATH_DELIMITER));

   protected override string RemoveCommandDescription() => Commands.RemoveManyParameterValues;

   protected override IMoBiCommand RemoveCommandFor(ParameterValuesBuildingBlock buildingBlock, ObjectPath path) =>
      new RemoveParameterValueFromBuildingBlockCommand(buildingBlock, path);
}