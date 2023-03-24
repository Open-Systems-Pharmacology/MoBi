using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_AddChartTemplateToSimulationSettingsCommand : ContextSpecification<AddChartTemplateToSimulationSettingsCommand>
   {
      protected IMoBiContext _context;
      protected CurveChartTemplate _chartTemplate;
      protected IMoBiSimulation _simulation;

      protected override void Context()
      {
         _context = A.Fake<IMoBiContext>();
         _chartTemplate = new CurveChartTemplate {Name = "Template"};
         _simulation = new MoBiSimulation{ Configuration = new SimulationConfiguration { SimulationSettings = new SimulationSettings()}};
         sut = new AddChartTemplateToSimulationSettingsCommand(_chartTemplate, _simulation);
      }
   }

   public class When_adding_a_chart_template_to_a_simulation : concern_for_AddChartTemplateToSimulationSettingsCommand
   {
      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void should_add_the_template_to_the_underlying_settings()
      {
         _simulation.ChartTemplates.ShouldContain(_chartTemplate);
      }
   }
}