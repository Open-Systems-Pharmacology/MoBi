using System.Collections.Generic;
using MoBi.Assets;
using OSPSuite.Presentation.MenuAndBars;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.UICommand;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Assets;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public class RootContextMenuForMoleculeBuildingBlock : RootContextMenuFor<IMoBiProject, IMoleculeBuildingBlock>
   {
      public RootContextMenuForMoleculeBuildingBlock(IObjectTypeResolver objectTypeResolver, IMoBiContext context) : base(objectTypeResolver, context)
      {
      }

      public override IEnumerable<IMenuBarItem> AllMenuItems()
      {
         yield return CreateAddNewItemFor(_context.CurrentProject);
         yield return CreateAddExistingItemFor(_context.CurrentProject);
         yield return CreateAddExistingFromTemplateItemFor(_context.CurrentProject);
         yield return CreateReportItemForCollection();

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.NewFromSelection)
            .WithIcon(ApplicationIcons.Molecule)
            .WithCommand<AddNewMoleculeBuildingBlockFromSelectionUICommand>();
      }
   }
}