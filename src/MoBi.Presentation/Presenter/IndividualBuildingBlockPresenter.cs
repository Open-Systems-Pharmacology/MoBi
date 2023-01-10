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
   public interface IIndividualBuildingBlockPresenter : IPresenter<IIndividualBuildingBlockView>, IPathAndValueBuildingBlockPresenter<IndividualParameterDTO>
   {
      void Edit(IndividualBuildingBlock individualBuildingBlock);
   }

   public class IndividualBuildingBlockPresenter : PathWithValueBuildingBlockPresenter<IIndividualBuildingBlockView, IIndividualBuildingBlockPresenter, IndividualBuildingBlock, IndividualParameter, IndividualParameterDTO>, IIndividualBuildingBlockPresenter
   {
      private readonly IIndividualBuildingBlockToIndividualBuildingBlockDTOMapper _individualToDTOMapper;
      private IndividualBuildingBlockDTO _individualBuildingBlockDTO;

      public IndividualBuildingBlockPresenter(IIndividualBuildingBlockView view, IIndividualBuildingBlockToIndividualBuildingBlockDTOMapper individualToDTOMapper,
         IInteractionTaskForIndividualBuildingBlock interactionTask, IFormulaToValueFormulaDTOMapper formulaToValueFormulaDTOMapper, IDimensionFactory dimensionFactory) : base(view, interactionTask, formulaToValueFormulaDTOMapper, dimensionFactory)
      {
         _individualToDTOMapper = individualToDTOMapper;
      }

      public override void Edit(IndividualBuildingBlock individualBuildingBlock)
      {
         _buildingBlock = individualBuildingBlock;
         _individualBuildingBlockDTO = _individualToDTOMapper.MapFrom(individualBuildingBlock);
         _view.BindTo(_individualBuildingBlockDTO);
      }

      public override object Subject => _buildingBlock;

      public bool HasAtLeastTwoDistinctValues(int pathElementIndex)
      {
         return _individualBuildingBlockDTO.ParameterDTOs.HasAtLeastTwoDistinctValues(pathElementIndex);
      }
   }
}