using System.Collections.Generic;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.UICommand;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public class RootContextMenuForMoleculeBuildingBlock : RootContextMenuFor<Module, MoleculeBuildingBlock>
   {
      public RootContextMenuForMoleculeBuildingBlock(IObjectTypeResolver objectTypeResolver, IMoBiContext context, IContainer container) : base(objectTypeResolver, context, container)
      {
      }

      public override IEnumerable<IMenuBarItem> AllMenuItems()
      {
         // TODO this is not called anywhere since there's no more root node for molecules
         // yield return CreateAddNewItemFor(_context.CurrentProject);
         // yield return CreateAddExistingItemFor(_context.CurrentProject);
         // yield return CreateAddExistingFromTemplateItemFor(_context.CurrentProject);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.NewFromSelection)
            .WithIcon(ApplicationIcons.Molecule)
            .WithCommand<AddNewMoleculeBuildingBlockFromSelectionUICommand>(_container);
      }
   }
}