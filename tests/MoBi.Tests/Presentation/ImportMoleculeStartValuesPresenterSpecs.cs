using System.Collections.Generic;
using System.Data;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Extensions;
using FakeItEasy;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation
{
   public class concern_for_ImportMoleculeStartValuesPresenter : ContextSpecification<ImportMoleculeStartValuePresenter>
   {
      protected IImportQuantityView _view;
      protected IDialogCreator _dialogCreator;
      protected IDataTableToImportQuantityDTOMapperForMolecules _dataTableToImportQuantityDTOMapperForMolecules;
      protected IMoleculeStartValuesTask _startValuesTask;
      protected IMoleculeStartValuesBuildingBlock _buildingBlock;
      private IMoBiContext _context;
      private IImportFromExcelTask _excelTask;
      protected override void Context()
      {
         base.Context();
         _context = A.Fake<IMoBiContext>();
         A.CallTo(() => _context.HistoryManager).Returns(A.Fake<IMoBiHistoryManager>());
         _startValuesTask = A.Fake<IMoleculeStartValuesTask>();
         _view = A.Fake<IImportQuantityView>();
         _dialogCreator = A.Fake<IDialogCreator>();
         _dataTableToImportQuantityDTOMapperForMolecules = A.Fake<IDataTableToImportQuantityDTOMapperForMolecules>();
         _buildingBlock = new MoleculeStartValuesBuildingBlock();
         _excelTask = A.Fake<IImportFromExcelTask>();
         sut = new ImportMoleculeStartValuePresenter(_view, _dialogCreator, _context, _excelTask, _startValuesTask, _dataTableToImportQuantityDTOMapperForMolecules);
         sut.Initialize();
      }
   }

   public class setup_for_general_import_case : concern_for_ImportMoleculeStartValuesPresenter
   {
      protected QuantityImporterDTO _quantityImporterDTO;

      protected override void Context()
      {
         base.Context();
         _quantityImporterDTO = new QuantityImporterDTO();
         
         new List<ImportedQuantityDTO>
         {
            new ImportedQuantityDTO {Name =  "drug", ContainerPath = new ObjectPath("First")},
            new ImportedQuantityDTO {Name =  "drug", ContainerPath = new ObjectPath("Second")},
         }.Each(_quantityImporterDTO.QuantitDTOs.Add);

         A.CallTo(() => _dataTableToImportQuantityDTOMapperForMolecules.MapFrom(A<DataTable>._, _buildingBlock)).Returns(_quantityImporterDTO);

         A.CallTo(() => _view.Display()).Invokes(() =>
         {
            sut.StartImport();
            sut.TransferImportedQuantities();
         });
      }

      protected override void Because()
      {
         sut.ImportStartValuesForBuildingBlock(_buildingBlock);
      }
   }

   public class when_validating_for_update : setup_for_general_import_case
   {
      protected override void Context()
      {
         base.Context();
         _buildingBlock.Add(new MoleculeStartValue{ContainerPath = new ObjectPath("First"), Name = "drug"});
         _buildingBlock.Add(new MoleculeStartValue{ContainerPath = new ObjectPath("Second"), Name = "drug"});

         // These are both valid update scenarios. The first scenario without start value specified is only valid for update, not insert
         _quantityImporterDTO.QuantitDTOs[0].QuantityInBaseUnit = double.NaN;
         _quantityImporterDTO.QuantitDTOs[0].IsQuantitySpecified = false;
         _quantityImporterDTO.QuantitDTOs[1].ScaleDivisor = double.NaN;
         _quantityImporterDTO.QuantitDTOs[1].IsScaleDivisorSpecified = false;
      }

      [Observation]
      public void should_allow_update_only_scale_factor_or_only_start_value()
      {
         A.CallTo(() => _view.BindTo(A<QuantityImporterDTO>.That.Matches(dto => dto.Count.Equals(2)))).MustHaveHappened();
      }
   }
   
   public class after_msv_successful_import : setup_for_general_import_case
   {
      [Observation]
      public void results_in_imported_values_returned()
      {
         A.CallTo(() => _startValuesTask.ImportStartValuesToBuildingBlock(_buildingBlock, _quantityImporterDTO.QuantitDTOs)).MustHaveHappened();
      }

      [Observation]
      public void should_allow_updates()
      {
         A.CallTo(() => _view.BindTo(A<QuantityImporterDTO>.That.Matches(dto => dto.Count.Equals(2)))).MustHaveHappened();
      }
   }


}
