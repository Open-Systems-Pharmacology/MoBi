using System.Collections.Generic;
using System.Data;
using FakeItEasy;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;
using OSPSuite.Infrastructure.Import.Services;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation
{
   public abstract class concern_for_ImportInitialConditionsPresenter : ContextSpecification<ImportInitialConditionsPresenter>
   {
      protected IImportQuantityView _view;
      protected IDialogCreator _dialogCreator;
      protected IDataTableToImportQuantityDTOMapperForMolecules _dataTableToImportQuantityDTOMapperForMolecules;
      protected IInitialConditionsTask _startValuesTask;
      protected InitialConditionsBuildingBlock _buildingBlock;
      private IMoBiContext _context;
      private IImportFromExcelTask _excelTask;
      protected QuantityImporterDTO _quantityImporterDTO;

      protected override void Context()
      {
         base.Context();
         _context = A.Fake<IMoBiContext>();
         A.CallTo(() => _context.HistoryManager).Returns(A.Fake<IMoBiHistoryManager>());
         _startValuesTask = A.Fake<IInitialConditionsTask>();
         _view = A.Fake<IImportQuantityView>();
         _dialogCreator = A.Fake<IDialogCreator>();
         _dataTableToImportQuantityDTOMapperForMolecules = A.Fake<IDataTableToImportQuantityDTOMapperForMolecules>();
         _buildingBlock = new InitialConditionsBuildingBlock();
         _excelTask = A.Fake<IImportFromExcelTask>();
         sut = new ImportInitialConditionsPresenter(_view, _dialogCreator, _context, _excelTask, _startValuesTask, _dataTableToImportQuantityDTOMapperForMolecules);
         sut.Initialize();

         _quantityImporterDTO = new QuantityImporterDTO();

         new List<ImportedQuantityDTO>
         {
            new ImportedQuantityDTO {Name = "drug", ContainerPath = new ObjectPath("First")},
            new ImportedQuantityDTO {Name = "drug", ContainerPath = new ObjectPath("Second")},
         }.Each(_quantityImporterDTO.QuantityDTOs.Add);

         A.CallTo(() => _dataTableToImportQuantityDTOMapperForMolecules.MapFrom(A<DataTable>._, _buildingBlock)).Returns(_quantityImporterDTO);

         A.CallTo(() => _view.Display()).Invokes(() =>
         {
            sut.StartImport();
            sut.TransferImportedQuantities();
         });
      }
   }

   public class When_the_import_molecule_start_values_presenter_is_starting_the_import_workflow : concern_for_ImportInitialConditionsPresenter
   {
      protected override void Because()
      {
         sut.ImportStartValuesForBuildingBlock(_buildingBlock);
      }

      protected override void Context()
      {
         base.Context();
         _buildingBlock.Add(new InitialCondition { ContainerPath = new ObjectPath("First"), Name = "drug"});
         _buildingBlock.Add(new InitialCondition { ContainerPath = new ObjectPath("Second"), Name = "drug"});

         // These are both valid update scenarios. The first scenario without start value specified is only valid for update, not insert
         _quantityImporterDTO.QuantityDTOs[0].QuantityInBaseUnit = double.NaN;
         _quantityImporterDTO.QuantityDTOs[0].IsQuantitySpecified = false;
         _quantityImporterDTO.QuantityDTOs[1].ScaleDivisor = double.NaN;
         _quantityImporterDTO.QuantityDTOs[1].IsScaleDivisorSpecified = false;
      }

      [Observation]
      public void should_allow_update_only_scale_factor_or_only_start_value()
      {
         A.CallTo(() => _view.BindTo(A<QuantityImporterDTO>.That.Matches(dto => dto.Count.Equals(2)))).MustHaveHappened();
      }

      [Observation]
      public void results_in_imported_values_returned()
      {
         A.CallTo(() => _startValuesTask.ImportPathAndValueEntitiesToBuildingBlock(_buildingBlock, _quantityImporterDTO.QuantityDTOs)).MustHaveHappened();
      }

      [Observation]
      public void should_allow_updates()
      {
         A.CallTo(() => _view.BindTo(A<QuantityImporterDTO>.That.Matches(dto => dto.Count.Equals(2)))).MustHaveHappened();
      }
   }
}