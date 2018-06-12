using System.Collections.Generic;

namespace MoBi.Presentation.Tasks
{
   public interface ISerializationTask : OSPSuite.Core.Serialization.ISerializationTask
   {
      void LoadProject(string fileName);
      void SaveProject();
      void CloseProject();
      void NewProject();
      IEnumerable<T> LoadMany<T>(string fileName, bool resetIds = false);
      void LoadJournal(string journalPath, string projectFullPath = null, bool showJournal = false);
   }
}