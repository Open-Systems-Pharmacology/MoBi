using OSPSuite.Core.Services;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Tasks;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Presenter
{
   public interface IImportStartValuesForStartValueBuildingBlockPresenter : IImportQuantitiesPresenter
   {
      /// <summary>
      /// Displays a user interface to aid a user in importing parameter start values
      /// </summary>
      /// <param name="startValuesBuildingBlock"></param>
      /// <returns>The imported parameter start values</returns>
      void ImportStartValuesForBuildingBlock(IBuildingBlock startValuesBuildingBlock);
   }

   public abstract class AbstractQuantitiesImporterPresenterForBuildingBlock<T, TStartValue> : AbstractQuantitiesImporterPresenter
      where T : class, IStartValuesBuildingBlock<TStartValue>
      where TStartValue : class, IStartValue
   {
      protected readonly IMoBiContext _context;
      protected T _startValuesBuildingBlock;
      private readonly IStartValuesTask<T, TStartValue> _startValuesTask;

      protected AbstractQuantitiesImporterPresenterForBuildingBlock(IImportQuantityView view, IDialogCreator dialogCreator, IMoBiContext context, IImportFromExcelTask excelTask, IStartValuesTask<T, TStartValue> startValuesTask)
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

         _startValuesBuildingBlock = startValuesBuildingBlock;
         _view.BindTo(_importExcelSheetSelectionDTO);

         _view.Display();
      }

      public void ImportStartValuesForBuildingBlock(IBuildingBlock startValuesBuildingBlock)
      {
         ImportStartValuesForBuildingBlock(startValuesBuildingBlock.DowncastTo<T>());
      }

      public override void TransferImportedQuantities()
      {
         AddCommand(_startValuesTask.ImportStartValuesToBuildingBlock(_startValuesBuildingBlock, _quantityDTOs));
      }
   }
}