using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.UICommand;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Utility.Container;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public class RootContextMenuForIndividualBuildingBlock : RootContextMenuFor<MoBiProject, IndividualBuildingBlock>
   {
      public RootContextMenuForIndividualBuildingBlock(IObjectTypeResolver objectTypeResolver, IMoBiContext context, IContainer container) : base(objectTypeResolver,
         context, container)
      {
      }

      protected override void CreateAddItems(MoBiProject parent)
      {
         _allMenuItems.Add(createAddNewIndividual());
         _allMenuItems.Add(CreateAddExistingItemFor(parent));
      }

      private IMenuBarButton createAddNewIndividual()
      {
         return CreateMenuButton.WithCaption(AppConstants.Captions.AddIndividual)
            .WithCommand<AddNewIndividualCommand>(_container)
            .WithIcon(ApplicationIcons.Individual);
      }
   }
}