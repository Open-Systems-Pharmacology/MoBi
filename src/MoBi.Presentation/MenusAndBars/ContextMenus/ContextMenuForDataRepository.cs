using System.Collections.Generic;
using MoBi.Assets;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Utility.Extensions;
using MoBi.Presentation.DTO;
using MoBi.Presentation.UICommand;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Extensions;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Presentation.Presenters.Nodes;
using OSPSuite.Presentation.UICommands;
using OSPSuite.Assets;
using OSPSuite.Utility.Container;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   internal class ContextMenuSpecificationFactoryForDataRepository : IContextMenuSpecificationFactory<IViewItem>
   {
      private readonly IContainer _container;

      public ContextMenuSpecificationFactoryForDataRepository(IContainer container)
      {
         _container = container;
      }

      public IContextMenu CreateFor(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         var contextMenu = new ContextMenuForDataRepository(_container);
         var repository = dataRepositoryFrom(viewItem);
         return contextMenu.InitializeWith(repository);
      }

      public bool IsSatisfiedBy(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return viewItem.IsAnImplementationOf<ObservedDataNode>() ||
                viewItem.IsAnImplementationOf<ObservedDataViewItem>();
      }

      private DataRepository dataRepositoryFrom(IViewItem viewItem)
      {
         var observedDataViewItem = viewItem.DowncastTo<ObservedDataViewItem>();
         return observedDataViewItem.Repository;
      }
   }

   internal class ContextMenuForDataRepository : ContextMenuBase
   {
      private readonly IContainer _container;

      public ContextMenuForDataRepository(IContainer container)
      {
         _container = container;
      }
      private IList<IMenuBarItem> _allMenuItems;

      public override IEnumerable<IMenuBarItem> AllMenuItems()
      {
         return _allMenuItems;
      }

      public IContextMenu InitializeWith(DataRepository dataRepository)
      {
         _allMenuItems = new List<IMenuBarItem>
         {
            editMenuItemFor(dataRepository),
            renameMenuItemFor(dataRepository),
            createSaveItemFor(dataRepository),
            createReloadItemFor(dataRepository),
            deleteMenuItemFor(dataRepository),
            exportToExcel(dataRepository),
            addToJournalMenuItemFor(dataRepository),
         };

         return this;
      }

      private IMenuBarItem exportToExcel(DataRepository dataRepository)
      {
         return CreateMenuButton.WithCaption(Captions.ExportToExcel.WithEllipsis())
            .WithCommandFor<ExportObservedDataToExcelCommand, DataRepository>(dataRepository, _container)
            .WithIcon(ApplicationIcons.Excel);
      }

      private IMenuBarItem addToJournalMenuItemFor(DataRepository dataRepository)
      {
         return ObjectBaseCommonContextMenuItems.AddToJournal(dataRepository, _container);
      }

     private IMenuBarButton renameMenuItemFor(DataRepository dataRepository)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.Rename)
            .WithIcon(ApplicationIcons.Rename)
            .WithCommandFor<RenameDataRepositoryUICommand, DataRepository>(dataRepository, _container);
      }

      private IMenuBarButton deleteMenuItemFor(DataRepository dataRepository)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.Delete)
            .WithCommandFor<RemoveDataRepositoryUICommand, DataRepository>(dataRepository, _container)
            .WithIcon(ApplicationIcons.Delete);
      }

      private IMenuBarItem createReloadItemFor(DataRepository dataRepository)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.ReloadAll) //ToDo: move to Core, also from PK-Sim
            .WithCommandFor<ReloadAllObservedDataCommand, DataRepository>(dataRepository, _container)
            .AsDisabledIf(string.IsNullOrEmpty(dataRepository.ConfigurationId))
            .WithIcon(ApplicationIcons.Excel);
      }

      private IMenuBarItem createSaveItemFor(DataRepository dataRepository)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.SaveAsPKML)
            .WithIcon(ApplicationIcons.PKMLSave)
            .WithCommandFor<SaveUICommandFor<DataRepository>, DataRepository>(dataRepository, _container);
      }

      private IMenuBarItem editMenuItemFor(DataRepository dataRepository)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.Edit)
            .WithCommandFor<EditObservedDataUICommand, DataRepository>(dataRepository, _container)
            .WithIcon(ApplicationIcons.Edit);
      }
   }
}