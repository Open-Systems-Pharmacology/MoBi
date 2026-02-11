using System;
using System.Collections.Generic;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Core.Services;

public interface IExtendPathAndValuesManager<T> : IMergeManager<T> where T : PathAndValueEntity
{
   /// <summary>
   ///    In the event of a conflict where user specifies that the newly merged element should replace the existing, this
   ///    method will be used to remove the existing element
   /// </summary>
   Action<T> RemoveAction { set; }

   /// <summary>
   ///    In the event of a conflict where user specifies that the newly merged element should replace the existing, this
   ///    method will be used to add the newly merged element
   /// </summary>
   Action<T> AddAction { set; }

   /// <summary>
   /// Runs commands to extend the path and value entities of the <paramref name="buildingBlock"/> based on the provided <paramref name="spatialStructure"/> and <paramref name="molecules"/>
   /// Each command has to run before the next one is created to ensure the formula cache is up to date for the next command to consolidate formulas where possible.
   /// </summary>
   /// <returns>The already run command</returns>
   IMoBiCommand ExtendPathAndValueEntitiesBasedOnUsedTemplates(SpatialStructure spatialStructure, IReadOnlyList<MoleculeBuilder> molecules, ILookupBuildingBlock<T> buildingBlock);

   /// <summary>
   /// Runs commands to extend the <paramref name="buildingBlockToExtend"/> with the provided <paramref name="pathAndValueEntities"/>.
   /// Each command has to run before the next one is created to ensure the formula cache is up to date for the next command to consolidate formulas where possible.
   /// </summary>
   /// <returns>The already run command</returns>
   IMoBiCommand Extend(IReadOnlyList<T> pathAndValueEntities, ILookupBuildingBlock<T> buildingBlockToExtend, bool retainConflictingEntities = true);
}

public abstract class ExtendPathAndValuesManager<T> : AbstractMergeManager<T>, IExtendPathAndValuesManager<T> where T : PathAndValueEntity
{
   private readonly IMoBiFormulaTask _moBiFormulaTask;
   private readonly IObjectTypeResolver _objectTypeResolver;
   private readonly IMoBiContext _context;

   protected ExtendPathAndValuesManager(IMoBiFormulaTask moBiFormulaTask, IObjectTypeResolver objectTypeResolver, IMoBiContext context)
   {
      _moBiFormulaTask = moBiFormulaTask;
      _objectTypeResolver = objectTypeResolver;
      _context = context;
   }

   public Action<T> RemoveAction
   {
      set => _removeAction = value;
   }

   public Action<T> AddAction
   {
      set => _addAction = value;
   }

   protected abstract IReadOnlyList<T> CreatePathAndValueEntitiesBasedOnUsedTemplates(SpatialStructure spatialStructure, IReadOnlyList<MoleculeBuilder> molecules, ILookupBuildingBlock<T> buildingBlock);

   public IMoBiCommand ExtendPathAndValueEntitiesBasedOnUsedTemplates(SpatialStructure spatialStructure, IReadOnlyList<MoleculeBuilder> molecules, ILookupBuildingBlock<T> buildingBlock)
   {
      return Extend(CreatePathAndValueEntitiesBasedOnUsedTemplates(spatialStructure, molecules, buildingBlock), buildingBlock);
   }

   private MoBiMacroCommand createExtendMacroCommand(string buildingBlockType)
   {
      var moBiMacroCommand = new BulkUpdateMacroCommand
      {
         CommandType = AppConstants.Commands.ExtendCommand,
         Description = AppConstants.Commands.ExtendDescription,
         ObjectType = buildingBlockType
      };
      return moBiMacroCommand;
   }

   private void prepareExtendActions(ILookupBuildingBlock<T> targetBuildingBlock, MoBiMacroCommand macro)
   {
      // Perform extend commands per-entity. That allows the formula references to be managed on up-to-date formula cache enabling consolidation of equivalent formulas
      AddAction = entityToMerge => macro.Add(generateAddCommandAndUpdateFormulaReferences(entityToMerge, targetBuildingBlock).RunCommand(_context));
      RemoveAction = entityToMerge => macro.Add(GenerateRemoveCommand(targetBuildingBlock, entityToMerge).RunCommand(_context));
   }

   private IMoBiMacroCommand generateAddCommandAndUpdateFormulaReferences(T entityToMerge, ILookupBuildingBlock<T> targetBuildingBlock)
   {
      var macroCommand = createAddBuilderMacroCommand(entityToMerge, targetBuildingBlock);

      macroCommand.Add(GenerateAddCommand(targetBuildingBlock, entityToMerge));
      macroCommand.Add(_moBiFormulaTask.AddFormulaToCacheOrFixReferenceCommand(targetBuildingBlock, entityToMerge));

      return macroCommand;
   }

   private MoBiMacroCommand createAddBuilderMacroCommand(T builder, IBuildingBlock targetBuildingBlock)
   {
      var objectType = _objectTypeResolver.TypeFor<T>();
      return new MoBiMacroCommand
      {
         Description = AppConstants.Commands.AddToDescription(objectType, builder.Name, targetBuildingBlock.Name),
         CommandType = AppConstants.Commands.AddCommand,
         ObjectType = objectType
      };
   }

   protected abstract IMoBiCommand GenerateRemoveCommand(ILookupBuildingBlock<T> targetBuildingBlock, T entityToRemove);

   public abstract IMoBiCommand GenerateAddCommand(ILookupBuildingBlock<T> targetBuildingBlock, T entityToAdd);

   public IMoBiCommand Extend(IReadOnlyList<T> pathAndValueEntities, ILookupBuildingBlock<T> buildingBlockToExtend, bool retainConflictingEntities = true)
   {
      var macro = createExtendMacroCommand(_objectTypeResolver.TypeFor(buildingBlockToExtend));

      prepareExtendActions(buildingBlockToExtend, macro);

      var cacheToExtend = pathAndValueEntities.ToCache();
      var targetCache = buildingBlockToExtend.ToCache();

      Merge(cacheToExtend, targetCache, mergeConflictOption: retainConflictingEntities ? MergeConflictOptions.SkipAll : MergeConflictOptions.ReplaceAll);

      return macro;
   }
}