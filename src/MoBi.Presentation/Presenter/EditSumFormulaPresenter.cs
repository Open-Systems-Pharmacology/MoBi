using MoBi.Core.Domain.Services;
using MoBi.Core.Events;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Events;

namespace MoBi.Presentation.Presenter
{
   public interface IEditSumFormulaPresenter :
      IEditTypedFormulaPresenter,
      IListener<FormulaChangedEvent>
   {
      void ChangeVariableName(string newVariableName);
      void Validate(string formula);
      void SetFormulaString(string newFormulaString);
   }

   public class EditSumFormulaPresenter : EditTypedFormulaPresenter<IEditSumFormulaView, IEditSumFormulaPresenter, SumFormula>, IEditSumFormulaPresenter
   {
      private readonly ISumFormulaToDTOSumFormulaMapper _sumFormulaDTOMapper;
      private readonly IDescriptorConditionListPresenter<SumFormula> _descriptorConditionListPresenter;
      private SumFormulaDTO _sumFormulaDTO;
      private readonly IMoBiFormulaTask _moBiFormulaTask;
      private readonly IEditFormulaPathListPresenter _editFormulaPathListPresenter;

      public EditSumFormulaPresenter(
         IEditSumFormulaView view,
         ISumFormulaToDTOSumFormulaMapper sumFormulaDTOMapper,
         IDescriptorConditionListPresenter<SumFormula> descriptorConditionListPresenter,
         IMoBiFormulaTask moBiFormulaTask,
         IDisplayUnitRetriever displayUnitRetriever,
         IEditFormulaPathListPresenter editFormulaPathListPresenter)
         : base(view, displayUnitRetriever)
      {
         _sumFormulaDTOMapper = sumFormulaDTOMapper;
         _descriptorConditionListPresenter = descriptorConditionListPresenter;
         _moBiFormulaTask = moBiFormulaTask;
         _editFormulaPathListPresenter = editFormulaPathListPresenter;
         _view.AddDescriptorConditionListView(_descriptorConditionListPresenter.View);
         _view.AddFormulaPathListView(_editFormulaPathListPresenter.View);
         AddSubPresenters(_descriptorConditionListPresenter, _editFormulaPathListPresenter);
      }

      public override void Edit(SumFormula sumFormula)
      {
         _formula = sumFormula;
         refreshView();
      }

      private void refreshView()
      {
         _sumFormulaDTO = _sumFormulaDTOMapper.MapFrom(_formula);
         _editFormulaPathListPresenter.Edit(_formula);
         _descriptorConditionListPresenter.Edit(_formula, x => x.Criteria, BuildingBlock);
         Validate(_formula.FormulaString);
         _view.Show(_sumFormulaDTO);
      }

      public void ChangeVariableName(string newVariableName)
      {
         AddCommand(_moBiFormulaTask.ChangeVariableName(_formula, newVariableName, BuildingBlock));
      }

      public void Validate(string formulaString)
      {
         var (_, message) = _moBiFormulaTask.Validate(formulaString, _formula, BuildingBlock);
         _view.SetValidationMessage(message);
      }

      public void SetFormulaString(string newFormulaString)
      {
         AddCommand(_moBiFormulaTask.SetFormulaString(_formula, newFormulaString, BuildingBlock));
      }

      public void Handle(FormulaChangedEvent eventToHandle)
      {
         if (!canHandle(eventToHandle))
            return;

         refreshView();
      }

      private bool canHandle(FormulaEvent formulaEvent)
      {
         return Equals(formulaEvent.Formula, _formula);
      }

      public override IBuildingBlock BuildingBlock
      {
         set
         {
            base.BuildingBlock = value;
            _editFormulaPathListPresenter.BuildingBlock = value;
         }
      }

      public override bool ReadOnly
      {
         set
         {
            base.ReadOnly = value;
            _editFormulaPathListPresenter.ReadOnly = value;
         }
      }
   }
}