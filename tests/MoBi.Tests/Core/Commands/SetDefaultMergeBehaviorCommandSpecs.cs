using FakeItEasy;
using MoBi.Core.Domain.Model;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;

namespace MoBi.Core.Commands
{
   internal abstract class concern_for_SetDefaultMergeBehaviorCommand : ContextSpecification<SetDefaultMergeBehaviorCommand>
   {
      protected Module _module;
      protected IMoBiContext _context;

      protected override void Context()
      {
         _module = new Module().WithId("moduleid");
         _context = A.Fake<IMoBiContext>();
         A.CallTo(() => _context.Get<Module>(_module.Id)).Returns(_module);
         sut = new SetDefaultMergeBehaviorCommand(_module, MergeBehavior);
      }

      public abstract MergeBehavior MergeBehavior { get; }
   }

   internal class when_setting_module_to_extend : concern_for_SetDefaultMergeBehaviorCommand
   {
      public override MergeBehavior MergeBehavior => MergeBehavior.Extend;

      protected override void Context()
      {
         base.Context();
         _module.DefaultMergeBehavior = MergeBehavior.Overwrite;
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void the_module_merge_behavior_should_be_updated()
      {
         _module.DefaultMergeBehavior.ShouldBeEqualTo(MergeBehavior.Extend);
      }
   }

   internal class when_setting_module_to_overwrite : concern_for_SetDefaultMergeBehaviorCommand
   {
      public override MergeBehavior MergeBehavior => MergeBehavior.Overwrite;

      protected override void Context()
      {
         base.Context();
         _module.DefaultMergeBehavior = MergeBehavior.Extend;
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void the_module_merge_behavior_should_be_updated()
      {
         _module.DefaultMergeBehavior.ShouldBeEqualTo(MergeBehavior.Overwrite);
      }
   }

   internal class when_reversing_setting_module_to_overwrite : concern_for_SetDefaultMergeBehaviorCommand
   {
      public override MergeBehavior MergeBehavior => MergeBehavior.Overwrite;

      protected override void Context()
      {
         base.Context();
         _module.DefaultMergeBehavior = MergeBehavior.Extend;
      }

      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void the_module_merge_behavior_should_be_reverted()
      {
         _module.DefaultMergeBehavior.ShouldBeEqualTo(MergeBehavior.Extend);
      }
   }

   internal class when_reversing_setting_module_to_extend : concern_for_SetDefaultMergeBehaviorCommand
   {
      public override MergeBehavior MergeBehavior => MergeBehavior.Extend;

      protected override void Context()
      {
         base.Context();
         _module.DefaultMergeBehavior = MergeBehavior.Overwrite;
      }

      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void the_module_merge_behavior_should_be_reverted()
      {
         _module.DefaultMergeBehavior.ShouldBeEqualTo(MergeBehavior.Overwrite);
      }
   }
}
