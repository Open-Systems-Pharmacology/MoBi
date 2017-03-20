using System.Data;
using MoBi.Assets;
using OSPSuite.Core.Services;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Tasks;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Presenter
{
   public interface IImportParameterStartValuesPresenter : IImportStartValuesForStartValueBuildingBlockPresenter
   {

   }

   public class ImportParameterStartValuesPresenter : AbstractQuantitiesImporterPresenterForBuildingBlock<IParameterStartValuesBuildingBlock, IParameterStartValue>, IImportParameterStartValuesPresenter
   {
      private readonly IDataTableToImportQuantityDTOMapperForParameters _mapper;

      public ImportParameterStartValuesPresenter(IImportQuantityView view,
         IDialogCreator dialogCreator,
         IMoBiContext context,
         IImportFromExcelTask excelTask,
         IParameterStartValuesTask parameterStartValuesTask,
         IDataTableToImportQuantityDTOMapperForParameters dataTableToImportParameterQuantityDTOMapper)
         : base(view, dialogCreator, context, excelTask, parameterStartValuesTask)
      {
         _mapper = dataTableToImportParameterQuantityDTOMapper;
      }

      public override void Initialize()
      {
         base.Initialize();
         _view.Text = AppConstants.Captions.ImportParameterStartValues;
         _view.HintLabel = AppConstants.Captions.ImportParameterQuantitiesFileFormatHint;
      }

      protected override QuantityImporterDTO ConvertTableToImportedQuantities(DataTable table)
      {
         return _mapper.MapFrom(table, _startValuesBuildingBlock);
      }
   }
}
