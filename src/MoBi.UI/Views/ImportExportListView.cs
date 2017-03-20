using System;
using System.Collections.Generic;
using DevExpress.XtraBars;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;

namespace MoBi.UI.Views
{
   public class ImportExportListView : BarSubItem, IImportExportListView
   {
      private IEnumerable<BarItem> _exportItems;
      private IEnumerable<BarItem> _importItems;
      private IEnumerable<BarItem> _loadItems;
      private IImportExportPresenter _presenter;

      public ImportExportListView()
      {
         Caption = "Import/Export";
      }

      public void AttachPresenter(IImportExportPresenter presenter)
      {
         _presenter = presenter;
         _importItems = AddItems("Import", name => _presenter.Import(name), presenter.ImporterNames, presenter.ImportEnabled);
         _loadItems = AddItems("Load", name => _presenter.Load(name), presenter.ImporterNames, true);
         _exportItems = AddItems("Export", name => _presenter.Export(name), presenter.ExporterNames, presenter.ExportEnabled);
      }

      public void EnableDisable(bool enable)
      {
         /*nothing to do here*/
      }

      private BarButtonItem AddItem(string itemCaption, Action clickAction, bool enabled)
      {
         var item = new BarButtonItem();
         item.Caption = itemCaption;
         item.ItemClick += delegate
         {
            clickAction();
         };
         item.Enabled = enabled;
         AddItem(item);
         return item;
      }

      private IEnumerable<BarItem> AddItems(string actionName, Action<string> clickAction, IEnumerable<string> names, bool enabled)
      {
         var list = new List<BarItem>();
         foreach (var name in names)
         {
            list.Add(AddItem(actionName + " " + name, () => clickAction(name), enabled));
         }
         return list;
      }
   }
}