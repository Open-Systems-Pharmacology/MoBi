using System.Linq;
using MoBi.Core.Events;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Extensions;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility.Events;

namespace MoBi.Presentation.Presenter
{
   public interface IIndividualBuildingBlockPresenter : 
      IPresenter<IIndividualBuildingBlockView>, 
      IPathAndValueBuildingBlockPresenter<IndividualParameterDTO>
   {
      void Edit(IndividualBuildingBlock individualBuildingBlock);
   }

   public class IndividualBuildingBlockPresenter : 
      PathAndValueBuildingBlockPresenter<IIndividualBuildingBlockView, IIndividualBuildingBlockPresenter, IndividualBuildingBlock, IndividualParameter, IndividualParameterDTO>, 
      IIndividualBuildingBlockPresenter,
      IListener<EntitySelectedEvent>
   {
      private readonly IIndividualBuildingBlockToIndividualBuildingBlockDTOMapper _individualToDTOMapper;
      private IndividualBuildingBlockDTO _individualBuildingBlockDTO;
      

      public IndividualBuildingBlockPresenter(IIndividualBuildingBlockView view, 
         IIndividualBuildingBlockToIndividualBuildingBlockDTOMapper individualToDTOMapper,
         IInteractionTasksForIndividualBuildingBlock interactionTask, 
         IFormulaToValueFormulaDTOMapper formulaToValueFormulaDTOMapper, 
         IDimensionFactory dimensionFactory,
         IIndividualDistributedPathAndValueEntityPresenter distributedPathAndValuePresenter) : base(view, interactionTask, formulaToValueFormulaDTOMapper, dimensionFactory, distributedPathAndValuePresenter)
      {
         _individualToDTOMapper = individualToDTOMapper;
      }

      public override void Edit(IndividualBuildingBlock individualBuildingBlock)
      {
         base.Edit(individualBuildingBlock);
         _individualBuildingBlockDTO = _individualToDTOMapper.MapFrom(individualBuildingBlock);
         _view.BindTo(_individualBuildingBlockDTO);
      }

      public bool HasAtLeastOneValue(int pathElementIndex)
      {
         return _individualBuildingBlockDTO.Parameters.HasAtLeastOneValue(pathElementIndex);
      }

      public void Handle(EntitySelectedEvent eventToHandle)
      {
         if (eventToHandle.ObjectBase is IndividualParameter individualParameter)
         {
            if (_buildingBlock.Contains(individualParameter))
               _view.Select(dtoFor(individualParameter));
         }
      }

      private IndividualParameterDTO dtoFor(IndividualParameter individualParameter)
      {
         return _individualBuildingBlockDTO.Parameters.FirstOrDefault(x => Equals(x.PathWithValueObject, individualParameter));
      }
   }
}