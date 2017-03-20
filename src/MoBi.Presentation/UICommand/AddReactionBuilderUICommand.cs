using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public abstract class AddReactionBuilderUICommand : ObjectUICommand<IReactionBuilder>
   {
      protected readonly IMoBiContext _context;
      protected readonly IActiveSubjectRetriever _activeSubjectRetriever;
      protected readonly IInteractionTasksForReactionBuilder _reactionTask;

      protected AddReactionBuilderUICommand(IMoBiContext context, IActiveSubjectRetriever activeSubjectRetriever, IInteractionTasksForReactionBuilder reactionTask)
      {
         _context = context;
         _activeSubjectRetriever = activeSubjectRetriever;
         _reactionTask = reactionTask;
      }

      protected IMoBiReactionBuildingBlock RetrieveActiveSubject()
      {
         var buildingBlock = _activeSubjectRetriever.Active<IMoBiReactionBuildingBlock>();
         return buildingBlock;
      }

      protected IEnumerable<string> ValidMoleculeNamesForPartner(IMoBiReactionBuildingBlock buildingBlock)
      {
         return _reactionTask.SelectMoleculeNames(buildingBlock, UnallowedNames(Subject), Subject.Name, PartnerType);
      }

      public abstract string PartnerType { get; }

      protected abstract IEnumerable<string> UnallowedNames(IReactionBuilder reactionBuilder);

      protected override void PerformExecute()
      {
         var buildingBlock = RetrieveActiveSubject();
         ValidMoleculeNamesForPartner(buildingBlock).Each(molecule => _context.AddToHistory(AddCommandFor(buildingBlock, molecule).Run(_context)));
      }

      protected abstract IMoBiCommand AddCommandFor(IMoBiReactionBuildingBlock buildingBlock, string molecule);
   }

   public class AddModifierUICommand : AddReactionBuilderUICommand
   {
      public AddModifierUICommand(IMoBiContext context, IActiveSubjectRetriever activeSubjectRetriever, IInteractionTasksForReactionBuilder reactionTask) : base(context, activeSubjectRetriever, reactionTask)
      {
      }

      public override string PartnerType => AppConstants.Captions.Modifiers;

      protected override IEnumerable<string> UnallowedNames(IReactionBuilder reactionBuilder)
      {
         return reactionBuilder.ModifierNames;
      }

      protected override IMoBiCommand AddCommandFor(IMoBiReactionBuildingBlock buildingBlock, string molecule)
      {
         return new AddItemToModifierCollectionCommand(buildingBlock, molecule, Subject);
      }
   }

   public class AddProductUICommand : AddReactionBuilderUICommand
   {
      public AddProductUICommand(IMoBiContext context, IActiveSubjectRetriever activeSubjectRetriever, IInteractionTasksForReactionBuilder reactionTask) : base(context, activeSubjectRetriever, reactionTask)
      {
      }

      public override string PartnerType => AppConstants.Captions.Products;

      protected override IEnumerable<string> UnallowedNames(IReactionBuilder reactionBuilder)
      {
         return reactionBuilder.Products.Select(x => x.MoleculeName);
      }

      protected override IMoBiCommand AddCommandFor(IMoBiReactionBuildingBlock buildingBlock, string molecule)
      {
         return new AddReactionPartnerToProductCollection(buildingBlock, new ReactionPartnerBuilder(molecule, 1.0), Subject);
      }
   }

   public class AddEductUICommand : AddReactionBuilderUICommand
   {
      public AddEductUICommand(IMoBiContext context, IActiveSubjectRetriever activeSubjectRetriever, IInteractionTasksForReactionBuilder reactionTask) : base(context, activeSubjectRetriever, reactionTask)
      {
      }

      public override string PartnerType => AppConstants.Captions.Educts;

      protected override IEnumerable<string> UnallowedNames(IReactionBuilder reactionBuilder)
      {
         return reactionBuilder.Educts.Select(x => x.MoleculeName);
      }

      protected override IMoBiCommand AddCommandFor(IMoBiReactionBuildingBlock buildingBlock, string molecule)
      {
         return new AddReactionPartnerToEductCollection(buildingBlock, new ReactionPartnerBuilder(molecule, 1.0), Subject);
      }
   }
}