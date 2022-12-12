using System;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Presentation.Mappers;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Core;
using OSPSuite.Utility.Collections;

namespace MoBi.Presentation.Tasks
{
   public interface IIgnoreReplaceMergeManager<T> where T : IObjectBase
   {
      /// <summary>
      ///    Manages the conflict of the two caches
      /// </summary>
      /// <param name="merge">The cache being merged into the target</param>
      /// <param name="target">The target cache of the merge</param>
      /// <param name="areElementsEquivalent">
      ///    Optionally specify a Func that will detect for equivalence between two conflicting
      ///    elements
      /// </param>
      void Merge(ICache<string, T> merge, ICache<string, T> target, Func<T, T, bool> areElementsEquivalent = null);

      /// <summary>
      ///    In the event of a conflict where user specifies that the newly merged element should replace the existing, this
      ///    method will be used to remove the existing element
      /// </summary>
      Action<T> RemoveAction { set; }

      /// <summary>
      ///    In the event of a conflict where user specifies that the newly merged element should replace the existing, this
      ///    method will be used to add the newly merged element
      /// </summary>
      Action<T> AddAction { set; }

      /// <summary>
      ///    In the event that the user cancels the merge part way through managing conflicts, this method will be run.
      /// </summary>
      Action CancelAction { set; }
   }

   public interface IMergeIgnoreReplaceMergeManager<T> : IIgnoreReplaceMergeManager<T> where T : IObjectBase
   {
      /// <summary>
      ///    In the event of a conflict where user specifies merge, this action will take two elements of the same type and merge
      ///    them together
      /// </summary>
      Action<T, T> MergeAction { set; }
   }

   public interface IIgnoreReplaceCloneMergeManager<T> : IIgnoreReplaceMergeManager<T> where T : IObjectBase
   {
      /// <summary>
      ///    In the event of a conflict where user specifies that both elements should be kept, this method will be used to add a
      ///    copy of the newly merged element with a new name.
      /// </summary>
      Action<T, string> CloneAction { set; }
   }

   public interface IMoleculeBuildingBlockCloneManager : IIgnoreReplaceCloneMergeManager<IMoleculeBuilder>
   {
   }

   public class MoleculeBuildingBlockCloneManager : IgnoreReplaceCloneMergeManager<IMoleculeBuilder>, IMoleculeBuildingBlockCloneManager
   {
      public MoleculeBuildingBlockCloneManager(
         IApplicationController applicationController,
         INameCorrector nameCorrector,
         IMoleculeBuilderToObjectBaseSummaryDTOMapper dtoMapper,
         IMoBiContext context)
         : base(applicationController, nameCorrector, dtoMapper, context)
      {
      }
   }

   public interface IReactionBuildingBlockMergeManager : IIgnoreReplaceCloneMergeManager<IReactionBuilder>
   {
      IMoBiReactionBuildingBlock SourceBuildingBlock { set; get; }
      IMoBiReactionBuildingBlock TargetBuildingBlock { set; get; }
   }

   public class ReactionBuildingBlockMergeManager : IgnoreReplaceCloneMergeManager<IReactionBuilder>, IReactionBuildingBlockMergeManager
   {
      public ReactionBuildingBlockMergeManager(
         IApplicationController applicationController,
         INameCorrector nameCorrector,
         IReactionBuilderToObjectBaseSummaryDTOMapper dtoMapper,
         IMoBiContext context)
         : base(applicationController, nameCorrector, dtoMapper, context)
      {
      }

      public IMoBiReactionBuildingBlock SourceBuildingBlock { get; set; }
      public IMoBiReactionBuildingBlock TargetBuildingBlock { get; set; }
   }

   public interface IEventBuildingBlockMergeManager : IIgnoreReplaceCloneMergeManager<IEventGroupBuilder>
   {
   }

   public class EventBuildingBlockMergeManager : IgnoreReplaceCloneMergeManager<IEventGroupBuilder>, IEventBuildingBlockMergeManager
   {
      public EventBuildingBlockMergeManager(
         IApplicationController applicationController,
         INameCorrector nameCorrector,
         IEventGroupBuilderToObjectBaseSummaryDTOMapper dtoMapper,
         IMoBiContext context)
         : base(applicationController, nameCorrector, dtoMapper, context)
      {
      }
   }

   public interface IObserverBuildingBlockMergeManager : IMergeIgnoreReplaceMergeManager<IObserverBuilder>
   {
   }

   public class ObserverBuildingBlockMergeManager : MergeIgnoreReplaceMergeManager<IObserverBuilder>, IObserverBuildingBlockMergeManager
   {
      public ObserverBuildingBlockMergeManager(IApplicationController applicationController, IObserverBuilderToObjectBaseSummaryDTOMapper dtoMapper, IMoBiContext context)
         : base(applicationController, dtoMapper, context)
      {
      }
   }

   public interface IParameterStartValueBuildingBlockMergeManager : IIgnoreReplaceMergeManager<ParameterStartValue>
   {
   }

   public interface IMoleculeStartValueBuildingBlockMergeManager : IIgnoreReplaceMergeManager<MoleculeStartValue>
   {
   }

   public class MoleculeStartValueBuildingBlockMergeManager : IgnoreReplaceMergeManager<MoleculeStartValue>, IMoleculeStartValueBuildingBlockMergeManager
   {
      public MoleculeStartValueBuildingBlockMergeManager(
         IApplicationController applicationController,
         IMoleculeStartValueToObjectBaseSummaryDTOMapper dtoMapper,
         IMoBiContext context)
         : base(applicationController, dtoMapper, context)
      {
      }
   }

   public class ParameterStartValueBuildingBlockMergeManager : IgnoreReplaceMergeManager<ParameterStartValue>, IParameterStartValueBuildingBlockMergeManager
   {
      public ParameterStartValueBuildingBlockMergeManager(
         IApplicationController applicationController,
         IParameterStartValueToObjectBaseSummaryDTOMapper dtoMapper,
         IMoBiContext context)
         : base(applicationController, dtoMapper, context)
      {
      }
   }

   public interface IPassiveTranportBuildingBlockMergeManager : IMergeIgnoreReplaceMergeManager<ITransportBuilder>
   {
   }

   public class PassiveTranportBuildingBlockMergeManager : MergeIgnoreReplaceMergeManager<ITransportBuilder>, IPassiveTranportBuildingBlockMergeManager
   {
      public PassiveTranportBuildingBlockMergeManager(
         IApplicationController applicationController,
         IPassiveTransportBuilderToObjectBaseSummaryDTOMapper dtoMapper,
         IMoBiContext context)
         : base(applicationController, dtoMapper, context)
      {
      }
   }
}