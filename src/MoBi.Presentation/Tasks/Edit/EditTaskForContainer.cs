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

      public EditTaskForContainer(IInteractionTaskContext interactionTaskContext, IMoBiSpatialStructureFactory spatialStructureFactory) : base(interactionTaskContext)
      {
         _spatialStructureFactory = spatialStructureFactory;
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

         var tmpSpatialStructure = (IMoBiSpatialStructure)_spatialStructureFactory.Create();

         // make a backup of the parent and reset that after as there is a side effect
         // of removing the reference to parent container.
         var parent = entityToSerialize.ParentContainer;
         tmpSpatialStructure.AddTopContainer(entityToSerialize);
         entityToSerialize.ParentContainer = parent;
         var existingSpatialStructure = _interactionTaskContext.Active<IMoBiSpatialStructure>();
         if (existingSpatialStructure != null)
         {
            var neighborhoods = getConnectingNeighborhoods(new[] { entityToSerialize }, existingSpatialStructure);
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

      private IEnumerable<INeighborhoodBuilder> getConnectingNeighborhoods(IEnumerable<IContainer> existingItems, ISpatialStructure tmpSpatialStructure)
      {
         var allImportedContainers = existingItems.SelectMany(
            cont => cont.GetAllContainersAndSelf<IContainer>().Where(x => !x.IsAnImplementationOf<IParameter>())).ToList();

         var neighborhoods = new List<INeighborhoodBuilder>();
         foreach (var neighborhood in tmpSpatialStructure.Neighborhoods)
         {
            bool firstFound = false;
            bool secondFound = false;
            foreach (var cont in allImportedContainers)
            {
               if (neighborhood.FirstNeighbor.Equals(cont))
               {
                  firstFound = true;
               }

               if (neighborhood.SecondNeighbor.Equals(cont))
               {
                  secondFound = true;
               }
            }

            if (firstFound && secondFound)
            {
               neighborhoods.Add(neighborhood);
            }
         }

         return neighborhoods;
      }
   }
}