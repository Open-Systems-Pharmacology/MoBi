using System.Collections.Generic;
using MoBi.Core.Domain.Model;

namespace MoBi.Presentation.Tasks
{
   public interface ISerializationTask : OSPSuite.Core.Serialization.ISerializationTask
   {
      void LoadProject(string fileName);
      void SaveProject();
      void CloseProject();
      MoBiProject NewProject();

      void LoadJournal(string journalPath, string projectFullPath = null, bool showJournal = false);

      /// <summary>
      ///    Loads multiple instances of the element type <typeparamref name="T" /> from the <paramref name="fileName" />. If
      ///    more than one element
      ///    of the type is present in the file, the user will confirm which elements to load by name. If
      ///    <paramref name="resetIds" /> is true, then all element ID's will
      ///    be set to new values on load.
      /// </summary>
      /// <returns>A new list of elements that were loaded</returns>
      IReadOnlyList<T> LoadMany<T>(string fileName, bool resetIds = false);

      /// <summary>
      ///    Loads all instances of the element type <typeparamref name="T" /> from the <paramref name="fileName" />. If
      ///    <paramref name="resetIds" /> is true, then all element ID's will
      ///    be set to new values on load.
      /// </summary>
      /// <returns>A new list of elements that were loaded</returns>
      IReadOnlyList<T> LoadAll<T>(string fileName, bool resetIds = false);
   }
}