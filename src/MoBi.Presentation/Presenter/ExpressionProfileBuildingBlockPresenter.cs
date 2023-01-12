using MoBi.Presentation.DTO;
using MoBi.Presentation.Extensions;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface IExpressionProfileBuildingBlockPresenter : IPresenter<IExpressionProfileBuildingBlockView>, IPathAndValueBuildingBlockPresenter<ExpressionParameterDTO>
   {
      void Edit(ExpressionProfileBuildingBlock expressionProfileBuildingBlock);
   }

   public class ExpressionProfileBuildingBlockPresenter : PathWithValueBuildingBlockPresenter<IExpressionProfileBuildingBlockView, IExpressionProfileBuildingBlockPresenter, ExpressionProfileBuildingBlock, ExpressionParameter, ExpressionParameterDTO>,
      IExpressionProfileBuildingBlockPresenter
   {
      private readonly IExpressionProfileBuildingBlockToExpressionProfileBuildingBlockDTOMapper _expressionProfileToDTOMapper;

      private ExpressionProfileBuildingBlockDTO _expressionProfileBuildingBlockDTO;

      public ExpressionProfileBuildingBlockPresenter(IExpressionProfileBuildingBlockView view, IExpressionProfileBuildingBlockToExpressionProfileBuildingBlockDTOMapper expressionProfileToDTOMapper,
         IInteractionTasksForExpressionProfileBuildingBlock interactionTaskForExpressionProfile, IFormulaToValueFormulaDTOMapper formulaToValueFormulaDTOMapper, IDimensionFactory dimensionFactory) : base(view, interactionTaskForExpressionProfile, formulaToValueFormulaDTOMapper, dimensionFactory)
      {
         _expressionProfileToDTOMapper = expressionProfileToDTOMapper;
      }

      public override void Edit(ExpressionProfileBuildingBlock expressionProfileBuildingBlock)
      {
         _buildingBlock = expressionProfileBuildingBlock;
         _expressionProfileBuildingBlockDTO = _expressionProfileToDTOMapper.MapFrom(expressionProfileBuildingBlock);
         _view.BindTo(_expressionProfileBuildingBlockDTO);
      }

      public override object Subject => _buildingBlock;

      public bool HasAtLeastTwoDistinctValues(int pathElementIndex)
      {
         return _expressionProfileBuildingBlockDTO.ParameterDTOs.HasAtLeastTwoDistinctValues(pathElementIndex);
      }
   }
}