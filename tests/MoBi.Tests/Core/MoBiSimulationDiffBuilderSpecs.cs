using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.IntegrationTests;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Comparison;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Container;

namespace MoBi.Core
{
   public abstract class concern_for_ObjectComparer : ContextForIntegration<IObjectComparer>
   {
      protected IObjectBase _object1;
      protected IObjectBase _object2;
      protected ComparerSettings _comparerSettings;
      protected DiffReport _report;

      protected override void Context()
      {
         sut = IoC.Resolve<IObjectComparer>();
         _comparerSettings = new ComparerSettings();
      }

      protected override void Because()
      {
         _report = sut.Compare(_object1, _object2, _comparerSettings);
      }
   }

   class When_comparing_two_similar_simulations : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var m1 = new Model().WithName("S1");
         var m2 = new Model().WithName("S1");

         var bc1 = new SimulationConfiguration();
         var bc2 = new SimulationConfiguration();

         var sim1 = new MoBiSimulation { Configuration = bc1, Model = m1 }.WithName("S1");
         var sim2 = new MoBiSimulation { Configuration = bc2, Model = m2 }.WithName("S1");

         _object1 = sim1;
         _object2 = sim2;
      }

      [Observation]
      public void should_not_report_any_differences()
      {
         _report.ShouldBeEmpty();
      }
   }
}