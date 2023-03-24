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
      ITreeNode CreateFor(SimulationConfiguration simulationConfiguration);
      ITreeNode CreateFor(IBuildingBlock buildingBlock);
      ITreeNode CreateFor(MoleculeBuildingBlock moleculeBuildingBlock);
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

         // if (simulation.MoBiBuildConfiguration.HasChangedBuildingBlocks())
         //    simNode.Icon = ApplicationIcons.SimulationRed;

         var buildConfigNode = CreateFor(simulation.Configuration);

         simNode.AddChild(buildConfigNode);
         if (simulation.ResultsDataRepository != null)
            simNode.AddChild(CreateFor(simulation.ResultsDataRepository));

         //uses reverse so that the first result is the last node
         simulation.HistoricResults.Reverse().Each(res => simNode.AddChild(CreateFor(res)));
         return simNode;
      }

      public ITreeNode CreateFor(SimulationConfiguration simulationConfiguration)
      {
         var buildConfigNode = new SimulationConfigurationNode(simulationConfiguration);
         //add one node for each Building Block
         addConfigurationNodeUnder(buildConfigNode, simulationConfiguration.SpatialStructure);
         addConfigurationNodeUnder(buildConfigNode, simulationConfiguration.Molecules);
         addConfigurationNodeUnder(buildConfigNode, simulationConfiguration.Reactions);
         addConfigurationNodeUnder(buildConfigNode, simulationConfiguration.PassiveTransports);
         addConfigurationNodeUnder(buildConfigNode, simulationConfiguration.Observers);
         addConfigurationNodeUnder(buildConfigNode, simulationConfiguration.EventGroups);
         addConfigurationNodeUnder(buildConfigNode, simulationConfiguration.SimulationSettings);
         addConfigurationNodeUnder(buildConfigNode, simulationConfiguration.MoleculeStartValues);
         addConfigurationNodeUnder(buildConfigNode, simulationConfiguration.ParameterStartValues);
         addConfigurationNodeUnder(buildConfigNode, simulationConfiguration.Individual);


         var expressionsNode = CreateFor(MoBiRootNodeTypes.ExpressionProfilesFolder)
            .Under(buildConfigNode);

         simulationConfiguration.ExpressionProfiles.Each(x => CreateFor(x).Under(expressionsNode));

         return buildConfigNode;
      }

      public ITreeNode CreateFor(IBuildingBlock buildingBlock)
      {
         var moleculeBuildingBlock = buildingBlock as MoleculeBuildingBlock;
         if (moleculeBuildingBlock != null)
            return CreateFor(moleculeBuildingBlock);

         return createFor(buildingBlock);
      }

      public ITreeNode CreateFor(MoleculeBuildingBlock moleculeBuildingBlock)
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

      private void addConfigurationNodeUnder(ITreeNode buildConfigNode, IBuildingBlock buildingBlock)
      {
         // TODO this used to use buildingBlockInfo to create the tree
         var statusIcon = ApplicationIcons.GreenOverlayFor(buildingBlock.Icon);
         // var statusIcon = buildingBlockInfo.BuildingBlockChanged
         //    ? ApplicationIcons.RedOverlayFor(buildingBlock.Icon)
         //    : ApplicationIcons.GreenOverlayFor(buildingBlock.Icon);

         CreateFor(buildingBlock)
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
            Icon = ApplicationIcons.Favorites,
            Id = Captions.Favorites
         }) { Icon = ApplicationIcons.Favorites };
      }

      public ITreeNode CreateForUserDefined()
      {
         return new ObjectWithIdAndNameNode<ObjectBaseDTO>(new UserDefinedNodeViewItem
            {
               Name = AppConstants.Captions.UserDefined,
               Icon = ApplicationIcons.UserDefinedVariability,
               Id = AppConstants.Captions.UserDefined
         }){ Icon = ApplicationIcons.UserDefinedVariability };
      }
   }
}