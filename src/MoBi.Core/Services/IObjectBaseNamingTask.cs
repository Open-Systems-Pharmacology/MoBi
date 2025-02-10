using OSPSuite.Core.Domain;
using System.Collections.Generic;

namespace MoBi.Core.Services
{
   public interface IObjectBaseNamingTask
   {

      /// <summary>
      /// Collects a new name for an object for renaming. The distinction between naming and renaming is that
      /// When renaming, you can use a case-only version of the original name even if the name is found in the <paramref name="forbiddenNames"/>
      /// </summary>
      /// <param name="objectToRename">The original object name can be reused if only casing is changed</param>
      /// <param name="forbiddenNames">Names from this list cannot be reused</param>
      /// <returns>Empty string if canceled, otherwise a new name</returns>
      string RenameFor(IObjectBase objectToRename, IReadOnlyList<string> forbiddenNames);

      /// <summary>
      /// Collects a new name for an object. No names from <paramref name="forbiddenValues"/> can be used even with case changes
      /// </summary>
      /// <returns>Empty string if canceled, otherwise a new name</returns>
      string NewName(
         string prompt,
         string title,
         string defaultValue = null,
         IEnumerable<string> forbiddenValues = null,
         IEnumerable<string> predefinedValues = null,
         string iconName = null);
   }
}
