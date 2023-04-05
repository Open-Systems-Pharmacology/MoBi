using System;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Helper;
using MoBi.Presentation.Presenter;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;

namespace MoBi.Presentation.Tasks
{
   public interface IMergeWorkflowTask
   {
      /// <summary>
      /// Starts a full merge asking the user to select a file containing a simulation that will be used as source for the merge
      /// </summary>
      void StartSimulationMerge();

      /// <summary>
      ///    Starts a simple merge workflow. The merge target is <paramref name="targetBuildingBlock" />
      /// </summary>
      void StartSingleBuildingBlockMerge<TBuildingBlock>(TBuildingBlock targetBuildingBlock) where TBuildingBlock : class, IBuildingBlock;
   }

   public class MergeWorkflowTask : IMergeWorkflowTask
   {
      private readonly IMoBiApplicationController _applicationController;
      private readonly IObjectTypeResolver _objectTypeResolver;

      public MergeWorkflowTask(IMoBiApplicationController applicationController, IObjectTypeResolver objectTypeResolver)
      {
         _applicationController = applicationController;
         _objectTypeResolver = objectTypeResolver;
      }

      private void startMerge<TObjectToMerge>(Action<IBuildingBlockMergePresenter, TObjectToMerge> mergeConfiguration, string caption, bool singleMerge)
      {
         using (var presenter = _applicationController.Start<ICreateBuildingBlockMergePresenter<TObjectToMerge>>())
         {
            if (singleMerge)
               presenter.StartSingleMerge(mergeConfiguration);
            else
               presenter.StartFullMerge(mergeConfiguration, caption);
         }
      }

      public void StartSingleBuildingBlockMerge<TBuildingBlock>(TBuildingBlock targetBuildingBlock) where TBuildingBlock : class, IBuildingBlock
      {
         startMerge<TBuildingBlock>((presenter, bb) => presenter.AddMappingForSingleBuildingBlock(bb, targetBuildingBlock),
            AppConstants.Captions.MergeBuildingBlockIntoTarget(_objectTypeResolver.TypeFor<TBuildingBlock>(), targetBuildingBlock.Name),
            singleMerge: true);
      }


      public void StartSimulationMerge()
      {
         // TODO SIMULATION_CONFIGURATION
         // startMerge<IMoBiBuildConfiguration>((presenter, buildConfiguration) =>
         // {
         //    presenter.AddMappingForAllBuildingBlocks(buildConfiguration.Molecules, shouldMergeByDefault: true);
         //    presenter.AddMappingForAllBuildingBlocks(buildConfiguration.Reactions, shouldMergeByDefault: true);
         //    presenter.AddMappingForAllBuildingBlocks(buildConfiguration.PassiveTransports, shouldMergeByDefault: true);
         //    presenter.AddMappingForAllBuildingBlocks(buildConfiguration.Observers, shouldMergeByDefault: false);
         //    presenter.AddMappingForAllBuildingBlocks(buildConfiguration.EventGroups, shouldMergeByDefault: false);
         //    presenter.AddMappingForAllBuildingBlocks(buildConfiguration.MoleculeStartValues, shouldMergeByDefault: true);
         //    presenter.AddMappingForAllBuildingBlocks(buildConfiguration.ParameterStartValues, shouldMergeByDefault: true);
         // }, AppConstants.Captions.MergeSimulationIntoProject, singleMerge: false);
      }
   }
}