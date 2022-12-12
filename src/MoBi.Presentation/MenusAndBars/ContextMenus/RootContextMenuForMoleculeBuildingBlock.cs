using System.Collections.Generic;
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

      public override IEnumerable<IMenuBarItem> AllMenuItems()
      {
         var newExpressionProfile = CreateSubMenu.WithCaption(MenuNames.NewExpressionProfile)
            .WithIcon(ApplicationIcons.ExpressionProfile)
            .WithDescription(MenuDescriptions.NewExpressionProfileDescription);

         var newEnzyme = CreateMenuButton.WithCaption(AppConstants.Captions.AddMetabolizingEnzyme)
            .WithCommand<AddNewMetabolizingEnzymeBuildingBlock>()
            .WithIcon(ApplicationIcons.Enzyme);

         var newTransporter = CreateMenuButton.WithCaption(AppConstants.Captions.AddTransportProtein)
            .WithCommand<AddNewIndividualTransporterBuildingBlock>()
            .WithIcon(ApplicationIcons.Transporter);

         var newSpecificBinding = CreateMenuButton.WithCaption(AppConstants.Captions.AddSpecificBindingPartner)
            .WithCommand<AddNewBindingPartnerBuildingBlock>()
            .WithIcon(ApplicationIcons.SpecificBinding);

         newExpressionProfile.AddItem(newEnzyme);
         newExpressionProfile.AddItem(newTransporter);
         newExpressionProfile.AddItem(newSpecificBinding);

         yield return newExpressionProfile;
      }
   }

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

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.NewFromSelection)
            .WithIcon(ApplicationIcons.Molecule)
            .WithCommand<AddNewMoleculeBuildingBlockFromSelectionUICommand>();
      }
   }
}