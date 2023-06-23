using System.Linq;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_RemoveChartTemplateFromBuildingBlockCommand : ContextSpecification<RemoveChartTemplateFromBuildingBlockCommand>
   {
      protected SimulationSettings _buildingBlock;
      protected CurveChartTemplate _chartTemplate;

      protected override void Context()
      {
         base.Context();
         _chartTemplate = new CurveChartTemplate().WithName("name");
         _buildingBlock = new SimulationSettings();
         _buildingBlock.AddChartTemplate(_chartTemplate);

         sut = new RemoveChartTemplateFromBuildingBlockCommand(_chartTemplate, _buildingBlock);
      }
   }

   public class When_removing_curve_chart_template_from_simulation_settings_building_block : concern_for_RemoveChartTemplateFromBuildingBlockCommand
   {
      private IMoBiContext _context;

      protected override void Context()
      {
         base.Context();
         _context = A.Fake<IMoBiContext>();
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void the_chart_template_should_have_been_removed()
      {
         _buildingBlock.ChartTemplates.ShouldBeEmpty();
      }
   }

   public class When_reversing_the_remove_chart_template_command : concern_for_RemoveChartTemplateFromBuildingBlockCommand
   {
      private IMoBiContext _context;

      protected override void Context()
      {
         base.Context();
         _context = A.Fake<IMoBiContext>();
         A.CallTo(() => _context.Get<SimulationSettings>(A<string>._)).Returns(_buildingBlock);

         var serialStream = new byte[0];
         A.CallTo(() => _context.Serialize(_chartTemplate)).Returns(serialStream);
         A.CallTo(() => _context.Deserialize<CurveChartTemplate>(serialStream)).Returns(new CurveChartTemplate().WithName(_chartTemplate.Name));
      }

      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void the_chart_template_should_be_in_the_building_block()
      {
         _buildingBlock.ChartTemplates.FirstOrDefault(x => string.Equals(_chartTemplate.Name, x.Name)).ShouldNotBeNull();
      }
   }
}