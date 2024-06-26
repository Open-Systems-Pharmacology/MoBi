using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Core.Helper;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Utility.Extensions;

namespace MoBi.Core.Commands
{
   public class SetMergeBehaviorCommand : MoBiReversibleCommand
   {
      private Module _module;
      private MergeBehavior _oldBehavior;
      private readonly MergeBehavior _newMergeBehavior;

      public SetMergeBehaviorCommand(Module module, MergeBehavior newMergeBehavior)
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
         _oldBehavior = _module.MergeBehavior;
         _module.MergeBehavior = _newMergeBehavior;
         context.CurrentProject.SimulationsUsing(_module).Each(x => context.PublishEvent(new SimulationStatusChangedEvent(x)));
      }

      protected override void ClearReferences() => _module = null;

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context) => new SetMergeBehaviorCommand(_module, _oldBehavior).AsInverseFor(this);

      public override void RestoreExecutionData(IMoBiContext context) => _module = context.Get<Module>(ModuleId);
   }
}