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
   public interface IInitialConditionsPresenter : IBuildingBlockWithInitialConditionsPresenter<InitialConditionsBuildingBlock>
   {

   }

   public class InitialConditionsPresenter : BuildingBlockWithInitialConditionsPresenter<
         IInitialConditionsView,
         IBuildingBlockWithInitialConditionsPresenter,
         InitialConditionsBuildingBlock>,
      IInitialConditionsPresenter
   {
      private readonly IInitialConditionsBuildingBlockToInitialConditionsBuildingBlockDTOMapper _buildingBlockMapper;

      public InitialConditionsPresenter(
         IInitialConditionsView view,
         IInitialConditionToInitialConditionDTOMapper dtoMapper,
         IInitialConditionsTask<InitialConditionsBuildingBlock> initialConditionsTask,
         IInitialConditionsCreator msvCreator,
         IMoBiContext context,
         IDeletePathAndValueEntityPresenter deletePathAndValueEntityPresenter,
         IFormulaToValueFormulaDTOMapper formulaToValueFormulaDTOMapper,
         IDimensionFactory dimensionFactory,
         IInitialConditionsDistributedPathAndValueEntityPresenter distributedParameterPresenter,
         IInitialConditionsBuildingBlockToInitialConditionsBuildingBlockDTOMapper buildingBlockMapper)
         : base(view, dtoMapper, initialConditionsTask, msvCreator, context, deletePathAndValueEntityPresenter, formulaToValueFormulaDTOMapper, dimensionFactory, distributedParameterPresenter)
      {
         _buildingBlockMapper = buildingBlockMapper;
      }

      protected override IReadOnlyList<InitialConditionDTO> ValueDTOsFor(InitialConditionsBuildingBlock buildingBlock) => _buildingBlockMapper.MapFrom(buildingBlock).ParameterDTOs;
   }
}