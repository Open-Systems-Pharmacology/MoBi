using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   internal abstract class concern_for_SetMergeBehaviorCommand : ContextSpecification<SetMergeBehaviorCommand>
   {
      protected Module _module;
      protected IMoBiContext _context;
      protected MoBiSimulation _affectedSimulation;
      protected MoBiProject _project;

      protected override void Context()
      {
         _module = new Module().WithId("moduleid");
         _context = A.Fake<IMoBiContext>();

         _affectedSimulation = new MoBiSimulation();
         _affectedSimulation.Configuration = new SimulationConfiguration();

         _affectedSimulation.Configuration.AddModuleConfiguration(new ModuleConfiguration(_module));
         _project = new MoBiProject();
         _project.AddSimulation(_affectedSimulation);

         A.CallTo(() => _context.CurrentProject).Returns(_project);
         A.CallTo(() => _context.Get<Module>(_module.Id)).Returns(_module);
         sut = new SetMergeBehaviorCommand(_module, MergeBehavior);
      }

      public abstract MergeBehavior MergeBehavior { get; }
   }

   internal class when_setting_module_to_extend : concern_for_SetMergeBehaviorCommand
   {
      public override MergeBehavior MergeBehavior => MergeBehavior.Extend;

      protected override void Context()
      {
         base.Context();
         _module.MergeBehavior = MergeBehavior.Overwrite;
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void the_module_merge_behavior_should_be_updated()
      {
         _module.MergeBehavior.ShouldBeEqualTo(MergeBehavior.Extend);
         A.CallTo(() => _context.PublishEvent(A<SimulationStatusChangedEvent>.That.Matches(x => x.Simulation.Equals(_affectedSimulation)))).MustHaveHappened(1, Times.Exactly);
      }
   }

   internal class when_setting_module_to_overwrite : concern_for_SetMergeBehaviorCommand
   {
      public override MergeBehavior MergeBehavior => MergeBehavior.Overwrite;

      protected override void Context()
      {
         base.Context();
         _module.MergeBehavior = MergeBehavior.Extend;
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void the_module_merge_behavior_should_be_updated()
      {
         _module.MergeBehavior.ShouldBeEqualTo(MergeBehavior.Overwrite);
         A.CallTo(() => _context.PublishEvent(A<SimulationStatusChangedEvent>.That.Matches(x => x.Simulation.Equals(_affectedSimulation)))).MustHaveHappened(1, Times.Exactly);
      }
   }

   internal class when_reversing_setting_module_to_overwrite : concern_for_SetMergeBehaviorCommand
   {
      public override MergeBehavior MergeBehavior => MergeBehavior.Overwrite;

      protected override void Context()
      {
         base.Context();
         _module.MergeBehavior = MergeBehavior.Extend;
      }

      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void the_module_merge_behavior_should_be_reverted()
      {
         _module.MergeBehavior.ShouldBeEqualTo(MergeBehavior.Extend);
      }
   }

   internal class when_reversing_setting_module_to_extend : concern_for_SetMergeBehaviorCommand
   {
      public override MergeBehavior MergeBehavior => MergeBehavior.Extend;

      protected override void Context()
      {
         base.Context();
         _module.MergeBehavior = MergeBehavior.Overwrite;
      }

      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void the_module_merge_behavior_should_be_reverted()
      {
         _module.MergeBehavior.ShouldBeEqualTo(MergeBehavior.Overwrite);
      }
   }
}
