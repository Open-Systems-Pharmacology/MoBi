using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_RemoveChartTemplateFromSimulationSettingsCommand : ContextSpecification<RemoveChartTemplateFromSimulationSettingsCommand>
   {
      protected IMoBiContext _context;
      protected CurveChartTemplate _chartTemplate;
      protected IMoBiSimulation _simulation;

      protected override void Context()
      {
         _context = A.Fake<IMoBiContext>();
         _chartTemplate = new CurveChartTemplate { Name = "Template" };
         _simulation = new MoBiSimulation { Configuration = new SimulationConfiguration { SimulationSettings = new SimulationSettings() } };
         _simulation.AddChartTemplate(_chartTemplate);
         sut = new RemoveChartTemplateFromSimulationSettingsCommand(_chartTemplate, _simulation);
      }
   }

   public class When_removing_a_chart_template_from_a_simulation : concern_for_RemoveChartTemplateFromSimulationSettingsCommand
   {
      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void should_remove_the_template_to_the_underlying_settings()
      {
         _simulation.ChartTemplates.Contains(_chartTemplate).ShouldBeFalse();
      }
   }
}	