using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Extensions;
using MoBi.Core.Services;
using MoBi.R.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Extensions;

namespace MoBi.R.Services;

public interface IPathAndValuesTask<TBuildingBlock, TBuilder> where TBuildingBlock : PathAndValueEntityBuildingBlock<TBuilder> where TBuilder : PathAndValueEntity
{
   string[] AllPathsFrom(TBuildingBlock buildingBlock);
   double[] AllValuesFrom(TBuildingBlock buildingBlock, params string[] paths);
   string[] AllDimensionsFrom(TBuildingBlock buildingBlock, params string[] paths);
   string[] AllUnitsFrom(TBuildingBlock buildingBlock, params string[] paths);
}

public abstract class ExtendablePathAndValuesTask<TBuildingBlock, TBuilder> : IPathAndValuesTask<TBuildingBlock, TBuilder> where TBuildingBlock : PathAndValueEntityBuildingBlock<TBuilder> where TBuilder : PathAndValueEntity
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

   protected string[] Extend(TBuildingBlock buildingBlock, SpatialStructure spatialStructure, MoleculeBuildingBlock moleculeBuildingBlock, string[] moleculeNames)
   {
      var molecules = moleculesFor(moleculeNames, moleculeBuildingBlock);

      var existingPaths = new HashSet<string>(buildingBlock.Select(x => x.Path.PathAsString));

      _context.AddToHistory(_extendManager.ExtendPathAndValueEntitiesBasedOnUsedTemplates(spatialStructure, molecules, buildingBlock));

      return buildingBlock.Where(x => !existingPaths.Contains(x.Path.PathAsString)).Select(x => x.Path.PathAsString).ToArray();
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

   public string[] AllPathsFrom(TBuildingBlock buildingBlock) => buildingBlock.AllPathsFrom();

   public double[] AllValuesFrom(TBuildingBlock buildingBlock, params string[] paths) => buildingBlock.AllValuesFrom(paths);

   public string[] AllDimensionsFrom(TBuildingBlock buildingBlock, params string[] paths) => buildingBlock.AllDimensionsFrom(paths);

   public string[] AllUnitsFrom(TBuildingBlock buildingBlock, params string[] paths) => buildingBlock.AllUnitsFrom(paths);
}