using System.Collections.Generic;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Presentation.Presenter
{
   public interface IExpressionProfileInitialConditionsPresenter : IBuildingBlockWithInitialConditionsPresenter<ExpressionProfileBuildingBlock>
   {
   }

   public class ExpressionProfileInitialConditionsPresenter : BuildingBlockWithInitialConditionsPresenter<IInitialConditionsView, IBuildingBlockWithInitialConditionsPresenter, ExpressionProfileBuildingBlock>, IExpressionProfileInitialConditionsPresenter
   {
      private readonly IExpressionProfileBuildingBlockToExpressionProfileBuildingBlockDTOMapper _buildingBlockMapper;

      public ExpressionProfileInitialConditionsPresenter(IInitialConditionsView view,
         IInitialConditionToInitialConditionDTOMapper dtoMapper,
         IMoleculeIsPresentSelectionPresenter isPresentSelectionPresenter,
         IMoleculeNegativeValuesAllowedSelectionPresenter negativeStartValuesAllowedSelectionPresenter,
         IInitialConditionsTask<ExpressionProfileBuildingBlock> initialConditionsTask,
         IInitialConditionsCreator msvCreator,
         IMoBiContext context,
         IDeletePathAndValueEntityPresenter deletePathAndValueEntityPresenter,
         IFormulaToValueFormulaDTOMapper formulaToValueFormulaDTOMapper,
         IDimensionFactory dimensionFactory,
         IInitialConditionsDistributedInExpressionProfilePresenter distributedParameterPresenter,
         IExpressionProfileBuildingBlockToExpressionProfileBuildingBlockDTOMapper buildingBlockMapper) : 
         base(view, 
            dtoMapper, 
            initialConditionsTask, 
            msvCreator, 
            context, 
            deletePathAndValueEntityPresenter, 
            formulaToValueFormulaDTOMapper, 
            dimensionFactory, 
            distributedParameterPresenter)
      {
         _buildingBlockMapper = buildingBlockMapper;
         HideIsPresentView();
         HideIsNotPresentView();
         HideDeleteView();
         HideDeleteColumn();
         HideIsPresentColumn();
         HideValueOriginColumn();
         DisablePathColumns();
      }

      protected override IReadOnlyList<InitialConditionDTO> ValueDTOsFor(ExpressionProfileBuildingBlock buildingBlock) => _buildingBlockMapper.MapFrom(buildingBlock).InitialConditionDTOs;
   }
}