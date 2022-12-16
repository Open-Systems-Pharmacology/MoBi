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
   public class RootContextMenuForExpressionProfileBuildingBlock : RootContextMenuFor<IMoBiProject, ExpressionProfileBuildingBlock>
   {
      public RootContextMenuForExpressionProfileBuildingBlock(IObjectTypeResolver objectTypeResolver, IMoBiContext context) : base(objectTypeResolver, context)
      {
      }

      protected override void CreateAddItems(IMoBiProject parent)
      {
         var newExpressionProfile = CreateSubMenu.WithCaption(MenuNames.NewExpressionProfile)
            .WithIcon(ApplicationIcons.ExpressionProfile)
            .WithDescription(MenuDescriptions.NewExpressionProfileDescription);

         var newEnzyme = CreateMenuButton.WithCaption(AppConstants.Captions.AddMetabolizingEnzyme)
            .WithCommandFor<AddExpressionProfileBuildingBlock, ExpressionType>(ExpressionTypes.MetabolizingEnzyme)
            .WithIcon(ApplicationIcons.Enzyme);

         var newTransporter = CreateMenuButton.WithCaption(AppConstants.Captions.AddTransportProtein)
            .WithCommandFor<AddExpressionProfileBuildingBlock, ExpressionType>(ExpressionTypes.TransportProtein)
            .WithIcon(ApplicationIcons.Transporter);

         var newSpecificBinding = CreateMenuButton.WithCaption(AppConstants.Captions.AddSpecificBindingPartner)
            .WithCommandFor<AddExpressionProfileBuildingBlock, ExpressionType>(ExpressionTypes.ProteinBindingPartner)
            .WithIcon(ApplicationIcons.SpecificBinding);

         newExpressionProfile.AddItem(newEnzyme);
         newExpressionProfile.AddItem(newTransporter);
         newExpressionProfile.AddItem(newSpecificBinding);

         _allMenuItems.Add(newExpressionProfile);

         _allMenuItems.Add(CreateAddExistingItemFor(parent));
      }
   }
}