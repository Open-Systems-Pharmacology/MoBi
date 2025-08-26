using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.UICommand;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public class MultipleModuleContextMenuFactory : MultipleNodeContextMenuFactory<ClassifiableModule>
   {
      private readonly IMoBiContext _context;
      private readonly IContainer _container;

      public MultipleModuleContextMenuFactory(IMoBiContext context, IContainer container)
      {
         _context = context;
         _container = container;
      }

      protected override IContextMenu CreateFor(IReadOnlyList<ClassifiableModule> modules, IPresenterWithContextMenu<IReadOnlyList<ITreeNode>> presenter)
      {
         return new MultipleModuleContextMenu(modules.Select(x => x.Subject).ToList(), _context, _container);
      }
   }

   public class MultipleModuleContextMenu : ContextMenu<IReadOnlyList<Module>, IMoBiContext>
   {
      public MultipleModuleContextMenu(IReadOnlyList<Module> modules, IMoBiContext context, IContainer container)
         : base(modules, context, container)
      {
      }

      protected override IEnumerable<IMenuBarItem> AllMenuItemsFor(IReadOnlyList<Module> modules, IMoBiContext context)
      {
         var menuItems = new List<IMenuBarItem>
         {
            CreateMenuButton.WithCaption(AppConstants.MenuNames.Delete)
               .WithCommandFor<RemoveMultipleModulesUICommand, IReadOnlyList<Module>>(modules, _container)
               .WithIcon(ApplicationIcons.Delete),
            CreateMenuButton.WithCaption(AppConstants.MenuNames.SaveAsPKML)
               .WithCommandFor<SaveUICommandForMultiple<Module>, IReadOnlyList<Module>>(modules, _container)
               .WithIcon(ApplicationIcons.SaveIconFor(nameof(Module)))
         };

         return menuItems;
      }
   }
}