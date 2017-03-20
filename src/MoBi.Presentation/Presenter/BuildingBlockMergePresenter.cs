using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using MoBi.Assets;
using OSPSuite.Utility;
using OSPSuite.Utility.Events;
using MoBi.Core.Domain.Repository;
using MoBi.Core.Events;
using MoBi.Core.Repositories;
using MoBi.Core.Services;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Tasks;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Assets;

namespace MoBi.Presentation.Presenter
{
   public interface IBuildingBlockMergePresenter : ICommandCollectorPresenter
   {
      /// <summary>
      ///    This action actually performs the merge operation defined by the user
      /// </summary>
      void PerformMerge();

      /// <summary>
      ///    Asks a user to select a pkml file that will be used as source for merging
      /// </summary>
      void LoadMergeConfiguration();

      /// <summary>
      ///    Load the given <paramref name="pkmlFile" /> as source for the merging
      /// </summary>
      void LoadMergeConfigurationFromFile(string pkmlFile);

      /// <summary>
      ///    event is triggered whenever the selected item was changed
      /// </summary>
      event Action SelectionChanged;

       /// <summary>
      ///    Adds a mapping for the merge for a specific building block. Options are created to map into any existing building
      ///    block of the same type
      /// </summary>
      /// <typeparam name="TBuildingBlock">The type of the building block</typeparam>
      /// <param name="buildingBlock">The building block being merged into the project</param>
      /// <param name="shouldMergeByDefault">
      ///    Sets whether the default action of merge is to actually merge, or ignore the
      ///    building block
      /// </param>
      void AddMappingForAllBuildingBlocks<TBuildingBlock>(TBuildingBlock buildingBlock, bool shouldMergeByDefault) where TBuildingBlock : class, IBuildingBlock;

      /// <summary>
      ///    Adds a mapping for the merge of a specific building block. Options are created to map into only one existing
      ///    building block, or import as new
      /// </summary>
      /// <typeparam name="TBuildingBlock">The type of the building block</typeparam>
      /// <param name="buildingBlock">The building block being merged into the project</param>
      /// <param name="buildingBlocksSelection">The building block targeted by the merge</param>
      void AddMappingForSingleBuildingBlock<TBuildingBlock>(TBuildingBlock buildingBlock, TBuildingBlock buildingBlocksSelection) where TBuildingBlock : class, IBuildingBlock;
   }

   public interface IBuildingBlockMergePresenter<TObjectToMerge> : IBuildingBlockMergePresenter
   {
      /// <summary>
      ///    An action that maps buildingblocks for the merge. The consumers of this presenter must set this action to inform the
      ///    presenter how to configure the merge.
      /// </summary>
      void SetMergeConfiguration(Action<IBuildingBlockMergePresenter, TObjectToMerge> mergeConfiguration);
   }

   public class BuildingBlockMergePresenter<TObjectToMerge> : AbstractCommandCollectorPresenter<IBuildingBlockMergeView, IBuildingBlockMergePresenter>, IBuildingBlockMergePresenter<TObjectToMerge>, IListener<MergeCanceledEvent>
   {
      private readonly IDialogCreator _dialogCreator;
      private readonly IMergeTask _mergeTask;
      private readonly IBuildingBlockRepository _buildingBlockRepository;
      private readonly IList<BuildingBlockMappingDTO> _allBuildingBlockMappings;
      private readonly IBuildingBlock _noneBuildingBlock;
      private readonly IBuildingBlock _importAsNewBuildingBlock;
      private readonly CancellationTokenSource _cancellationTokenSource;
      private readonly IIconRepository _iconRepository;
      private readonly ISerializationTask _serializationTask;
      private Action<IBuildingBlockMergePresenter, TObjectToMerge> _mergeConfiguration;

      public event Action SelectionChanged = delegate { };

      public BuildingBlockMergePresenter( IBuildingBlockMergeView view,IDialogCreator dialogCreator,
         IMergeTask mergeTask,IBuildingBlockRepository buildingBlockRepository,
         IIconRepository iconRepository,ISerializationTask serializationTask) : base(view)
      {
         _cancellationTokenSource = new CancellationTokenSource();
         _dialogCreator = dialogCreator;
         _mergeTask = mergeTask;
         _buildingBlockRepository = buildingBlockRepository;
         _allBuildingBlockMappings = new List<BuildingBlockMappingDTO>();
         _noneBuildingBlock = new NullBuildingBlock(AppConstants.None);
         _importAsNewBuildingBlock = new NullBuildingBlock(AppConstants.Captions.ImportAsNew);
         _iconRepository = iconRepository;
         _serializationTask = serializationTask;
      }

