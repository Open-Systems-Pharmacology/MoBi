using System.Collections.Generic;

namespace MoBi.Presentation.Presenter
{
   public interface IImportExportPresenter
   {
      IEnumerable<string> ImporterNames { get; }
      IEnumerable<string> ExporterNames { get; }
      bool ImportEnabled { get; }
      bool ExportEnabled { get; }
      void Import(string importKey);
      void Export(string exportKey);
      void Load(string importKey);
   }
}