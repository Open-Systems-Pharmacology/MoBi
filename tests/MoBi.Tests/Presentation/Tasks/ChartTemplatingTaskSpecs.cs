using System.Collections.Generic;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands.Core;
using FakeItEasy;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Chart.Mappers;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Views.Charts;
using OSPSuite.Utility.Exceptions;

namespace MoBi.Presentation.Tasks
{
   public abstract class concern_for_ChartTemplatingTask : ContextSpecification<IChartTemplatingTask>
   {
      protected IChartTemplatePersistor _chartTemplatePeristor;
      protected IChartFromTemplateService _chartFromTeplateService;
      protected ICurveChartToCurveChartTemplateMapper _chartTemplateMapper;
      protected IMoBiApplicationController _applicationController;
      protected IDialogCreator _messagePresenter;
      protected ICloneManagerForModel _cloneManager;
      protected List<CurveChartTemplate> _existingTemplates;
      private IMoBiContext _executionContext;

      protected override void Context()
      {
         _chartTemplatePeristor = A.Fake<IChartTemplatePersistor>();
         _chartFromTeplateService = A.Fake<IChartFromTemplateService>();
         _chartTemplateMapper = A.Fake<ICurveChartToCurveChartTemplateMapper>();
         _applicationController = A.Fake<IMoBiApplicationController>();
         _messagePresenter = A.Fake<IDialogCreator>();
         _cloneManager = A.Fake<ICloneManagerForModel>();
         _executionContext = A.Fake<IMoBiContext>();

         sut = new ChartTemplatingTask(_chartTemplatePeristor, _chartFromTeplateService, _chartTemplateMapper, _applicationController,
            _messagePresenter, _cloneManager, _executionContext);

         _existingTemplates = new List<CurveChartTemplate>();
      }
   }

   public class When_cloning_a_given_chart_template_and_the_user_cancels_the_action : concern_for_ChartTemplatingTask
   {
      private CurveChartTemplate _templateToClone;

      protected override void Context()
      {
         base.Context();
         _templateToClone = new CurveChartTemplate();
         var cloneTemplate = new CurveChartTemplate();
         cloneTemplate.Curves.Add(new CurveTemplate());
         A.CallTo(() => _cloneManager.Clone(_templateToClone)).Returns(cloneTemplate);
         A.CallTo(_messagePresenter).WithReturnType<string>().Returns(string.Empty);
      }

      [Observation]
      public void should_reutrn_null()
      {
         sut.CloneTemplate(_templateToClone, _existingTemplates).ShouldBeNull();
      }
   }

   public class When_cloning_a_given_chart_template : concern_for_ChartTemplatingTask
   {
      private CurveChartTemplate _templateToClone;
      private CurveChartTemplate _cloneTemplate;
      private CurveChartTemplate _result;

      protected override void Context()
      {
         base.Context();
         _templateToClone = new CurveChartTemplate();
         _cloneTemplate = new CurveChartTemplate();
         _cloneTemplate.Curves.Add(new CurveTemplate());
         A.CallTo(() => _cloneManager.Clone(_templateToClone)).Returns(_cloneTemplate);
         A.CallTo(_messagePresenter).WithReturnType<string>().Returns("New Name");
      }

      protected override void Because()
      {
         _result = sut.CloneTemplate(_templateToClone, _existingTemplates);
      }

      [Observation]
      public void should_ask_the_user_to_enter_a_name_for_the_clone()
      {
         _result.Name.ShouldBeEqualTo("New Name");
      }

      [Observation]
      public void should_return_the_clone()
      {
         _result.ShouldBeEqualTo(_cloneTemplate);
      }
   }

   public class When_loading_a_simulation_template_from_file : concern_for_ChartTemplatingTask
   {
      private string _filePath;
      private CurveChartTemplate _template;
      private CurveChartTemplate _result;

      protected override void Context()
      {
         base.Context();
         _filePath = "ABC";
         _template = new CurveChartTemplate().WithName("TEMPLATE");
         _existingTemplates.Add(new CurveChartTemplate().WithName("TEMPLATE"));
         _existingTemplates.Add(new CurveChartTemplate().WithName("TEMPLATE2"));
         A.CallTo(() => _chartTemplatePeristor.DeserializeFromFile(_filePath)).Returns(_template);
         A.CallTo(_messagePresenter).WithReturnType<string>().Returns("NEW NAME");
      }

      protected override void Because()
      {
         _result = sut.LoadTemplateFromFile(_filePath, _existingTemplates);
      }

