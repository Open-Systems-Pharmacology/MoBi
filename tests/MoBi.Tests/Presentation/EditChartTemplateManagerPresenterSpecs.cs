using System.Collections.Generic;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands.Core;
using FakeItEasy;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks;
using MoBi.Presentation.Views;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation
{
   public abstract class concern_for_EditChartTemplateManagerPresenter : ContextSpecification<EditChartTemplateManagerPresenter>
   {
      protected IEditChartTemplateManagerView _view;
      protected IChartTemplateManagerPresenter _chartTemplateManagerPresenter;
      protected IChartTemplatingTask _chartTemplatingTask;
      protected ICloneManager _cloneManager;
      protected ISimulationSettings _simulationSettings;

      protected override void Context()
      {
         _view = A.Fake<IEditChartTemplateManagerView>();
         _chartTemplateManagerPresenter = A.Fake<IChartTemplateManagerPresenter>();
         _chartTemplatingTask = A.Fake<IChartTemplatingTask>();
         _cloneManager = A.Fake<ICloneManager>();
         _simulationSettings = A.Fake<ISimulationSettings>();

         sut = new EditChartTemplateManagerPresenter(_view, _chartTemplateManagerPresenter, _cloneManager, _chartTemplatingTask);
         sut.InitializeWith(A.Fake<ICommandCollector>());
         sut.Edit(_simulationSettings);
      }
   }

   public class When_editing_curve_templates : concern_for_EditChartTemplateManagerPresenter
   {
      private List<CurveChartTemplate> _editedTemplates;
      private CurveChartTemplate _template1;
      private CurveChartTemplate _template1Clone;

      protected override void Context()
      {
         base.Context();
         _template1 = new CurveChartTemplate();
         _template1Clone = new CurveChartTemplate();
         A.CallTo(() => _simulationSettings.ChartTemplates).Returns(new List<CurveChartTemplate>{_template1});

         A.CallTo(() => _cloneManager.Clone(_template1)).Returns(_template1Clone);
         A.CallTo(() => _chartTemplateManagerPresenter.EditTemplates(A<IEnumerable<CurveChartTemplate>>._))
            .Invokes(x => _editedTemplates = x.GetArgument<IEnumerable<CurveChartTemplate>>(0).ToList());
      }

      protected override void Because()
      {
         sut.Edit(_simulationSettings);
      }

      [Observation]
      public void should_edit_clones_of_the_original_templates()
      {
         _editedTemplates.ShouldOnlyContain(_template1Clone);
      }
   }

   public class When_committing_changes_and_changes_have_been_made : concern_for_EditChartTemplateManagerPresenter
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _chartTemplateManagerPresenter.HasChanged).Returns(true);
      }

      protected override void Because()
      {
         sut.CommitChanges();
      }

      [Observation]
      public void A_call_to_the_templating_task_to_issue_commands_must_not_have_been_made()
      {
         A.CallTo(() => _chartTemplatingTask.ReplaceTemplatesInBuildingBlockCommand(A<ISimulationSettings>._, A<IEnumerable<CurveChartTemplate>>._)).
            MustHaveHappened();
      }
   }

   public class When_committing_changes_and_no_changes_have_actually_been_made : concern_for_EditChartTemplateManagerPresenter
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _chartTemplateManagerPresenter.HasChanged).Returns(false);
      }

      protected override void Because()
      {
         sut.CommitChanges();
      }

      [Observation]
      public void A_call_to_the_templating_task_to_issue_commands_must_not_have_been_made()
      {
         A.CallTo(() => _chartTemplatingTask.ReplaceTemplatesInBuildingBlockCommand(A<ISimulationSettings>._, A<IEnumerable<CurveChartTemplate>>._)).
            MustNotHaveHappened();
      }
   }
}
