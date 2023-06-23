using OSPSuite.Core.Services;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Tasks;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Infrastructure.Import.Services;

namespace MoBi.Presentation.Presenter
{
   public interface IImportStartValuesForStartValueBuildingBlockPresenter : IImportQuantitiesPresenter
   {
      /// <summary>
      /// Displays a user interface to aid a user in importing parameter start values
      /// </summary>
      /// <param name="buildingBlock"></param>
      /// <returns>The imported parameter start values</returns>
      void ImportStartValuesForBuildingBlock(IBuildingBlock buildingBlock);
   }

   public abstract class AbstractQuantitiesImporterPresenterForBuildingBlock<T, TPathAndValueEntity> : AbstractQuantitiesImporterPresenter
      where T : PathAndValueEntityBuildingBlock<TPathAndValueEntity>
      where TPathAndValueEntity : PathAndValueEntity
   {
      protected readonly IMoBiContext _context;
      protected T _buildingBlock;
      private readonly IStartValuesTask<T, TPathAndValueEntity> _startValuesTask;

      protected AbstractQuantitiesImporterPresenterForBuildingBlock(IImportQuantityView view, IDialogCreator dialogCreator, IMoBiContext context, IImportFromExcelTask excelTask, IStartValuesTask<T, TPathAndValueEntity> startValuesTask)
         : base(view, dialogCreator, excelTask)
      {
         _context = context;
         _startValuesTask = startValuesTask;
      }

      public override void Initialize()
      {
         InitializeWith(_context.HistoryManager);
      }

      public void ImportStartValuesForBuildingBlock(T startValuesBuildingBlock)
      {
         _importExcelSheetSelectionDTO = new ImportExcelSheetSelectionDTO();

         _buildingBlock = startValuesBuildingBlock;
         _view.BindTo(_importExcelSheetSelectionDTO);

         _view.Display();
      }

      public void ImportStartValuesForBuildingBlock(IBuildingBlock startValuesBuildingBlock)
      {
         ImportStartValuesForBuildingBlock(startValuesBuildingBlock.DowncastTo<T>());
      }

      public override void TransferImportedQuantities()
      {
         AddCommand(_startValuesTask.ImportPathAndValueEntitiesToBuildingBlock(_buildingBlock, _quantityDTOs));
      }
   }
}