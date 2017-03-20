using System.Collections.Generic;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.UI;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.RepositoryItems;
using DevExpress.XtraEditors.Repository;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Assets;
using OSPSuite.UI.Controls;
using OSPSuite.Presentation.Extensions;
using OSPSuite.UI.Services;

namespace MoBi.UI.Views
{
   public partial class BuildingBlockMergeView : BaseUserControl, IBuildingBlockMergeView
   {
      private IBuildingBlockMergePresenter _presenter;
      private readonly GridViewBinder<BuildingBlockMappingDTO> _gridViewBinder;
      private readonly RepositoryItemImageComboBox _repositoryForProjectBuildingBlock;

      public BuildingBlockMergeView(IImageListRetriever imageListRetriever)
      {
         InitializeComponent();
         _gridViewBinder = new GridViewBinder<BuildingBlockMappingDTO>(gridView);
         _repositoryForProjectBuildingBlock = new UxRepositoryItemImageComboBox(gridView, imageListRetriever);
         gridView.AllowsFiltering = false;
      }

      public void AttachPresenter(IBuildingBlockMergePresenter presenter)
      {
         _presenter = presenter;
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();

         _gridViewBinder.Bind(x => x.BuildingBlockToMerge)
            .WithCaption(AppConstants.Captions.BuildingBlockToMerge)
            .AsReadOnly();

         _gridViewBinder.Bind(x => x.ProjectBuildingBlock)
            .WithCaption(AppConstants.Captions.TargetBuildingBlock)
            .WithRepository(projectBuildingBlockRepository);

         _gridViewBinder.Changed += () => OnEvent(NotifyViewChanged);
         btnSimulationPath.ButtonClick += (o, e) => OnEvent(_presenter.LoadMergeConfiguration);
      }

      private RepositoryItem projectBuildingBlockRepository(BuildingBlockMappingDTO buildingBlockMappingDTO)
      {
         _repositoryForProjectBuildingBlock.FillImageComboBoxRepositoryWith(buildingBlockMappingDTO.AllAvailableBuildingBlocks, dto => ApplicationIcons.IconIndex(buildingBlockMappingDTO.BuildingBlockIcon));
         return _repositoryForProjectBuildingBlock;
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         layoutItemSimulationPath.Text = AppConstants.Captions.SimulationPathForMerge.FormatForLabel();
      }

      public string SimulationFile
      {
         set { btnSimulationPath.Text = value; }
      }

      public void BindTo(IEnumerable<BuildingBlockMappingDTO> allBuildingBlockMappings)
      {
         _gridViewBinder.BindToSource(allBuildingBlockMappings);
         gridView.BestFitColumns();
      }
   }
}