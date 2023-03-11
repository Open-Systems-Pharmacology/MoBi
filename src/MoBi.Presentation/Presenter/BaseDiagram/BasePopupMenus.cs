using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.MenusAndBars;
using MoBi.Presentation.UICommand;
using OSPSuite.Assets;
using OSPSuite.Core;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Views.ContextMenus;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Presenter.BaseDiagram
{
   public class PopupMenuFullEntityNode<T> : DiagramPopupMenuBaseWithContext where T : class, IEntity
   {
      public PopupMenuFullEntityNode(IMoBiBaseDiagramPresenter presenter, IMoBiContext context, IStartOptions runOptions) : base(presenter, context, runOptions)
      {
      }

      protected override void SetModelMenuItems(IContextMenuView contextMenuView, IContainerBase containerBase, IBaseNode node)
      {
         var entity = Get<T>(node.Id);
         contextMenuView.AddMenuItem(CreateMenuButton.WithCaption(AppConstants.MenuNames.Rename)
            .WithCommandFor<RenameObjectCommand<T>, T>(entity, _context.Container).WithIcon(ApplicationIcons.Rename));

         contextMenuView.AddMenuItem(CreateMenuButton.WithCaption(AppConstants.MenuNames.Edit)
            .WithCommandFor<EditCommandFor<T>, T>(entity, _context.Container).WithIcon(ApplicationIcons.Edit));

         contextMenuView.AddMenuItem(CreateMenuButton.WithCaption(AppConstants.MenuNames.SaveAsPKML)
            .WithCommandFor<SaveUICommandFor<T>, T>(entity, _context.Container).WithIcon(ApplicationIcons.SaveIconFor(typeof(T).Name)));

         createRemoveCommandFor(contextMenuView, entity);

         base.SetModelMenuItems(contextMenuView, containerBase, node);
      }

      private void createRemoveCommandFor(IContextMenuView contextMenuView, T entity)
      {
         if (entity.IsAnImplementationOf<IReactionBuilder>())
         {
            contextMenuView.AddMenuItem(CreateMenuButton.WithCaption(AppConstants.MenuNames.Delete)
               .WithRemoveCommand(Presenter.Subject.DowncastTo<IMoBiReactionBuildingBlock>(), entity).WithIcon(ApplicationIcons.Delete));
         }
         else
         {
            contextMenuView.AddMenuItem(CreateMenuButton.WithCaption(AppConstants.MenuNames.Delete)
               .WithRemoveCommand(entity.ParentContainer, entity).WithIcon(ApplicationIcons.Delete));
         }
      }
   }

   public class PopupMenuFullBaseDiagram : DiagramPopupMenuBase
   {
      public PopupMenuFullBaseDiagram(IMoBiBaseDiagramPresenter presenter, IMoBiContext context, IStartOptions runOptions) : base(presenter, context, runOptions)
      {
      }

      protected override void SetModelMenuItems(IContextMenuView contextMenuView, IContainerBase containerBase, IBaseNode node)
      {
         var parent = Presenter.Subject as IMoBiSpatialStructure;
         var menuItem = CreateMenuButton.WithCaption(AppConstants.MenuNames.AddNew("Top Container"))
            .WithCommandFor<AddNewTopContainerCommand, IMoBiSpatialStructure>(parent, _context.Container)
            .WithIcon(ApplicationIcons.ContainerAdd);
         contextMenuView.AddMenuItem(menuItem);

         var menuItem2 = CreateMenuButton.WithCaption(AppConstants.MenuNames.AddExisting("Top Container"))
            .WithCommand<AddExistingTopContainerCommand>(_context.Container)
            .WithIcon(ApplicationIcons.ContainerLoad);
         contextMenuView.AddMenuItem(menuItem2);

         base.SetModelMenuItems(contextMenuView, containerBase, node);
      }
   }
}