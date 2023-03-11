using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Model.Diagram;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;
using OSPSuite.Assets;
using IContainer = OSPSuite.Core.Domain.IContainer;

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

      protected IMoBiSpatialStructure GetSpatialStructure()
      {
         return _interactionTaskContext.Active<IMoBiSpatialStructure>();
      }

      public override IMoBiCommand AddExisting(TParent parent, IBuildingBlock buildingBlockWithFormulaCache)
      {
         var filename = InteractionTask.AskForFileToOpen(AppConstants.Dialog.Load(_editTask.ObjectName), Constants.Filter.PKML_FILE_FILTER, Constants.DirectoryKey.MODEL_PART);
         if (filename.IsNullOrEmpty())
            return new MoBiEmptyCommand();

         var sourceSpatialStructure = InteractionTask.LoadItems<IMoBiSpatialStructure>(filename).FirstOrDefault();
         if (sourceSpatialStructure == null)
            return new MoBiEmptyCommand();

         var allAvailableContainersToImport = sourceSpatialStructure.TopContainers
            .SelectMany(x => x.GetAllContainersAndSelf<IContainer>(cont => !cont.IsAnImplementationOf<IParameter>()));

         var allImportedContainers = selectContainersToImport(allAvailableContainersToImport).ToList();
         var allImportedNeighborhoods = getConnectingNeighborhoods(allImportedContainers, sourceSpatialStructure);

         allImportedContainers.Each(registerLoadedIn);
         allImportedNeighborhoods.Each(registerLoadedIn);

         var targetSpatialStructure = GetSpatialStructure();

         var command = AddItemsToProject(allImportedContainers, parent, buildingBlockWithFormulaCache);
         if (command.IsEmpty())
            return new MoBiEmptyCommand();


         var addNeighborhoodsCommand = addNeighborhoodsToProject(allImportedNeighborhoods, targetSpatialStructure);

         if (addNeighborhoodsCommand.IsEmpty()) return command;

         var macroCommand = new MoBiMacroCommand() {CommandType = command.CommandType, ObjectType = command.ObjectType, Comment = command.Comment, Description = command.Description, ExtendedDescription = command.ExtendedDescription};
         macroCommand.Add(command);
         macroCommand.Add(addNeighborhoodsCommand);

         if (sourceSpatialStructure.DiagramModel == null || targetSpatialStructure.DiagramModel == null) return macroCommand;
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

      private IMoBiCommand addNeighborhoodsToProject(IReadOnlyList<NeighborhoodBuilder> neighborhoods, IMoBiSpatialStructure spatialStructure)
      {
         if (neighborhoods == null || !neighborhoods.Any()) return new MoBiEmptyCommand();
         var command = new MoBiMacroCommand
         {
            CommandType = AppConstants.Commands.AddCommand,
            ObjectType = ObjectTypes.Neighborhood,
            Description = AppConstants.Commands.AddDependentDescription(spatialStructure, ObjectTypes.Neighborhood, ObjectTypes.SpatialStructure)
         };

         return neighborhoods.Any(existingItem => !addNeighborhood(existingItem, command, spatialStructure)) ? CancelCommand(command) : command;
      }

      private bool addNeighborhood(NeighborhoodBuilder neighborhoodBuilder, MoBiMacroCommand command, IMoBiSpatialStructure spatialStructure)
      {
         var forbiddenNames = spatialStructure.NeighborhoodsContainer.Children.Select(x => x.Name).Union(AppConstants.UnallowedNames).ToList();
         if (forbiddenNames.Contains(neighborhoodBuilder.Name))
         {
            var newName = _interactionTaskContext.DialogCreator.AskForInput(AppConstants.Dialog.AskForChangedName(neighborhoodBuilder.Name, ObjectTypes.Neighborhood), AppConstants.Captions.NewName, neighborhoodBuilder.Name, forbiddenNames);

            if (string.IsNullOrEmpty(newName))
               return false;
            neighborhoodBuilder.Name = newName;
         }

         command.AddCommand(new AddContainerToSpatialStructureCommand(spatialStructure.NeighborhoodsContainer, neighborhoodBuilder, spatialStructure).Run(Context));
         return true;
      }

      private IEnumerable<IContainer> selectContainersToImport(IEnumerable<IContainer> containers)
      {
         using (var modal = Context.Resolve<IModalPresenter>())
         {
            var presenter = Context.Resolve<ISelectManyPresenter<IContainer>>();
            modal.Text = AppConstants.Captions.SelectEntitiesToLoad(_editTask.ObjectName);
            presenter.InitializeWith(containers);
            modal.Encapsulate(presenter);
            if (!modal.Show())
               return Enumerable.Empty<IContainer>();

            var containerToImport = presenter.Selections.ToList();
            var allChildContainer = containerToImport
               .SelectMany(container => container.GetAllChildren<IContainer>(cont => !cont.IsAnImplementationOf<IParameter>()));

            var containerDoubleImported = containerToImport.Where(allChildContainer.Contains).ToList();
            if (containerDoubleImported.Any())
            {
               containerDoubleImported.Each(c => containerToImport.Remove(c));
               DialogCreator.MessageBoxInfo(AppConstants.Exceptions.RemovedToPreventErrorDoubleImport(containerDoubleImported));
            }

            return containerToImport;
         }
      }

      private IReadOnlyList<NeighborhoodBuilder> getConnectingNeighborhoods(IEnumerable<IContainer> existingItems, ISpatialStructure tmpSpatialStructure)
      {
         var allImportedContainers = existingItems
            .SelectMany(cont => cont.GetAllContainersAndSelf<IContainer>(x => !x.IsAnImplementationOf<IParameter>()))
            .ToList();

         var neighborhoods = new List<NeighborhoodBuilder>();
         foreach (var neighborhood in tmpSpatialStructure.Neighborhoods)
         {
            var firstFound = false;
            var secondFound = false;
            foreach (var cont in allImportedContainers)
            {
               var contObjectPath = _objectPathFactory.CreateAbsoluteObjectPath(cont);
               if (Equals(neighborhood.FirstNeighborPath, contObjectPath))
                  firstFound = true;

               if (Equals(neighborhood.SecondNeighborPath, contObjectPath))
                  secondFound = true;
            }

            if (firstFound && secondFound) neighborhoods.Add(neighborhood);
         }

         return neighborhoods;
      }

      private void registerLoadedIn(IObjectBase deserializedObject)
      {
         Context.Register(deserializedObject);
      }
   }
}