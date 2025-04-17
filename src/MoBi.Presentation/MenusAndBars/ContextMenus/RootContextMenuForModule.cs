using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.UICommand;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Extensions;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Presentation.Presenters.Nodes;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public class RootContextMenuForModule : RootContextMenuFor<MoBiProject, Module>
   {
      public RootContextMenuForModule(IObjectTypeResolver objectTypeResolver, IMoBiContext context, IContainer container) : base(objectTypeResolver, context, container)
      {
      }

      private IMenuBarItem addNewWithContent()
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.AddModuleWithBuildingBlocks.WithEllipsis())
            .WithIcon(ApplicationIcons.AddIconFor(nameof(Module)))
            .WithCommand<NewModuleWithBuildingBlocksUICommand>(_container);
      }

      public override IContextMenu InitializeWith(RootNodeType rootNodeType, IExplorerPresenter presenter)
      {
         var moduleFolderNode = presenter.NodeByType(rootNodeType);
         _allMenuItems.Add(addNewWithContent());
         _allMenuItems.Add(loadExisting());
         _allMenuItems.Add(ClassificationCommonContextMenuItems.CreateClassificationUnderMenu(moduleFolderNode, presenter).AsGroupStarter());
         return this;
      }

      private IMenuBarItem loadExisting()
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.AddExisting(ObjectTypeName))
            .WithIcon(ApplicationIcons.LoadIconFor(nameof(Module)))
            .WithCommand<LoadModuleUICommand>(_container);
      }
   }
}