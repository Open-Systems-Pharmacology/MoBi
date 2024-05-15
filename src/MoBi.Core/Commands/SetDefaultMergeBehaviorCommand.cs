using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Helper;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;

namespace MoBi.Core.Commands
{
   public class SetDefaultMergeBehaviorCommand : MoBiReversibleCommand
   {
      private Module _module;
      private MergeBehavior _oldBehavior;
      private readonly MergeBehavior _newMergeBehavior;

      public SetDefaultMergeBehaviorCommand(Module module, MergeBehavior newMergeBehavior)
      {
         ObjectType = new ObjectTypeResolver().TypeFor<Module>();
         CommandType = AppConstants.Commands.EditCommand;
         Description = AppConstants.Commands.SetModuleMergeBehavior(module.Name, newMergeBehavior.ToString());

         _module = module;
         _newMergeBehavior = newMergeBehavior;
         ModuleId = module.Id;
         
         _module = module;
      }

      public string ModuleId { get; set; }

      protected override void ExecuteWith(IMoBiContext context)
      {
         _oldBehavior = _module.DefaultMergeBehavior;
         _module.DefaultMergeBehavior = _newMergeBehavior;
      }

      protected override void ClearReferences()
      {
         _module = null;
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new SetDefaultMergeBehaviorCommand(_module, _oldBehavior);
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         _module = context.Get<Module>(ModuleId);
      }
   }
}