using System.Collections.Generic;
using System.IO;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Repositories;
using MoBi.Core.Services;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Tasks.Interaction
{
   public interface IInteractionTask
   {
      IReadOnlyCollection<T> LoadItems<T>(string filename) where T : class, IObjectBase;
      void Save<T>(IReadOnlyList<T> entitiesToSerialize) where T : IObjectBase;
      void Save<T>(T entityToSerialize, string fileName);
      string IconFor<T>(T entity) where T : IObjectBase;
      bool CorrectName<T>(T objectBase, IEnumerable<string> forbiddenNames) where T : IObjectBase;
      T Clone<T>(T objectToClone) where T : class, IObjectBase;
      bool AdjustFormula<T>(T objectBase, IBuildingBlock buildingBlockWithFormulaCache, IMoBiMacroCommand macroCommand) where T : IObjectBase;
      string TypeFor<T>(T objectRequestingType) where T : class;
      IEnumerable<string> ForbiddenNamesFor<T>(T objectBase) where T : IObjectBase;
      string AskForFileToOpen(string title, string filter, string directoryKey);
      string AskForFileToSave(string title, string filter, string directoryKey, string defaultName);
      string AskForFolder(string title, string directoryKey);
      T LoadTransfer<T>(string filePath);
      string PromptForNewName<T>(T objectBase, IEnumerable<string> forbiddenNames) where T : IObjectBase;
   }

   public class InteractionTask : IInteractionTask
   {
      private readonly ISerializationTask _serializationTask;
      private readonly IDialogCreator _dialogCreator;
      private readonly IIconRepository _iconRepository;
      private readonly INameCorrector _nameCorrector;
      private readonly ICloneManagerForBuildingBlock _cloneManagerForBuildingBlock;
      private readonly IAdjustFormulasVisitor _adjustFormulasVisitor;
      private readonly IObjectTypeResolver _objectTypeResolver;
      private readonly IForbiddenNamesRetriever _forbiddenNamesRetriever;

      public InteractionTask(ISerializationTask serializationTask, IDialogCreator dialogCreator, IIconRepository iconRepository,
         INameCorrector nameCorrector, ICloneManagerForBuildingBlock cloneManagerForBuildingBlock,
         IAdjustFormulasVisitor adjustFormulasVisitor, IObjectTypeResolver objectTypeResolver, IForbiddenNamesRetriever forbiddenNamesRetriever)
      {
         _serializationTask = serializationTask;
         _dialogCreator = dialogCreator;
         _iconRepository = iconRepository;
         _nameCorrector = nameCorrector;
         _cloneManagerForBuildingBlock = cloneManagerForBuildingBlock;
         _adjustFormulasVisitor = adjustFormulasVisitor;
         _objectTypeResolver = objectTypeResolver;
         _forbiddenNamesRetriever = forbiddenNamesRetriever;
      }

      public virtual IReadOnlyCollection<T> LoadItems<T>(string filename) where T : class, IObjectBase
      {
         var loadedItems = _serializationTask.LoadMany<T>(filename);
         return loadedItems.Select(Clone).ToList();
      }

      public virtual void Save<T>(IReadOnlyList<T> entitiesToSerialize) where T : IObjectBase
      {
         if (entitiesToSerialize.Count == 1)
         {
            var entityToSerialize = entitiesToSerialize.First();
            var fileName = _dialogCreator.AskForFileToSave(AppConstants.Captions.Save, Constants.Filter.PKML_FILE_FILTER, Constants.DirectoryKey.PROJECT, entityToSerialize.Name);
            if (fileName.IsNullOrEmpty()) return;

            _serializationTask.SaveModelPart(entityToSerialize, fileName);
         }
         else
         {
            saveMultiple(entitiesToSerialize);
         }
      }

      public void Save<T>(T entityToSerialize, string fileName) => _serializationTask.SaveModelPart(entityToSerialize, fileName);

      public virtual T Clone<T>(T objectToClone) where T : class, IObjectBase
      {
         var formulaCache = new FormulaCache();
         var clone = _cloneManagerForBuildingBlock.Clone(objectToClone, formulaCache);

         if (clone is IBuildingBlock cloneBuildingBlock)
            updateFormulaCacheOfClone(objectToClone.DowncastTo<IBuildingBlock>(), cloneBuildingBlock, formulaCache);

         return clone;
      }

      private void updateFormulaCacheOfClone(IBuildingBlock originBuildingBlock, IBuildingBlock cloneBuildingBlock, FormulaCache formulaCache)
      {
         //reset formula cache of cloneManager at that stage
         _cloneManagerForBuildingBlock.FormulaCache = new FormulaCache();

         //  add a clone of each formula that is not present in the clone by name
         originBuildingBlock.FormulaCache
            .Where(f => !formulaCache.ExistsByName(f.Name))
            .Each(f => formulaCache.Add(_cloneManagerForBuildingBlock.Clone(f)));

         // we nee to add the Formulas to the clones of BuildingBlockWithFormulaCaches Formula cache
         formulaCache.Each(cloneBuildingBlock.AddFormula);
      }

      public bool AdjustFormula<T>(T objectBase, IBuildingBlock buildingBlockWithFormulaCache, IMoBiMacroCommand macroCommand) where T : IObjectBase
      {
         var (formulaCommands, canceled) = _adjustFormulasVisitor.AdjustFormulasIn(objectBase, buildingBlockWithFormulaCache);
         if (canceled)
            return false;

         macroCommand.AddRange(formulaCommands);
         return true;
      }

      public string TypeFor<T>(T objectRequestingType) where T : class => _objectTypeResolver.TypeFor(objectRequestingType);

      public IEnumerable<string> ForbiddenNamesFor<T>(T objectBase) where T : IObjectBase => _forbiddenNamesRetriever.For(objectBase);

      public string AskForFileToOpen(string title, string filter, string directoryKey) => _dialogCreator.AskForFileToOpen(title, filter, directoryKey);

      public string AskForFolder(string title, string directoryKey) => _dialogCreator.AskForFolder(title, directoryKey);

      public T LoadTransfer<T>(string filePath) => _serializationTask.Load<T>(filePath);

      public string AskForFileToSave(string title, string filter, string directoryKey, string defaultName) => _dialogCreator.AskForFileToSave(title, filter, directoryKey, defaultName);

      public string IconFor<T>(T entity) where T : IObjectBase => _iconRepository.IconNameFor(entity);

      public bool CorrectName<T>(T objectBase, IEnumerable<string> forbiddenNames) where T : IObjectBase => _nameCorrector.CorrectName(forbiddenNames, objectBase);

      public string PromptForNewName<T>(T objectBase, IEnumerable<string> forbiddenNames) where T : IObjectBase => _nameCorrector.PromptForCorrectName(forbiddenNames.ToList(), objectBase);

      private void saveMultiple<T>(IReadOnlyList<T> entitiesToSerialize)
      {
         var folderNameToSave = _dialogCreator.AskForFolder("Select input folder", Constants.DirectoryKey.REPORT);
         if (string.IsNullOrEmpty(folderNameToSave))
            return;
         foreach (var entity in entitiesToSerialize)
         {
            var fileName = Path.Combine(folderNameToSave, $"{entity.ToString()}{Constants.Filter.PKML_EXTENSION}");
            Save(entity, fileName);
         }
      }
   }
}