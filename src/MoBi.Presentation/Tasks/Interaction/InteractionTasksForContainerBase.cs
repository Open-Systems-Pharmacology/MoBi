using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Model.Diagram;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Assets;
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
      private readonly IObjectPathFactory _objectPathFactory;

      protected InteractionTasksForContainerBase(
         IInteractionTaskContext interactionTaskContext,
         IEditTaskFor<IContainer> editTask,
         IObjectPathFactory objectPathFactory)
         : base(interactionTaskContext, editTask)
      {
         _objectPathFactory = objectPathFactory;
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

      protected MoBiSpatialStructure GetSpatialStructure()
      {
         return _interactionTaskContext.Active<MoBiSpatialStructure>();
      }

      public override IMoBiCommand AddExisting(TParent parent, IBuildingBlock buildingBlockWithFormulaCache)
      {
         var filename = InteractionTask.AskForFileToOpen(AppConstants.Dialog.Load(_editTask.ObjectName), Constants.Filter.PKML_FILE_FILTER, Constants.DirectoryKey.MODEL_PART);
         if (filename.IsNullOrEmpty())
            return new MoBiEmptyCommand();

         var sourceSpatialStructure = InteractionTask.LoadItems<MoBiSpatialStructure>(filename).FirstOrDefault();
         if (sourceSpatialStructure == null)
            return new MoBiEmptyCommand();

         var allImportedContainers = sourceSpatialStructure.TopContainers;
         var allImportedNeighborhoods = sourceSpatialStructure.GetConnectingNeighborhoods(allImportedContainers, _objectPathFactory);

         allImportedContainers.Each(registerLoadedIn);
         allImportedNeighborhoods.Each(registerLoadedIn);

         var targetSpatialStructure = GetSpatialStructure();

         // Keep track of imported containers original names because they could be renamed when being added to the project.
         var nameCache = initializeNameChangeTracking(allImportedContainers);
         var command = AddTo(allImportedContainers, parent, buildingBlockWithFormulaCache);
         if (command.IsEmpty())
            return new MoBiEmptyCommand();

         // For all the containers that were imported, check if their names have changed and update neighborhoods accordingly
         nameCache.Keys.Each(x => updateNeighborhoodsForNewContainerName(x.Name, nameCache[x], allImportedNeighborhoods));

         var addNeighborhoodsCommand = addNeighborhoodsToSpatialStructure(allImportedNeighborhoods, targetSpatialStructure);

         if (addNeighborhoodsCommand.IsEmpty())
            return command;

         var macroCommand = new MoBiMacroCommand
         {
            CommandType = command.CommandType,
            ObjectType = command.ObjectType,
            Comment = command.Comment,
            Description = command.Description,
            ExtendedDescription = command.ExtendedDescription
         };
         macroCommand.Add(command);
         macroCommand.Add(addNeighborhoodsCommand);

         if (sourceSpatialStructure.DiagramModel == null || targetSpatialStructure.DiagramModel == null)
            return macroCommand;

         var lcs = new LayoutCopyService();

         foreach (var container in allImportedContainers)
         {
            var sourceContainer = sourceSpatialStructure.DiagramModel.GetNode<IContainerNode>(container.Id);
            var targetContainer = targetSpatialStructure.DiagramModel.GetNode<IContainerNode>(container.Id);
            try
            {
               targetSpatialStructure.DiagramModel.BeginUpdate();
               lcs.Copy(sourceContainer, targetContainer);
            }
            finally
            {
               targetSpatialStructure.DiagramModel.EndUpdate();
            }
         }

         if (targetSpatialStructure.DiagramManager.IsInitialized)
            targetSpatialStructure.DiagramManager.RefreshFromDiagramOptions();

         return macroCommand;
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

      private IMoBiCommand addNeighborhoodsToSpatialStructure(IReadOnlyList<NeighborhoodBuilder> neighborhoods, MoBiSpatialStructure spatialStructure)
      {
         var spatialStructureCast = spatialStructure as TParent;
         //we only add neighborhoods when adding a top container. 
         if (neighborhoods == null || !neighborhoods.Any() || spatialStructureCast == null)
            return new MoBiEmptyCommand();

         //we need to make sure that we use the correct interaction task for the spatial structure in order to add the neighborhoods to the top container
         var task = Context.Resolve<IInteractionTasksForChildren<IContainer, IContainer>>();
         return task.AddTo(neighborhoods, spatialStructure.NeighborhoodsContainer, spatialStructure);
      }

      private bool addNeighborhood(NeighborhoodBuilder neighborhoodBuilder, MoBiMacroCommand command, MoBiSpatialStructure spatialStructure)
      {
         var forbiddenNames = spatialStructure.NeighborhoodsContainer.AllNames().Union(AppConstants.UnallowedNames).ToList();

         if (forbiddenNames.Contains(neighborhoodBuilder.Name))
         {
            var newName = _interactionTaskContext.NamingTask.NewName(AppConstants.Dialog.AskForChangedName(neighborhoodBuilder.Name, ObjectTypes.Neighborhood), AppConstants.Captions.NewName, neighborhoodBuilder.Name, forbiddenNames);

            if (string.IsNullOrEmpty(newName))
               return false;
            neighborhoodBuilder.Name = newName;
         }

         command.AddCommand(new AddContainerToSpatialStructureCommand(spatialStructure.NeighborhoodsContainer, neighborhoodBuilder, spatialStructure).Run(Context));
         return true;
      }

      private void registerLoadedIn(IObjectBase deserializedObject)
      {
         Context.Register(deserializedObject);
      }
   }
}