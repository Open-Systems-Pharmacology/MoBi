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
   internal class ContextMenuForEductBuilder : ContextMenuBase
   {
      private readonly IReactionBuilder _reactionBuilder;
      private readonly IReactionPartnerBuilder _reactionPartnerBuilder;
      private readonly IContainer _container;

      public ContextMenuForEductBuilder(IReactionBuilder reactionBuilder, IReactionPartnerBuilder reactionPartnerBuilder, IContainer container)
      {
         _reactionBuilder = reactionBuilder;
         _reactionPartnerBuilder = reactionPartnerBuilder;
         _container = container;
      }

      public override IEnumerable<IMenuBarItem> AllMenuItems()
      {
         if (_reactionPartnerBuilder != null)
            yield return createRemoveItem(_reactionBuilder, _reactionPartnerBuilder);

         yield return createAddItem(_reactionBuilder);
      }

      private IMenuBarItem createAddItem(IReactionBuilder reactionBuilder)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.New.WithEllipsis())
            .WithIcon(ApplicationIcons.Create).WithCommandFor<AddEductUICommand, IReactionBuilder>(reactionBuilder, _container);
      }

      private IMenuBarItem createRemoveItem(IReactionBuilder reactionBuilder, IReactionPartnerBuilder reactionPartnerBuilder)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.Delete)
            .WithIcon(ApplicationIcons.Delete)
            .WithCommand(IoC.Resolve<RemoveEductUICommand>().Initialize(reactionPartnerBuilder, reactionBuilder));
      }
   }
}