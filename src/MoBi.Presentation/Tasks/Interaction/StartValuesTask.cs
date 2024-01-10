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
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Utility;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Tasks.Interaction
{
   public abstract class StartValuesTask<TBuildingBlock, TPathAndValueEntity> : InteractionTasksForPathAndValueEntity<Module, TBuildingBlock, TPathAndValueEntity>, IStartValuesTask<TBuildingBlock, TPathAndValueEntity>
      where TBuildingBlock : class, ILookupBuildingBlock<TPathAndValueEntity>, IBuildingBlock
      where TPathAndValueEntity : PathAndValueEntity
   {
      protected IExtendPathAndValuesManager<TPathAndValueEntity> _extendManager;
      protected readonly ICloneManagerForBuildingBlock _cloneManagerForBuildingBlock;

      protected readonly ISpatialStructureFactory _spatialStructureFactory;
      private readonly IMapper<ImportedQuantityDTO, TPathAndValueEntity> _dtoToQuantityToParameterValueMapper;
      private readonly IStartValuePathTask<ILookupBuildingBlock<TPathAndValueEntity>, TPathAndValueEntity> _entityPathTask;

      protected StartValuesTask(IInteractionTaskContext interactionTaskContext, IEditTasksForBuildingBlock<TBuildingBlock> editTask,
         IExtendPathAndValuesManager<TPathAndValueEntity> extendManager, ICloneManagerForBuildingBlock cloneManagerForBuildingBlock,
         IMoBiFormulaTask moBiFormulaTask, ISpatialStructureFactory spatialStructureFactory, IMapper<ImportedQuantityDTO, TPathAndValueEntity> dtoToQuantityToParameterValueMapper,
         IStartValuePathTask<ILookupBuildingBlock<TPathAndValueEntity>, TPathAndValueEntity> entityPathTask)
         : base(interactionTaskContext, editTask, moBiFormulaTask)
      {
         _extendManager = extendManager;
         _cloneManagerForBuildingBlock = cloneManagerForBuildingBlock;
         _spatialStructureFactory = spatialStructureFactory;
         _dtoToQuantityToParameterValueMapper = dtoToQuantityToParameterValueMapper;
         _entityPathTask = entityPathTask;
      }

      protected override double? ValueFromBuilder(TPathAndValueEntity builder)
      {
         return builder.Value;
      }

      /// <summary>
      ///    Updates the start values defined in <paramref name="buildingBlockToUpdate" /> with the values defined in
      ///    <paramref name="buildingBlock" />. Returns a template cache containing all values defined in the template
      /// </summary>
      public ICache<string, TPathAndValueEntity> UpdateValuesFromTemplate(TBuildingBlock buildingBlockToUpdate, TBuildingBlock buildingBlock)
      {
         var templateBuildingBlock = buildingBlock;
         // if (startValueInfo.BuildingBlockIsTemplate)
         //    templateStartValues = startValueInfo.TemplateBuildingBlock;

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
      ///    Returns the default dimension for the start value type
      /// </summary>
      /// <returns>The default dimension</returns>
      public abstract IDimension GetDefaultDimension();

      protected T BuildingBlockById<T>(string buildingBlockId) where T : class, IBuildingBlock
      {
         if (!Context.ObjectRepository.ContainsObjectWithId(buildingBlockId))
            throw new MoBiException(AppConstants.Exceptions.SourceBuildingBlockNotInProject(new ObjectTypeResolver().TypeFor<T>()));

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
      ///    Checks that the formula is equivalent for the start value. This includes evaluation of constant formula to a double
      /// </summary>
      /// <param name="pathAndValueEntity">The start value to check</param>
      /// <param name="targetFormula">The formula being evaluated</param>
      /// <returns>True if the formula is equivalent to the start value formula</returns>
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

      protected IMoBiCommand Extend(IReadOnlyList<TPathAndValueEntity> startValues, ILookupBuildingBlock<TPathAndValueEntity> buildingBlockToExtend)
      {
         var macro = CreateExtendMacroCommand(_interactionTaskContext.GetTypeFor(buildingBlockToExtend));

         prepareExtendActions(buildingBlockToExtend, macro);

         var cacheToExtend = startValues.ToCache();
         var targetCache = buildingBlockToExtend.ToCache();

         // Use the merge manager to implement the extend. We can take advantage of the equivalency checker to favor the existing 
         // start value if a conflict is found (always prefer the existing start value)
         _extendManager.Merge(cacheToExtend, targetCache, areElementsEquivalent: (s1, s2) => true);

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
      protected abstract IReadOnlyList<TPathAndValueEntity> CreateStartValuesBasedOnUsedTemplates(SpatialStructure spatialStructure, IReadOnlyList<MoleculeBuilder> molecules, TBuildingBlock buildingBlock);
      public abstract IMoBiCommand AddPathAndValueEntityToBuildingBlock(TBuildingBlock buildingBlock, TPathAndValueEntity pathAndValueEntity);
      public abstract IMoBiCommand ImportPathAndValueEntitiesToBuildingBlock(TBuildingBlock buildingBlock, IEnumerable<ImportedQuantityDTO> startQuantities);
      public abstract IMoBiCommand RemovePathAndValueEntityFromBuildingBlockCommand(TPathAndValueEntity pathAndValueEntity, TBuildingBlock buildingBlock);

      public virtual void ExtendStartValueBuildingBlock(TBuildingBlock buildingBlock)
      {
         var commonModule = buildingBlock.Module;
         var (spatialStructure, molecules) = selectBuildingBlocksForExtend(commonModule.Molecules, commonModule.SpatialStructure);
         if (spatialStructure == null || molecules == null || !molecules.Any())
            return;

         var newStartValues = CreateStartValuesBasedOnUsedTemplates(spatialStructure, molecules, buildingBlock);
         AddCommand(Extend(newStartValues, buildingBlock));
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

      private (SpatialStructure spatialStructure, IReadOnlyList<MoleculeBuilder> molecules) selectBuildingBlocksForExtend(MoleculeBuildingBlock defaultMolecules, SpatialStructure defaultSpatialStructure)
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
            selectorPresenter.SelectBuildingBlocksForExtend(defaultMolecules, defaultSpatialStructure);
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
   }
}