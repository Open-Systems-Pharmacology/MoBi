using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Commands;
using MoBi.Core.Domain;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Presentation.Presenter;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;

namespace MoBi.Presentation.Tasks.Interaction
{
   public interface IObjectBaseTask
   {
      IMoBiCommand Rename(IObjectBase objectBase, IEnumerable<string> allreadyNames, IBuildingBlock buildingBlock);

      /// <summary>
      ///    Search for usages of <paramref name="oldName" /> in the project and asks the user if they should be renamed as well
      /// </summary>
      /// <param name="newName">Name that will be used to replace usage of <paramref name="oldName" /></param>
      /// <param name="oldName">
      ///    Typically the name of <paramref name="renamedObject" />. Can also be another property of
      ///    <paramref name="renamedObject" />. That's why
      ///    we are not simply using <paramref name="renamedObject" />.Name
      /// </param>
      /// <param name="renamedObject">The object being renamed</param>
      /// <param name="commandCollector">Command collector</param>
      /// <returns></returns>
      bool CheckUsagesFor(string newName, string oldName, IObjectBase renamedObject, ICommandCollector commandCollector);
   }

   internal class ObjectBaseTask : IObjectBaseTask
   {
      private readonly IObjectTypeResolver _objectTypeResolver;
      private readonly IMoBiContext _context;
      private readonly ICheckNameVisitor _checkNamesVisitor;
      private readonly IMoBiApplicationController _applicationController;
      private readonly IDialogCreator _dialogCreator;

      public ObjectBaseTask(IObjectTypeResolver objectTypeResolver, IMoBiContext context, ICheckNameVisitor checkNamesVisitor, IMoBiApplicationController applicationController, IDialogCreator dialogCreator)
      {
         _objectTypeResolver = objectTypeResolver;
         _context = context;
         _checkNamesVisitor = checkNamesVisitor;
         _applicationController = applicationController;
         _dialogCreator = dialogCreator;
      }

      public IMoBiCommand Rename(IObjectBase objectBase, IEnumerable<string> allreadyNames, IBuildingBlock buildingBlock)
      {
         var unallowedNames = new List<string>(allreadyNames);
         unallowedNames.AddRange(AppConstants.UnallowedNames);
         var objectName = _objectTypeResolver.TypeFor(objectBase);
         string newName = _dialogCreator.AskForInput(AppConstants.Dialog.AskForNewName(objectBase.Name), AppConstants.Captions.NewName, objectBase.Name, unallowedNames);

         if (string.IsNullOrEmpty(newName))
            return new MoBiEmptyCommand();


         var commandCollector = new MoBiMacroCommand
         {
            CommandType = AppConstants.Commands.RenameCommand,
            ObjectType = objectName,
            Description = AppConstants.Commands.RenameDescription(objectBase, newName)
         };

         if (CheckUsagesFor(newName, objectBase.Name, objectBase, commandCollector))
            commandCollector.AddCommand(new RenameObjectBaseCommand(objectBase, newName, buildingBlock) {ObjectType = objectName});

         commandCollector.Run(_context);
         return commandCollector;
      }

      public bool CheckUsagesFor(string newName, string oldName, IObjectBase renamedObject, ICommandCollector commandCollector)
      {
         if (renamedObject.IsAnImplementationOf<IModelCoreSimulation>())
            return changeUsagesInSumulation(newName, renamedObject.DowncastTo<IModelCoreSimulation>(), commandCollector);

         return checkUsagesInBuildingBlocks(newName, renamedObject, commandCollector, oldName);
      }

      private bool checkUsagesInBuildingBlocks(string newName, IObjectBase renamedObject, ICommandCollector commandCollector, string oldName)
      {
         var buildingBlocks = _context.CurrentProject.AllBuildingBlocks();
         var possibleChangings = new List<IStringChange>();

         foreach (var buildingBlock in buildingBlocks)
         {
            possibleChangings.AddRange(_checkNamesVisitor.GetPossibleChangesFrom(renamedObject, newName, buildingBlock, oldName));
         }

         if (!possibleChangings.Any())
            return true;

         using (var selectRenamingsPresenter = _applicationController.Start<ISelectRenamingPresenter>())
         {
            selectRenamingsPresenter.InitializeWith(possibleChangings);

            if (renamedObject.IsAnImplementationOf<IBuildingBlock>())
               selectRenamingsPresenter.SetCheckedStateForAll(checkedState: false);

            if (!selectRenamingsPresenter.SelectRenamings())
               return false;

            var commands = selectRenamingsPresenter.SelectedCommands();
            commands.Each(commandCollector.AddCommand);
            return true;
         }
      }

      private static bool changeUsagesInSumulation(string newName, IModelCoreSimulation simulation, ICommandCollector commandCollector)
      {
         commandCollector.AddCommand(new RenameObjectBaseCommand(simulation.Model, newName, null));
         commandCollector.AddCommand(new RenameModelCommand(simulation.Model, newName));
         // Dont rename core Container to avoid refernce Corruption

         return true;
      }
   }
}