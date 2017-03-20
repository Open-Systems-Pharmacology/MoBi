using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;

namespace MoBi.Core.Services
{
   public interface INameCorrector
   {
      /// <summary>
      ///    Modifies a name on a named object if its name is already part of the enumeration
      ///    <paramref name="alreadyExistingNamedObjects" />.
      ///    A dialog will be used to ask the user for confirmation of the new name
      /// </summary>
      /// <param name="alreadyExistingNamedObjects">
      ///    The list of objects whose names are exlcuded from consideration for a name
      ///    correction
      /// </param>
      /// <param name="objectForRename">The object whose name is being corrected</param>
      /// <returns>true if a modification was made, otherwise false</returns>
      bool CorrectName<T>(IEnumerable<T> alreadyExistingNamedObjects, T objectForRename) where T : IObjectBase;

      /// <summary>
      ///    Modifies a name on a named object if its name is already part of the enumeration
      ///    <paramref name="alreadyUsedNames" />.
      ///    A dialog will be used to ask the user for confirmation of the new name
      /// </summary>
      /// <param name="alreadyUsedNames">
      ///    The list of names that are already used. These names cannot be considered for a name
      ///    correction
      /// </param>
      /// <param name="objectForRename">The object whose name is being corrected</param>
      /// <returns>true if a modification was made, otherwise false</returns>
      bool CorrectName<T>(IEnumerable<string> alreadyUsedNames, T objectForRename) where T : IObjectBase;

      /// <summary>
      ///    Modifies a name on a named object if its name is already part of the enumeration
      ///    <paramref name="alreadyUsedNames" />.
      ///    User confirmation of name change is not required
      /// </summary>
      /// <param name="alreadyUsedNames">
      ///    The list of names that are already used. These names cannot be considered for a name
      ///    correction
      /// </param>
      /// <param name="objectForRename">The object whose name is being corrected</param>
      void AutoCorrectName<T>(IEnumerable<string> alreadyUsedNames, T objectForRename) where T : IObjectBase;
   }

   internal class NameCorrector : INameCorrector
   {
      private readonly IDialogCreator _dialogCreator;
      private readonly IObjectTypeResolver _objectTypeResolver;
      private readonly IContainerTask _containerTask;

      public NameCorrector(IDialogCreator dialogCreator, IObjectTypeResolver objectTypeResolver, IContainerTask containerTask)
      {
         _dialogCreator = dialogCreator;
         _objectTypeResolver = objectTypeResolver;
         _containerTask = containerTask;
      }

      public bool CorrectName<T>(IEnumerable<T> alreadyExistingNamedObjects, T objectForRename) where T : IObjectBase
      {
         var usedNames = alreadyExistingNamedObjects.Select(x => x.Name).ToList();
         return CorrectName(usedNames, objectForRename);
      }

      public bool CorrectName<T>(IEnumerable<string> alreadyUsedNames, T objectForRename) where T : IObjectBase
      {
         var usedNames = alreadyUsedNames.ToList();
         var oldName = objectForRename.Name;
         var newName = oldName;
         var typeName = _objectTypeResolver.TypeFor<T>();
         usedNames.AddRange(AppConstants.UnallowedNames);

         if (usedNames.Contains(oldName))
         {
            newName = _dialogCreator.AskForInput(AppConstants.Dialog.AskForChangedName(oldName, typeName),
               AppConstants.Captions.NameInUse, getNextSuggestedName(usedNames, oldName), usedNames);
         }
         //Rename was canceled
         if (newName.IsNullOrEmpty())
            return false;

         objectForRename.Name = newName;
         return true;
      }

      public void AutoCorrectName<T>(IEnumerable<string> alreadyUsedNames, T objectForRename) where T : IObjectBase
      {
         var oldName = objectForRename.Name;
         var updatedName = getNextSuggestedName(alreadyUsedNames, oldName);

         objectForRename.Name = updatedName;
      }

      private string getNextSuggestedName(IEnumerable<string> usedNames, string oldName)
      {
         return _containerTask.CreateUniqueName(usedNames.ToList(), oldName);
      }
   }
}