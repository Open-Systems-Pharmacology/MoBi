using OSPSuite.Core.Commands.Core;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.UICommands;


namespace MoBi.Presentation.UICommand
{
   public abstract class RemoveReactionBuilderUICommand<TBuilder> : ObjectUICommand<ReactionBuilder>
   {
      protected readonly IActiveSubjectRetriever _activeSubjectRetriever;
      protected readonly IMoBiContext _context;
      protected ReactionBuilder _builder;
      protected TBuilder _partnerBuilder;

      protected RemoveReactionBuilderUICommand(IMoBiContext context, IActiveSubjectRetriever activeSubjectRetriever)
      {
         _context = context;
         _activeSubjectRetriever = activeSubjectRetriever;
      }

      public RemoveReactionBuilderUICommand<TBuilder> Initialize(TBuilder partnerBuilder, ReactionBuilder builder)
      {
         _partnerBuilder = partnerBuilder;
         _builder = builder;
         return this;
      }

      protected MoBiReactionBuildingBlock RetrieveActiveSubject()
      {
         var reactionBuildingBlock = _activeSubjectRetriever.Active<MoBiReactionBuildingBlock>();
         return reactionBuildingBlock;
      }

      protected override void PerformExecute()
      {
         var reactionBuildingBlock = RetrieveActiveSubject();
         _context.AddToHistory(RemoveCommandFor(reactionBuildingBlock).Run(_context));
      }

      protected abstract IMoBiCommand RemoveCommandFor(MoBiReactionBuildingBlock reactionBuildingBlock);
   }

   public class RemoveModifierUICommand : RemoveReactionBuilderUICommand<string>
   {
      public RemoveModifierUICommand(IMoBiContext context, IActiveSubjectRetriever activeSubjectRetriever) : base(context, activeSubjectRetriever)
      {
      }

      protected override IMoBiCommand RemoveCommandFor(MoBiReactionBuildingBlock reactionBuildingBlock)
      {
         return new RemoveItemFromModifierCollectionCommand(_builder, _partnerBuilder, reactionBuildingBlock);
      }
   }

   public class RemoveEductUICommand : RemoveReactionBuilderUICommand<ReactionPartnerBuilder>
   {
      public RemoveEductUICommand(IActiveSubjectRetriever activeSubjectRetriever, IMoBiContext context) : base(context, activeSubjectRetriever)
      {

      }

      protected override IMoBiCommand RemoveCommandFor(MoBiReactionBuildingBlock reactionBuildingBlock)
      {
         return new RemoveReactionPartnerFromEductCollection(_builder, _partnerBuilder, reactionBuildingBlock);
      }
   }

   internal class RemoveProductUICommand : RemoveReactionBuilderUICommand<ReactionPartnerBuilder>
   {
      public RemoveProductUICommand(IActiveSubjectRetriever activeSubjectRetriever, IMoBiContext context) : base(context, activeSubjectRetriever)
      {
      }


      protected override IMoBiCommand RemoveCommandFor(MoBiReactionBuildingBlock reactionBuildingBlock)
      {
         return new RemoveReactionPartnerFromProductCollection(_builder, _partnerBuilder, reactionBuildingBlock);
      }
   }


}