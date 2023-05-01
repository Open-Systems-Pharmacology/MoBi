using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Extensions;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Events;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility.Events;

namespace MoBi.Presentation.Presenter
{
   public interface IExpressionProfileBuildingBlockPresenter : IPresenter<IExpressionProfileBuildingBlockView>, IPathAndValueBuildingBlockPresenter<ExpressionParameterDTO>
   {
      void Edit(ExpressionProfileBuildingBlock expressionProfileBuildingBlock);
      void LoadExpressionFromPKSimDatabaseQuery();
   }

   public class ExpressionProfileBuildingBlockPresenter : PathWithValueBuildingBlockPresenter<IExpressionProfileBuildingBlockView, IExpressionProfileBuildingBlockPresenter, MoBiProject, ExpressionProfileBuildingBlock, ExpressionParameter, ExpressionParameterDTO>,
      IExpressionProfileBuildingBlockPresenter, IListener<RenamedEvent>
   {
      private readonly IExpressionProfileBuildingBlockToExpressionProfileBuildingBlockDTOMapper _expressionProfileToDTOMapper;

      private ExpressionProfileBuildingBlockDTO _expressionProfileBuildingBlockDTO;
      private readonly IInteractionTasksForExpressionProfileBuildingBlock _interactionTasksForExpressionProfile;

      public ExpressionProfileBuildingBlockPresenter(IExpressionProfileBuildingBlockView view, IExpressionProfileBuildingBlockToExpressionProfileBuildingBlockDTOMapper expressionProfileToDTOMapper,
         IInteractionTasksForExpressionProfileBuildingBlock interactionTaskForExpressionProfile, IFormulaToValueFormulaDTOMapper formulaToValueFormulaDTOMapper, IDimensionFactory dimensionFactory) : base(view, interactionTaskForExpressionProfile, formulaToValueFormulaDTOMapper, dimensionFactory)
      {
         _expressionProfileToDTOMapper = expressionProfileToDTOMapper;
         _interactionTasksForExpressionProfile = interactionTaskForExpressionProfile;
      }

      public override void Edit(ExpressionProfileBuildingBlock expressionProfileBuildingBlock)
      {
         _buildingBlock = expressionProfileBuildingBlock;
         rebind();
      }

      public void LoadExpressionFromPKSimDatabaseQuery()
      {
         AddCommand(_interactionTasksForExpressionProfile.UpdateExpressionProfileFromDatabase(_buildingBlock));
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

      public void Handle(RenamedEvent eventToHandle)
      {
         if (Equals(eventToHandle.RenamedObject, _buildingBlock))
            rebind();
      }
   }
}