using System.Collections.Generic;
using MoBi.Assets;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Utility.Container;
using MoBi.Presentation.UICommand;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Extensions;
using OSPSuite.Presentation.Core;
using OSPSuite.Assets;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public class ContextMenuForModifierBuilder : ContextMenuBase
   {
      private readonly string _reactionModifier;
      private readonly IReactionBuilder _reactionBuilder;

      public ContextMenuForModifierBuilder(IReactionBuilder reactionBuilder, string reactionModifier)
      {
         _reactionModifier = reactionModifier;
         _reactionBuilder = reactionBuilder;
      }

      public override IEnumerable<IMenuBarItem> AllMenuItems()
      {
         if (!string.IsNullOrEmpty(_reactionModifier))
            yield return createRemoveItem(_reactionBuilder, _reactionModifier);

         yield return createAddItem(_reactionBuilder);
      }

      private IMenuBarItem createAddItem(IReactionBuilder reactionBuilder)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.New.WithEllipsis())
            .WithIcon(ApplicationIcons.Add)
            .WithCommandFor<AddModifierUICommand, IReactionBuilder>(reactionBuilder);
      }

      private IMenuBarItem createRemoveItem(IReactionBuilder reactionBuilder, string reactionPartnerBuilder)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.Delete)
            .WithIcon(ApplicationIcons.Delete)
            .WithCommand(IoC.Resolve<RemoveModifierUICommand>().Initialize(reactionPartnerBuilder, reactionBuilder));
      }
   }
}