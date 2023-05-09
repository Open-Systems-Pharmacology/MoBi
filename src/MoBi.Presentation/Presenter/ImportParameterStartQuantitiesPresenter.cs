using System.Data;
using MoBi.Assets;
using OSPSuite.Core.Services;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Infrastructure.Import.Services;

namespace MoBi.Presentation.Presenter
{
   public interface IImportParameterValuesPresenter : IImportStartValuesForStartValueBuildingBlockPresenter
   {

   }

   public class ImportParameterValuesPresenter : AbstractQuantitiesImporterPresenterForBuildingBlock<ParameterValuesBuildingBlock, ParameterValue>, IImportParameterValuesPresenter
   {
      private readonly IDataTableToImportQuantityDTOMapperForParameters _mapper;

      public ImportParameterValuesPresenter(IImportQuantityView view,
         IDialogCreator dialogCreator,
         IMoBiContext context,
         IImportFromExcelTask excelTask,
         IParameterValuesTask parameterValuesTask,
         IDataTableToImportQuantityDTOMapperForParameters dataTableToImportParameterQuantityDTOMapper)
         : base(view, dialogCreator, context, excelTask, parameterValuesTask)
      {
         _mapper = dataTableToImportParameterQuantityDTOMapper;
      }

      public override void Initialize()
      {
         base.Initialize();
         _view.Text = AppConstants.Captions.ImportParameterValues;
         _view.HintLabel = AppConstants.Captions.ImportParameterQuantitiesFileFormatHint;
      }

      protected override QuantityImporterDTO ConvertTableToImportedQuantities(DataTable table)
      {
         return _mapper.MapFrom(table, _buildingBlock);
      }
   }
}
