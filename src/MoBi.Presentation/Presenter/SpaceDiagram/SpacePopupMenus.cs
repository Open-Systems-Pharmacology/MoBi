using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Presenter.BaseDiagram;
using MoBi.Presentation.UICommand;
using OSPSuite.Assets;
using OSPSuite.Core;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Views.ContextMenus;

namespace MoBi.Presentation.Presenter.SpaceDiagram
{
   public class PopupMenuFullContainerWithParametersNode : PopupMenuFullEntityNode<IContainer>
   {
      public PopupMenuFullContainerWithParametersNode(IMoBiBaseDiagramPresenter presenter, IMoBiContext context, IStartOptions runOptions) : base(presenter, context, runOptions)
      {
      }

      protected override void SetModelMenuItems(IContextMenuView contextMenuView, IContainerBase containerBase, IBaseNode node)
      {
         var container = Get<IContainer>(node.Id);

         contextMenuView.AddMenuItem(CreateMenuButton.WithCaption(AppConstants.MenuNames.AddNew(ObjectTypes.Container))
            .WithCommandFor<AddNewCommandFor<IContainer, IContainer>, IContainer>(container, _context.Container)
            .WithIcon(ApplicationIcons.ContainerAdd));

         contextMenuView.AddMenuItem(CreateMenuButton.WithCaption(AppConstants.MenuNames.AddExisting(ObjectTypes.Container))
            .WithCommandFor<AddExistingCommandFor<IContainer, IContainer>, IContainer>(container, _context.Container)
            .WithIcon(ApplicationIcons.ContainerLoad));

         contextMenuView.AddMenuItem(CreateMenuButton.WithCaption(AppConstants.MenuNames.AddNew(ObjectTypes.Parameter))
            .WithCommandFor<AddNewCommandFor<IContainer, IParameter>, IContainer>(container, _context.Container)
            .WithIcon(ApplicationIcons.Parameters));

         contextMenuView.AddMenuItem(CreateMenuButton.WithCaption(AppConstants.MenuNames.AddExisting(ObjectTypes.Parameter))
            .WithCommandFor<AddExistingCommandFor<IContainer, IParameter>, IContainer>(container, _context.Container)
            .WithIcon(ApplicationIcons.PKMLLoad));

         contextMenuView.AddMenuItem(CreateMenuButton.WithCaption(AppConstants.MenuNames.AddNew(ObjectTypes.DistributedParameter))
            .WithCommandFor<AddNewCommandFor<IContainer, IDistributedParameter>, IContainer>(container, _context.Container)
            .WithIcon(ApplicationIcons.ParameterDistribution));

         base.SetModelMenuItems(contextMenuView, containerBase, node);

         if (container.ParentContainer == null) // container is TopContainer
         {
            contextMenuView.RemoveMenuItem(AppConstants.MenuNames.Delete);
            contextMenuView.AddMenuItem(CreateMenuButton.WithCaption(AppConstants.MenuNames.Delete)
               .WithCommandFor<RemoveTopContainerCommand, IContainer>(Get<IContainer>(node.Id), _context.Container)
               .WithIcon(ApplicationIcons.Delete));
         }
      }

      protected override void SetLayoutMenuItems(IContextMenuView contextMenuView, IContainerBase containerBase)
      {
         AddTemplateLayoutMenuItems(contextMenuView, containerBase);
         AddAutoLayoutMenuItems(contextMenuView, containerBase);
         contextMenuView.AddMenuItem(SubMenuLayout);
      }
   }

   public class PopupMenuSpaceDiagram : PopupMenuFullBaseDiagram
   {
      public PopupMenuSpaceDiagram(IMoBiBaseDiagramPresenter presenter, IMoBiContext context, IStartOptions runOptions) : base(presenter, context, runOptions)
      {
      }

      protected override void SetLayoutMenuItems(IContextMenuView contextMenuView, IContainerBase containerBase)
      {
         AddTemplateLayoutMenuItems(contextMenuView, containerBase);
         AddAutoLayoutMenuItems(contextMenuView, containerBase);
         contextMenuView.AddMenuItem(SubMenuLayout);
      }
   }
}