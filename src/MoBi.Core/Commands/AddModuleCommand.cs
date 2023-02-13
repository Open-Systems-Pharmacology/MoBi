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
   public class AddModuleCommand : MoBiReversibleCommand, ISilentCommand
   {
      protected Module _module;
      public string ModuleId { get; private set; }
      public bool Silent { get; set; }

      public AddModuleCommand(Module module)
      {
         ObjectType = new ObjectTypeResolver().TypeFor<Module>();
         CommandType = AppConstants.Commands.AddCommand;
         _module = module;
         ModuleId = module.Id;
         Description = AppConstants.Commands.AddToProjectDescription(ObjectType, module.Name);
         Silent = false;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         var project = context.CurrentProject;
         context.Register(_module);
         _module.AllBuildingBlocks().Each(context.Register);
         addToProject(project);

         if (!Silent)
            context.PublishEvent(new AddedEvent<Module>(_module, project));
      }

      // TODO test with implementation of https://github.com/Open-Systems-Pharmacology/MoBi/issues/822
      public override void RestoreExecutionData(IMoBiContext context)
      {
         _module = context.Get<Module>(ModuleId);
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveModuleCommand(_module).AsInverseFor(this);
      }

      protected override void ClearReferences()
      {
         _module = null;
      }

      private void addToProject(IMoBiProject project)
      {
         project.AddModule(_module);
      }
   }
}