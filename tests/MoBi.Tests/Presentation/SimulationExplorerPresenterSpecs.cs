using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Presentation.Nodes;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Presentation.Nodes;
using MoBi.Presentation.Presenter.Main;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.Classifications;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Presentation.Regions;
using OSPSuite.Presentation.Services;
using OSPSuite.Presentation.Views;
using OSPSuite.Assets;
using ITreeNodeFactory = MoBi.Presentation.Nodes.ITreeNodeFactory;

namespace MoBi.Presentation
{
   public abstract class concern_for_SimulationExplorerPresenter : ContextSpecification<ISimulationExplorerPresenter>
   {
      protected ISimulationExplorerView _view;
      private IRegionResolver _regionResolver;
      private ITreeNodeFactory _treeNodeFactory;
      protected IViewItemContextMenuFactory _viewItemContextMenuFactory;
      protected IMoBiContext _context;
      private IClassificationPresenter _classificationPresenter;
      private IToolTipPartCreator _toolTipPartCreator;
      private IMultipleTreeNodeContextMenuFactory _multipleTreeNodeContextMenuFactory;
      private IProjectRetriever _projectRetriever;
      private IInteractionTasksForSimulation _interactionTaskForSimulation;
      private IParameterAnalysablesInExplorerPresenter _parameterAnalysablesInExplorerPresenter;

      protected override void Context()
      {
         _projectRetriever = A.Fake<IProjectRetriever>();
         _view = A.Fake<ISimulationExplorerView>();
         _regionResolver = A.Fake<IRegionResolver>();
         _treeNodeFactory = A.Fake<ITreeNodeFactory>();
         _viewItemContextMenuFactory = A.Fake<IViewItemContextMenuFactory>();
         _context = A.Fake<IMoBiContext>();
         _classificationPresenter = A.Fake<IClassificationPresenter>();
         _toolTipPartCreator = A.Fake<IToolTipPartCreator>();
         _multipleTreeNodeContextMenuFactory = A.Fake<IMultipleTreeNodeContextMenuFactory>();
         _interactionTaskForSimulation= A.Fake<IInteractionTasksForSimulation>();
         _parameterAnalysablesInExplorerPresenter= A.Fake<IParameterAnalysablesInExplorerPresenter>();
         sut = new SimulationExplorerPresenter(_view, _regionResolver, _treeNodeFactory, _viewItemContextMenuFactory, _context,
            _classificationPresenter, _toolTipPartCreator, _multipleTreeNodeContextMenuFactory, _projectRetriever,_interactionTaskForSimulation, _parameterAnalysablesInExplorerPresenter);
      }
   }


   public class When_treen_node_double_clicked_is_a_used_building_block : concern_for_SimulationExplorerPresenter
   {
      private BuildingBlockInfoNode _treeNode;
      private IBuildingBlockInfo _buildingBlockInfo;
      private ClassifiableSimulation _classifiableSimulation;

      protected override void Context()
      {
         base.Context();
         _buildingBlockInfo = A.Fake<IBuildingBlockInfo>();
         _classifiableSimulation = new ClassifiableSimulation();
         _classifiableSimulation.Subject = new MoBiSimulation();
         _treeNode = new BuildingBlockInfoNode(_buildingBlockInfo);
         _treeNode.ParentNode = new SimulationNode(_classifiableSimulation);
      }

      protected override void Because()
      {
         sut.NodeDoubleClicked(_treeNode);
      }

      [Observation]
      public void should_not_call_the_context_menu_for_that_node()
      {
         A.CallTo(() => _viewItemContextMenuFactory.CreateFor(A<IViewItem>._, sut)).MustNotHaveHappened();
      }
   }


   public class When_simulation_explorer_handles_a_simulation_Status_changed_event : concern_for_SimulationExplorerPresenter
   {
      private SimulationStatusChangedEvent _simulationStatusChangedEvent;
      private IMoBiSimulation _simulation;
      private ITreeNode _simulationNode;
      private IUxTreeView _treeView;
      private ITreeNode _nodeToChange;
      private ITreeNode _nodeNotToChange;
      private IBuildingBlockInfo _buildingBlockInfoDTOUnchanged;
      private IBuildingBlockInfo _buildingBlockInfoDTOChanged;

