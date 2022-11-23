using System.Collections.Generic;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Extensions;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.Presenters;

using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Presenter
{
   public interface IExpressionProfileBuildingBlockPresenter : IPresenter<IExpressionProfileBuildingBlockView>, IBreadCrumbsPresenter
   {
      void Edit(ExpressionProfileBuildingBlock expressionProfileBuildingBlock);
      void SetExpressionParameterValue(ExpressionParameterDTO expressionParameterDTO, double? newValue);
      void SetUnit(ExpressionParameterDTO expressionParameter, Unit unit);
      void SetFormula(ExpressionParameterDTO expressionParameterDTO, IFormula newValueFormula);
      IEnumerable<ValueFormulaDTO> AllFormulas();
      void AddNewFormula(ExpressionParameterDTO expressionParameterDTO);
   }

   public class ExpressionProfileBuildingBlockPresenter : PathWithValueBuildingBlockPresenter<IExpressionProfileBuildingBlockView, IExpressionProfileBuildingBlockPresenter, ExpressionProfileBuildingBlock, ExpressionParameter, ExpressionParameterDTO>, 
      IExpressionProfileBuildingBlockPresenter
   {
      private readonly IExpressionProfileBuildingBlockToExpressionProfileBuildingBlockDTOMapper _expressionProfileToDTOMapper;
      private readonly IInteractionTasksForExpressionProfileBuildingBlock _interactionTaskForExpressionProfile;
      private ExpressionProfileBuildingBlockDTO _expressionProfileBuildingBlockDTO;

      public ExpressionProfileBuildingBlockPresenter(IExpressionProfileBuildingBlockView view, IExpressionProfileBuildingBlockToExpressionProfileBuildingBlockDTOMapper expressionProfileToDTOMapper, 
         IInteractionTasksForExpressionProfileBuildingBlock interactionTaskForExpressionProfile) : base(view, interactionTaskForExpressionProfile)
      {
         _expressionProfileToDTOMapper = expressionProfileToDTOMapper;
         _interactionTaskForExpressionProfile = interactionTaskForExpressionProfile;
      }

      public override void Edit(ExpressionProfileBuildingBlock expressionProfileBuildingBlock)
      {
         _buildingBlock = expressionProfileBuildingBlock;
         _expressionProfileBuildingBlockDTO = _expressionProfileToDTOMapper.MapFrom(expressionProfileBuildingBlock);
         _view.BindTo(_expressionProfileBuildingBlockDTO);
      }


      public override object Subject => _buildingBlock;

      public void SetExpressionParameterValue(ExpressionParameterDTO expressionParameterDTO, double? newValue)
      {
         AddCommand(_interactionTaskForExpressionProfile.SetValue(_buildingBlock, newValue, expressionParameterDTO.ExpressionParameter));
      }

      public void SetFormula(ExpressionParameterDTO expressionParameterDTO, IFormula formula)
      {
         SetFormulaInBuilder(expressionParameterDTO, formula, expressionParameterDTO.ExpressionParameter);
      }

      public void AddNewFormula(ExpressionParameterDTO expressionParameterDTO)
      {
         AddNewFormula(expressionParameterDTO, expressionParameterDTO.ExpressionParameter);
      }

      public void SetUnit(ExpressionParameterDTO expressionParameter, Unit unit)
      {
         SetUnit(expressionParameter.ExpressionParameter, unit);
      }

      public bool HasAtLeastTwoDistinctValues(int pathElementIndex)
      {
         return _expressionProfileBuildingBlockDTO.ExpressionParameters.HasAtLeastTwoDistinctValues(pathElementIndex);
      }
   }

   public interface IExpressionProfileBuildingBlockView : IView<IExpressionProfileBuildingBlockPresenter>
   {
      void BindTo(ExpressionProfileBuildingBlockDTO buildingBlockDTO);
   }
}