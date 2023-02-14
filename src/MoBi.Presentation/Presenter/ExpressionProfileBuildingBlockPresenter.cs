using MoBi.Core.Services;
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
      void LoadExpressionFromPKSimDatabaseQuery();
   }

   public class ExpressionProfileBuildingBlockPresenter : PathWithValueBuildingBlockPresenter<IExpressionProfileBuildingBlockView, IExpressionProfileBuildingBlockPresenter, ExpressionProfileBuildingBlock, ExpressionParameter, ExpressionParameterDTO>,
      IExpressionProfileBuildingBlockPresenter
   {
      private readonly IExpressionProfileBuildingBlockToExpressionProfileBuildingBlockDTOMapper _expressionProfileToDTOMapper;
      private readonly IPKSimStarter _pkSimStarter;

      private ExpressionProfileBuildingBlockDTO _expressionProfileBuildingBlockDTO;

      public ExpressionProfileBuildingBlockPresenter(IExpressionProfileBuildingBlockView view, IExpressionProfileBuildingBlockToExpressionProfileBuildingBlockDTOMapper expressionProfileToDTOMapper,
         IInteractionTasksForExpressionProfileBuildingBlock interactionTaskForExpressionProfile, IFormulaToValueFormulaDTOMapper formulaToValueFormulaDTOMapper, IDimensionFactory dimensionFactory, IPKSimStarter pkSimStarter) : base(view, interactionTaskForExpressionProfile, formulaToValueFormulaDTOMapper, dimensionFactory)
      {
         _expressionProfileToDTOMapper = expressionProfileToDTOMapper;
         _pkSimStarter = pkSimStarter;
      }

      public override void Edit(ExpressionProfileBuildingBlock expressionProfileBuildingBlock)
      {
         _buildingBlock = expressionProfileBuildingBlock;
         rebind();
      }

      public void LoadExpressionFromPKSimDatabaseQuery()
      {
         _pkSimStarter.GetQueryResultsFromDatabase(_expressionProfileBuildingBlockDTO.Species);
      }

      private void rebind()
      {
         _expressionProfileBuildingBlockDTO = _expressionProfileToDTOMapper.MapFrom(_buildingBlock);
         _view.BindTo(_expressionProfileBuildingBlockDTO);
      }

      public override object Subject => _buildingBlock;

      public bool HasAtLeastTwoDistinctValues(int pathElementIndex)
      {
         return _expressionProfileBuildingBlockDTO.ParameterDTOs.HasAtLeastTwoDistinctValues(pathElementIndex);
      }
   }
}