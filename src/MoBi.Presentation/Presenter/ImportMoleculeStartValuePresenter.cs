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
   public interface IImportMoleculeStartValuePresenter : IImportStartValuesForStartValueBuildingBlockPresenter
   {
   }

   public class ImportMoleculeStartValuePresenter : AbstractQuantitiesImporterPresenterForBuildingBlock<IMoleculeStartValuesBuildingBlock, IMoleculeStartValue>, IImportMoleculeStartValuePresenter
   {
      private readonly IDataTableToImportQuantityDTOMapperForMolecules _mapper;

      public ImportMoleculeStartValuePresenter(IImportQuantityView view,
         IDialogCreator dialogCreator,
         IMoBiContext context,
         IImportFromExcelTask excelTask,
         IMoleculeStartValuesTask moleculeStartValuesTask,
         IDataTableToImportQuantityDTOMapperForMolecules mapper)
         : base(view, dialogCreator, context, excelTask, moleculeStartValuesTask)
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
         _view.Text = AppConstants.Captions.ImportMoleculeStartValues;
         _view.HintLabel = AppConstants.Captions.ImportMoleculeStartValuesFileFormatHint;
      }
   }
}
