using OSPSuite.BDDHelper;
using OSPSuite.Core.Services;
using FakeItEasy;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Presentation.UICommand;
using OSPSuite.Core.Domain;
using MoBi.Core.Mappers;
using OSPSuite.Core.Domain.Builder;
using MoBi.Presentation.Tasks.Interaction;

namespace MoBi.Presentation
{
   public abstract class ExportParameterValuesBuildingBlockToExcelUICommandSpecs : ContextSpecification<ExportParameterValuesBuildingBlockToExcelUICommand>
   {
      protected IMoBiProjectRetriever _projectRetriever;
      protected IDialogCreator _dialogCreator;
      protected IParameterValueBuildingBlockToParameterValuesDataTableMapper _mapper;
      protected ParameterValuesBuildingBlock _buildingBlock;

      protected override void Context()
      {
         _buildingBlock = A.Fake<ParameterValuesBuildingBlock>();
         _projectRetriever = A.Fake<IMoBiProjectRetriever>();
         _dialogCreator = A.Fake<IDialogCreator>();
         _mapper = A.Fake<IParameterValueBuildingBlockToParameterValuesDataTableMapper>();

         A.CallTo(() => _buildingBlock.Name).Returns("ParameterValues");
         sut = new ExportParameterValuesBuildingBlockToExcelUICommand(A.Fake<IParameterValuesTask>() , _mapper) { Subject = _buildingBlock };
      }
   }
  

   public class When_exporting_parameter_values_with_project_name : ExportParameterValuesBuildingBlockToExcelUICommandSpecs
   {
      private MoBiProject _project;
      private string _defaultFileName;

      protected override void Context()
      {
         base.Context();
         _defaultFileName = "project_ParameterValues_Building_Block";
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
      public void should_call_mapper_to_export_to_excel()
      {
         A.CallTo(() => _mapper.MapFrom(_buildingBlock)).MustHaveHappened();
      }
   }
}
