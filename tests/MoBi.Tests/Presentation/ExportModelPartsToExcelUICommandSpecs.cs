using OSPSuite.BDDHelper;
using OSPSuite.Core.Services;
using FakeItEasy;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Presentation.UICommand;
using OSPSuite.Core.Domain;

namespace MoBi.Presentation
{

   public abstract class ExportModelPartsToExcelUICommandSpecs : ContextSpecification<ExportModelPartsToExcelUICommand>
   {
      protected IMoBiProjectRetriever _projectRetriever;
      protected IModelPartsToExcelExporterTask _modelPartsToExcelExporterTask;
      protected IDialogCreator _dialogCreator;
      protected IMoBiSimulation _simulation;

      protected override void Context()
      {
         _simulation = A.Fake<IMoBiSimulation>();
         _projectRetriever = A.Fake<IMoBiProjectRetriever>();
         _modelPartsToExcelExporterTask = A.Fake<IModelPartsToExcelExporterTask>();
         _dialogCreator = A.Fake<IDialogCreator>();

         A.CallTo(() => _simulation.Name).Returns("Subject");
         sut = new ExportModelPartsToExcelUICommand(_projectRetriever, _modelPartsToExcelExporterTask, _dialogCreator){Subject = _simulation};
      }
   }

   public class When_creating_export_where_project_has_no_path : ExportModelPartsToExcelUICommandSpecs
   {
      protected override void Because()
      {
         sut.Execute();
      }

      [Observation]
      public void path_name_is_undefined_but_no_exception_thrown()
      {
         A.CallTo(() => _dialogCreator.AskForFileToSave(AppConstants.Captions.ExportModelAsTables, Constants.Filter.EXCEL_SAVE_FILE_FILTER, Constants.DirectoryKey.MODEL_PART, A<string>._,null)).MustHaveHappened();
      }
   }

   public class When_creatign_export_where_project_has_path : ExportModelPartsToExcelUICommandSpecs
   {
      private MoBiProject _project;
      private string _defaultFileName;

      protected override void Context()
      {
         base.Context();
         _defaultFileName = "project_Subject_Model_Parts";
         _project = A.Fake<MoBiProject>();
         A.CallTo(() => _project.Name).Returns("project");
         A.CallTo(() => _projectRetriever.CurrentProject).Returns(_project);
         A.CallTo(_dialogCreator).WithReturnType<string>().Returns(_defaultFileName);
      }

      protected override void Because()
      {
         sut.Execute();
      }

      [Observation]
      public void path_name_is_undefined_but_no_exception_thrown()
      {
         A.CallTo(() => _dialogCreator.AskForFileToSave(AppConstants.Captions.ExportModelAsTables, Constants.Filter.EXCEL_SAVE_FILE_FILTER, Constants.DirectoryKey.MODEL_PART, _defaultFileName, null)).MustHaveHappened();
      }

      [Observation]
      public void must_call_task_to_export_to_tables()
      {
         A.CallTo(() => _modelPartsToExcelExporterTask.ExportModelPartsToExcelFile(_defaultFileName, _simulation, true)).MustHaveHappened();
      }
   }
}
