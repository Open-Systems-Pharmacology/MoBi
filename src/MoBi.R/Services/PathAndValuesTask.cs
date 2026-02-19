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

public abstract class PathAndValuesTask<TBuildingBlock, TBuilder> where TBuildingBlock : PathAndValueEntityBuildingBlock<TBuilder> where TBuilder : PathAndValueEntity
{
   protected readonly IObjectTypeResolver _objectTypeResolver;
   private readonly IExtendPathAndValuesManager<TBuilder> _extendManager;
   protected readonly IMoBiContext _context;

   protected PathAndValuesTask(IMoBiContext context, IObjectTypeResolver objectTypeResolver, IExtendPathAndValuesManager<TBuilder> extendManager)
   {
      _objectTypeResolver = objectTypeResolver;
      _extendManager = extendManager;
      _context = context;
   }

   protected void Delete(TBuildingBlock buildingBlock, string[] pathsToDelete)
   {
      var foundPaths = pathsToDelete.Where(x => buildingBlock.FindByPath(x) != null).ToList();
      var macroCommand = new MoBiMacroCommand
      {
         Description = AppConstants.Commands.RemoveMultipleInitialConditions,
         CommandType = AppConstants.Commands.DeleteCommand,
         ObjectType = _objectTypeResolver.TypeFor<TBuilder>()
      };

      macroCommand.AddRange(foundPaths.Select(x => RemoveCommandFor(buildingBlock, x.ToObjectPath())));

      _context.AddToHistory(macroCommand.RunCommand(_context));
   }

   protected void Extend(TBuildingBlock buildingBlock, MoBiSpatialStructure spatialStructure, MoleculeBuildingBlock moleculeBuildingBlock, string[] moleculeNames)
   {
      var molecules = moleculesFor(moleculeNames, moleculeBuildingBlock);

      _context.AddToHistory(_extendManager.ExtendPathAndValueEntitiesBasedOnUsedTemplates(spatialStructure, molecules, buildingBlock));
   }

   private static IReadOnlyList<MoleculeBuilder> moleculesFor(string[] moleculeNames, MoleculeBuildingBlock moleculeBuildingBlock)
   {
      IReadOnlyList<MoleculeBuilder> molecules;
      if (moleculeNames == null || !moleculeNames.Any())
         molecules = moleculeBuildingBlock.All().ToList();
      else
         molecules = moleculeBuildingBlock.Where(x => moleculeNames.Contains(x.Name)).ToList();
      return molecules;
   }

   protected abstract IMoBiCommand RemoveCommandFor(TBuildingBlock buildingBlock, ObjectPath path);

   protected static bool ArrayLengthsAreConsistent(params Array[] arrays) => arrays.All(x => x.Length == arrays[0].Length);

   protected MoBiMacroCommand MacroCommandForUpdateAndInsert()
   {
      return new MoBiMacroCommand
      {
         CommandType = AppConstants.Commands.ExtendCommand,
         Description = AppConstants.Commands.ExtendDescription,
         ObjectType = _objectTypeResolver.TypeFor<TBuildingBlock>()
      };
   }
}