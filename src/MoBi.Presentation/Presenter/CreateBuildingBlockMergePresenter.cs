using System;
using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Services;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface ICreateBuildingBlockMergePresenter : IPresenter<ICreateBuildingBlockMergeView>, IDisposablePresenter, IInitializablePresenter<ICommandCollector>
   {
   }

   public interface ICreateBuildingBlockMergePresenter<TObjectToMerge> : ICreateBuildingBlockMergePresenter
   {
      /// <summary>
      ///    Starts the full merge action.
      /// </summary>
      /// <param name="configuration">
      ///    An action that maps buildingblocks for the merge. The consumers of this presenter must set this action to inform the
      ///    presenter how to configure the merge.
      /// </param>
      /// <param name="caption">Caption to set in the view</param>
      void StartFullMerge(Action<IBuildingBlockMergePresenter, TObjectToMerge> configuration, string caption);

      /// <summary>
      ///    Starts the merge action for a single building block
      /// </summary>
      /// <param name="configuration">
      ///    An action that maps buildingblocks for the merge. The consumers of this presenter must set this action to inform the
      ///    presenter how to configure the merge.
      /// </param>
      void StartSingleMerge(Action<IBuildingBlockMergePresenter, TObjectToMerge> configuration);
   }

   public class CreateBuildingBlockMergePresenter<TObjectToMerge> : AbstractDisposableCommandCollectorPresenter<ICreateBuildingBlockMergeView, ICreateBuildingBlockMergePresenter>, ICreateBuildingBlockMergePresenter<TObjectToMerge>
   {
      private readonly IBuildingBlockMergePresenter<TObjectToMerge> _buildingBlockMergePresenter;
      private readonly IDialogCreator _dialogCreator;
      private readonly IObjectTypeResolver _objectTypeResolver;

      public CreateBuildingBlockMergePresenter(ICreateBuildingBlockMergeView view, IBuildingBlockMergePresenter<TObjectToMerge> buildingBlockMergePresenter,
         IMoBiContext context, IDialogCreator dialogCreator, IObjectTypeResolver objectTypeResolver) : base(view)
      {
         _buildingBlockMergePresenter = buildingBlockMergePresenter;
         _dialogCreator = dialogCreator;
         _objectTypeResolver = objectTypeResolver;
         _buildingBlockMergePresenter.SelectionChanged += updateStatus;
         view.AddView(_buildingBlockMergePresenter.BaseView);
         InitializeWith(context.HistoryManager);
         AddSubPresenters(_buildingBlockMergePresenter);
      }

      public void StartFullMerge(Action<IBuildingBlockMergePresenter, TObjectToMerge> configuration, string caption)
      {
         _buildingBlockMergePresenter.SetMergeConfiguration(configuration);
         View.Caption = caption;
         updateStatus();
         View.Display();
         if (View.Canceled)
            return;

         _buildingBlockMergePresenter.PerformMerge();
      }

      public void StartSingleMerge(Action<IBuildingBlockMergePresenter, TObjectToMerge> configuration)
      {
         _buildingBlockMergePresenter.SetMergeConfiguration(configuration);
         var pkmlFile = _dialogCreator.AskForFileToOpen(AppConstants.Captions.SourceBuildingBlockFileForMerge(_objectTypeResolver.TypeFor<TObjectToMerge>()), Constants.Filter.PKML_FILE_FILTER, Constants.DirectoryKey.MODEL_PART);
         if (string.IsNullOrEmpty(pkmlFile)) return;

         _buildingBlockMergePresenter.LoadMergeConfigurationFromFile(pkmlFile);
         _buildingBlockMergePresenter.PerformMerge();
      }

      private void updateStatus()
      {
         View.OkEnabled = CanClose;
      }
   }
}