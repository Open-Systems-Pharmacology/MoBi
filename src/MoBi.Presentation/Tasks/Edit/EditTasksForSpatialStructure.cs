using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Presentation.Tasks.Edit
{
   public interface IEditTasksForSpatialStructure:IEditTasksForBuildingBlock<IMoBiSpatialStructure>
   {
   }

   public class EditTasksForSpatialStructure : EditTasksForBuildingBlock<IMoBiSpatialStructure>, IEditTasksForSpatialStructure
   {
      public EditTasksForSpatialStructure(IInteractionTaskContext interactionTaskContext, IObjectTypeResolver objectTypeResolver, ICheckNameVisitor checkNamesVisitor) : base(interactionTaskContext, objectTypeResolver, checkNamesVisitor)
      {
      }

      public override bool EditEntityModal(IMoBiSpatialStructure entity, IEnumerable<IObjectBase> existingObjectsInParent, ICommandCollector commandCollector, IBuildingBlock buildingBlock)
      {
         // we edit the properties of the top container, not of Spatial Structure.
         var topContainer = entity.TopContainers.First();
         bool resolved;
         using (var modalPresenter = _applicationController.GetCreateViewFor(topContainer, commandCollector))
         {
            modalPresenter.Text = AppConstants.Captions.NewWindow(ObjectName);
            var editContainerPresenter = modalPresenter.SubPresenter.DowncastTo<IEditContainerPresenter>();
            editContainerPresenter.BuildingBlock = entity;
            editContainerPresenter.Edit(topContainer);
            resolved = modalPresenter.Show();
            //Set SpatialStructure name to container to have them more speaking
            entity.Name = topContainer.Name;
         }

         return resolved;
      }
   }
}