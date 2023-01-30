using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.UICommand;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public class RootContextMenuForIndividualBuildingBlock : RootContextMenuFor<IMoBiProject, IndividualBuildingBlock>
   {
      public RootContextMenuForIndividualBuildingBlock(IObjectTypeResolver objectTypeResolver, IMoBiContext context) : base(objectTypeResolver,
         context)
      {
      }

      protected override void CreateAddItems(IMoBiProject parent)
      {
         _allMenuItems.Add(createAddNewIndividual());

         _allMenuItems.Add(CreateAddExistingItemFor(parent));
      }

      private static IMenuBarButton createAddNewIndividual()
      {
         return CreateMenuButton.WithCaption(AppConstants.Captions.AddIndividual)
            .WithCommand<AddNewIndividualCommand>()
            .WithIcon(ApplicationIcons.Individual);
      }
   }
}