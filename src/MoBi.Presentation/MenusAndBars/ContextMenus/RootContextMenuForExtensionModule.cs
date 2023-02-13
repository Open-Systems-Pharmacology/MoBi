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

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public class RootContextMenuForExtensionModule : RootContextMenuFor<IMoBiProject, Module>
   {
      public RootContextMenuForExtensionModule(IObjectTypeResolver objectTypeResolver, IMoBiContext context) : base(objectTypeResolver, context)
      {
      }

      private IMenuBarItem addNewWithContent(IMoBiProject project)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.AddModuleWithBuildingBlocks())
            .WithIcon(ApplicationIcons.AddIconFor(nameof(Module)))
            .WithCommandFor<NewModuleWithBuildingBlocksUICommand, IMoBiProject>(project);
      }

      private IMenuBarItem addNewEmpty(IMoBiProject project)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.AddEmptyModule())
            .WithIcon(ApplicationIcons.AddIconFor(nameof(Module)))
            .WithCommandFor<NewEmptyModuleUICommand, IMoBiProject>(project);
      }

      public override IContextMenu InitializeWith(IPresenter presenter)
      {
         _allMenuItems.Add(addNewEmpty(_context.CurrentProject));
         _allMenuItems.Add(addNewWithContent(_context.CurrentProject));
         _allMenuItems.Add(loadExisting(_context.CurrentProject));

         return this;
      }

      private IMenuBarItem loadExisting(IMoBiProject project)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.AddExisting(ObjectTypeName))
            .WithIcon(ApplicationIcons.LoadIconFor(nameof(Module)))
            .WithCommandFor<AddExistingCommandFor<IMoBiProject, Module>, IMoBiProject>(project);
      }
   }
}