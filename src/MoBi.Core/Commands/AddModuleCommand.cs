﻿using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Events;
using MoBi.Core.Helper;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;

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

         var moduleRegistrationVisitor = context.Resolve<IModuleRegisterVisitor>();

         _module.AcceptVisitor(moduleRegistrationVisitor);
         
         addToProject(project);

         if (!Silent)
            context.PublishEvent(new AddedEvent<Module>(_module, project));
      }

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