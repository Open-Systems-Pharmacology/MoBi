using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Model.Diagram;
using MoBi.Core.Serialization.Exchange;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Tasks.Interaction
{
   public abstract class InteractionTasksForContainerBase<TParent> : InteractionTasksForChildren<TParent, IContainer> where TParent : class, IObjectBase
   {
      protected readonly IObjectPathFactory _objectPathFactory;
      private readonly IParameterValuesTask _parameterValuesTask;

      protected InteractionTasksForContainerBase(
         IInteractionTaskContext interactionTaskContext,
         IEditTaskFor<IContainer> editTask,
         IObjectPathFactory objectPathFactory,
         IParameterValuesTask parameterValuesTask)
         : base(interactionTaskContext, editTask)
      {
         _objectPathFactory = objectPathFactory;
         _parameterValuesTask = parameterValuesTask;
      }

      public override IContainer CreateNewEntity(TParent parent)
      {
         var newEntity = base.CreateNewEntity(parent);
         var moleculeProperties = Context.Create<IContainer>()
            .WithName(Constants.MOLECULE_PROPERTIES)
            .WithMode(ContainerMode.Logical);
         newEntity.Add(moleculeProperties);
         return newEntity;
      }

      private MoBiSpatialStructure getSpatialStructure(IBuildingBlock buildingBlockWithFormulaCache)
      {
         return buildingBlockWithFormulaCache as MoBiSpatialStructure ?? _interactionTaskContext.Active<MoBiSpatialStructure>();
      }

      public override IMoBiCommand AddExisting(TParent parent, IBuildingBlock buildingBlockWithFormulaCache)
      {
         var filename = InteractionTask.AskForFileToOpen(AppConstants.Dialog.Load(_editTask.ObjectName), Constants.Filter.PKML_FILE_FILTER, Constants.DirectoryKey.MODEL_PART);
         if (filename.IsNullOrEmpty())
            return new MoBiEmptyCommand();

         var (importedSpatialStructure, parameterValues) = loadFromPKML(filename);
         if (importedSpatialStructure == null)
            return new MoBiEmptyCommand();

         var allImportedContainers = importedSpatialStructure.TopContainers;
         var allImportedNeighborhoods = importedSpatialStructure.GetConnectingNeighborhoods(allImportedContainers, _objectPathFactory);

         allImportedContainers.Each(registerLoadedIn);
         allImportedNeighborhoods.Each(registerLoadedIn);

         var spatialStructure = getSpatialStructure(buildingBlockWithFormulaCache);

         // Keep track of imported containers original names because they could be renamed when being added to the project.
         var nameCache = initializeNameChangeTracking(allImportedContainers);
         var command = AddTo(allImportedContainers, parent, buildingBlockWithFormulaCache);
         if (command.IsEmpty())
            return new MoBiEmptyCommand();

         var macroCommand = new MoBiMacroCommand
         {
            CommandType = command.CommandType,
            ObjectType = command.ObjectType,
            Comment = command.Comment,
            Description = command.Description,
            ExtendedDescription = command.ExtendedDescription
         };
         macroCommand.Add(command);

         // For all the containers that were imported, check if their names have changed and update neighborhoods accordingly
         nameCache.Keys.Each(x => updateNamesForRenamedContainer(x, nameCache, allImportedNeighborhoods, parameterValues));

         var addParameterValuesCommand = addParameterValues(parameterValues, spatialStructure.Module);

         if (!addParameterValuesCommand.IsEmpty())
            macroCommand.Add(addParameterValuesCommand);

         var addNeighborhoodsCommand = AddNeighborhoodsToSpatialStructure(allImportedNeighborhoods, spatialStructure);

         if (!addNeighborhoodsCommand.IsEmpty())
            macroCommand.Add(addNeighborhoodsCommand);

         if (importedSpatialStructure.DiagramModel == null || spatialStructure.DiagramModel == null)
            return macroCommand;

         updateDiagramLayout(allImportedContainers, importedSpatialStructure, spatialStructure);

         return macroCommand;
      }

      private void updateNamesForRenamedContainer(IContainer x, Cache<IContainer, string> nameCache, IReadOnlyList<NeighborhoodBuilder> allImportedNeighborhoods, ParameterValuesBuildingBlock parameterValuesBuildingBlock)
      {
         updateNeighborhoodsForNewContainerName(x.Name, nameCache[x], allImportedNeighborhoods);
         updateParameterValuesForNewContainerName(x.Name, nameCache[x], parameterValuesBuildingBlock);
      }

      private void updateParameterValuesForNewContainerName(string newName, string oldName, ParameterValuesBuildingBlock parameterValuesBuildingBlock)
      {
         if (string.Equals(newName, oldName) || parameterValuesBuildingBlock == null)
            return;

         parameterValuesBuildingBlock.Name = parameterValuesBuildingBlock.Name.Replace(oldName, newName);
         parameterValuesBuildingBlock.Each(x =>
         {
            x.ContainerPath.Replace(oldName, newName);
         });
      }

      private ICommand addParameterValues(ParameterValuesBuildingBlock parameterValues, Module module)
      {
         if (parameterValues == null || !parameterValues.Any())
            return new MoBiEmptyCommand();

         var parameterValuesBuildingBlock = buildingBlockToAdd(module);

         if (parameterValuesBuildingBlock != null)
            return addToExistingBuildingBlock(parameterValues.ToList(), parameterValuesBuildingBlock);

         return addAsNewBuildingBlock(parameterValues, module);
      }

      private ICommand addToExistingBuildingBlock(IReadOnlyList<ParameterValue> parameterValuesToAdd, ParameterValuesBuildingBlock parameterValuesBuildingBlock)
      {
         return _parameterValuesTask.ExtendBuildingBlockWith(parameterValuesBuildingBlock, parameterValuesToAdd);
      }

      private ParameterValuesBuildingBlock buildingBlockToAdd(Module module)
      {
         var moduleBuildingBlocks = module.ParameterValuesCollection;

         // If there are no existing building blocks, then you can only add as a new building block
         if (!moduleBuildingBlocks.Any())
            return null;

         using (var modal = ApplicationController.Start<IModalPresenter>())
         {
            var presenter = ApplicationController.Start<ISelectSinglePresenter<ParameterValuesBuildingBlock>>();
            presenter.SetDescription(AppConstants.Captions.SelectTheBuildingBlockWhereParameterValuesWillBeAddedOrUpdated);
            modal.Text = AppConstants.Captions.SelectParameterValuesBuildingBlock;
            modal.Encapsulate(presenter);

            var allItems = new List<ParameterValuesBuildingBlock>(moduleBuildingBlocks)
            {
               NullPathAndValueEntityBuildingBlocks.NewParameterValues
            };
            presenter.InitializeWith(allItems, x => !Equals(x, NullPathAndValueEntityBuildingBlocks.NewParameterValues));
            modal.CanCancel = false;
            modal.Show(AppConstants.Dialog.SELECT_SINGLE_SIZE);
            return existingBuildingBlockSelected(presenter.Selection) ? presenter.Selection : null;
         }
      }

      private static bool existingBuildingBlockSelected(ParameterValuesBuildingBlock selectedBuildingBlock) => !ReferenceEquals(selectedBuildingBlock, NullPathAndValueEntityBuildingBlocks.NewParameterValues);

      private ICommand addAsNewBuildingBlock(ParameterValuesBuildingBlock parameterValues, Module module)
      {
         var cloneOfParameterValues = _interactionTaskContext.InteractionTask.Clone(parameterValues);
         return new AddBuildingBlockToModuleCommand<ParameterValuesBuildingBlock>(cloneOfParameterValues, module).Run(_interactionTaskContext.Context);
      }

      private (MoBiSpatialStructure, ParameterValuesBuildingBlock) loadFromPKML(string filename)
      {
         try
         {
            var spatialStructureTransfer = InteractionTask.LoadTransfer<SpatialStructureTransfer>(filename);
            // Clone here because we will receive all original Ids from InteractionTask
            return (Context.Clone(spatialStructureTransfer.SpatialStructure), Context.Clone(spatialStructureTransfer.ParameterValues));
         }
         catch
         {
            return (InteractionTask.LoadItems<MoBiSpatialStructure>(filename).FirstOrDefault(), null);
         }
      }

      private void updateNeighborhoodsForNewContainerName(string newName, string oldName, IReadOnlyList<NeighborhoodBuilder> allImportedNeighborhoods)
      {
         if (string.Equals(newName, oldName))
            return;

         allImportedNeighborhoods.Each(x => updateNeighborhood(x, newName, oldName));
      }

      private static void updateNeighborhood(NeighborhoodBuilder neighborhood, string newName, string oldName)
      {
         neighborhood.Name = neighborhood.Name.Replace(oldName, newName);
         neighborhood.FirstNeighborPath.Replace(oldName, newName);
         neighborhood.SecondNeighborPath.Replace(oldName, newName);
      }

      /// <summary>
      ///    Creates a cache of container objects and their names
      /// </summary>
      private Cache<IContainer, string> initializeNameChangeTracking(IReadOnlyList<IContainer> containers)
      {
         var cache = new Cache<IContainer, string>();
         containers.Each(x => cache[x] = x.Name);
         return cache;
      }

      protected abstract IMoBiCommand AddNeighborhoodsToSpatialStructure(IReadOnlyList<NeighborhoodBuilder> neighborhoods, MoBiSpatialStructure spatialStructure);

      private void registerLoadedIn(IObjectBase deserializedObject)
      {
         Context.Register(deserializedObject);
      }

      private static void updateDiagramLayout(IReadOnlyList<IContainer> allImportedContainers, MoBiSpatialStructure importedSpatialStructure, MoBiSpatialStructure spatialStructure)
      {
         var lcs = new LayoutCopyService();

         foreach (var container in allImportedContainers)
         {
            var sourceContainer = importedSpatialStructure.DiagramModel.GetNode<IContainerNode>(container.Id);
            var targetContainer = spatialStructure.DiagramModel.GetNode<IContainerNode>(container.Id);
            try
            {
               spatialStructure.DiagramModel.BeginUpdate();
               lcs.Copy(sourceContainer, targetContainer);
            }
            finally
            {
               spatialStructure.DiagramModel.EndUpdate();
            }
         }

         if (spatialStructure.DiagramManager.IsInitialized)
            spatialStructure.DiagramManager.RefreshFromDiagramOptions();
      }
   }
}