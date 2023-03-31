using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.UICommand;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public class RootContextMenuForExtensionModule : RootContextMenuFor<MoBiProject, Module>
   {
      public RootContextMenuForExtensionModule(IObjectTypeResolver objectTypeResolver, IMoBiContext context, IContainer container) : base(objectTypeResolver, context, container)
      {
      }

      private IMenuBarItem addNewWithContent(MoBiProject project)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.AddModuleWithBuildingBlocks)
            .WithIcon(ApplicationIcons.AddIconFor(nameof(Module)))
            .WithCommandFor<NewModuleWithBuildingBlocksUICommand, MoBiProject>(project, _container);
      }

      private IMenuBarItem addNewEmpty(MoBiProject project)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.AddEmptyModule)
            .WithIcon(ApplicationIcons.AddIconFor(nameof(Module)))
            .WithCommandFor<NewEmptyModuleUICommand, MoBiProject>(project, _container);
      }

      public override IContextMenu InitializeWith(IPresenter presenter)
      {
         _allMenuItems.Add(addNewEmpty(_context.CurrentProject));
         _allMenuItems.Add(addNewWithContent(_context.CurrentProject));
         _allMenuItems.Add(loadExisting(_context.CurrentProject));

         return this;
      }

      private IMenuBarItem loadExisting(MoBiProject project)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.AddExisting(ObjectTypeName))
            .WithIcon(ApplicationIcons.LoadIconFor(nameof(Module)))
            .WithCommandFor<AddExistingCommandFor<MoBiProject, Module>, MoBiProject>(project, _container);
      }
   }
}