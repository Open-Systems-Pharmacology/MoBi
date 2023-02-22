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
   public class RootContextMenuForExpressionProfileBuildingBlock : RootContextMenuFor<IMoBiProject, ExpressionProfileBuildingBlock>
   {
      public RootContextMenuForExpressionProfileBuildingBlock(IObjectTypeResolver objectTypeResolver, IMoBiContext context, IContainer container) : base(objectTypeResolver, context, container)
      {
      }

      protected override void CreateAddItems(IMoBiProject parent)
      {
         var newExpressionProfile = CreateSubMenu.WithCaption(MenuNames.NewExpressionProfile)
            .WithIcon(ApplicationIcons.ExpressionProfile)
            .WithDescription(MenuDescriptions.NewExpressionProfileDescription);

         var newEnzyme = CreateMenuButton.WithCaption(AppConstants.Captions.AddMetabolizingEnzyme)
            .WithCommandFor<AddExpressionProfileBuildingBlock, ExpressionType>(ExpressionTypes.MetabolizingEnzyme, _container)
            .WithIcon(ApplicationIcons.Enzyme);

         var newTransporter = CreateMenuButton.WithCaption(AppConstants.Captions.AddTransportProtein)
            .WithCommandFor<AddExpressionProfileBuildingBlock, ExpressionType>(ExpressionTypes.TransportProtein, _container)
            .WithIcon(ApplicationIcons.Transporter);

         var newSpecificBinding = CreateMenuButton.WithCaption(AppConstants.Captions.AddSpecificBindingPartner)
            .WithCommandFor<AddExpressionProfileBuildingBlock, ExpressionType>(ExpressionTypes.ProteinBindingPartner, _container)
            .WithIcon(ApplicationIcons.SpecificBinding);

         newExpressionProfile.AddItem(newEnzyme);
         newExpressionProfile.AddItem(newTransporter);
         newExpressionProfile.AddItem(newSpecificBinding);

         _allMenuItems.Add(newExpressionProfile);
         _allMenuItems.Add(CreateAddExistingItemFor(parent));
      }
   }
}