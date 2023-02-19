using System.Linq;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using OSPSuite.Assets;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Repositories;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Presentation.Presenters.Nodes;
using OSPSuite.Presentation.Services;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Nodes
{
   public interface ITreeNodeFactory : OSPSuite.Presentation.Nodes.ITreeNodeFactory
   {
      ITreeNode<RootNodeType> CreateFor(RootNodeType rootNode);
      ITreeNode CreateFor(ObjectBaseDTO objectBase);
      ITreeNode CreateFor(DataRepository dataRepository);
      ITreeNode CreateFor(ClassifiableSimulation classifiableSimulation);
      ITreeNode CreateFor(CurveChart chart);
      ITreeNode CreateFor(IMoBiBuildConfiguration buildConfiguration);
      ITreeNode CreateFor(IBuildingBlock buildingBlock);
      ITreeNode CreateFor(IMoleculeBuildingBlock moleculeBuildingBlock);
      ITreeNode CreateFor(IMoleculeBuilder moleculeBuilder);
      ITreeNode CreateFor(IBuildingBlockInfo buildingBlockInfo);
      ITreeNode CreateForFavorites();
      ITreeNode CreateForUserDefined();
   }

   public class TreeNodeFactory : OSPSuite.Presentation.Nodes.TreeNodeFactory, ITreeNodeFactory
   {
      public TreeNodeFactory(IObservedDataRepository observedDataRepository, IToolTipPartCreator toolTipPartCreator) : base(observedDataRepository, toolTipPartCreator)
      {
      }

      public ITreeNode<RootNodeType> CreateFor(RootNodeType rootNode) => new RootNode(rootNode);

      public ITreeNode CreateFor(ObjectBaseDTO objectBase)
      {
         return new ObjectWithIdAndNameNode<ObjectBaseDTO>(objectBase);
      }

      public ITreeNode CreateFor(DataRepository dataRepository)
      {
         return new HistoricalResultsNode(dataRepository);
      }

      public ITreeNode CreateFor(ClassifiableSimulation classifiableSimulation)
      {
         var simNode = new SimulationNode(classifiableSimulation);
         var simulation = classifiableSimulation.Simulation;

         if (simulation.MoBiBuildConfiguration.HasChangedBuildingBlocks())
            simNode.Icon = ApplicationIcons.SimulationRed;

         var buildConfigNode = CreateFor(simulation.MoBiBuildConfiguration);

         simNode.AddChild(buildConfigNode);
         if (simulation.ResultsDataRepository != null)
            simNode.AddChild(CreateFor(simulation.ResultsDataRepository));

         //uses reverse so that the first result is the last node
         simulation.HistoricResults.Reverse().Each(res => simNode.AddChild(CreateFor(res)));
         return simNode;
      }

      public ITreeNode CreateFor(IMoBiBuildConfiguration buildConfiguration)
      {
         var buildConfigNode = new BuildConfigurationNode(buildConfiguration);
         //add one node for each Building Block
         addConfigurationNodeUnder(buildConfigNode, buildConfiguration.SpatialStructureInfo);
         addConfigurationNodeUnder(buildConfigNode, buildConfiguration.MoleculesInfo);
         addConfigurationNodeUnder(buildConfigNode, buildConfiguration.ReactionsInfo);
         addConfigurationNodeUnder(buildConfigNode, buildConfiguration.PassiveTransportsInfo);
         addConfigurationNodeUnder(buildConfigNode, buildConfiguration.ObserversInfo);
         addConfigurationNodeUnder(buildConfigNode, buildConfiguration.EventGroupsInfo);
         addConfigurationNodeUnder(buildConfigNode, buildConfiguration.SimulationSettingsInfo);
         addConfigurationNodeUnder(buildConfigNode, buildConfiguration.MoleculeStartValuesInfo);
         addConfigurationNodeUnder(buildConfigNode, buildConfiguration.ParameterStartValuesInfo);
         return buildConfigNode;
      }

      public ITreeNode CreateFor(IBuildingBlock buildingBlock)
      {
         var moleculeBuildingBlock = buildingBlock as IMoleculeBuildingBlock;
         if (moleculeBuildingBlock != null)
            return CreateFor(moleculeBuildingBlock);

         return createFor(buildingBlock);
      }

      public ITreeNode CreateFor(IMoleculeBuildingBlock moleculeBuildingBlock)
      {
         var moleculeBuildingBlockNode = createFor(moleculeBuildingBlock);
         foreach (var molecule in moleculeBuildingBlock)
         {
            var moleculeNode = CreateFor(molecule);
            moleculeBuildingBlockNode.AddChild(moleculeNode);
         }
         return moleculeBuildingBlockNode;
      }

      public ITreeNode CreateFor(IMoleculeBuilder moleculeBuilder)
      {
         return createFor(moleculeBuilder);
      }

      public ITreeNode CreateFor(IBuildingBlockInfo buildingBlockInfo)
      {
         return new BuildingBlockInfoNode(buildingBlockInfo);
      }

      private ITreeNode createFor<T>(T objectBase) where T : class, IObjectBase
      {
         return new ObjectWithIdAndNameNode<T>(objectBase)
            .WithIcon(ApplicationIcons.IconByName(objectBase.Icon));
      }

      private void addConfigurationNodeUnder(ITreeNode buildConfigNode, IBuildingBlockInfo buildingBlockInfo)
      {
         var buildingBlock = buildingBlockInfo.UntypedBuildingBlock;

         var statusIcon = buildingBlockInfo.BuildingBlockChanged
            ? ApplicationIcons.RedOverlayFor(buildingBlock.Icon)
            : ApplicationIcons.GreenOverlayFor(buildingBlock.Icon);

         CreateFor(buildingBlockInfo)
            .WithIcon(statusIcon)
            .Under(buildConfigNode);
      }

      public ITreeNode CreateFor(CurveChart chart)
      {
         return new ChartNode(chart).WithIcon(ApplicationIcons.SimulationComparison);
      }

      public ITreeNode CreateForFavorites()
      {
         return new ObjectWithIdAndNameNode<ObjectBaseDTO>(new FavoritesNodeViewItem
         {
            Name = Captions.Favorites,
            Icon = ApplicationIcons.Favorites.IconName,
            Id = Captions.Favorites
         }) {Icon = ApplicationIcons.Favorites};
      }

      public ITreeNode CreateForUserDefined()
      {
         return new ObjectWithIdAndNameNode<ObjectBaseDTO>(new UserDefinedNodeViewItem
            {
               Name = AppConstants.Captions.UserDefined,
               Icon = ApplicationIcons.UserDefinedVariability.IconName,
               Id = AppConstants.Captions.UserDefined
         }){ Icon = ApplicationIcons.UserDefinedVariability };
      }
   }
}