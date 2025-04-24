using System.Collections.Generic;
using System.Linq;
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
         IInitialConditionsTask<ExpressionProfileBuildingBlock> initialConditionsTask,
         IInitialConditionsCreator msvCreator,
         IMoBiContext context,
         IFormulaToValueFormulaDTOMapper formulaToValueFormulaDTOMapper,
         IDimensionFactory dimensionFactory,
         IInitialConditionsDistributedInExpressionProfilePresenter distributedParameterPresenter,
         IExpressionProfileBuildingBlockToExpressionProfileBuildingBlockDTOMapper buildingBlockMapper) : 
         base(view, 
            dtoMapper, 
            initialConditionsTask, 
            msvCreator, 
            context, 
            formulaToValueFormulaDTOMapper, 
            dimensionFactory, 
            distributedParameterPresenter)
      {
         _buildingBlockMapper = buildingBlockMapper;
         view.HideElements(HidablePathAndValuesViewElement.PresenceRibbon | HidablePathAndValuesViewElement.DeleteButton | HidablePathAndValuesViewElement.DeleteColumn | HidablePathAndValuesViewElement.ValueOriginColumn | HidablePathAndValuesViewElement.IsPresentColumn);
         DisablePathColumns();
      }

      protected override IReadOnlyList<InitialConditionDTO> ValueDTOsFor(ExpressionProfileBuildingBlock buildingBlock) => _buildingBlockMapper.MapFrom(buildingBlock).InitialConditionDTOs;
      protected override void SelectEntity(InitialConditionDTO dto)
      {
         _view.Select(dto);
      }

      protected override InitialConditionDTO DTOForBuilder(InitialCondition builder)
      {
         return _startValueDTOs.FirstOrDefault(x => Equals(x.PathWithValueObject, builder));
      }
   }
}