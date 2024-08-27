using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Exceptions;
using MoBi.Core.Helper;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Assets.Extensions;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Services;
using OSPSuite.Utility;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Tasks.Interaction
{
   public abstract class InteractionTasksForExtendablePathAndValueEntity<TBuildingBlock, TPathAndValueEntity> : InteractionTasksForPathAndValueEntity<Module, TBuildingBlock, TPathAndValueEntity>, IInteractionTasksForExtendablePathAndValueEntity<TBuildingBlock, TPathAndValueEntity>
      where TBuildingBlock : class, ILookupBuildingBlock<TPathAndValueEntity>, IBuildingBlock, new() where TPathAndValueEntity : PathAndValueEntity
   {
      protected IExtendPathAndValuesManager<TPathAndValueEntity> _extendManager;
      protected readonly ICloneManagerForBuildingBlock _cloneManagerForBuildingBlock;

      protected readonly ISpatialStructureFactory _spatialStructureFactory;
      private readonly IMapper<ImportedQuantityDTO, TPathAndValueEntity> _dtoToQuantityToParameterValueMapper;
      protected readonly IPathAndValueEntityPathTask<ILookupBuildingBlock<TPathAndValueEntity>, TPathAndValueEntity> _entityPathTask;

      private readonly TBuildingBlock _newBuildingBlock;
      private readonly IObjectTypeResolver _objectTypeResolver;

      protected InteractionTasksForExtendablePathAndValueEntity(IInteractionTaskContext interactionTaskContext, IEditTasksForBuildingBlock<TBuildingBlock> editTask,
         IExtendPathAndValuesManager<TPathAndValueEntity> extendManager, ICloneManagerForBuildingBlock cloneManagerForBuildingBlock,
         IMoBiFormulaTask moBiFormulaTask, ISpatialStructureFactory spatialStructureFactory, IMapper<ImportedQuantityDTO, TPathAndValueEntity> dtoToQuantityToParameterValueMapper,
         IPathAndValueEntityPathTask<ILookupBuildingBlock<TPathAndValueEntity>, TPathAndValueEntity> entityPathTask,
         IParameterFactory parameterFactory, IObjectTypeResolver objectTypeResolver, IExportDataTableToExcelTask exportDataTableToExcelTask)
         : base(interactionTaskContext, editTask, moBiFormulaTask, parameterFactory, exportDataTableToExcelTask)
      {
         _extendManager = extendManager;
         _cloneManagerForBuildingBlock = cloneManagerForBuildingBlock;
         _spatialStructureFactory = spatialStructureFactory;
         _dtoToQuantityToParameterValueMapper = dtoToQuantityToParameterValueMapper;
         _entityPathTask = entityPathTask;
         _objectTypeResolver = objectTypeResolver;
         _newBuildingBlock = new TBuildingBlock().WithName(AppConstants.Captions.NewWindow(_objectTypeResolver.TypeFor<TBuildingBlock>()));
      }

      protected override double? ValueFromBuilder(TPathAndValueEntity builder)
      {
         return builder.Value;
      }

      /// <summary>
      ///    Updates the path and value entities defined in <paramref name="buildingBlockToUpdate" /> with the values defined in
      ///    <paramref name="buildingBlock" />. Returns a template cache containing all values defined in the template
      /// </summary>
      public ICache<string, TPathAndValueEntity> UpdateValuesFromTemplate(TBuildingBlock buildingBlockToUpdate, TBuildingBlock buildingBlock)
      {
         var templateBuildingBlock = buildingBlock;

         var entityCache = buildingBlockToUpdate.ToCache();
         var templateCache = templateBuildingBlock.ToCache();

         _cloneManagerForBuildingBlock.FormulaCache = buildingBlockToUpdate.FormulaCache;

         try
         {
            foreach (var templateKeyValue in templateCache.KeyValues)
            {
               var pathAndValueEntity = entityCache[templateKeyValue.Key];

               if (pathAndValueEntity == null)
                  addEntityToCache(buildingBlockToUpdate, templateKeyValue.Value);
               else
                  pathAndValueEntity.UpdatePropertiesFrom(templateKeyValue.Value, _cloneManagerForBuildingBlock);
            }

            buildingBlockToUpdate.Version = templateBuildingBlock.Version;
         }
         finally
         {
            _cloneManagerForBuildingBlock.FormulaCache = null;
         }

         return templateCache;
      }

      private void addEntityToCache(TBuildingBlock addEntityToCache, TPathAndValueEntity pathAndValueEntity)
      {
         var clonedEntity = _cloneManagerForBuildingBlock.Clone(pathAndValueEntity, addEntityToCache.FormulaCache);
         addEntityToCache.Add(clonedEntity);
      }

      /// <summary>
      ///    Returns the default dimension for the path and value entity type
      /// </summary>
      /// <returns>The default dimension</returns>
      public abstract IDimension GetDefaultDimension();

      protected T BuildingBlockById<T>(string buildingBlockId) where T : class, IBuildingBlock
      {
         if (!Context.ObjectRepository.ContainsObjectWithId(buildingBlockId))
            throw new MoBiException(AppConstants.Exceptions.SourceBuildingBlockNotInProject(_objectTypeResolver.TypeFor<T>()));

         return Context.Get<T>(buildingBlockId);
      }

      public IEnumerable<string> GetContainerPathItemsForBuildingBlock(TBuildingBlock buildingBlock)
      {
         return buildingBlock.SelectMany(x => x.Path.Select(y => y)).Distinct();
         // TODO 
         // var nameList = new List<string>();
         //
         // spatialStructure.Each(container => container.GetAllContainersAndSelf<IContainer>().Each(x => nameList.Add(x.Name)));
         // moleculeBuildingBlock.Each(builder => nameList.Add(builder.Name));
         //
         // return nameList.Distinct();
      }

      /// <summary>
      ///    Checks that the formula is equivalent for the path and value entity. This includes evaluation of constant formula to
      ///    a double
      /// </summary>
      /// <param name="pathAndValueEntity">The path and value entity to check</param>
      /// <param name="targetFormula">The formula being evaluated</param>
      /// <returns>True if the formula is equivalent to the path and value entity formula</returns>
      protected bool HasEquivalentFormula(PathAndValueEntity pathAndValueEntity, IFormula targetFormula)
      {
         return _entityPathTask.HasEquivalentFormula(pathAndValueEntity, targetFormula);
      }

      protected static bool HasEquivalentPathAndValueEntity(PathAndValueEntity pathAndValueEntity, IParameter parameter)
      {
         var (value, _) = parameter.TryGetValue();
         return HasEquivalentPathAndValueEntity(pathAndValueEntity, value);
      }

      protected static bool HasEquivalentPathAndValueEntity(PathAndValueEntity pathAndValueEntity, double? originalValue)
      {
         if (!originalValue.HasValue)
            return double.IsNaN(pathAndValueEntity.Value.GetValueOrDefault(double.NaN));

         if (!pathAndValueEntity.Value.HasValue)
            return false;

         return (ValueComparer.AreValuesEqual(originalValue.Value, pathAndValueEntity.Value.Value));
      }

      protected static bool HasEquivalentDimension(IWithDimension subject, IWithDimension target)
      {
         return target.Dimension == subject.Dimension;
      }

      protected MoBiMacroCommand CreateExtendMacroCommand(string buildingBlockType)
      {
         var moBiMacroCommand = new BulkUpdateMacroCommand
         {
            CommandType = AppConstants.Commands.ExtendCommand,
            Description = AppConstants.Commands.ExtendDescription,
            ObjectType = buildingBlockType
         };
         return moBiMacroCommand;
      }

      protected IMoBiCommand Extend(IReadOnlyList<TPathAndValueEntity> pathAndValueEntities, ILookupBuildingBlock<TPathAndValueEntity> buildingBlockToExtend, bool retainConflictingEntities = true)
      {
         var macro = CreateExtendMacroCommand(_interactionTaskContext.GetTypeFor(buildingBlockToExtend));

         prepareExtendActions(buildingBlockToExtend, macro);

         var cacheToExtend = pathAndValueEntities.ToCache();
         var targetCache = buildingBlockToExtend.ToCache();

         _extendManager.Merge(cacheToExtend, targetCache, defaultOption: retainConflictingEntities ? MergeConflictOptions.SkipAll : MergeConflictOptions.ReplaceAll);

         macro.Run(Context);

         return macro;
      }

      private void prepareExtendActions(ILookupBuildingBlock<TPathAndValueEntity> targetBuildingBlock, MoBiMacroCommand macro)
      {
         _extendManager.AddAction = entityToMerge => macro.Add(GenerateAddCommandAndUpdateFormulaReferences(entityToMerge, targetBuildingBlock));
         _extendManager.RemoveAction = entityToMerge => macro.Add(GenerateRemoveCommand(targetBuildingBlock, entityToMerge));
         _extendManager.CancelAction = macro.Clear;
      }

      protected IMoBiMacroCommand GenerateAddCommandAndUpdateFormulaReferences(TPathAndValueEntity entityToMerge, ILookupBuildingBlock<TPathAndValueEntity> targetBuildingBlock, string originalBuilderName = null)
      {
         var macroCommand = CreateAddBuilderMacroCommand(entityToMerge, targetBuildingBlock);

         macroCommand.Add(GenerateAddCommand(targetBuildingBlock, entityToMerge));
         macroCommand.Add(_moBiFormulaTask.AddFormulaToCacheOrFixReferenceCommand(targetBuildingBlock, entityToMerge));

         return macroCommand;
      }

      protected abstract IMoBiCommand GenerateRemoveCommand(ILookupBuildingBlock<TPathAndValueEntity> targetBuildingBlock, TPathAndValueEntity entityToRemove);
      protected abstract IMoBiCommand GenerateAddCommand(ILookupBuildingBlock<TPathAndValueEntity> targetBuildingBlock, TPathAndValueEntity entityToAdd);
      protected abstract IReadOnlyList<TPathAndValueEntity> CreatePathAndValueEntitiesBasedOnUsedTemplates(SpatialStructure spatialStructure, IReadOnlyList<MoleculeBuilder> molecules, TBuildingBlock buildingBlock);
      public abstract IMoBiCommand AddPathAndValueEntityToBuildingBlock(TBuildingBlock buildingBlock, TPathAndValueEntity pathAndValueEntity);
      public abstract IMoBiCommand ImportPathAndValueEntitiesToBuildingBlock(TBuildingBlock buildingBlock, IEnumerable<ImportedQuantityDTO> startQuantities);
      public abstract IMoBiCommand RemovePathAndValueEntityFromBuildingBlockCommand(TPathAndValueEntity pathAndValueEntity, TBuildingBlock buildingBlock);

      public virtual void ExtendPathAndValueEntityBuildingBlock(TBuildingBlock buildingBlock)
      {
         var commonModule = buildingBlock.Module;
         var (spatialStructure, molecules) = selectBuildingBlocksForExtend(commonModule.Molecules, commonModule.SpatialStructure);
         if (spatialStructure == null || molecules == null || !molecules.Any())
            return;

         var newPathAndValueEntities = CreatePathAndValueEntitiesBasedOnUsedTemplates(spatialStructure, molecules, buildingBlock);
         AddCommand(Extend(newPathAndValueEntities, buildingBlock));
      }

      public IMoBiCommand UpdatePathAndValueEntityDimension(TBuildingBlock pathAndValueEntitiesBuildingBlock, TPathAndValueEntity pathAndValueEntity, IDimension newDimension)
      {
         return new UpdateDimensionInPathAndValueEntityCommand<TPathAndValueEntity>(pathAndValueEntity, newDimension, _interactionTaskContext.DisplayUnitFor(newDimension), pathAndValueEntitiesBuildingBlock).Run(Context);
      }

      public ICommand CloneAndAddToParent(TBuildingBlock buildingBlockToClone, Module parentModule)
      {
         var name = GetNewNameForClone(buildingBlockToClone);

         if (string.IsNullOrEmpty(name))
            return new MoBiEmptyCommand();

         var clone = InteractionTask.Clone(buildingBlockToClone).WithName(name);

         return AddTo(clone, parentModule, null);
      }

      protected static bool ShouldFormulaBeOverridden(ImportedQuantityDTO quantityDTO, TPathAndValueEntity pathAndValueEntity)
      {
         return quantityDTO.IsQuantitySpecified && pathAndValueEntity.Formula != null;
      }

      protected IMoBiCommand GetChangePathAndValueEntityFormulaCommand(TBuildingBlock buildingBlock, TPathAndValueEntity pathAndValueEntity, IFormula newFormula, IFormula oldFormula)
      {
         return new ChangeValueFormulaCommand<TPathAndValueEntity>(buildingBlock, pathAndValueEntity, newFormula, oldFormula);
      }

      protected abstract IMoBiCommand GetUpdatePathAndValueEntityInBuildingBlockCommand(TBuildingBlock buildingBlock, ImportedQuantityDTO dto);

      protected void GetImportPathAndValueEntityMacroCommand(TBuildingBlock buildingBlock, IEnumerable<ImportedQuantityDTO> startQuantities, BulkUpdateMacroCommand macroCommand)
      {
         startQuantities.Each(quantityDTO =>
         {
            var pathAndValueEntity = buildingBlock.SingleOrDefault(x => Equals(x.Path, quantityDTO.Path));

            if (pathAndValueEntity == null)
               macroCommand.Add(GenerateAddCommand(buildingBlock, _dtoToQuantityToParameterValueMapper.MapFrom(quantityDTO)));
            else
            {
               if (ShouldFormulaBeOverridden(quantityDTO, pathAndValueEntity))
                  macroCommand.Add(GetChangePathAndValueEntityFormulaCommand(buildingBlock, pathAndValueEntity: pathAndValueEntity, newFormula: null, oldFormula: pathAndValueEntity.Formula));

               macroCommand.Add(GetUpdatePathAndValueEntityInBuildingBlockCommand(buildingBlock, quantityDTO));
            }
         });
      }

      public IMoBiCommand EditPathAndValueEntityName(TBuildingBlock buildingBlock, TPathAndValueEntity pathAndValueEntity, string newValue)
      {
         return _entityPathTask.UpdateName(buildingBlock, pathAndValueEntity, newValue);
      }

      public IMoBiCommand EditPathAndValueEntityContainerPath(TBuildingBlock buildingBlock, TPathAndValueEntity pathAndValueEntity, int indexToUpdate, string newValue)
      {
         return _entityPathTask.UpdateContainerPath(buildingBlock, pathAndValueEntity, indexToUpdate, newValue);
      }

      public override IMoBiCommand GetRemoveCommand(TBuildingBlock objectToRemove, Module parent, IBuildingBlock buildingBlock)
      {
         return new RemoveBuildingBlockFromModuleCommand<TBuildingBlock>(objectToRemove, parent);
      }

      public override IMoBiCommand GetAddCommand(TBuildingBlock itemToAdd, Module parent, IBuildingBlock buildingBlock)
      {
         return new AddBuildingBlockToModuleCommand<TBuildingBlock>(itemToAdd, parent);
      }

      private (SpatialStructure spatialStructure, IReadOnlyList<MoleculeBuilder> molecules) selectBuildingBlocksForExtend(MoleculeBuildingBlock defaultMolecules, SpatialStructure defaultSpatialStructure) =>
         selectBuildingBlocks(x => x.SelectBuildingBlocksForExtend(defaultMolecules, defaultSpatialStructure));

      protected (SpatialStructure spatialStructure, IReadOnlyList<MoleculeBuilder> molecules) SelectBuildingBlocksForRefresh(MoleculeBuildingBlock defaultMolecules, SpatialStructure defaultSpatialStructure, IReadOnlyList<string> selectableBuilders) =>
         selectBuildingBlocks(x => x.SelectMoleculesForRefresh(defaultMolecules, selectableBuilders));

      private (SpatialStructure spatialStructure, IReadOnlyList<MoleculeBuilder> molecules) selectBuildingBlocks(Action<ISelectSpatialStructureAndMoleculesPresenter> actionToSelectBuildingBlocks)
      {
         var moleculeBlockCollection = _interactionTaskContext.BuildingBlockRepository.MoleculeBlockCollection;
         var spatialStructureCollection = _interactionTaskContext.BuildingBlockRepository.SpatialStructureCollection;

         if (moleculeBlockCollection.Count == 0 && spatialStructureCollection.Count == 0)
            return (null, Enumerable.Empty<MoleculeBuilder>().ToList());

         // If there is only one option that could be selected for each required building block, then we just use those options and don't
         // need to ask the user to make a selection
         if (!shouldSelectBuildingBlocks(moleculeBlockCollection, spatialStructureCollection))
         {
            return (spatialStructureCollection.Single(), moleculeBlockCollection.Single().ToList());
         }

         using (var selectorPresenter = Context.Resolve<ISelectSpatialStructureAndMoleculesPresenter>())
         {
            actionToSelectBuildingBlocks(selectorPresenter);
            return (selectorPresenter.SelectedSpatialStructure, selectorPresenter.SelectedMolecules);
         }
      }

      private static bool shouldSelectBuildingBlocks(IReadOnlyList<MoleculeBuildingBlock> moleculeBlockCollection, IReadOnlyList<MoBiSpatialStructure> spatialStructureCollection)
      {
         return shouldSelectMolecules(moleculeBlockCollection) || spatialStructureCollection.Count > 1;
      }

      private static bool shouldSelectMolecules(IReadOnlyList<MoleculeBuildingBlock> moleculeBlockCollection)
      {
         return moleculeBlockCollection.Count > 1 || moleculeBlockCollection.Any(x => x.Count() > 1);
      }

      public IMoBiCommand AddOrExtendWith(TBuildingBlock buildingBlock, Module module)
      {
         if (buildingBlock == null || !buildingBlock.Any())
            return new MoBiEmptyCommand();

         var buildingBlockToAddTo = buildingBlockToAdd(module);

         if (buildingBlockToAddTo != null)
            return ExtendBuildingBlockWith(buildingBlockToAddTo, buildingBlock.ToList());

         return new AddBuildingBlockToModuleCommand<TBuildingBlock>(_interactionTaskContext.InteractionTask.Clone(buildingBlock), module).Run(_interactionTaskContext.Context);
      }

      public IMoBiCommand ExtendBuildingBlockWith(TBuildingBlock buildingBlock, IReadOnlyList<TPathAndValueEntity> newEntities)
      {
         var newStartValues = FilterEntitiesToRetain(buildingBlock, newEntities);
         return Extend(newStartValues, buildingBlock, retainConflictingEntities: false);
      }

      public IReadOnlyList<TPathAndValueEntity> FilterEntitiesToRetain(TBuildingBlock originalBuildingBlock, IReadOnlyList<TPathAndValueEntity> newEntities)
      {
         using (var pathSelectionPresenter = Context.Resolve<IPathAndValueEntitySelectionPresenter>())
         {
            return pathSelectionPresenter.SelectReplacementEntities(newEntities, originalBuildingBlock);
         }
      }

      private TBuildingBlock buildingBlockToAdd(Module module)
      {
         var moduleBuildingBlocks = module.BuildingBlocks.OfType<TBuildingBlock>().ToList();

         // If there are no existing building blocks, then you can only add as a new building block
         if (!moduleBuildingBlocks.Any())
            return null;

         using (var modal = ApplicationController.Start<IModalPresenter>())
         {
            var presenter = ApplicationController.Start<ISelectSinglePresenter<TBuildingBlock>>();
            presenter.SetDescription(AppConstants.Captions.SelectTheBuildingBlockWhereEntitiesWillBeAddedOrUpdated(new ObjectTypeResolver().TypeFor<TPathAndValueEntity>().Pluralize()));
            modal.Text = AppConstants.Captions.SelectBuildingBlock;
            modal.Encapsulate(presenter);

            var allItems = new List<TBuildingBlock>(moduleBuildingBlocks)
            {
               _newBuildingBlock
            };
            presenter.InitializeWith(allItems, x => !ReferenceEquals(_newBuildingBlock, x));
            modal.CanCancel = false;
            modal.Show(AppConstants.Dialog.SELECT_SINGLE_SIZE);
            return existingBuildingBlockSelected(presenter.Selection) ? presenter.Selection : null;
         }
      }

      private bool existingBuildingBlockSelected(TBuildingBlock selectedBuildingBlock) => !ReferenceEquals(_newBuildingBlock, selectedBuildingBlock);
   }
}