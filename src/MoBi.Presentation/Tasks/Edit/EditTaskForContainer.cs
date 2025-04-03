using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Extensions;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Tasks.Edit
{
   public interface IEditTaskForContainer : IEditTaskFor<IContainer>
   {
      IMoBiCommand SetContainerMode(IBuildingBlock buildingBlock, IContainer container, ContainerMode containerMode);

      /// <summary>
      ///    Opens a dialog for the user to select file path
      /// </summary>
      /// <returns>The path if dialog is dismissed with ok, empty string if canceled</returns>
      string BrowseSavePathFor(string name);

      /// <summary>
      ///    The user can select an individual to combine with the container and export combined parameters.
      ///    The individual parameters are exported if they match the path of the <paramref name="container" /> tree
      /// </summary>
      void SaveWithIndividualAndExpression(IContainer container);
   }

   public class EditTaskForContainer : EditTaskFor<IContainer>, IEditTaskForContainer
   {
      private readonly ISpatialStructureContentExporter _spatialStructureContentExporter;

      public EditTaskForContainer(IInteractionTaskContext interactionTaskContext, ISpatialStructureContentExporter spatialStructureContentExporter) : base(interactionTaskContext)
      {
         _spatialStructureContentExporter = spatialStructureContentExporter;
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

      public void SaveWithIndividualAndExpression(IContainer container) => _spatialStructureContentExporter.SaveWithIndividualAndExpression(container);

      public override void Save(IContainer container) => _spatialStructureContentExporter.Save(container);

      public IMoBiCommand SetContainerMode(IBuildingBlock buildingBlock, IContainer container, ContainerMode containerMode) => 
         new SetContainerModeCommand(buildingBlock, container, containerMode).RunCommand(_context);

      public string BrowseSavePathFor(string name) => 
         _interactionTask.AskForFileToSave(AppConstants.Captions.Save, Constants.Filter.PKML_FILE_FILTER, Constants.DirectoryKey.MODEL_PART, name);

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