using System.Linq;
using FakeItEasy;
using MoBi.Assets;
using MoBi.Core.Services;
using MoBi.Presentation.MenusAndBars.ContextMenus;
using MoBi.Presentation.UICommand;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Commands;
using OSPSuite.Infrastructure.Container.Castle;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Views.ContextMenus;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace MoBi.Presentation
{
   public abstract class concern_for_ContextMenuForSummaryChart : ContextSpecification<ContextMenuForSummaryChart>
   {
      protected CurveChart _chart;
      protected RenameSummaryChartUICommand _renameCommand;

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
         _renameCommand = new RenameSummaryChartUICommand(A.Fake<IOSPSuiteExecutionContext>(), A.Fake<IEventPublisher>(), A.Fake<IApplicationController>(), A.Fake<IMoBiProjectRetriever>());

         var container = A.Fake<IContainer>();
         A.CallTo(() => container.Resolve<RenameSummaryChartUICommand>()).Returns(_renameCommand);
         sut = new ContextMenuForSummaryChart(container);
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

      [Observation]
      public void the_rename_item_should_rename_the_selected_chart()
      {
         var renameItem = sut.AllMenuItems().Single(x => Equals(x.Caption, AppConstants.MenuNames.Rename)).DowncastTo<IMenuBarButton>();
         renameItem.Command.ShouldBeEqualTo(_renameCommand);
         _renameCommand.Subject.ShouldBeEqualTo(_chart);
      }
   }
}
