using MoBi.Core.Domain.Services;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Services;

namespace MoBi.Presentation.Presenter
{
   public interface IEditSumFormulaPresenter : IEditTypedFormulaPresenter
   {
      void ChangeVariableName(string newVariableName, string oldVariableName);
   }

   public class EditSumFormulaPresenter : EditTypedFormulaPresenter<IEditSumFormulaView, IEditSumFormulaPresenter, SumFormula>, IEditSumFormulaPresenter
   {
      private readonly ISumFormulaToDTOSumFormulaMapper _sumFormulaToDTOSumFormulaMapper;
      private readonly IDescriptorConditionListPresenter<SumFormula> _descriptorConditionListPresenter;
      private SumFormulaDTO _sumFormulaDTO;
      private readonly IMoBiFormulaTask _moBiFormulaTask;

      public EditSumFormulaPresenter(IEditSumFormulaView view, ISumFormulaToDTOSumFormulaMapper sumFormulaToDTOSumFormulaMapper, IDescriptorConditionListPresenter<SumFormula> descriptorConditionListPresenter,
         IMoBiFormulaTask moBiFormulaTask, IDisplayUnitRetriever displayUnitRetriever)
         : base(view, displayUnitRetriever)
      {
         _sumFormulaToDTOSumFormulaMapper = sumFormulaToDTOSumFormulaMapper;
         _descriptorConditionListPresenter = descriptorConditionListPresenter;
         _view.AddDescriptorConditionListView(_descriptorConditionListPresenter.View);
         AddSubPresenters(_descriptorConditionListPresenter);
         _moBiFormulaTask = moBiFormulaTask;
      }

      public override void Edit(SumFormula sumFormula)
      {
         _formula = sumFormula;
         refreshView();
      }

      private void refreshView()
      {
         _sumFormulaDTO = _sumFormulaToDTOSumFormulaMapper.MapFrom(_formula);
         _descriptorConditionListPresenter.Edit(_formula, x => x.Criteria, BuildingBlock);
         _view.Show(_sumFormulaDTO);
      }

      public void ChangeVariableName(string newVariableName, string oldVariableName)
      {
         AddCommand(_moBiFormulaTask.ChangeVariableName(_formula, newVariableName, oldVariableName, BuildingBlock));
         _sumFormulaDTO.VariablePattern = _formula.VariablePattern;
      }
   }
}