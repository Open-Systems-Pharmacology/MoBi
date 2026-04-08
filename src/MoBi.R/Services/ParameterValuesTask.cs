using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Extensions;
using MoBi.Core.Services;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Extensions;
using System;
using static MoBi.Assets.AppConstants;

namespace MoBi.R.Services;

public interface IParameterValuesTask : IPathAndValuesTask<ParameterValuesBuildingBlock, ParameterValue>
{
   void SetParameterValues(ParameterValuesBuildingBlock buildingBlock, string[] quantityPaths, double[] quantityValues, string[] dimensionNames);
   
   void SetParameterValues(ParameterValuesBuildingBlock buildingBlock, string quantityPath, double quantityValue, string dimensionName);

   void DeleteParameterValues(ParameterValuesBuildingBlock buildingBlock, params string[] pathsToDelete);
   
   void DeleteParameterValues(ParameterValuesBuildingBlock buildingBlock, string pathToDelete);

   string[] AddLocalMoleculeParameters(ParameterValuesBuildingBlock buildingBlock, MoBiSpatialStructure spatialStructure, MoleculeBuildingBlock moleculeBuildingBlock, params string[] moleculeNames);
}

public class ParameterValuesTask : ExtendablePathAndValuesTask<ParameterValuesBuildingBlock, ParameterValue>, IParameterValuesTask
{
   private readonly IParameterValueBuildingBlockExtendManager _extendManager;

   public ParameterValuesTask(IMoBiContext context, IObjectTypeResolver objectTypeResolver, IParameterValueBuildingBlockExtendManager extendManager) : base(context, objectTypeResolver, extendManager)
   {
      _extendManager = extendManager;
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

   public string[] AddLocalMoleculeParameters(ParameterValuesBuildingBlock buildingBlock, MoBiSpatialStructure spatialStructure, MoleculeBuildingBlock moleculeBuildingBlock, string[] moleculeNames) =>
      Extend(buildingBlock, spatialStructure, moleculeBuildingBlock, moleculeNames);

   protected override string RemoveCommandDescription() => Commands.RemoveManyParameterValues;

   protected override IMoBiCommand RemoveCommandFor(ParameterValuesBuildingBlock buildingBlock, ObjectPath path) => 
      new RemoveParameterValueFromBuildingBlockCommand(buildingBlock, path);
}