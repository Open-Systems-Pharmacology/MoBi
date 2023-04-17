using System.Collections.Generic;
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
      ITreeNode CreateFor(MoleculeBuilder moleculeBuilder);
      ITreeNode CreateForFavorites();
      ITreeNode CreateForUserDefined();
      ITreeNode CreateFor(Module module);
      ITreeNode CreateFor(ModuleConfiguration moduleConfiguration);
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


         //TODO SIMULATION_CONFIGURATION
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

      public ITreeNode CreateFor(Module module)
      {
         var moduleNode = createFor(module);
         addModuleBuildingBlocks(moduleNode, module);
         addStartValueCollections(moduleNode, module);

         return moduleNode;
      }

      private void addStartValueCollections(ITreeNode moduleNode, Module module)
      {
         var psvCollectionNode = collectionNodeFor(module.ParameterStartValuesCollection, MoBiRootNodeTypes.ParameterStartValuesFolder, moduleNode);
         module.ParameterStartValuesCollection.Each(psv => { createAndAddNodeUnder(psvCollectionNode, psv); });

         var msvCollectionNode = collectionNodeFor(module.MoleculeStartValuesCollection, MoBiRootNodeTypes.MoleculeStartValuesFolder, moduleNode);
         module.MoleculeStartValuesCollection.Each(msv => createAndAddNodeUnder(msvCollectionNode, msv));
      }

      private ITreeNode collectionNodeFor<T>(IReadOnlyList<IStartValuesBuildingBlock<T>> startValueBlockCollection, RootNodeType rootNodeType, ITreeNode moduleNode) where T : class, IStartValue
      {
         return startValueBlockCollection.Count > 1 ? CreateFor(rootNodeType).Under(moduleNode) : moduleNode;
      }

      public ITreeNode CreateFor(SimulationConfiguration simulationConfiguration)
      {
         var buildConfigNode = new SimulationConfigurationNode(simulationConfiguration);
         //add one node for each Building Block

         simulationConfiguration.ModuleConfigurations.Each(moduleConfiguration =>
         {
            var moduleConfigurationNode = CreateFor(moduleConfiguration);

            moduleConfigurationNode.Under(buildConfigNode);
         });

         createAndAddNodeUnder(buildConfigNode, simulationConfiguration.SimulationSettings);
         createAndAddNodeUnder(buildConfigNode, simulationConfiguration.Individual);


         var expressionsNode = CreateFor(MoBiRootNodeTypes.ExpressionProfilesFolder)
            .Under(buildConfigNode);

         simulationConfiguration.ExpressionProfiles.Each(x => CreateFor(x).Under(expressionsNode));

         return buildConfigNode;
      }

      public ITreeNode CreateFor(ModuleConfiguration moduleConfiguration)
      {
         var moduleConfigurationNode = new ModuleConfigurationNode(moduleConfiguration).WithIcon(ApplicationIcons.Module);
         var module = moduleConfiguration.Module;

         addModuleBuildingBlocks(moduleConfigurationNode, module);

         createAndAddNodeUnder(moduleConfigurationNode, moduleConfiguration.SelectedMoleculeStartValues);
         createAndAddNodeUnder(moduleConfigurationNode, moduleConfiguration.SelectedParameterStartValues);

         return moduleConfigurationNode;
      }

      private void addModuleBuildingBlocks(ITreeNode rootTreeNode, Module module)
      {
         addBuildingBlockNodeUnder(rootTreeNode, module.SpatialStructure);
         addBuildingBlockNodeUnder(rootTreeNode, module.Molecules);
         addBuildingBlockNodeUnder(rootTreeNode, module.Reactions);
         addBuildingBlockNodeUnder(rootTreeNode, module.PassiveTransports);
         addBuildingBlockNodeUnder(rootTreeNode, module.Observers);
         addBuildingBlockNodeUnder(rootTreeNode, module.EventGroups);
      }

      private void addBuildingBlockNodeUnder(ITreeNode rootTreeNode, IBuildingBlock buildingBlock)
      {
         createAndAddNodeUnder(rootTreeNode, buildingBlock);
      }

      public ITreeNode CreateFor(IBuildingBlock buildingBlock)
      {
         if (buildingBlock is MoleculeBuildingBlock moleculeBuildingBlock)
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

      public ITreeNode CreateFor(MoleculeBuilder moleculeBuilder)
      {
         return createFor(moleculeBuilder);
      }

      private ITreeNode createFor<T>(T objectBase) where T : class, IObjectBase
      {
         return new ObjectWithIdAndNameNode<T>(objectBase)
            .WithIcon(ApplicationIcons.IconByName(objectBase.Icon));
      }

      private void createAndAddNodeUnder(ITreeNode rootNode, IBuildingBlock buildingBlock)
      {
         if (buildingBlock == null)
            return;
         
         // TODO this used to use buildingBlockInfo to create the tree SIMULATION_CONFIGURATION
         var statusIcon = ApplicationIcons.GreenOverlayFor(buildingBlock.Icon);
         // var statusIcon = buildingBlockInfo.BuildingBlockChanged
         //    ? ApplicationIcons.RedOverlayFor(buildingBlock.Icon)
         //    : ApplicationIcons.GreenOverlayFor(buildingBlock.Icon);

         CreateFor(buildingBlock)
            .WithIcon(statusIcon)
            .Under(rootNode);
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
         }) { Icon = ApplicationIcons.UserDefinedVariability };
      }
   }
}