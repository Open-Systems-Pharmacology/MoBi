using System.Data;
using MoBi.Assets;
using OSPSuite.Core.Services;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using OSPSuite.Infrastructure.Import.Services;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Presenter
{
   public interface IImportInitialConditionsPresenter : IImportStartValuesForStartValueBuildingBlockPresenter
   {
   }

   public class ImportInitialConditionsPresenter : AbstractQuantitiesImporterPresenterForBuildingBlock<InitialConditionsBuildingBlock, InitialCondition>, IImportInitialConditionsPresenter
   {
      private readonly IDataTableToImportQuantityDTOMapperForMolecules _mapper;

      public ImportInitialConditionsPresenter(IImportQuantityView view,
         IDialogCreator dialogCreator,
         IMoBiContext context,
         IImportFromExcelTask excelTask,
         IInitialConditionsTask initialConditionsTask,
         IDataTableToImportQuantityDTOMapperForMolecules mapper)
         : base(view, dialogCreator, context, excelTask, initialConditionsTask)
      {
         _mapper = mapper;
      }

      protected override QuantityImporterDTO ConvertTableToImportedQuantities(DataTable table)
      {
         return _mapper.MapFrom(table, _startValuesBuildingBlock);
      }

      public override void Initialize()
      {
         base.Initialize();
         _view.Text = AppConstants.Captions.ImportInitialConditions;
         _view.HintLabel = AppConstants.Captions.ImportInitialConditionsFileFormatHint;
      }
   }
}
