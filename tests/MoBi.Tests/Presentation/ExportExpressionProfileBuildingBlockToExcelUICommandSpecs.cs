using FakeItEasy;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Mappers;
using MoBi.Core.Services;
using MoBi.Presentation.UICommand;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;

namespace MoBi.Presentation
{
   public abstract class ExportExpressionProfileBuildingBlockToExcelUICommandSpecs : ContextSpecification<ExportExpressionProfilesBuildingBlockToExcelUICommand>
   {
      protected IMoBiProjectRetriever _projectRetriever;
      protected IDialogCreator _dialogCreator;
      protected IExpressionProfileBuildingBlockToDataTableMapper _mapper;
      protected ExpressionProfileBuildingBlock _buildingBlock;

      protected override void Context()
      {
         _buildingBlock = A.Fake<ExpressionProfileBuildingBlock>();
         _projectRetriever = A.Fake<IMoBiProjectRetriever>();
         _dialogCreator = A.Fake<IDialogCreator>();
         _mapper = A.Fake<IExpressionProfileBuildingBlockToDataTableMapper>();

         A.CallTo(() => _buildingBlock.Name).Returns("ExpressionProfile");
         sut = new ExportExpressionProfilesBuildingBlockToExcelUICommand(_projectRetriever, _dialogCreator, _mapper) { Subject = _buildingBlock };
      }
   }

   public class When_exporting_expression_profile_with_no_project_name : ExportExpressionProfileBuildingBlockToExcelUICommandSpecs
   {
      protected override void Because()
      {
         sut.Execute();
      }

      [Observation]
      public void should_prompt_for_file_save_dialog()
      {
         A.CallTo(() => _dialogCreator.AskForFileToSave(AppConstants.Captions.ExportToExcel, Constants.Filter.EXCEL_SAVE_FILE_FILTER, Constants.DirectoryKey.MODEL_PART, A<string>.Ignored, null)).MustHaveHappened();
      }
   }

   public class When_exporting_expression_profile_with_project_name : ExportExpressionProfileBuildingBlockToExcelUICommandSpecs
   {
      private MoBiProject _project;
      private string _defaultFileName;

      protected override void Context()
      {
         base.Context();
         _defaultFileName = "project_ExpressionProfile_Building_Block";
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
      public void should_prompt_for_file_save_dialog_with_default_filename()
      {
         A.CallTo(() => _dialogCreator.AskForFileToSave(AppConstants.Captions.ExportToExcel, Constants.Filter.EXCEL_SAVE_FILE_FILTER, Constants.DirectoryKey.MODEL_PART, _defaultFileName, null)).MustHaveHappened();
      }

      [Observation]
      public void should_call_mapper_to_export_to_excel()
      {
         A.CallTo(() => _mapper.MapFrom(_buildingBlock)).MustHaveHappened();
      }
   }
}