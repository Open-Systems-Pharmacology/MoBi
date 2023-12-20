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
      ITreeNode CreateFor(IBuildingBlock buildingBlock);
      ITreeNode CreateFor(MoleculeBuildingBlock moleculeBuildingBlock);
      ITreeNode CreateFor(MoleculeBuilder moleculeBuilder);
      ITreeNode CreateForFavorites();
      ITreeNode CreateForUserDefined();
      ITreeNode CreateFor(Module module);
      ITreeNode CreateFor(ModuleConfigurationDTO moduleConfiguration);
      ITreeNode ParameterValuesFolderNodeForModuleUnder(ModuleNode moduleNode);
      ITreeNode InitialConditionsFolderNodeForModuleUnder(ModuleNode moduleNode);
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

         simNode.AddChild(CreateFor(new SimulationSettingsDTO(simulation))
            .WithText(AppConstants.Captions.SimulationSettings)
            .WithIcon(ApplicationIcons.IconByName(simulation.Settings.Icon)));
         
         createFor(simulation.Configuration).Each(x => simNode.AddChild(x));

         if (simulation.ResultsDataRepository != null)
            simNode.AddChild(CreateFor(simulation.ResultsDataRepository));

         //uses reverse so that the first result is the last node
         simulation.HistoricResults.Reverse().Each(res => simNode.AddChild(CreateFor(res)));
         return simNode;
      }

      public ITreeNode CreateFor(Module module)
      {
         var moduleNode = new ModuleNode(module);
         addModuleBuildingBlocks(moduleNode, module);
         addStartValueCollections(moduleNode, module);

         return moduleNode;
      }

      private void addStartValueCollections(ModuleNode moduleNode, Module module)
      {
         if (module.ParameterValuesCollection.Any())
         {
            var parameterValuesFolderNode = ParameterValuesFolderNodeForModuleUnder(moduleNode);
            module.ParameterValuesCollection.Each(psv => { createAndAddNodeUnder(parameterValuesFolderNode, psv); });
         }

         if (module.InitialConditionsCollection.Any())
         {
            var initialConditionsFolderNode = InitialConditionsFolderNodeForModuleUnder(moduleNode);
            module.InitialConditionsCollection.Each(msv => createAndAddNodeUnder(initialConditionsFolderNode, msv));
         }
      }

      public ITreeNode InitialConditionsFolderNodeForModuleUnder(ModuleNode moduleNode)
      {
         return new InitialConditionsFolderNode(moduleNode.Tag).Under(moduleNode);
      }

      public ITreeNode ParameterValuesFolderNodeForModuleUnder(ModuleNode moduleNode)
      {
         return new ParameterValuesFolderNode(moduleNode.Tag).Under(moduleNode);
      }

      private IReadOnlyList<ITreeNode> createFor(SimulationConfiguration simulationConfiguration)
      {
         var expressionProfileFolderNode = new ExpressionProfileFolderNode();
         var nodes = simulationConfiguration.ModuleConfigurations.Select(moduleConfiguration => CreateFor(new ModuleConfigurationDTO(moduleConfiguration))).ToList();
         if (simulationConfiguration.Individual != null)
            nodes.Add(CreateFor(simulationConfiguration.Individual));

         simulationConfiguration.ExpressionProfiles.Each(x => createAndAddNodeUnder(expressionProfileFolderNode, x));
         nodes.Add(expressionProfileFolderNode);

         return nodes;
      }

      public ITreeNode CreateFor(ModuleConfigurationDTO moduleConfiguration)
      {
         var moduleConfigurationNode = new ModuleConfigurationNode(moduleConfiguration);
         var module = moduleConfiguration.Module;

         addModuleBuildingBlocks(moduleConfigurationNode, module);

         if (moduleConfiguration.HasInitialConditions)
            createAndAddNodeUnder(moduleConfigurationNode, moduleConfiguration.SelectedInitialConditions);
         if (moduleConfiguration.HasParameterValues)
            createAndAddNodeUnder(moduleConfigurationNode, moduleConfiguration.SelectedParameterValues);

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

         return createForBuildingBlock(buildingBlock);
      }

      public ITreeNode CreateFor(MoleculeBuildingBlock moleculeBuildingBlock)
      {
         var moleculeBuildingBlockNode = createForBuildingBlock(moleculeBuildingBlock);
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

      private ITreeNode createForBuildingBlock(IBuildingBlock buildingBlock)
      {
         return new BuildingBlockNode(buildingBlock)
            .WithIcon(ApplicationIcons.IconByName(buildingBlock.Icon));
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

         CreateFor(buildingBlock)
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
         })
         { Icon = ApplicationIcons.Favorites };
      }

      public ITreeNode CreateForUserDefined()
      {
         return new ObjectWithIdAndNameNode<ObjectBaseDTO>(new UserDefinedNodeViewItem
         {
            Name = AppConstants.Captions.UserDefined,
            Icon = ApplicationIcons.UserDefinedVariability,
            Id = AppConstants.Captions.UserDefined
         })
         { Icon = ApplicationIcons.UserDefinedVariability };
      }
   }
}