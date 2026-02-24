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

public interface IPathAndValuesTask<TBuildingBlock, TBuilder> where TBuildingBlock : PathAndValueEntityBuildingBlock<TBuilder> where TBuilder : PathAndValueEntity
{
   string[] AllPathsFrom(TBuildingBlock buildingBlock);
   double[] AllValuesFrom(TBuildingBlock buildingBlock, string[] paths);
   string[] AllDimensionsFrom(TBuildingBlock buildingBlock, string[] paths);
}

public abstract class ExtendablePathAndValuesTask<TBuildingBlock, TBuilder>: IPathAndValuesTask<TBuildingBlock, TBuilder> where TBuildingBlock : PathAndValueEntityBuildingBlock<TBuilder> where TBuilder : PathAndValueEntity
{
   private readonly IObjectTypeResolver _objectTypeResolver;
   private readonly IExtendPathAndValuesManager<TBuilder> _extendManager;
   protected readonly IMoBiContext _context;

   protected ExtendablePathAndValuesTask(IMoBiContext context, IObjectTypeResolver objectTypeResolver, IExtendPathAndValuesManager<TBuilder> extendManager)
   {
      _objectTypeResolver = objectTypeResolver;
      _extendManager = extendManager;
      _context = context;
   }

   protected void Delete(TBuildingBlock buildingBlock, string[] pathsToDelete)
   {
      var foundPaths = pathsToDelete.Where(x => buildingBlock.FindByPath(x) != null).ToList();
      if (!foundPaths.Any())
         return;

      var macroCommand = new MoBiMacroCommand
      {
         Description = RemoveCommandDescription(),
         CommandType = AppConstants.Commands.DeleteCommand,
         ObjectType = _objectTypeResolver.TypeFor<TBuilder>()
      };

      macroCommand.AddRange(foundPaths.Select(x => RemoveCommandFor(buildingBlock, x.ToObjectPath())));

      _context.AddToHistory(macroCommand.RunCommand(_context));
   }

   protected abstract string RemoveCommandDescription();

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

   protected MoBiMacroCommand MacroCommandForUpdateAndInsert()
   {
      return new MoBiMacroCommand
      {
         CommandType = AppConstants.Commands.ExtendCommand,
         Description = AppConstants.Commands.ExtendDescription,
         ObjectType = _objectTypeResolver.TypeFor<TBuildingBlock>()
      };
   }

   protected T[] AllFrom<T>(TBuildingBlock buildingBlock, string[] paths, Func<TBuilder, T> selector)
   {
      if (paths != null && paths.Length != 0)
      {
         var pathSet = new HashSet<string>(paths);

         if (pathSet.Count != paths.Length)
            throw new ArgumentException(AppConstants.Exceptions.DuplicatePathsInInput(paths.Length, pathSet.Count));

         var entities = buildingBlock.Where(x => pathSet.Contains(x.Path)).ToList();

         if (entities.Count != paths.Length)
            throw new ArgumentException(AppConstants.Exceptions.NotAllPathsFoundInBuildingBlock(paths.Length, entities.Count));

         return entities.Select(selector).ToArray();
      }

      return buildingBlock.Select(selector).ToArray();
   }

   public string[] AllPathsFrom(TBuildingBlock buildingBlock) => AllFrom(buildingBlock, paths:null, x => x.Path.PathAsString);

   public double[] AllValuesFrom(TBuildingBlock buildingBlock, string[] paths) => AllFrom(buildingBlock, paths, x => x.Value ?? double.NaN);

   public string[] AllDimensionsFrom(TBuildingBlock buildingBlock, string[] paths) => AllFrom(buildingBlock, paths, x => x.Dimension.Name);
}