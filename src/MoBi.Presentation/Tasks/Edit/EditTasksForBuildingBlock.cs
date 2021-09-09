using System.Collections.Generic;
using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Utility.Extensions;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Domain.Services;
using IContainer = OSPSuite.Utility.Container.IContainer;
using MoBi.Core.Serialization.Xml.Services;

namespace MoBi.Presentation.Tasks.Edit
{
   public interface IEditTasksForBuildingBlock<T> : IEditTaskFor<T> where T : class,IObjectBase
   {
      void EditBuildingBlock(IBuildingBlock buildingBlock);
   }

   public class EditTasksForBuildingBlock<T> : EditTaskFor<T>, IEditTasksForBuildingBlock<T> where T : class, IObjectBase
   {
      public EditTasksForBuildingBlock(
         IInteractionTaskContext interactionTaskContext,
         IMoBiXmlSerializerRepository xmlSerializerRepository = null,
         IContainer container = null,
         IDimensionFactory dimensionFactory = null,
         IObjectBaseFactory objectBaseFactory = null,
         ICloneManagerForModel cloneManagerForModel = null) : base(interactionTaskContext, xmlSerializerRepository, container, dimensionFactory, objectBaseFactory, cloneManagerForModel)
      {
      }

      public override void Edit(T buildingBlock)
      {
         editEntiy(buildingBlock);
         base.Edit(buildingBlock);
      }

      public void EditBuildingBlock(IBuildingBlock buildingBlock)
      {
         Edit(buildingBlock.DowncastTo<T>());
      }

      private void editEntiy(T entity)
      {
         editPresenterFor(entity).Edit(entity);
      }

      private IEditPresenter editPresenterFor(T entity)
      {
         return _applicationController.Open(entity, _context.HistoryManager);
      }

      public override bool EditEntityModal(T entity, IEnumerable<IObjectBase> existingObjectsInParent, ICommandCollector commandCollector, IBuildingBlock buildingBlock)
      {
         var forbiddenNames = GetForbiddenNamesWithoutSelf(entity, existingObjectsInParent);
         var name = _interactionTaskContext.DialogCreator.AskForInput(AppConstants.Dialog.AskForNewName(ObjectName),
            AppConstants.Captions.NewWindow(ObjectName), string.Empty,forbiddenNames);

         if (name.IsNullOrEmpty())
            return false;

         entity.Name = name;
         return true;
      }

      protected override IEnumerable<string> GetUnallowedNames(T objectBase, IEnumerable<IObjectBase> existingObjectsInParent)
      {
         return _context.CurrentProject.All<T>().AllNames();
      }
   }
}