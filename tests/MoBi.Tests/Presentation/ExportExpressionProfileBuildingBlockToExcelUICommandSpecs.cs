﻿using FakeItEasy;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Mappers;
using MoBi.Core.Services;
using MoBi.Presentation.Tasks.Interaction;
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
         sut = new ExportExpressionProfilesBuildingBlockToExcelUICommand(A.Fake<IInteractionTasksForExpressionProfileBuildingBlock>() , _mapper) { Subject = _buildingBlock };
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
      public void should_call_mapper_to_export_to_excel()
      {
         A.CallTo(() => _mapper.MapFrom(_buildingBlock)).MustHaveHappened();
      }
   }
}