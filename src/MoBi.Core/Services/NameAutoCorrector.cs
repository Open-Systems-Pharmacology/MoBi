using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Core.Services;

public interface INameAutoCorrector
{
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

internal class NameAutoCorrector : INameAutoCorrector
{
   protected readonly IContainerTask _containerTask;

   public NameAutoCorrector(IContainerTask containerTask)
   {
      _containerTask = containerTask;
   }

   public void AutoCorrectName<T>(IEnumerable<string> alreadyUsedNames, T objectForRename) where T : IObjectBase
   {
      var oldName = objectForRename.Name;
      var updatedName = GetNextSuggestedName(alreadyUsedNames, oldName, canUseBaseName: true);

      objectForRename.Name = updatedName;
   }

   protected string GetNextSuggestedName(IEnumerable<string> usedNames, string oldName, bool canUseBaseName = false) => _containerTask.CreateUniqueName(usedNames.ToList(), oldName, canUseBaseName);
}