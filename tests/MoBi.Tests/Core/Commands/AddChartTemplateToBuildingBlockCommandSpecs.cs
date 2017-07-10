using FakeItEasy;
using MoBi.Core.Domain.Model;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_AddChartTemplateToBuildingBlockCommand : ContextSpecification<AddChartTemplateToBuildingBlockCommand>
   {
      protected ISimulationSettings _buildingBlock;
      protected CurveChartTemplate _chartTemplate;

      protected override void Context()
      {
         base.Context();
         _chartTemplate = new CurveChartTemplate().WithName("name");
         _buildingBlock = new SimulationSettings();

         sut = new AddChartTemplateToBuildingBlockCommand(_chartTemplate, _buildingBlock);
      }
   }

   public class When_adding_a_new_chart_template_to_a_building_block : concern_for_AddChartTemplateToBuildingBlockCommand
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
      public void the_building_block_should_have_added_the_new_template()
      {
         _buildingBlock.ChartTemplates.ShouldContain(_chartTemplate);
      }
   }

   public class When_reversing_the_add_chart_template_command : concern_for_AddChartTemplateToBuildingBlockCommand
   {
      private IMoBiContext _context;

      protected override void Context()
      {
         base.Context();
         _context = A.Fake<IMoBiContext>();
         A.CallTo(() => _context.Get<ISimulationSettings>(A<string>._)).Returns(_buildingBlock);
      }

      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void the_chart_template_should_not_be_in_the_building_block()
      {
         _buildingBlock.ChartTemplates.ShouldBeEmpty();
      }
   }

}
