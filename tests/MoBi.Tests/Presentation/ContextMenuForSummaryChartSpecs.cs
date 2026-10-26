using System.Linq;
using FakeItEasy;
using MoBi.Assets;
using MoBi.Presentation.MenusAndBars.ContextMenus;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Infrastructure.Container.Castle;
using OSPSuite.Presentation.Views.ContextMenus;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace MoBi.Presentation
{
   public abstract class concern_for_ContextMenuForSummaryChart : ContextSpecification<ContextMenuForSummaryChart>
   {
      protected CurveChart _chart;

      public override void GlobalContext()
      {
         base.GlobalContext();
         var container = new CastleWindsorContainer();
         OSPSuite.Utility.Container.IoC.InitializeWith(container);
         container.RegisterImplementationOf(A.Fake<IContextMenuView>());
      }

      protected override void Context()
      {
         _chart = new CurveChart {Name = "Comparison"};
         sut = new ContextMenuForSummaryChart(A.Fake<IContainer>());
      }
   }

   public class When_creating_the_context_menu_for_a_results_comparison_chart : concern_for_ContextMenuForSummaryChart
   {
      protected override void Because()
      {
         sut.InitializeWith(_chart);
      }

      [Observation]
      public void should_offer_edit_rename_and_delete_in_that_order()
      {
         sut.AllMenuItems().Select(x => x.Caption).ShouldOnlyContainInOrder(AppConstants.MenuNames.Edit, AppConstants.MenuNames.Rename, AppConstants.MenuNames.Delete);
      }
   }
}
