using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Builder;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Tasks.Edit
{
   public interface IEditTaskForContainer : IEditTaskFor<IContainer>
   {
      IMoBiCommand SetContainerMode(IBuildingBlock buildingBlock, IContainer container, ContainerMode containerMode);
   }

   public class EditTaskForContainer : EditTaskFor<IContainer>, IEditTaskForContainer
   {
      private readonly IMoBiSpatialStructureFactory _spatialStructureFactory;
      private readonly IObjectPathFactory _objectPathFactory;

      public EditTaskForContainer(IInteractionTaskContext interactionTaskContext, IMoBiSpatialStructureFactory spatialStructureFactory, IObjectPathFactory objectPathFactory) : base(interactionTaskContext)
      {
         _spatialStructureFactory = spatialStructureFactory;
         _objectPathFactory = objectPathFactory;
      }

      protected override IEnumerable<string> GetUnallowedNames(IContainer container, IEnumerable<IObjectBase> existingObjectsInParent)
      {
         if (existingObjectsInParent != null)
            return existingObjectsInParent.AllNames();

         var spatialStructure = _interactionTaskContext.Active<ISpatialStructure>();
         if (spatialStructure == null)
            return Enumerable.Empty<string>();

         return spatialStructure.TopContainers.Select(x => x.Name).Union(AppConstants.UnallowedNames);
      }

      public override void Save(IContainer entityToSerialize)
      {
         var fileName = _interactionTask.AskForFileToSave(AppConstants.Captions.Save, Constants.Filter.PKML_FILE_FILTER, Constants.DirectoryKey.MODEL_PART, entityToSerialize.Name);
         if (fileName.IsNullOrEmpty()) return;

         var tmpSpatialStructure = (IMoBiSpatialStructure) _spatialStructureFactory.Create();

         // make a backup of the parent and reset that after as there is a side effect
         // of removing the reference to parent container.
         var parent = entityToSerialize.ParentContainer;
         tmpSpatialStructure.AddTopContainer(entityToSerialize);
         entityToSerialize.ParentContainer = parent;
         var existingSpatialStructure = _interactionTaskContext.Active<IMoBiSpatialStructure>();
         if (existingSpatialStructure != null)
         {
            var neighborhoods = getConnectingNeighborhoods(new[] {entityToSerialize}, existingSpatialStructure);
            neighborhoods.Each(tmpSpatialStructure.AddNeighborhood);
            if (existingSpatialStructure.DiagramModel != null)
               tmpSpatialStructure.DiagramModel = existingSpatialStructure.DiagramModel.CreateCopy(entityToSerialize.Id);
         }

         _interactionTask.Save(tmpSpatialStructure, fileName);
      }

      public IMoBiCommand SetContainerMode(IBuildingBlock buildingBlock, IContainer container, ContainerMode containerMode)
      {
         return new SetContainerModeCommand(buildingBlock, container, containerMode).Run(_context);
      }


      //TODO EXACT COPY PASTE from InteractionTasksForContainerBase
      private IReadOnlyList<NeighborhoodBuilder> getConnectingNeighborhoods(IEnumerable<IContainer> existingItems, ISpatialStructure tmpSpatialStructure)
      {
         var allImportedContainers = existingItems.SelectMany(
            cont => cont.GetAllContainersAndSelf<IContainer>().Where(x => !x.IsAnImplementationOf<IParameter>())).ToList();

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
   }
}