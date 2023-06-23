using System.Linq;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.UI.Diagram.Elements;

namespace MoBi.UI.UICommands
{
   internal class AddNamedPartnerUICommand : IUICommand
   {
      private readonly ReactionLinkType _linkType;
      private readonly IMoBiContext _context;
      private readonly MoBiReactionBuildingBlock _reactionBuildingBlock;
      private readonly ReactionBuilder _reactionBuilder;
      private readonly string _moleculeName;

      public AddNamedPartnerUICommand(IMoBiContext context, MoBiReactionBuildingBlock reactionBuildingBlock, ReactionBuilder reactionBuilder, string moleculeName, ReactionLinkType linkType)
      {
         _context = context;
         _reactionBuildingBlock = reactionBuildingBlock;
         _reactionBuilder = reactionBuilder;
         _moleculeName = moleculeName;
         _linkType = linkType;
      }


      public void Execute()
      {
         IMoBiCommand command = null;

         switch (_linkType)
         {
            case ReactionLinkType.Educt:
               // to avoid adding duplicate entries
               if (_reactionBuilder.Educts.All(rpb => rpb.MoleculeName != _moleculeName))
                  command = new AddReactionPartnerToEductCollection(_reactionBuildingBlock, new ReactionPartnerBuilder(_moleculeName, 1.0), _reactionBuilder);
               break;
            case ReactionLinkType.Product:
               if (_reactionBuilder.Products.All(rpb => rpb.MoleculeName != _moleculeName))
                  command = new AddReactionPartnerToProductCollection(_reactionBuildingBlock, new ReactionPartnerBuilder(_moleculeName, 1.0), _reactionBuilder);
               break;
            case ReactionLinkType.Modifier:
               if (!_reactionBuilder.ModifierNames.Contains(_moleculeName))
                  command = new AddItemToModifierCollectionCommand(_reactionBuildingBlock, _moleculeName, _reactionBuilder);
               break;
         }
         if (command == null) return;
         _context.AddToHistory(command.Run(_context));
      }
   }

   internal class RemoveNamedPartnerUICommand : IUICommand
   {
      private readonly ReactionLinkType _linkType;
      private readonly IMoBiContext _context;
      private readonly MoBiReactionBuildingBlock _reactionBuildingBlock;
      private readonly ReactionBuilder _reactionBuilder;
      private readonly string _moleculeName;

      public RemoveNamedPartnerUICommand(IMoBiContext context, MoBiReactionBuildingBlock reactionBuildingBlock, ReactionBuilder reactionBuilder, string moleculeName, ReactionLinkType linkType)
      {
         _context = context;
         _reactionBuildingBlock = reactionBuildingBlock;
         _reactionBuilder = reactionBuilder;
         _moleculeName = moleculeName;
         _linkType = linkType;
      }

      public void Execute()
      {
         ReactionPartnerBuilder reactionPartnerBuilder;
         IMoBiCommand command = null;
         switch (_linkType)
         {
            case ReactionLinkType.Educt:
               reactionPartnerBuilder = _reactionBuilder.Educts.Single(educt => educt.MoleculeName.Equals(_moleculeName));
               command = new RemoveReactionPartnerFromEductCollection(_reactionBuilder, reactionPartnerBuilder, _reactionBuildingBlock);
               break;
            case ReactionLinkType.Product:
               reactionPartnerBuilder = _reactionBuilder.Products.Single(product => product.MoleculeName.Equals(_moleculeName));
               command = new RemoveReactionPartnerFromProductCollection(_reactionBuilder, reactionPartnerBuilder, _reactionBuildingBlock);
               break;
            case ReactionLinkType.Modifier:
               string modifierName = _reactionBuilder.ModifierNames.Single(name => name.Equals(_moleculeName));
               command = new RemoveItemFromModifierCollectionCommand(_reactionBuilder, modifierName, _reactionBuildingBlock);
               break;
         }

         if (command == null) return;
         _context.AddToHistory(command.Run(_context));
      }
   }
}