      protected override void Context()
      {
         base.Context();
         _simulation = A.Fake<IMoBiSimulation>();
         _simulation.Id = "toto";
         _simulationStatusChangedEvent = new SimulationStatusChangedEvent(_simulation);
         _simulationNode = new Nodes.TextNode("Simulation");

         var buildConfiguration = A.Fake<IMoBiBuildConfiguration>();

         var spatialStructure = A.Fake<IMoBiSpatialStructure>().WithId("SpatialStructure");
         spatialStructure.Version = 5;
         var templateSpatialStructure = A.Fake<IMoBiSpatialStructure>().WithId("TemplateSpatialStructure");
         var spatialStructureInfo = new SpatialStructureInfo();
         spatialStructureInfo.TemplateBuildingBlock = templateSpatialStructure;
         spatialStructureInfo.BuildingBlock = spatialStructure;
         buildConfiguration.SpatialStructureInfo = spatialStructureInfo;
         A.CallTo(() => buildConfiguration.BuildingInfoForTemplateById(spatialStructureInfo.TemplateBuildingBlockId)).Returns(spatialStructureInfo);

         var moleculesInfo = new MoleculesInfo();
         var templateBuildingBlock = A.Fake<MoleculeBuildingBlock>().WithId("TemplateMolecules");
         var moleculeBuildingBlock = A.Fake<MoleculeBuildingBlock>().WithId("Molecules");
         moleculesInfo.TemplateBuildingBlock = templateBuildingBlock;
         moleculesInfo.BuildingBlock = moleculeBuildingBlock;
         A.CallTo(() => buildConfiguration.BuildingInfoForTemplateById(moleculesInfo.TemplateBuildingBlockId)).Returns(moleculesInfo);

         buildConfiguration.MoleculesInfo = moleculesInfo;
         A.CallTo(() => buildConfiguration.AllBuildingBlockInfos()).Returns(new IBuildingBlockInfo[] {spatialStructureInfo, moleculesInfo});
         A.CallTo(() => buildConfiguration.HasChangedBuildingBlocks()).Returns(true);

         _buildingBlockInfoDTOChanged = spatialStructureInfo;
         _buildingBlockInfoDTOUnchanged = moleculesInfo;

         _treeView = A.Fake<IUxTreeView>();
         var configNode = new SimulationConfigurationNode(buildConfiguration).Under(_simulationNode);
         _nodeToChange = new BuildingBlockInfoNode(_buildingBlockInfoDTOChanged)
            .WithIcon(ApplicationIcons.SpatialStructureGreen)
            .Under(configNode);

         _nodeNotToChange = new BuildingBlockInfoNode(_buildingBlockInfoDTOUnchanged)
            .WithIcon(ApplicationIcons.MoleculeGreen)
            .Under(configNode);

         A.CallTo(() => _view.TreeView).Returns(_treeView);
         A.CallTo(() => _treeView.NodeById(_simulation.Id)).Returns(_simulationNode);
         A.CallTo(() => _simulation.MoBiBuildConfiguration).Returns(buildConfiguration);
      }

      protected override void Because()
      {
         sut.Handle(_simulationStatusChangedEvent);
      }

      [Observation]
      public void should_ask_view_for_simulatuion_node()
      {
         A.CallTo(() => _treeView.NodeById(_simulation.Id)).MustHaveHappened();
      }

      [Observation]
      public void should_change_nodes_icon_to_simulation_red_icon()
      {
         _simulationNode.Icon.ShouldBeEqualTo(ApplicationIcons.SimulationRed);
      }

      [Observation]
      public void should_change_BuildingBlockChanged_property_of_dto_for_changed_building_blocks()
      {
         _buildingBlockInfoDTOChanged.BuildingBlockChanged.ShouldBeTrue();
      }

      [Observation]
      public void should_change_icon_of_changed_building_blocks_Node()
      {
         _nodeToChange.Icon.ShouldBeEqualTo(ApplicationIcons.SpatialStructureRed);
      }

      [Observation]
      public void should_not_change_icon_of_unchanged_building_blocks_Node()
      {
         _nodeNotToChange.Icon.ShouldBeEqualTo(ApplicationIcons.MoleculeGreen);
      }

      [Observation]
      public void should_not_change_BuildingBlockChanged_property_of_dto_for_unchanged_of_changed_building_blocks()
      {
         _buildingBlockInfoDTOUnchanged.BuildingBlockChanged.ShouldBeFalse();
      }
   }
}