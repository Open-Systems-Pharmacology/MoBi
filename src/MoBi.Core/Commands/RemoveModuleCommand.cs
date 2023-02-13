using MoBi.Assets;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Core.Helper;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Utility.Extensions;

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
         _module.AllBuildingBlocks().Each(context.Unregister);
         _serializationStream = context.Serialize(_module);
         context.PublishEvent(new RemovedEvent(_module, project));
      }

      // TODO test with implementation of https://github.com/Open-Systems-Pharmacology/MoBi/issues/822
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

      private void removeFromProject(IMoBiProject project)
      {
         project.RemoveModule(_module);
      }
   }
}