      public void PerformMerge()
      {
         var allBuildingBlockToMergeMapping = _allBuildingBlockMappings.Where(selectionIsDefined).ToList();
         var buildingBlocksToMerge = allBuildingBlockToMergeMapping.Select(x => x.BuildingBlockToMerge).ToList();
         var projectBuildingBlocks = allBuildingBlockToMergeMapping.Select(x => realBuildingBlockFrom(x.ProjectBuildingBlock)).ToList();

         try
         {
            AddCommand(_mergeTask.MergeBuildingBlocks(buildingBlocksToMerge, projectBuildingBlocks, _cancellationTokenSource.Token));
         }
         catch (OperationCanceledException)
         {
         }
      }

      private IBuildingBlock realBuildingBlockFrom(IBuildingBlock projectBuildingBlock)
      {
         return projectBuildingBlock == _importAsNewBuildingBlock ? null : projectBuildingBlock;
      }

      public void LoadMergeConfiguration()
      {
         var pkmlFile = _dialogCreator.AskForFileToOpen(AppConstants.Captions.SourceSimulationFileForMerge, Constants.Filter.PKML_FILE_FILTER, Constants.DirectoryKey.MODEL_PART);
         if (string.IsNullOrEmpty(pkmlFile)) return;

         LoadMergeConfigurationFromFile(pkmlFile);
      }

      public void LoadMergeConfigurationFromFile(string pkmlFile)
      {
         _allBuildingBlockMappings.Clear();

         var objectToMerge = _serializationTask.Load<TObjectToMerge>(pkmlFile, resetIds: true);
         _mergeConfiguration(this, objectToMerge);

         _view.BindTo(_allBuildingBlockMappings);
         _view.SimulationFile = FileHelper.ShortenPathName(pkmlFile, 30);
         SelectionChanged();
      }

      public void SetMergeConfiguration(Action<IBuildingBlockMergePresenter, TObjectToMerge> mergeConfiguration)
      {
         _mergeConfiguration = mergeConfiguration;
      }

      public void AddMappingForAllBuildingBlocks<TBuildingBlock>(TBuildingBlock buildingBlock, bool shouldMergeByDefault) where TBuildingBlock : class, IBuildingBlock
      {
         var all = getAllAvailableBuildingBlocks<TBuildingBlock>();
         all.AddRange(new[] {_importAsNewBuildingBlock, _noneBuildingBlock});
         mapBuildingBlocks(buildingBlock, all, shouldMergeByDefault);
      }

      public void AddMappingForSingleBuildingBlock<TBuildingBlock>(TBuildingBlock buildingBlock, TBuildingBlock buildingBlocksSelection) where TBuildingBlock : class, IBuildingBlock
      {
         var allBuildingBlocks = new List<IBuildingBlock> {buildingBlocksSelection, _importAsNewBuildingBlock};
         mapBuildingBlocks(buildingBlock, allBuildingBlocks, shouldMergeByDefault: true);
      }

      private void mapBuildingBlocks<TBuildingBlock>(TBuildingBlock buildingBlock, IEnumerable<IBuildingBlock> allBuildingBlocks, bool shouldMergeByDefault) where TBuildingBlock : class, IBuildingBlock
      {
         var mapping = new BuildingBlockMappingDTO
         {
            AllAvailableBuildingBlocks = allBuildingBlocks,
            BuildingBlockToMerge = buildingBlock ?? _noneBuildingBlock,
            BuildingBlockIcon = ApplicationIcons.IconByName(_iconRepository.IconFor(buildingBlock))
         };

         mapping.ProjectBuildingBlock = shouldMergeByDefault ? mapping.AllAvailableBuildingBlocks.First() : _noneBuildingBlock;
         _allBuildingBlockMappings.Add(mapping);
      }

      private List<IBuildingBlock> getAllAvailableBuildingBlocks<TBuildingBlock>() where TBuildingBlock : class, IBuildingBlock
      {
         var allAvailableBuildingBlocks = new List<IBuildingBlock>(_buildingBlockRepository.All<TBuildingBlock>());

         return allAvailableBuildingBlocks;
      }

      public override void ViewChanged()
      {
         SelectionChanged();
      }

      public override bool CanClose
      {
         get { return base.CanClose && _allBuildingBlockMappings.Any(selectionIsDefined); }
      }

      private bool selectionIsDefined(BuildingBlockMappingDTO buildingBlockMappingDTO)
      {
         if (buildingBlockMappingDTO.BuildingBlockToMerge == _noneBuildingBlock)
            return false;
         return buildingBlockMappingDTO.ProjectBuildingBlock != _noneBuildingBlock;
      }

      private class NullBuildingBlock : BuildingBlock
      {
         public NullBuildingBlock(string name)
         {
            Name = name;
         }
      }

      public void Handle(MergeCanceledEvent eventToHandle)
      {
         _cancellationTokenSource.Cancel();
      }
   }
}