using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Events;
using MoBi.Core.Helper;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Views;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility.Events;

namespace MoBi.Presentation.Presenter
{
   public interface IEditFormulaInContainerPresenter : ICommandCollectorPresenter, ISubjectPresenter, IEditFormulaPresenter
   {
      /// <summary>
      ///    Triggers the use case to create a new formula
      /// </summary>
      void AddNewFormula();
   }

   public class EditFormulaInContainerPresenter : EditFormulaPresenter<IEditFormulaInContainerView, IEditFormulaInContainerPresenter>, IEditFormulaInContainerPresenter,
      IListener<RemovedEvent>,
      IListener<FormulaChangedEvent>
   {
      public EditFormulaInContainerPresenter(IEditFormulaInContainerView view, IFormulaPresenterCache formulaPresenterCache, IMoBiContext context,
         IFormulaToFormulaInfoDTOMapper formulaDTOMapper, FormulaTypeCaptionRepository formulaTypeCaptionRepository,
         IMoBiFormulaTask formulaTask, ICircularReferenceChecker circularReferenceChecker) : base(view, formulaPresenterCache, context, formulaDTOMapper, formulaTask, formulaTypeCaptionRepository, circularReferenceChecker)
      {
         
      }

      public void AddNewFormula()
      {
         var formulaType = _formulaDTO == null ? DefaultFormulaType : _formulaDTO.Type;
         var (command, formula) = _formulaTask.CreateNewFormulaInBuildingBlock(formulaType, FormulaDimension, AllFormulaNames, _buildingBlock);
         if (formula == null)
            return;

         AddCommand(command);

         SelectFormula(formula);
         UpdateFormula();

         //once setup has been performed, raise the change event to notify presenters that formula was added
         OnStatusChanged();
      }

      public void FormulaTypeSelectionChanged(string formulaName)
      {
         SelectFormulaByTypeAndName(_formulaDTO.Type, formulaName);
         UpdateFormula();
      }
   }
}