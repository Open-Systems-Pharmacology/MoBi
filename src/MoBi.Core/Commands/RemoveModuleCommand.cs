using System.Collections.Generic;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Core.Helper;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;

namespace MoBi.Core.Commands
{
   public class RemoveModuleCommand : MoBiReversibleCommand
   {
      protected Module _module;
      private byte[] _serializationStream;

      public RemoveModuleCommand(Module module)
      {
         ObjectType = new ObjectTypeResolver().TypeFor<Module>();
         CommandType = AppConstants.Commands.DeleteCommand;
         Description = AppConstants.Commands.RemoveFromProjectDescription(ObjectType, module.Name);
         _module = module;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         var project = context.CurrentProject;
         removeFromProject(project);
         context.Unregister(_module);

         _serializationStream = context.Serialize(_module);

         // When removing a module, we are implicitly removing all the building blocks it contains.
         var removedObjects = allRemovedObjectsFrom(_module);
         context.PublishEvent(new RemovedEvent(removedObjects));
      }

      private List<IObjectBase> allRemovedObjectsFrom(Module module)
      {
         var removedObjects = new List<IObjectBase> { module };
         removedObjects.AddRange(module.BuildingBlocks);
         return removedObjects;
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         _module = context.Deserialize<Module>(_serializationStream);
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddModuleCommand(_module).AsInverseFor(this);
      }

      protected override void ClearReferences()
      {
         _module = null;
      }

      private void removeFromProject(MoBiProject project)
      {
         project.RemoveModule(_module);
      }
   }
}