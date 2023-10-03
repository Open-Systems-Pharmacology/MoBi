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

         var spatialStructure = _interactionTaskContext.Active<SpatialStructure>();
         if (spatialStructure == null)
            return Enumerable.Empty<string>();

         return spatialStructure.TopContainers.Select(x => x.Name).Union(AppConstants.UnallowedNames);
      }

      public override void Save(IContainer entityToSerialize)
      {
         var fileName = _interactionTask.AskForFileToSave(AppConstants.Captions.Save, Constants.Filter.PKML_FILE_FILTER, Constants.DirectoryKey.MODEL_PART, entityToSerialize.Name);
         if (fileName.IsNullOrEmpty()) 
            return;

         var tmpSpatialStructure = (MoBiSpatialStructure) _spatialStructureFactory.Create();

         // make a backup of the parent and reset that after as there is a side effect
         // of removing the reference to parent container.
         var originalParent = entityToSerialize.ParentContainer;
         var originalParentPath = entityToSerialize.ParentPath;
         var parentPath = originalParentPath ?? _objectPathFactory.CreateAbsoluteObjectPath(originalParent);
         //this call reset the original parent. We add it back
         tmpSpatialStructure.AddTopContainer(entityToSerialize);
         entityToSerialize.ParentContainer = originalParent;

         var existingSpatialStructure = _interactionTaskContext.Active<MoBiSpatialStructure>();
         if (existingSpatialStructure != null)
         {
            var neighborhoods = existingSpatialStructure.GetConnectingNeighborhoods(new[] {entityToSerialize}, _objectPathFactory);
            neighborhoods.Each(tmpSpatialStructure.AddNeighborhood);
            if (existingSpatialStructure.DiagramModel != null)
               tmpSpatialStructure.DiagramModel = existingSpatialStructure.DiagramModel.CreateCopy(entityToSerialize.Id);
         }

         //Save the parent path before we serialize to make sure it's set properly when we import
         entityToSerialize.ParentPath = parentPath;
         _interactionTask.Save(tmpSpatialStructure, fileName);

         entityToSerialize.ParentPath = originalParentPath;

      }

      public IMoBiCommand SetContainerMode(IBuildingBlock buildingBlock, IContainer container, ContainerMode containerMode)
      {
         return new SetContainerModeCommand(buildingBlock, container, containerMode).Run(_context);
      }

      protected override IMoBiCommand GetRenameCommandFor(IContainer container, IBuildingBlock buildingBlock, string newName, string objectType)
      {
         //when renaming a container in a spatial structure, we need to ensure that we are also renaming the path in the neighborhood
         if (buildingBlock.IsAnImplementationOf<SpatialStructure>() && containerCanBePartOfNeighborhoodPath(container))
            return new RenameContainerCommand(container, newName, buildingBlock.DowncastTo<SpatialStructure>());

         return base.GetRenameCommandFor(container, buildingBlock, newName, objectType);
      }

      private bool containerCanBePartOfNeighborhoodPath(IContainer container)
      {
         return !(container.IsAnImplementationOf<NeighborhoodBuilder>() ||
                  container.IsAnImplementationOf<IParameter>() ||
                  container.IsNamed(Constants.MOLECULE_PROPERTIES));
      }
   }
}