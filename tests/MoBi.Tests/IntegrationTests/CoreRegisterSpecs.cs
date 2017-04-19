using MoBi.Core;
using MoBi.Core.Domain.Comparison;
using MoBi.Core.Domain.Model;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Comparison;
using OSPSuite.Presentation.Presenters.Diagram;
using OSPSuite.Utility.Container;

namespace MoBi.IntegrationTests
{
   public class When_registering_all_components_for_the_application_start : ContextForIntegration<CoreRegister>
   {
      protected override void Context()
      {
      }

      [Observation]
      public void should_be_able_to_find_a_diff_builder_for_a_simulation()
      {
         var diffBuilderRepository = IoC.Resolve<IDiffBuilderRepository>();
         var simuationDiffBuilder = diffBuilderRepository.BuilderFor(new MoBiSimulation());
         simuationDiffBuilder.ShouldBeAnInstanceOf<MoBiSimulationDiffBuilder>();
      }

      [Observation]
      public void should_be_able_to_find_a_base_presenter_for_a_simulation_diamgram_manager()
      {
         var presenter = IoC.Resolve<IBaseDiagramPresenter<IMoBiSimulation>>();
         presenter.ShouldNotBeNull();
      }
   }
}