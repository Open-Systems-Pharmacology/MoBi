using System;
using System.Collections.Generic;
using System.Linq;
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

public interface IInitialConditionsTask
{
   void DeleteInitialConditions(InitialConditionsBuildingBlock buildingBlock, string[] pathsToDelete);

   void ExtendInitialConditions(InitialConditionsBuildingBlock buildingBlock, MoBiSpatialStructure spatialStructure, MoleculeBuildingBlock moleculeBuildingBlock, string[] moleculeNames);

   void SetInitialConditions(InitialConditionsBuildingBlock buildingBlock, string[] quantityPaths, string[] dimensionNames, int[] quantityValues, int[] scaleDivisors, bool[] isPresent, bool[] negativeAllowed);
}

public class InitialConditionsTask : IInitialConditionsTask
{
   private readonly IMoBiContext _context;
   private readonly IObjectTypeResolver _objectTypeResolver;
   private readonly IInitialConditionsBuildingBlockExtendManager _extendManager;

   public InitialConditionsTask(IMoBiContext context, IObjectTypeResolver objectTypeResolver, IInitialConditionsBuildingBlockExtendManager extendManager)
   {
      _context = context;
      _objectTypeResolver = objectTypeResolver;
      _extendManager = extendManager;
   }

   public void DeleteInitialConditions(InitialConditionsBuildingBlock buildingBlock, string[] pathsToDelete)
   {
      var initialConditionsToDelete = pathsToDelete.Select(buildingBlock.FindByPath).Where(x => x != null).ToList();
      var macroCommand = new MoBiMacroCommand
      {
         Description = AppConstants.Commands.RemoveMultipleInitialConditions,
         CommandType = AppConstants.Commands.DeleteCommand,
         ObjectType = _objectTypeResolver.TypeFor<InitialCondition>()
      };

      macroCommand.AddRange(initialConditionsToDelete.Select(ic => new RemoveInitialConditionFromBuildingBlockCommand(buildingBlock, ic.Path)));

      _context.AddToHistory(macroCommand.RunCommand(_context));
   }

   public void ExtendInitialConditions(InitialConditionsBuildingBlock buildingBlock, MoBiSpatialStructure spatialStructure, MoleculeBuildingBlock moleculeBuildingBlock, string[] moleculeNames)
   {
      IReadOnlyList<MoleculeBuilder> molecules;
      if (moleculeNames == null || !moleculeNames.Any())
         molecules = moleculeBuildingBlock.All().ToList();
      else
         molecules = moleculeBuildingBlock.Where(x => moleculeNames.Contains(x.Name)).ToList();

      _context.AddToHistory(_extendManager.ExtendPathAndValueEntitiesBasedOnUsedTemplates(spatialStructure, molecules, buildingBlock));
   }

   public void SetInitialConditions(InitialConditionsBuildingBlock buildingBlock, string[] quantityPaths, string[] dimensionNames, int[] quantityValues, int[] scaleDivisors, bool[] isPresent, bool[] negativeAllowed)
   {
      if (!arrayLengthsAreConsistent(quantityPaths, dimensionNames, quantityValues, scaleDivisors, isPresent, negativeAllowed))
         throw new ArgumentException("All input arrays must have the same length.");

      var macroCommand = new MoBiMacroCommand
      {
         CommandType = AppConstants.Commands.ExtendCommand,
         Description = AppConstants.Commands.ExtendDescription,
         ObjectType = _objectTypeResolver.TypeFor<InitialConditionsBuildingBlock>()
      };

      quantityPaths.Each((quantityPath, i) => macroCommand.Add(_extendManager.MergeWithUpdate(buildingBlock, quantityPath.ToObjectPath(), dimensionNames[i], quantityValues[i], scaleDivisors[i], isPresent[i], negativeAllowed[i])));

      _context.AddToHistory(macroCommand.RunCommand(_context));
   }

   private static bool arrayLengthsAreConsistent(params Array[] arrays)
   {
      return arrays.All(x => x.Length == arrays[0].Length);
   }
}