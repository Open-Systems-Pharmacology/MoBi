using System.Collections.Generic;
using System.Data;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Extensions;
using FakeItEasy;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Helpers;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using OSPSuite.Infrastructure.Import.Services;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation
{
   public abstract class concern_for_ImportParameterStartValuesPresenter : ContextSpecification<IImportStartValuesForStartValueBuildingBlockPresenter>
   {
      protected IImportQuantityView _view;
      protected IDialogCreator _dialogCreator;
      protected IDataTableToImportQuantityDTOMapperForParameters _dataTableToImportParameterQuantityDTOMapperForMolecules;
      protected IParameterStartValuesTask _startValuesTask;
      protected IParameterStartValuesBuildingBlock _buildingBlock;
      private IMoBiContext _context;
      protected IImportFromExcelTask _excelTask;
      protected ImportExcelSheetSelectionDTO _importExcelSheetDTO;

      protected override void Context()
      {
         _context = A.Fake<IMoBiContext>();
         A.CallTo(() => _context.HistoryManager).Returns(A.Fake<IMoBiHistoryManager>());
         _startValuesTask = A.Fake<IParameterStartValuesTask>();
         _view = A.Fake<IImportQuantityView>();
         _dialogCreator = A.Fake<IDialogCreator>();
         _dataTableToImportParameterQuantityDTOMapperForMolecules = A.Fake<IDataTableToImportQuantityDTOMapperForParameters>();
         _buildingBlock = A.Fake<IParameterStartValuesBuildingBlock>();
         _excelTask = A.Fake<IImportFromExcelTask>();
         sut = new ImportParameterStartValuesPresenter(_view, _dialogCreator, _context, _excelTask, _startValuesTask, _dataTableToImportParameterQuantityDTOMapperForMolecules);
         sut.Initialize();

         A.CallTo(() => _view.BindTo(A<ImportExcelSheetSelectionDTO>._))
            .Invokes(x => _importExcelSheetDTO = x.GetArgument<ImportExcelSheetSelectionDTO>(0));

      }
   }

   public class When_retrieiving_sheets_from_excel_file : concern_for_ImportParameterStartValuesPresenter
   {
      private string _path;
      private IReadOnlyList<string> _allSheets;

      protected override void Context()
      {
         base.Context();
         _allSheets = new List<string> {"Sheet1", "Sheet2"};
         _path = DomainHelperForSpecs.TestFileFullPath("psv.xlsx");
         A.CallTo(_dialogCreator).WithReturnType<string>().Returns(_path);
         A.CallTo(() => _excelTask.RetrieveExcelSheets(_path, true)).Returns(_allSheets);
         sut.ImportStartValuesForBuildingBlock(_buildingBlock);
      }

      protected override void Because()
      {
         sut.SelectFile();
      }

      [Observation]
      public void should_have_set_the_expected_files_in_the_view()
      {
         _importExcelSheetDTO.AllSheetNames.ShouldOnlyContain("Sheet1", "Sheet2");
      }

      [Observation]
      public void should_have_selected_the_first_sheet()
      {
         _importExcelSheetDTO.SelectedSheet.ShouldBeEqualTo("Sheet1");
      }
   }


   public class When_the_import_start_value_presenter_is_importing_values_for_a_given_building_block : concern_for_ImportParameterStartValuesPresenter
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _view.Display()).Invokes(sut.SelectFile);
      }

      protected override void Because()
      {
         sut.ImportStartValuesForBuildingBlock(_buildingBlock);
      }

      [Observation]
      public void should_ask_the_user_to_select_a_file()
      {
         A.CallTo(() => _dialogCreator.AskForFileToOpen(AppConstants.BrowseForFile, Constants.Filter.EXCEL_OPEN_FILE_FILTER, Constants.DirectoryKey.XLS_IMPORT, null, null)).MustHaveHappened();
      }

      [Observation]
      public void should_display_the_view()
      {
         A.CallTo(() => _view.Display()).MustHaveHappened();
      }

      [Observation]
      public void view_must_be_bound_to_dto()
      {
         A.CallTo(() => _view.BindTo(A<ImportExcelSheetSelectionDTO>._)).MustHaveHappened();
      }
   }

   public class When_the_import_start_value_presenter_is_importing_values_for_a_given_building_block_and_the_import_is_successful : concern_for_ImportParameterStartValuesPresenter
   {
      protected QuantityImporterDTO _quantityImporterDTO;


      protected override void Context()
      {
         base.Context();
         _quantityImporterDTO = new QuantityImporterDTO();
         new List<ImportedQuantityDTO>
         {
            new ImportedQuantityDTO{ContainerPath=new ObjectPath(new[] { "Path1" })}, 
            new ImportedQuantityDTO{ContainerPath=new ObjectPath(new[] { "Path2" })},
            new ImportedQuantityDTO{ContainerPath=new ObjectPath(new[] { "Path3" }),QuantityInBaseUnit = 0.0}
         }.Each(_quantityImporterDTO.QuantityDTOs.Add);

         A.CallTo(() => _dataTableToImportParameterQuantityDTOMapperForMolecules.MapFrom(A<DataTable>._, A<IParameterStartValuesBuildingBlock>.Ignored)).Returns(_quantityImporterDTO);
         A.CallTo(() => _buildingBlock[_quantityImporterDTO.QuantityDTOs[0].ContainerPath]).Returns(null);
         A.CallTo(() => _buildingBlock[_quantityImporterDTO.QuantityDTOs[1].ContainerPath]).Returns(null);
         A.CallTo(() => _buildingBlock[_quantityImporterDTO.QuantityDTOs[2].ContainerPath]).Returns(new ParameterStartValue{Path=new ObjectPath(new[] { "Path3" })});
         A.CallTo(() => _view.Display()).Invokes(() =>
         {
            sut.StartImport();
            sut.TransferImportedQuantities();
         });

         sut.ImportStartValuesForBuildingBlock(_buildingBlock);
      }

      [Observation]
      public void imported_values_added_via_start_values_task()
      {
         A.CallTo(() => _startValuesTask.ImportStartValuesToBuildingBlock(_buildingBlock, _quantityImporterDTO.QuantityDTOs)).MustHaveHappened();
      }
   }

   public class When_the_import_start_value_presenter_is_importing_values_for_a_given_building_block_and_the_import_is_cancelled : concern_for_ImportParameterStartValuesPresenter
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _view.Display()).Invokes(() => sut.StartImport());
         A.CallTo(() => _view.Canceled).Returns(true);
      }

      protected override void Because()
      {
         sut.ImportStartValuesForBuildingBlock(_buildingBlock);
      }

      [Observation]
      public void returns_empty_list_of_imported_start_values()
      {
         A.CallTo(() => _startValuesTask.AddStartValueToBuildingBlock(_buildingBlock, A<ParameterStartValue>.Ignored)).MustNotHaveHappened();
      }
   }
}
