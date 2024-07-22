using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Tasks.Edit
{
   public interface IEditTaskFor<T>
   {
      void Edit(T objectToEdit);
      void Save(T entityToSerialize);

      /// <summary>
      ///    Gets the forbidden names for the given object, from the local list the objects name is removed.
      ///    This is done to ensure that an already added objects name is not considered as forbidden, for the object itself.
      /// </summary>
      /// <returns> The forbidden names.</returns>
      IEnumerable<string> GetForbiddenNamesWithoutSelf(T objectBase, IEnumerable<IObjectBase> existingObjectsInParent);

      /// <summary>
      ///    Gets the forbidden names here the active objects name maybe forbidden.
      /// </summary>
      /// <returns> The forbidden names.</returns>
      IEnumerable<string> GetForbiddenNames(T objectBase, IEnumerable<IObjectBase> existingObjectsInParent);

      void Rename<TEntity>(TEntity entity, IBuildingBlock buildingBlock) where TEntity : T, IEntity;
      void Rename(T objectToRename, IEnumerable<IObjectBase> existingObjectsInParent, IBuildingBlock buildingBlock);
      string ObjectName { get; }
      bool EditEntityModal(T newEntity, IEnumerable<IObjectBase> existingObjectsInParent, ICommandCollector commandCollector, IBuildingBlock buildingBlock);
      string IconFor(IObjectBase objectBase);
      void SaveMultiple(IReadOnlyList<T> entitiesToSerialize);
   }

   public abstract class EditTaskFor<T> : IEditTaskFor<T> where T : class, IObjectBase
   {
      protected readonly IMoBiContext _context;
      protected readonly IMoBiApplicationController _applicationController;
      protected readonly IInteractionTask _interactionTask;
      protected readonly IInteractionTaskContext _interactionTaskContext;
      private readonly ICheckNameVisitor _checkNamesVisitor;
      public string ObjectName { get; private set; }

      protected EditTaskFor(IInteractionTaskContext interactionTaskContext)
      {
         _interactionTaskContext = interactionTaskContext;
         _checkNamesVisitor = _interactionTaskContext.CheckNamesVisitor;
         _applicationController = interactionTaskContext.ApplicationController;
         _interactionTask = interactionTaskContext.InteractionTask;
         _context = interactionTaskContext.Context;
         ObjectName = _interactionTaskContext.GetTypeFor<T>();
      }

      public virtual void Edit(T objectToEdit)
      {
         _context.PublishEvent(new EntitySelectedEvent(objectToEdit, this));
      }

      public virtual void Rename<TEntity>(TEntity entity, IBuildingBlock buildingBlock) where TEntity : T, IEntity
      {
         Rename(entity, entity.ParentContainer, buildingBlock);
      }

      public virtual void Rename(T objectBase, IEnumerable<IObjectBase> existingObjectsInParent, IBuildingBlock buildingBlock)
      {
         var forbiddenNames = GetForbiddenNames(objectBase, existingObjectsInParent);
         _context.AddToHistory(rename(objectBase, forbiddenNames, buildingBlock));
      }

      protected virtual string NewNameFor(T objectBase, IReadOnlyList<string> prohibitedNames)
      {
         return _interactionTaskContext.NamingTask.RenameFor(objectBase, prohibitedNames);
      }

      private IMoBiCommand rename(T objectBase, IEnumerable<string> forbiddenNames, IBuildingBlock buildingBlock)
      {
         var newName = NewNameFor(objectBase, forbiddenNames.ToList());

         var objectType = _interactionTaskContext.GetTypeFor(objectBase);

         if (string.IsNullOrEmpty(newName))
            return new MoBiEmptyCommand();

         var commandCollector = new MoBiMacroCommand
         {
            CommandType = AppConstants.Commands.RenameCommand,
            ObjectType = objectType,
            Description = AppConstants.Commands.RenameDescription(objectBase, newName)
         };

         if (CheckUsagesFor(newName, objectBase.Name, objectBase, commandCollector, buildingBlock?.Module))
            commandCollector.AddCommand(GetRenameCommandFor(objectBase, buildingBlock, newName, objectType));

         commandCollector.Run(_context);
         return commandCollector;
      }

      protected virtual IMoBiCommand GetRenameCommandFor(T objectBase, IBuildingBlock buildingBlock, string newName, string objectType)
      {
         return new RenameObjectBaseCommand(objectBase, newName, buildingBlock) { ObjectType = objectType };
      }

      public bool CheckUsagesFor(string newName, string oldName, IObjectBase renamedObject, ICommandCollector commandCollector, Module module)
      {
         if (renamedObject.IsAnImplementationOf<IModelCoreSimulation>())
            return changeUsagesInSimulation(newName, renamedObject.DowncastTo<IModelCoreSimulation>(), commandCollector);

         return checkUsagesInModule(newName, renamedObject, commandCollector, oldName, module);
      }

      private bool checkUsagesInModule(string newName, IObjectBase renamedObject, ICommandCollector commandCollector, string oldName, Module module)
      {
         if (module == null)
            return true;

         var possibleChanges = new List<IStringChange>();

         foreach (var buildingBlock in module.BuildingBlocks)
         {
            possibleChanges.AddRange(_checkNamesVisitor.GetPossibleChangesFrom(renamedObject, newName, buildingBlock, oldName));
         }

         if (!possibleChanges.Any())
            return true;

         using (var selectRenamingPresenter = _applicationController.Start<ISelectRenamingPresenter>())
         {
            selectRenamingPresenter.InitializeWith(possibleChanges);

            if (renamedObject.IsAnImplementationOf<IBuildingBlock>())
               selectRenamingPresenter.SetCheckedStateForAll(checkedState: false);

            if (!selectRenamingPresenter.Show())
               return false;

            var commands = selectRenamingPresenter.SelectedCommands();
            commands.Each(commandCollector.AddCommand);
            return true;
         }
      }

      private static bool changeUsagesInSimulation(string newName, IModelCoreSimulation simulation, ICommandCollector commandCollector)
      {
         commandCollector.AddCommand(new RenameObjectBaseCommand(simulation.Model, newName, null));
         commandCollector.AddCommand(new RenameModelCommand(simulation.Model, newName));
         // Don't rename core Container to avoid reference Corruption

         return true;
      }

      public IEnumerable<string> GetForbiddenNamesWithoutSelf(T objectBase, IEnumerable<IObjectBase> existingObjectsInParent)
      {
         var forbiddenNames = GetForbiddenNames(objectBase, existingObjectsInParent).ToList();
         forbiddenNames.Remove(objectBase.Name);
         return forbiddenNames;
      }

      public IEnumerable<string> GetForbiddenNames(T objectBase, IEnumerable<IObjectBase> existingObjectsInParent)
      {
         var unallowedNames = GetUnallowedNames(objectBase, existingObjectsInParent).ToList();
         return unallowedNames.Union(_interactionTask.ForbiddenNamesFor(objectBase));
      }

      public string IconFor(IObjectBase objectBase)
      {
         return _interactionTask.IconFor(objectBase);
      }

      protected virtual IEnumerable<string> GetUnallowedNames(T objectBase, IEnumerable<IObjectBase> existingObjectsInParent)
      {
         return existingObjectsInParent.AllNames();
      }

      public virtual void SaveMultiple(IReadOnlyList<T> entitiesToSerialize)
      {
         _interactionTask.SaveMultiple(entitiesToSerialize);
      }

      public virtual void Save(T entityToSerialize)
      {
         _interactionTask.Save(entityToSerialize);
      }

      public virtual bool EditEntityModal(T entity, IEnumerable<IObjectBase> existingObjectsInParent, ICommandCollector commandCollector, IBuildingBlock buildingBlock)
      {
         using (var modalPresenter = GetCreateViewFor(entity, commandCollector))
         {
            InitializeSubPresenter(modalPresenter.SubPresenter, buildingBlock, entity);
            ((ICreatePresenter<T>)modalPresenter.SubPresenter).Edit(entity, existingObjectsInParent.ToList());
            return modalPresenter.Show();
         }
      }

      protected virtual IModalPresenter GetCreateViewFor(T entity, ICommandCollector command)
      {
         return _applicationController.GetCreateViewFor(entity, command);
      }

      /// <summary>
      ///    Initializes the sub presenter. Should be overridden for special initializations
      /// </summary>
      protected virtual void InitializeSubPresenter(IPresenter subPresenter, IBuildingBlock buildingBlock, T entity)
      {
         if (subPresenter is IPresenterWithBuildingBlock presenterWithFormulaCache)
         {
            presenterWithFormulaCache.BuildingBlock = buildingBlock;
         }
      }
   }
}