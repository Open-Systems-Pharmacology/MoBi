using System.Collections.Generic;
using MoBi.Assets;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.UICommand;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Assets;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public interface IContextMenuFor : IContextMenu
   {
      IContextMenu InitializeWith(IObjectBaseDTO dto, IPresenter presenter);
   }

   public interface IContextMenuFor<TObjectBase> : IContextMenuFor
   {
   }

   public interface IContextMenuForBuildingBlock<TBuildingBlock> : IContextMenuFor<TBuildingBlock> where TBuildingBlock : IBuildingBlock
   {
   }

   public abstract class ContextMenuFor<TObjectBase> : ContextMenuBase, IContextMenuFor<TObjectBase> where TObjectBase : class, IObjectBase
   {
      protected ContextMenuFor(IMoBiContext context, IObjectTypeResolver objectTypeResolver)
      {
         _context = context;
         _objectTypeResolver = objectTypeResolver;
      }

      protected readonly IMoBiContext _context;
      protected IList<IMenuBarItem> _allMenuItems;
      protected IObjectTypeResolver _objectTypeResolver;

      public virtual IContextMenu InitializeWith(IObjectBaseDTO dto, IPresenter presenter)
      {
         try
         {
            var objectBase = _context.Get<TObjectBase>(dto.Id);

            _allMenuItems = new List<IMenuBarItem> {CreateEditItemFor(objectBase)};

            if (dto.Name.IsSpecialName()) return this;

            _allMenuItems.Add(CreateRenameItemFor(objectBase));
            _allMenuItems.Add(createSaveItemFor(objectBase));
            _allMenuItems.Add(CreateDeleteItemFor(objectBase));
            _allMenuItems.Add(CreateReportItemFor(objectBase));
            return this;
         }
         catch (InterfaceResolutionException)
         {
            return new EmptyContextMenu();
         }
      }

      protected abstract IMenuBarItem CreateDeleteItemFor(TObjectBase objectBase);

      private IMenuBarItem createSaveItemFor(TObjectBase objectBase)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.SaveAsPKML)
            .WithIcon(ApplicationIcons.SaveIconFor(typeof(TObjectBase).Name))
            .WithCommandFor<SaveUICommandFor<TObjectBase>, TObjectBase>(objectBase);
      }

      protected IMenuBarItem CreateEditItemFor(TObjectBase objectToEdit)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.Edit)
            .WithCommandFor<EditCommandFor<TObjectBase>, TObjectBase>(objectToEdit)
            .WithIcon(ApplicationIcons.Edit);
      }

      protected IMenuBarItem CreateRenameItemFor(TObjectBase objectToEdit)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.Rename)
            .WithCommandFor<RenameObjectCommand<TObjectBase>, TObjectBase>(objectToEdit)
            .WithIcon(ApplicationIcons.Rename);
      }

      public override IEnumerable<IMenuBarItem> AllMenuItems()
      {
         return _allMenuItems;
      }

      protected virtual IMenuBarItem CreateReportItemFor(TObjectBase objectToExport)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.ExportToPDF)
            .WithCommandFor<ExportToPDFCommand<TObjectBase>, TObjectBase>(objectToExport)
            .WithIcon(ApplicationIcons.ExportToPDF);
      }

      protected string ObjectTypeName
      {
         get { return _objectTypeResolver.TypeFor<TObjectBase>(); }
      }


      protected IMenuBarItem AddToJournal(TObjectBase objectBase)
      {
         return ObjectBaseCommonContextMenuItems.AddToJournal(objectBase);
      }
   }
}