      [Observation]
      public void should_load_the_file_and_ask_the_user_to_enter_a_new_name_if_a_template_with_the_given_name_already_exists()
      {
         _result.Name.ShouldBeEqualTo("NEW NAME");
      }

      [Observation]
      public void should_return_the_template()
      {
         _result.ShouldBeEqualTo(_template);
      }
   }

   public class When_creating_a_new_template_from_a_chart_and_the_chart_does_not_contain_any_curve : concern_for_ChartTemplatingTask
   {
      [Observation]
      public void should_throw_a_managed_exception()
      {
         The.Action(() => sut.CreateNewTemplateFromChart(new CurveChart(), _existingTemplates)).ShouldThrowAn<OSPSuiteException>();
      }
   }

   public class When_starting_the_template_management_workflow : concern_for_ChartTemplatingTask
   {
      private IMoBiSimulation _simulation;
      private ICommand _result;
      private IModalChartTemplateManagerPresenter _chartModalTemplateManagerPresenter;
      private IChartTemplateManagerPresenter _chartTemplateManagerPresenter;
      private CurveChartTemplate _curveChartTemplate1;
      private CurveChartTemplate _curveChartTemplate2;
      protected CurveChartTemplate _cloneChartTemplate2;
      protected CurveChartTemplate _cloneChartTemplate1;

      protected override void Context()
      {
         base.Context();
         _simulation = A.Fake<IMoBiSimulation>();
         A.CallTo(() => _simulation.ChartTemplates).Returns(_existingTemplates);
         _chartTemplateManagerPresenter = A.Fake<IChartTemplateManagerPresenter>();
         _chartModalTemplateManagerPresenter = new ModalChartTemplateManagerPresenter(A.Fake<IModalChartTemplateManagerView>(), _chartTemplateManagerPresenter, _cloneManager);
         A.CallTo(() => _applicationController.Start<IModalChartTemplateManagerPresenter>()).Returns(_chartModalTemplateManagerPresenter);
         A.CallTo(_chartTemplateManagerPresenter).WithReturnType<bool>().Returns(true);

         _cloneChartTemplate1 = new CurveChartTemplate().WithName("TEMPLATE");
         _cloneChartTemplate2 = new CurveChartTemplate().WithName("TEMPLATE2");

         _curveChartTemplate1 = new CurveChartTemplate().WithName("TEMPLATE");
         _existingTemplates.Add(_curveChartTemplate1);
         _curveChartTemplate2 = new CurveChartTemplate().WithName("TEMPLATE2");
         _existingTemplates.Add(_curveChartTemplate2);

         A.CallTo(() => _cloneManager.Clone(_curveChartTemplate1)).Returns(_cloneChartTemplate1);
         A.CallTo(() => _cloneManager.Clone(_curveChartTemplate2)).Returns(_cloneChartTemplate2);
      }

      protected override void Because()
      {
         _result = sut.ManageTemplates(_simulation);
      }

      [Observation]
      public void should_retrieve_the_template_management_presenter_and_start_it()
      {
         A.CallTo(() => _chartTemplateManagerPresenter.EditTemplates(A<IEnumerable<CurveChartTemplate>>.That.Contains(_cloneChartTemplate1))).MustHaveHappened();
         A.CallTo(() => _chartTemplateManagerPresenter.EditTemplates(A<IEnumerable<CurveChartTemplate>>.That.Contains(_cloneChartTemplate2))).MustHaveHappened();
      }

      [Observation]
      public void should_return_a_replace_template_command()
      {
         _result.ShouldBeAnInstanceOf<ReplaceSimulationTemplatesCommand>();
      }
   }

   public class When_starting_the_template_management_workflow_and_the_user_cancel_the_action : concern_for_ChartTemplatingTask
   {
      private IMoBiSimulation _simulation;
      private ICommand _result;
      private IChartTemplateManagerPresenter _chartTemplateManagerPresenter;

      protected override void Context()
      {
         base.Context();
         _simulation = A.Fake<IMoBiSimulation>();
         _chartTemplateManagerPresenter = A.Fake<IChartTemplateManagerPresenter>();
         A.CallTo(() => _applicationController.Start<IChartTemplateManagerPresenter>()).Returns(_chartTemplateManagerPresenter);
         A.CallTo(_chartTemplateManagerPresenter).WithReturnType<bool>().Returns(false);
      }

      protected override void Because()
      {
         _result = sut.ManageTemplates(_simulation);
      }

      [Observation]
      public void should_return_an_empty_command()
      {
         _result.IsEmpty().ShouldBeTrue();
      }
   }
}