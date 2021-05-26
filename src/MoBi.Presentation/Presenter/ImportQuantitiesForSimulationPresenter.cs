using System.Data;
using MoBi.Assets;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using OSPSuite.Infrastructure.Import.Services;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Assets;

namespace MoBi.Presentation.Presenter
{
   public interface IImportQuantitiesForSimulationPresenter : IImportQuantitiesPresenter
   {
      /// <summary>
      /// Displays a user interface to aid a user in importing parameter quantities to a simulation
      /// </summary>
      /// <param name="simulation">The simulation being targeted by import</param>
      void ImportQuantitiesForSimulation(IMoBiSimulation simulation);
   }

   public class ImportQuantitiesForSimulationPresenter : AbstractQuantitiesImporterPresenter, IImportQuantitiesForSimulationPresenter
   {
      private IMoBiSimulation _simulation;
      private readonly IDataTableToImportQuantityDTOMapperForSimulations _mapper;
      private readonly IQuantityTask _quantityTask;
      private readonly IMoBiContext _context;

      public ImportQuantitiesForSimulationPresenter(
         IImportQuantityView view, 
         IDialogCreator dialogCreator, 
         IImportFromExcelTask excelTask, 
         IDataTableToImportQuantityDTOMapperForSimulations mapper,
         IQuantityTask quantityTask,
         IMoBiContext context) 
         : base(view, dialogCreator, excelTask)
      {
         _quantityTask = quantityTask;
         _mapper = mapper;
         _context = context;
      }

      protected override QuantityImporterDTO ConvertTableToImportedQuantities(DataTable table)
      {
         return _mapper.MapFrom(table, _simulation);
      }

      public override void Initialize()
      {
         InitializeWith(_context.HistoryManager);
         _view.Text = AppConstants.Captions.ImportSimulationParameters;
         _view.HintLabel = AppConstants.Captions.ImportParameterQuantitiesFileFormatHint;
      }

      public override void TransferImportedQuantities()
      {
         var macroCommand = new BulkUpdateMacroCommand
         {
            Description = AppConstants.Commands.ImportMultipleParameters,
            CommandType = AppConstants.Commands.EditCommand,
            ObjectType =ObjectTypes.Parameter
         };
         _quantityDTOs.Each(dto => macroCommand.Add(_quantityTask.SetQuantityBaseValue(dto.Path.TryResolve<IParameter>(_simulation.Model.Root), dto.QuantityInBaseUnit, _simulation)));

         AddCommand(macroCommand);
      }

      public void ImportQuantitiesForSimulation(IMoBiSimulation simulation)
      {
         _importExcelSheetSelectionDTO = new ImportExcelSheetSelectionDTO();

         _simulation = simulation;
         _view.BindTo(_importExcelSheetSelectionDTO);

         _view.Display();
      }
   }
}