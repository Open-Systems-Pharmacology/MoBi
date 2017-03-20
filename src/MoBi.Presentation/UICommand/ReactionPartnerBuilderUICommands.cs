using OSPSuite.Core.Commands.Core;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.UICommands;


namespace MoBi.Presentation.UICommand
{
   public abstract class RemoveReactionBuilderUICommand<TBuilder> : ObjectUICommand<IReactionBuilder>
   {
      protected readonly IActiveSubjectRetriever _activeSubjectRetriever;
      protected readonly IMoBiContext _context;
      protected IReactionBuilder _builder;
      protected TBuilder _partnerBuilder;

      protected RemoveReactionBuilderUICommand(IMoBiContext context, IActiveSubjectRetriever activeSubjectRetriever)
      {
         _context = context;
         _activeSubjectRetriever = activeSubjectRetriever;
      }

      public RemoveReactionBuilderUICommand<TBuilder> Initialize(TBuilder partnerBuilder, IReactionBuilder builder)
      {
         _partnerBuilder = partnerBuilder;
         _builder = builder;
         return this;
      }

      protected IMoBiReactionBuildingBlock RetrieveActiveSubject()
      {
         var reactionBuildingBlock = _activeSubjectRetriever.Active<IMoBiReactionBuildingBlock>();
         return reactionBuildingBlock;
      }

      protected override void PerformExecute()
      {
         var reactionBuildingBlock = RetrieveActiveSubject();
         _context.AddToHistory(RemoveCommandFor(reactionBuildingBlock).Run(_context));
      }

      protected abstract IMoBiCommand RemoveCommandFor(IMoBiReactionBuildingBlock reactionBuildingBlock);
   }

   public class RemoveModifierUICommand : RemoveReactionBuilderUICommand<string>
   {
      public RemoveModifierUICommand(IMoBiContext context, IActiveSubjectRetriever activeSubjectRetriever) : base(context, activeSubjectRetriever)
      {
      }

      protected override IMoBiCommand RemoveCommandFor(IMoBiReactionBuildingBlock reactionBuildingBlock)
      {
         return new RemoveItemFromModifierCollectionCommand(_builder, _partnerBuilder, reactionBuildingBlock);
      }
   }

   public class RemoveEductUICommand : RemoveReactionBuilderUICommand<IReactionPartnerBuilder>
   {
      public RemoveEductUICommand(IActiveSubjectRetriever activeSubjectRetriever, IMoBiContext context) : base(context, activeSubjectRetriever)
      {

      }

      protected override IMoBiCommand RemoveCommandFor(IMoBiReactionBuildingBlock reactionBuildingBlock)
      {
         return new RemoveReactionPartnerFromEductCollection(_builder, _partnerBuilder, reactionBuildingBlock);
      }
   }

   internal class RemoveProductUICommand : RemoveReactionBuilderUICommand<IReactionPartnerBuilder>
   {
      public RemoveProductUICommand(IActiveSubjectRetriever activeSubjectRetriever, IMoBiContext context) : base(context, activeSubjectRetriever)
      {
      }


      protected override IMoBiCommand RemoveCommandFor(IMoBiReactionBuildingBlock reactionBuildingBlock)
      {
         return new RemoveReactionPartnerFromProductCollection(_builder, _partnerBuilder, reactionBuildingBlock);
      }
   }


}